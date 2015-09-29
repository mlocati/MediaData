namespace MLocati.MediaData
{
    partial class frmAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
            this.picMainLogo = new System.Windows.Forms.PictureBox();
            this.lblProduct = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.lnkWebsite = new System.Windows.Forms.LinkLabel();
            this.lnkLicenses = new System.Windows.Forms.LinkLabel();
            this.lnkCheckUpdates = new System.Windows.Forms.LinkLabel();
            this.bgwCheckUpdates = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.picMainLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // picMainLogo
            // 
            resources.ApplyResources(this.picMainLogo, "picMainLogo");
            this.picMainLogo.Name = "picMainLogo";
            this.picMainLogo.TabStop = false;
            // 
            // lblProduct
            // 
            resources.ApplyResources(this.lblProduct, "lblProduct");
            this.lblProduct.Name = "lblProduct";
            // 
            // lblVersion
            // 
            resources.ApplyResources(this.lblVersion, "lblVersion");
            this.lblVersion.Name = "lblVersion";
            // 
            // lblCopyright
            // 
            resources.ApplyResources(this.lblCopyright, "lblCopyright");
            this.lblCopyright.Name = "lblCopyright";
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // lnkWebsite
            // 
            resources.ApplyResources(this.lnkWebsite, "lnkWebsite");
            this.lnkWebsite.Name = "lnkWebsite";
            this.lnkWebsite.TabStop = true;
            this.lnkWebsite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkWebsite_LinkClicked);
            // 
            // lnkLicenses
            // 
            resources.ApplyResources(this.lnkLicenses, "lnkLicenses");
            this.lnkLicenses.Name = "lnkLicenses";
            this.lnkLicenses.TabStop = true;
            this.lnkLicenses.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLicenses_LinkClicked);
            // 
            // lnkCheckUpdates
            // 
            resources.ApplyResources(this.lnkCheckUpdates, "lnkCheckUpdates");
            this.lnkCheckUpdates.Name = "lnkCheckUpdates";
            this.lnkCheckUpdates.TabStop = true;
            this.lnkCheckUpdates.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCheckUpdates_LinkClicked);
            // 
            // bgwCheckUpdates
            // 
            this.bgwCheckUpdates.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwCheckUpdates_DoWork);
            this.bgwCheckUpdates.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwCheckUpdates_RunWorkerCompleted);
            // 
            // frmAbout
            // 
            this.AcceptButton = this.btnClose;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.Controls.Add(this.lnkCheckUpdates);
            this.Controls.Add(this.lnkLicenses);
            this.Controls.Add(this.lnkWebsite);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblCopyright);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblProduct);
            this.Controls.Add(this.picMainLogo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAbout";
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAbout_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.picMainLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picMainLogo;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.LinkLabel lnkWebsite;
        private System.Windows.Forms.LinkLabel lnkLicenses;
        private System.Windows.Forms.LinkLabel lnkCheckUpdates;
        private System.ComponentModel.BackgroundWorker bgwCheckUpdates;
    }
}