using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MLocati.MediaData
{
    public class Tool
    {

        #region Events

        public class OutputReceivedEventArgs : EventArgs
        {
            public readonly string Text;
            public OutputReceivedEventArgs(string text)
            {
                this.Text = text;
            }
        }
        public delegate void OutputReceivedEventHandler(object sender, OutputReceivedEventArgs e);
        public event OutputReceivedEventHandler StdOutReceived;
        public event OutputReceivedEventHandler StdErrReceived;

        public delegate void StartedEventHandler(object sender);
        public event StartedEventHandler Started;
        private void FireStarted()
        {
            lock (this)
            {
                this.Fire(this.Started, this);
            }
        }
       
        public delegate void CompletedEventHandler(object sender, Result e);
        public event CompletedEventHandler Completed;
        private void FireFailed(Exception error)
        {
            lock (this)
            {
                this._failed = true;
                this._running = false;
                this._lastResult = new Result(this._stdOut.ToString(), this._stdErr.ToString(), this._exitCode, error);
                Monitor.Pulse(this);
                this.Fire(this.Completed, this, this._lastResult);
            }
        }
        private void FireCancelled()
        {
            lock (this)
            {
                this._cancelled = true;
                this._running = false;
                this._lastResult = new Result(this._stdOut.ToString(), this._stdErr.ToString(), null, new OperationCanceledException());
                Monitor.Pulse(this);
                this.Fire(this.Completed, this, this._lastResult);
            }
        }
        private void FireSucceeded()
        {
            lock (this)
            {
                this._succeded = true;
                this._running = false;
                this._lastResult = new Result(this._stdOut.ToString(), this._stdErr.ToString(), this._exitCode);
                Monitor.Pulse(this);
                this.Fire(this.Completed, this, this._lastResult);
            }
        }

        #endregion


        #region Types

        private class WhichAttribute : Attribute
        {
            public readonly string Name;
            public readonly string ExeName;
            public readonly string DownloadURL;
            public readonly string AltName;
            public WhichAttribute(string name, string exeName, string downloadURL)
                : this(name, exeName, downloadURL, null)
            { }
            public WhichAttribute(string name, string exeName, string downloadURL, string altNameID)
                : base()
            {
                this.Name = name;
                this.ExeName = exeName;
                this.DownloadURL = downloadURL;
                this.AltName = (altNameID == null) ? name : Localizer.GetString(altNameID);
            }
            private static Dictionary<Which, WhichAttribute> _getForCache = null;
            public static WhichAttribute GetFor(Which which)
            {
                if (WhichAttribute._getForCache == null)
                {
                    WhichAttribute._getForCache = new Dictionary<Which, WhichAttribute>();
                }
                if (!WhichAttribute._getForCache.ContainsKey(which))
                {
                    WhichAttribute._getForCache.Add(which, (WhichAttribute)typeof(Which).GetMember(which.ToString())[0].GetCustomAttributes(typeof(WhichAttribute), false)[0]);
                }
                return WhichAttribute._getForCache[which];
            }
            private string _fullExePath = null;
            public string FullExePath
            {
                get
                {
                    if (this._fullExePath == null)
                    {
                        return Path.Combine(Tool.Directory, this.ExeName);
                    }
                    return this._fullExePath;
                }
            }
        }

        public enum Which
        {
            [Which("Exiv2", "exiv2.exe", "http://www.exiv2.org/download.html")]
            Exiv2,
            [Which("FFmpeg", "ffmpeg.exe", "https://www.ffmpeg.org/download.html#build-windows")]
            FFmpeg,
            [Which("FFprobe", "ffprobe.exe", "https://www.ffmpeg.org/download.html#build-windows", "FFprobe_part_of_FFmpeg")]
            FFprobe,
        }

        public class AlreadyRunningException : ApplicationException
        {
            public AlreadyRunningException() :
                base(i18n.Async_operation_already_running)
            { }
        }

        public class ProgramFailedException : ApplicationException
        {
            public ProgramFailedException(int exitCode, string stdErr) :
                base(string.Format(i18n.Program_failed_with_returncode, exitCode) + (string.IsNullOrEmpty(stdErr) ? "" : (":\n" + stdErr)))
            { }
        }

        public class Result
        {
            public readonly string StdOut;
            public readonly string StdErr;
            private int? _exitCode;
            public int? ExitCode
            {
                get
                {
                    return this._exitCode.HasValue ? new int?(this._exitCode.Value) : null;
                }
            }
            public readonly Exception Error;
            public bool Cancelled
            {
                get
                {
                    return (this.Error != null) && (this.Error is OperationCanceledException);
                }
            }
            public bool Failed
            {
                get
                {
                    return !(this.Succeeded || this.Cancelled);
                }
            }
            public bool Succeeded
            {
                get
                {
                    return this.Error == null;
                }
            }
            public Result(string stdOut, string stdErr, int? exitCode)
                : this(stdOut, stdErr, exitCode, null)
            { }
            public Result(string stdOut, string stdErr, int? exitCode, Exception error)
            {
                this.StdOut = stdOut;
                this.StdErr = stdErr;
                this._exitCode = exitCode.HasValue ? new int?(exitCode.Value) : null;
                this.Error = error;
            }
        }

        #endregion


        #region Static properties

        private static string _directory = null;
        public static string Directory
        {
            get
            {
                if (Tool._directory == null)
                {
                    Tool._directory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "tools");
                }
                return Tool._directory;
            }
        }

        #endregion


        #region Instance properties

        private ISynchronizeInvoke _syncTarget;

        private ProcessStartInfo _startInfo;
        public ProcessStartInfo StartInfo
        {
            get
            {
                return this._startInfo;
            }
        }

        private Process _process;

        private bool _running;

        private bool _cancelPending;
        private bool CancelPending
        {
            get
            {
                lock (this)
                {
                    return this._cancelPending;
                }
            }
        }

        private bool _cancelled;
        public bool Cancelled
        {
            get
            {
                lock (this)
                {
                    return this._cancelled;
                }
            }
        }

        private bool _failed;
        public bool Failed
        {
            get
            {
                lock (this)
                {
                    return this._failed;
                }
            }
        }

        private bool _succeded;
        private bool Succeeded
        {
            get
            {
                lock (this)
                {
                    return this._succeded;
                }
            }
        }

        public bool Running
        {
            get
            {
                lock (this)
                {
                    return this._running;
                }
            }
        }

        private int? _exitCode;
        public int? ExitCode
        {
            get
            {
                lock (this)
                {
                    return this._exitCode.HasValue ? (int?)this._exitCode.Value : null;
                }
            }
        }

        private StringBuilder _stdOut;
        private StringBuilder _stdErr;
        private int[] _validExitCodes;
        private Result _lastResult;
        public Result LastResult
        {
            get
            {
                lock (this)
                {
                    return this._lastResult;
                }
            }
        }

        #endregion


        #region Constructors

        public Tool(Which which, string[] arguments)
            : this(which, arguments, null)
        {
        }

        public Tool(Which which, string[] arguments, int[] validExitCodes)
        {
            this._syncTarget = null;
            this._startInfo = new ProcessStartInfo();
            this._startInfo.UseShellExecute = false;
            this._startInfo.RedirectStandardOutput = true;
            this._startInfo.RedirectStandardError = true;
            this._startInfo.CreateNoWindow = true;
            this._startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            this._startInfo.FileName = WhichAttribute.GetFor(which).FullExePath;
            StringBuilder args = null;
            if (arguments != null)
            {
                foreach (string argument in arguments)
                {
                    if (argument != null)
                    {
                        string a = argument.Trim();
                        if (a.Length > 0)
                        {
                            if (args == null)
                            {
                                args = new StringBuilder();
                            }
                            else
                            {
                                args.Append(' ');
                            }
                            args.Append(a);
                        }
                    }
                }
            }
            this._startInfo.Arguments = ((args == null) ? "" : args.ToString());
            if (validExitCodes == null || validExitCodes.Length == 0)
            {
                this._validExitCodes = null;
            }
            else
            {
                this._validExitCodes = (int[])validExitCodes.Clone();
            }
            this._running = false;
            this._exitCode = null;
            this._stdOut = new StringBuilder();
            this._stdErr = new StringBuilder();
            this._lastResult = null;
        }

        #endregion


        #region Public methods

        public Tool.Result RunGUI(bool autoCloseOnSuccess)
        {
            return this.RunGUI(null, autoCloseOnSuccess);
        }

        public Tool.Result RunGUI(IWin32Window parentWindow, bool autoCloseOnSuccess)
        {
            lock (this)
            {
                if (this._running)
                {
                    return new Result("", "", null, new AlreadyRunningException());
                }
                this._running = true;
            }
            this.PrepareForLaunch();
            using (frmTool f = new frmTool(this, autoCloseOnSuccess))
            {
                this._syncTarget = f;
                f.Shown += delegate (object sender, EventArgs e)
                {
                    new MethodInvoker(this.Launch).BeginInvoke(null, null);
                };
                if (f.ShowDialog(parentWindow) == DialogResult.OK)
                {
                    return this._lastResult;
                }
                else
                {
                    return new Result("", "", null, new OperationCanceledException());
                }
            }
        }

        public Tool.Result Run()
        {
            lock (this)
            {
                if (this._running)
                {
                    return new Result("", "", null, new AlreadyRunningException());
                }
                this._running = true;
            }
            this.PrepareForLaunch();
            new MethodInvoker(this.Launch).BeginInvoke(null, null);
            this.WaitUntilDone();
            return this._lastResult;
        }

        public void Cancel()
        {
            this.Cancel(false);
        }

        public void Cancel(bool wait)
        {
            lock (this)
            {
                this._cancelPending = true;
                if (wait)
                {
                    while (this.Running)
                    {
                        Monitor.Wait(this, 1000);
                    }
                }
            }
        }

        public bool WaitUntilDone()
        {
            lock (this)
            {
                while (this.Running)
                {
                    Monitor.Wait(this, 1000);
                }
            }
            return this.Succeeded;
        }

        #endregion


        #region Public static methods

        public static bool Check()
        {
            foreach (Which w in Enum.GetValues(typeof(Which)))
            {
                WhichAttribute a = WhichAttribute.GetFor(w);
                if (!File.Exists(a.FullExePath))
                {
                    if (MessageBox.Show(string.Format(i18n.Missing_tool_message, a.AltName, a.FullExePath), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        Process.Start(a.DownloadURL);
                    }
                    return false;
                }
            }
            return true;
        }

        public static string Escape(string argument)
        {
            if (string.IsNullOrEmpty(argument))
            {
                return "";
            }
            return argument.Contains(" ") ? string.Format("\"{0}\"", argument) : argument;
        }

        #endregion


        #region Private methods

        private void PrepareForLaunch()
        {
            lock (this)
            {
                this._syncTarget = null;
                this._cancelPending = false;
                this._cancelled = false;
                this._failed = false;
                this._succeded = false;
                this._exitCode = null;
                this._exitCode = null;
                this._lastResult = null;
                this._stdOut.Clear();
                this._stdErr.Clear();
            }
        }

        private void Launch()
        {
            using (this._process = new Process())
            {
                try
                {
                    this._process.StartInfo = this._startInfo;
                    this._process.Start();
                    this.FireStarted();
                    new MethodInvoker(this.ReadStdOut).BeginInvoke(null, null);
                    new MethodInvoker(this.ReadStdErr).BeginInvoke(null, null);
                    while (!this._process.HasExited)
                    {
                        Thread.Sleep(100);
                        if (this.CancelPending)
                        {
                            this._process.Kill();
                            this.FireCancelled();
                        }
                    }
                }
                catch (Exception e)
                {
                    try
                    {
                        this.FireFailed(e);
                    }
                    catch
                    { }
                    if (e is SystemException)
                    {
                        throw;
                    }
                }
                lock (this)
                {
                    if (!this._cancelled && !this._failed)
                    {
                        this._exitCode = new int?(this._process.ExitCode);
                        if (this._validExitCodes != null && Array.IndexOf(this._validExitCodes, this._exitCode.Value) < 0)
                        {
                            this.FireFailed(new ProgramFailedException(this._exitCode.Value, this._stdErr.ToString()));
                        }
                        else
                        {
                            this.FireSucceeded();
                        }
                    }
                }
            }
        }

        private void Fire(Delegate dlg, params object[] pList)
        {
            if (dlg != null && this._syncTarget != null)
            {
                this._syncTarget.BeginInvoke(dlg, pList);
            }
        }

        private void ReadStdOut()
        {
            string line;
            for (;;)
            {
                try
                {
                    line = this._process.StandardOutput.ReadLine();
                }
                catch
                {
                    line = null;
                }
                if (line == null)
                {
                    break;
                }
                this._stdOut.AppendLine(line);
                this.Fire(this.StdOutReceived, this, new OutputReceivedEventArgs(line));
            }
        }
        private void ReadStdErr()
        {
            string line;
            for (;;)
            {
                try
                {
                    line = this._process.StandardError.ReadLine();
                }
                catch
                {
                    line = null;
                }
                if (line == null)
                {
                    break;
                }
                this._stdErr.AppendLine(line);
                this.Fire(this.StdErrReceived, this, new OutputReceivedEventArgs(line));
            }
        }

        #endregion

    }
}
