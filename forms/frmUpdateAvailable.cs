using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace MLocati.MediaData
{
    public partial class frmUpdateAvailable : Form
    {

        #region Private types

        private enum States
        {
            Info,
            Downloading,
        }

        private class DownloadInfo
        {
            private Uri _remoteURL;
            private string _localFile;
            private long _totalSize;
            private DateTime? _realDownloadStarted;
            private long _downloadedBytes;

            [Browsable(false)]
            public string LocalFile
            {
                get
                { return this._localFile; }
                set
                {
                    this._localFile = value;
                }
            }

            
            [Browsable(false)]
            public long TotalSize
            {
                get
                { return this._totalSize; }
                set
                {
                    this._totalSize = value;
                    if (value > 0L && !this._realDownloadStarted.HasValue)
                    {
                        this._realDownloadStarted = DateTime.UtcNow;
                    }
                }
            }

            
            [Browsable(false)]
            public long DownloadedBytes
            {
                get
                { return this._downloadedBytes; }
                set
                { this._downloadedBytes = value; }
            }

            [Browsable(false)]
            public TimeSpan? ElapsedTime
            {
                get
                {
                    return this._realDownloadStarted.HasValue ? (DateTime.UtcNow - this._realDownloadStarted.Value) : (TimeSpan?)null;
                }
            }

            [Browsable(false)]
            public TimeSpan? TotalTime
            {
                get
                {
                    TimeSpan? result = null;
                    if (this._totalSize > 0 && this._downloadedBytes > 0)
                    {
                        TimeSpan? elapsed = this.ElapsedTime;
                        if (elapsed.HasValue)
                        {
                            double totalTicks = Math.Floor(Convert.ToDouble(elapsed.Value.Ticks) * Convert.ToDouble(this._totalSize) / Convert.ToDouble(this._downloadedBytes));
                            if (totalTicks < long.MaxValue)
                            {
                                result = new TimeSpan(Convert.ToInt64(totalTicks));
                            }
                        }
                    }
                    return result;
                }
            }

            [Browsable(false)]
            public TimeSpan? RemainingTime
            {
                get
                {
                    TimeSpan? result = null;
                    TimeSpan? elapsed = this.ElapsedTime;
                    if (elapsed.HasValue)
                    {
                        TimeSpan? total = this.TotalTime;
                        if (total.HasValue && total.Value >= elapsed.Value)
                        {
                            result = total.Value - elapsed.Value;
                        }
                    }
                    return result;
                }
            }


            [Browsable(true), Localizer.DisplayName("URL")]
            public string Showtime_RemoteURL
            {
                get
                { return this._remoteURL.AbsoluteUri; }
            }

            [Browsable(true), Localizer.DisplayName("Local_file_name")]
            public string Showtime_LocalFile
            {
                get
                { return this._localFile; }
            }

            [Browsable(true), Localizer.DisplayName("Total_size")]
            public string Showtime_TotalSize
            {
                get
                { return (this._totalSize > 0L) ? Localizer.FormatSize(this._totalSize) : ""; }
            }

            [Browsable(true), Localizer.DisplayName("Downloaded_bytes")]
            public string Showtime_DownloadedSize
            {
                get
                { return Localizer.FormatSize(this._downloadedBytes); }
            }

            [Browsable(true), Localizer.DisplayName("Time_elapsed")]
            public string Showtime_ElapsedTime
            {
                get
                {
                    return DownloadInfo.Show(this.ElapsedTime);
                }
            }

            [Browsable(true), Localizer.DisplayName("Time_remaining")]
            public string Showtime_RemainingTime
            {
                get
                {
                    return DownloadInfo.Show(this.RemainingTime);
                }
            }

            [Browsable(true), Localizer.DisplayName("Time_total")]
            public string Showtime_TotalTime
            {
                get
                {
                    return DownloadInfo.Show(this.TotalTime);
                }
            }

            [Browsable(true), Localizer.DisplayName("Download_speed")]
            public string Showtime_DownloadSpeed
            {
                get
                {
                    string result = "";
                    if (this._downloadedBytes > 0L)
                    {
                        TimeSpan? elapsed = this.ElapsedTime;
                        if (elapsed.HasValue)
                        {
                            double bytesPerSecond = Convert.ToDouble(this._downloadedBytes) / elapsed.Value.TotalSeconds;
                            result = Localizer.FormatDownloadSpeed(bytesPerSecond);
                        }
                    }
                    return result;
                }
            }

            public DownloadInfo(Uri remoteURL)
            {
                this._remoteURL = remoteURL;
                this._localFile = "";
                this._totalSize = -1L;
                this._downloadedBytes = 0L;
                this._realDownloadStarted = null;
            }

            private static string Show(TimeSpan? ts)
            {
                if (ts.HasValue)
                {
                    string format;
                    if (ts.Value.TotalDays >= 1D)
                    {
                        format = @"d\:hh\:mm\:ss";
                    }
                    else if(ts.Value.TotalHours >= 1D)
                    {
                        format = @"hh\:mm\:ss";
                    }
                    else
                    {
                        format = @"mm\:ss";
                    }
                    return ts.Value.ToString(format);
                }
                else
                {
                    return "";
                }
            }
        }

        #endregion


        #region Instance properties

        private Uri _setupUrl;

        private States _state;
        private States State
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
                this.pnlMain.Visible = value == States.Info;
                this.pnlDownloading.Visible = value == States.Downloading;
                switch (value)
                {
                    case States.Info:
                        this.CancelButton = this.btnClose;
                        break;
                    case States.Downloading:
                        this.CancelButton = this.btnCancelDownload;
                        break;
                }
            }
        }

        private BackgroundWorker _bgwDownloader = null;

        #endregion


        #region Constructors

        public frmUpdateAvailable(string updateInfo, Uri setupUrl)
        {
            InitializeComponent();
            this.Icon = Program.Icon;
            this.tbxUpdateInfo.Text = updateInfo;
            this._setupUrl = setupUrl;
            this.btnInstall.Enabled = setupUrl != null;
            this.pnlDownloading.Dock = this.pnlMain.Dock = DockStyle.Fill;
            this.State = States.Info;
        }

        #endregion


        #region GUI events

        private void btnWebsite_Click(object sender, EventArgs e)
        {
            using (BackgroundWorker bgw = new BackgroundWorker())
            {
                bgw.DoWork += delegate (object s, DoWorkEventArgs ea)
                {
                    using (Process process = new Process())
                    {
                        process.StartInfo.UseShellExecute = true;
                        process.StartInfo.FileName = "https://github.com/mlocati/MediaData/releases";
                        process.Start();
                    }
                };
                bgw.RunWorkerAsync();
            }
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            this.pbDownload.Style = ProgressBarStyle.Marquee;
            this.State = States.Downloading;
            this.pgDownload.SelectedObject = new DownloadInfo(this._setupUrl);
            this._bgwDownloader = new BackgroundWorker();
            this._bgwDownloader.WorkerReportsProgress = true;
            this._bgwDownloader.WorkerSupportsCancellation = true;
            this._bgwDownloader.DoWork += this.DownloadUpdate;
            this._bgwDownloader.ProgressChanged += this.DownloadProgress;
            this._bgwDownloader.RunWorkerCompleted += this.DownloadCompleted;
            this._bgwDownloader.RunWorkerAsync();
        }

        private void btnCancelDownload_Click(object sender, EventArgs e)
        {
            if (this._bgwDownloader != null && this._bgwDownloader.IsBusy)
            {
                this._bgwDownloader.CancelAsync();
            }
        }

        private void frmUpdateAvailable_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.State == States.Downloading)
            {
                e.Cancel = true;
                this.btnCancelDownload_Click(null, null);
            }
        }

        #endregion


        #region Instance methods

        private void DownloadUpdate(object sender, DoWorkEventArgs e)
        {
            WebResponse webResponse = null;
            string saveFileName = "";
            long[] state = new long[] { -1, 0 };
            bool success = false;
            try
            {
                string downloadsFolder = MyIO.GetDownloadsFolder();
                string[] urlParths = this._setupUrl.LocalPath.Split('/');
                string fName = Path.GetFileNameWithoutExtension(urlParths[urlParths.Length - 1]);
                string fExt = Path.GetExtension(urlParths[urlParths.Length - 1]);
                for (int i = 0; ; i++)
                {
                    string s = Path.Combine(downloadsFolder, fName + ((i == 0) ? "" : string.Format(" ({0})", i)) + fExt);
                    if (!(File.Exists(s) || Directory.Exists(s)))
                    {
                        saveFileName = s;
                        break;
                    }
                }
                this._bgwDownloader.ReportProgress(0, saveFileName);
                using (FileStream fileStream = new FileStream(saveFileName, FileMode.CreateNew, FileAccess.Write, FileShare.Read))
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this._setupUrl);
                    request.AllowAutoRedirect = true;
                    request.MaximumAutomaticRedirections = 5;
                    request.UserAgent = string.Format("{0} v{1}", Application.ProductName, Application.ProductVersion);
                    request.Credentials = CredentialCache.DefaultCredentials;
                    if (this._bgwDownloader.CancellationPending)
                    {
                        throw new OperationCanceledException();
                    }
                    try
                    {
                        webResponse = request.GetResponse();
                    }
                    catch (WebException x)
                    {
                        webResponse = x.Response;
                        if (webResponse == null)
                        {
                            throw x;
                        }
                    }
                    HttpWebResponse response = (HttpWebResponse)webResponse;
                    if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 300)
                    {
                        throw new Exception(response.StatusDescription);
                    }
                    if (this._bgwDownloader.CancellationPending)
                    {
                        throw new OperationCanceledException();
                    }
                    state[0] = response.ContentLength;
                    int bufferSize = 0;
                    if (state[0] > 1L)
                    {
                        bufferSize = Convert.ToInt32(state[0] >> 10);
                    }
                    if (bufferSize < 4096)
                    {
                        bufferSize = 4096;
                    }
                    byte[] buffer = new byte[bufferSize];
                    int readBytes;
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        for (;;)
                        {
                            if (this._bgwDownloader.CancellationPending)
                            {
                                throw new OperationCanceledException();
                            }
                            readBytes = responseStream.Read(buffer, 0, bufferSize);
                            if (readBytes <= 0)
                            {
                                break;
                            }
                            fileStream.Write(buffer, 0, readBytes);
                            state[1] += readBytes;
                            this._bgwDownloader.ReportProgress(0, state);
                        }
                    }
                }
                success = true;
                e.Result = saveFileName;
            }
            catch (Exception x)
            {
                e.Result = x;
            }
            finally
            {
                if (webResponse != null)
                {
                    try
                    { webResponse.Close(); }
                    catch
                    { }
                }
                if (!success && saveFileName.Length > 0 && File.Exists(saveFileName))
                {
                    try
                    { File.Delete(saveFileName); }
                    catch
                    { }
                }
            }
        }

        private void DownloadProgress(object sender, ProgressChangedEventArgs e)
        {
            DownloadInfo di = (DownloadInfo)this.pgDownload.SelectedObject;
            if (e.UserState is string)
            {
                di.LocalFile = (string)e.UserState;
            }
            else if (e.UserState is long[])
            {
                long[] progress = (long[])e.UserState;
                if (progress[0] > 0L && di.TotalSize < 0)
                {
                    di.TotalSize = progress[0];
                    this.pbDownload.Minimum = this.pbDownload.Value = 0;
                    this.pbDownload.Maximum = Int32.MaxValue;
                    this.pbDownload.Maximum = Convert.ToInt32(di.TotalSize >> 7);
                    this.pbDownload.Style = ProgressBarStyle.Continuous;
                }
                if (this.pbDownload.Style != ProgressBarStyle.Marquee)
                {
                    this.pbDownload.Value = Convert.ToInt32(progress[1] >> 7);
                }
                di.DownloadedBytes = progress[1];
            }
            this.pgDownload.Refresh();
            this.pbDownload.Refresh();
        }

        private void DownloadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this._bgwDownloader.Dispose();
            this._bgwDownloader = null;
            Exception error = e.Error;
            if (error == null)
            {
                error = e.Result as Exception;
                if (error == null)
                {
                    if (e.Cancelled)
                    {
                        error = new OperationCanceledException();
                    }
                }
            }
            if (error != null)
            {
                this.State = States.Info;
                if (!(error is OperationCanceledException))
                {
                    MessageBox.Show(error.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }
            string downloadedFile = (string)e.Result;
            try
            {
                
                using (Process process = new Process())
                {
                    process.StartInfo.CreateNoWindow = false;
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.WorkingDirectory = Path.GetDirectoryName(downloadedFile);
                    process.StartInfo.FileName = downloadedFile;
                    process.StartInfo.Arguments = new System.Text.StringBuilder()
                        .Append("/DIR=").Append(Tool.Escape(Path.GetDirectoryName(Application.ExecutablePath)))
                        .Append(' ')
                        .Append("/SILENT")
                        .ToString();
                    process.Start();
                }
            }
            catch(Exception x)
            {
                this.State = States.Info;
                MessageBox.Show(string.Format(i18n.Unable_to_launch_updater_X_error_Y, downloadedFile, x.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Environment.Exit(0);
        }

        #endregion

    }
}
