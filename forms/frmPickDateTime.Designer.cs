namespace MLocati.MediaData
{
    partial class frmPickDateTime
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPickDateTime));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAccept = new System.Windows.Forms.Button();
            this.btnNone = new System.Windows.Forms.Button();
            this.myDateTimePicker = new MLocati.MediaData.ctxDateTime();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnAccept
            // 
            resources.ApplyResources(this.btnAccept, "btnAccept");
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // btnNone
            // 
            resources.ApplyResources(this.btnNone, "btnNone");
            this.btnNone.Name = "btnNone";
            this.btnNone.UseVisualStyleBackColor = true;
            this.btnNone.Click += new System.EventHandler(this.btnNone_Click);
            // 
            // myDateTimePicker
            // 
            resources.ApplyResources(this.myDateTimePicker, "myDateTimePicker");
            this.myDateTimePicker.Name = "myDateTimePicker";
            this.myDateTimePicker.SelectedValue = new System.DateTime(2015, 9, 29, 16, 34, 25, 0);
            // 
            // frmPickDateTime
            // 
            this.AcceptButton = this.btnAccept;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.myDateTimePicker);
            this.Controls.Add(this.btnNone);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPickDateTime";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnNone;
        private ctxDateTime myDateTimePicker;
    }
}