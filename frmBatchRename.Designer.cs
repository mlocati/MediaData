namespace MLocati.MediaData
{
    partial class frmBatchRename
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBatchRename));
            this.dgvRename = new System.Windows.Forms.DataGridView();
            this.colOriginalFilename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNewFilename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblFormat = new System.Windows.Forms.Label();
            this.cbxFormat = new System.Windows.Forms.ComboBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.lblFormatHelpLabel = new System.Windows.Forms.Label();
            this.flpFormat = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblExtension = new System.Windows.Forms.Label();
            this.cbxExtension = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRename)).BeginInit();
            this.flpFormat.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvRename
            // 
            this.dgvRename.AllowUserToAddRows = false;
            this.dgvRename.AllowUserToDeleteRows = false;
            this.dgvRename.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgvRename, "dgvRename");
            this.dgvRename.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRename.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRename.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colOriginalFilename,
            this.colNewFilename});
            this.dgvRename.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvRename.MultiSelect = false;
            this.dgvRename.Name = "dgvRename";
            this.dgvRename.ReadOnly = true;
            this.dgvRename.RowHeadersVisible = false;
            this.dgvRename.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // colOriginalFilename
            // 
            this.colOriginalFilename.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colOriginalFilename.DataPropertyName = "OriginalFilename";
            resources.ApplyResources(this.colOriginalFilename, "colOriginalFilename");
            this.colOriginalFilename.Name = "colOriginalFilename";
            this.colOriginalFilename.ReadOnly = true;
            // 
            // colNewFilename
            // 
            this.colNewFilename.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colNewFilename.DataPropertyName = "NewFilenameShown";
            resources.ApplyResources(this.colNewFilename, "colNewFilename");
            this.colNewFilename.Name = "colNewFilename";
            this.colNewFilename.ReadOnly = true;
            // 
            // lblFormat
            // 
            resources.ApplyResources(this.lblFormat, "lblFormat");
            this.lblFormat.Name = "lblFormat";
            // 
            // cbxFormat
            // 
            resources.ApplyResources(this.cbxFormat, "cbxFormat");
            this.cbxFormat.Name = "cbxFormat";
            this.cbxFormat.TextChanged += new System.EventHandler(this.cbxFormat_TextChanged);
            this.cbxFormat.Enter += new System.EventHandler(this.cbxFormat_Enter);
            this.cbxFormat.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cbxFormat_KeyUp);
            this.cbxFormat.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cbxFormat_MouseUp);
            // 
            // btnApply
            // 
            resources.ApplyResources(this.btnApply, "btnApply");
            this.btnApply.Name = "btnApply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // lblFormatHelpLabel
            // 
            resources.ApplyResources(this.lblFormatHelpLabel, "lblFormatHelpLabel");
            this.lblFormatHelpLabel.Name = "lblFormatHelpLabel";
            // 
            // flpFormat
            // 
            resources.ApplyResources(this.flpFormat, "flpFormat");
            this.flpFormat.Controls.Add(this.lblFormatHelpLabel);
            this.flpFormat.Name = "flpFormat";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblExtension
            // 
            resources.ApplyResources(this.lblExtension, "lblExtension");
            this.lblExtension.Name = "lblExtension";
            // 
            // cbxExtension
            // 
            resources.ApplyResources(this.cbxExtension, "cbxExtension");
            this.cbxExtension.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxExtension.Name = "cbxExtension";
            this.cbxExtension.SelectedIndexChanged += new System.EventHandler(this.cbxExtension_SelectedIndexChanged);
            // 
            // frmBatchRename
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.lblExtension);
            this.Controls.Add(this.flpFormat);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.cbxExtension);
            this.Controls.Add(this.cbxFormat);
            this.Controls.Add(this.lblFormat);
            this.Controls.Add(this.dgvRename);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBatchRename";
            ((System.ComponentModel.ISupportInitialize)(this.dgvRename)).EndInit();
            this.flpFormat.ResumeLayout(false);
            this.flpFormat.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvRename;
        private System.Windows.Forms.Label lblFormat;
        private System.Windows.Forms.ComboBox cbxFormat;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Label lblFormatHelpLabel;
        private System.Windows.Forms.FlowLayoutPanel flpFormat;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblExtension;
        private System.Windows.Forms.ComboBox cbxExtension;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOriginalFilename;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNewFilename;
    }
}