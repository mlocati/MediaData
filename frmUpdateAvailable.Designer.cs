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
            // frmUpdateAvailable
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnWebsite);
            this.Controls.Add(this.tbxUpdateInfo);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUpdateAvailable";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxUpdateInfo;
        private System.Windows.Forms.Button btnWebsite;
        private System.Windows.Forms.Button btnClose;
    }
}