using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace MLocati.MediaData
{
    public partial class frmUpdateAvailable : Form
    {
        public frmUpdateAvailable(string updateInfo)
        {
            InitializeComponent();
            this.Icon = Program.Icon;
            this.tbxUpdateInfo.Text = updateInfo;
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
    }
}
