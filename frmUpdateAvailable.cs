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
            private Uri _downloadingFile;
            [Browsable(true), Localizer.DisplayName("URL")]
            public string DownloadingFileShown
            {
                get
                { return this._downloadingFile.AbsoluteUri; }
            }

            private long _totalSize;
            [Browsable(false)]
            public long TotalSize
            {
                get
                { return this._totalSize; }
                set
                { this._totalSize = value; }
            }
            [Browsable(true), Localizer.DisplayName("Total_size")]
            public string TotalSizeShown
            {
                get
                { return (this._totalSize > 0L) ? Localizer.FormatSize(this._totalSize) : "?"; }
            }

            private long _downloadedBytes;
            [Browsable(false)]
            public long DownloadedBytes
            {
                get
                { return this._downloadedBytes; }
                set
                { this._downloadedBytes = value; }
            }
            [Browsable(true), Localizer.DisplayName("Downloaded_bytes")]
            public string DownloadedBytesShown
            {
                get
                { return Localizer.FormatSize(this._downloadedBytes); }
            }


            public DownloadInfo(Uri downloadingFile)
            {
                this._downloadingFile = downloadingFile;
                this._totalSize = -1L;
                this._downloadedBytes = 0L;
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
                        for(;;)
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
            long[] progress = (long[])e.UserState;
            if (progress[0] > 0L && di.TotalSize < 0)
            {
                di.TotalSize = progress[0];
                this.pbDownload.Minimum = this.pbDownload.Value = 0;
                this.pbDownload.Maximum = Int32.MaxValue;
                this.pbDownload.Maximum = Convert.ToInt32(di.TotalSize);
                this.pbDownload.Style = ProgressBarStyle.Continuous;
            }
            di.DownloadedBytes = progress[1];
            if (this.pbDownload.Style != ProgressBarStyle.Marquee)
            {
                this.pbDownload.Value = Convert.ToInt32(progress[1]);
            }
            this.pgDownload.SelectedObject = di;
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
            MessageBox.Show(string.Format("Close all and launch {0}", downloadedFile));
        }

        #endregion

    }
}
