namespace MLocati.MediaData
{
    partial class frmUpdateAvailable
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUpdateAvailable));
            this.tbxUpdateInfo = new System.Windows.Forms.TextBox();
            this.btnWebsite = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnInstall = new System.Windows.Forms.Button();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlDownloading = new System.Windows.Forms.Panel();
            this.pgDownload = new System.Windows.Forms.PropertyGrid();
            this.pbDownload = new System.Windows.Forms.ProgressBar();
            this.btnCancelDownload = new System.Windows.Forms.Button();
            this.lblDownloadWarning = new System.Windows.Forms.Label();
            this.pnlMain.SuspendLayout();
            this.pnlDownloading.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbxUpdateInfo
            // 
            resources.ApplyResources(this.tbxUpdateInfo, "tbxUpdateInfo");
            this.tbxUpdateInfo.Name = "tbxUpdateInfo";
            this.tbxUpdateInfo.ReadOnly = true;
            // 
            // btnWebsite
            // 
            resources.ApplyResources(this.btnWebsite, "btnWebsite");
            this.btnWebsite.Name = "btnWebsite";
            this.btnWebsite.UseVisualStyleBackColor = true;
            this.btnWebsite.Click += new System.EventHandler(this.btnWebsite_Click);
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnInstall
            // 
            resources.ApplyResources(this.btnInstall, "btnInstall");
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.tbxUpdateInfo);
            this.pnlMain.Controls.Add(this.btnWebsite);
            this.pnlMain.Controls.Add(this.btnInstall);
            this.pnlMain.Controls.Add(this.btnClose);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Name = "pnlMain";
            // 
            // pnlDownloading
            // 
            this.pnlDownloading.Controls.Add(this.lblDownloadWarning);
            this.pnlDownloading.Controls.Add(this.pgDownload);
            this.pnlDownloading.Controls.Add(this.pbDownload);
            this.pnlDownloading.Controls.Add(this.btnCancelDownload);
            resources.ApplyResources(this.pnlDownloading, "pnlDownloading");
            this.pnlDownloading.Name = "pnlDownloading";
            // 
            // pgDownload
            // 
            resources.ApplyResources(this.pgDownload, "pgDownload");
            this.pgDownload.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pgDownload.CommandsVisibleIfAvailable = false;
            this.pgDownload.Name = "pgDownload";
            this.pgDownload.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.pgDownload.ToolbarVisible = false;
            // 
            // pbDownload
            // 
            resources.ApplyResources(this.pbDownload, "pbDownload");
            this.pbDownload.Name = "pbDownload";
            // 
            // btnCancelDownload
            // 
            resources.ApplyResources(this.btnCancelDownload, "btnCancelDownload");
            this.btnCancelDownload.Name = "btnCancelDownload";
            this.btnCancelDownload.UseVisualStyleBackColor = true;
            this.btnCancelDownload.Click += new System.EventHandler(this.btnCancelDownload_Click);
            // 
            // lblDownloadWarning
            // 
            resources.ApplyResources(this.lblDownloadWarning, "lblDownloadWarning");
            this.lblDownloadWarning.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblDownloadWarning.Name = "lblDownloadWarning";
            // 
            // frmUpdateAvailable
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.Controls.Add(this.pnlDownloading);
            this.Controls.Add(this.pnlMain);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUpdateAvailable";
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUpdateAvailable_FormClosing);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.pnlDownloading.ResumeLayout(false);
            this.pnlDownloading.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbxUpdateInfo;
        private System.Windows.Forms.Button btnWebsite;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlDownloading;
        private System.Windows.Forms.Button btnCancelDownload;
        private System.Windows.Forms.ProgressBar pbDownload;
        private System.Windows.Forms.PropertyGrid pgDownload;
        private System.Windows.Forms.Label lblDownloadWarning;
    }
}