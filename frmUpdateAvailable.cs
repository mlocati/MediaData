﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace MLocati.MediaData
{
    public partial class frmUpdateAvailable : Form
    {
        private Uri _setupUrl;

        public frmUpdateAvailable(string updateInfo, Uri setupUrl)
        {
            InitializeComponent();
            this.Icon = Program.Icon;
            this.tbxUpdateInfo.Text = updateInfo;
            this._setupUrl = setupUrl;
            this.btnInstall.Enabled = setupUrl != null;
        }

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
            MessageBox.Show(string.Format("Download and install\n{0}", this._setupUrl));
        }
    }
}
