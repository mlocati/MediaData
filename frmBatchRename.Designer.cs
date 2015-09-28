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
            this.dgvRename = new System.Windows.Forms.DataGridView();
            this.colOriginalFilename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNewFilename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblFormat = new System.Windows.Forms.Label();
            this.cbxFormat = new System.Windows.Forms.ComboBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.lblFormatHelpLabel = new System.Windows.Forms.Label();
            this.lblFormatHelp = new System.Windows.Forms.Label();
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
            this.dgvRename.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRename.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRename.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRename.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colOriginalFilename,
            this.colNewFilename});
            this.dgvRename.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvRename.Location = new System.Drawing.Point(12, 98);
            this.dgvRename.MultiSelect = false;
            this.dgvRename.Name = "dgvRename";
            this.dgvRename.ReadOnly = true;
            this.dgvRename.RowHeadersVisible = false;
            this.dgvRename.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRename.Size = new System.Drawing.Size(411, 226);
            this.dgvRename.TabIndex = 5;
            // 
            // colOriginalFilename
            // 
            this.colOriginalFilename.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colOriginalFilename.DataPropertyName = "OriginalFilename";
            this.colOriginalFilename.HeaderText = "Original file name";
            this.colOriginalFilename.Name = "colOriginalFilename";
            this.colOriginalFilename.ReadOnly = true;
            this.colOriginalFilename.Width = 204;
            // 
            // colNewFilename
            // 
            this.colNewFilename.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colNewFilename.DataPropertyName = "NewFilenameShown";
            this.colNewFilename.HeaderText = "New file name";
            this.colNewFilename.Name = "colNewFilename";
            this.colNewFilename.ReadOnly = true;
            this.colNewFilename.Width = 204;
            // 
            // lblFormat
            // 
            this.lblFormat.AutoSize = true;
            this.lblFormat.Location = new System.Drawing.Point(12, 15);
            this.lblFormat.Name = "lblFormat";
            this.lblFormat.Size = new System.Drawing.Size(39, 13);
            this.lblFormat.TabIndex = 0;
            this.lblFormat.Text = "Format";
            // 
            // cbxFormat
            // 
            this.cbxFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxFormat.Location = new System.Drawing.Point(79, 12);
            this.cbxFormat.Name = "cbxFormat";
            this.cbxFormat.Size = new System.Drawing.Size(344, 21);
            this.cbxFormat.TabIndex = 1;
            this.cbxFormat.TextChanged += new System.EventHandler(this.cbxFormat_TextChanged);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(348, 330);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 7;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            // 
            // lblFormatHelpLabel
            // 
            this.lblFormatHelpLabel.AutoSize = true;
            this.lblFormatHelpLabel.Location = new System.Drawing.Point(3, 0);
            this.lblFormatHelpLabel.Name = "lblFormatHelpLabel";
            this.lblFormatHelpLabel.Size = new System.Drawing.Size(105, 13);
            this.lblFormatHelpLabel.TabIndex = 0;
            this.lblFormatHelpLabel.Text = "Format placeholders:";
            // 
            // lblFormatHelp
            // 
            this.lblFormatHelp.AutoSize = true;
            this.lblFormatHelp.Location = new System.Drawing.Point(114, 0);
            this.lblFormatHelp.Name = "lblFormatHelp";
            this.lblFormatHelp.Size = new System.Drawing.Size(194, 26);
            this.lblFormatHelp.TabIndex = 1;
            this.lblFormatHelp.Text = "<Y>: year, <M>: month, <D>: day\r\n<h>: hours, <m>: minutes, <s>: seconds";
            // 
            // flpFormat
            // 
            this.flpFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpFormat.Controls.Add(this.lblFormatHelpLabel);
            this.flpFormat.Controls.Add(this.lblFormatHelp);
            this.flpFormat.Location = new System.Drawing.Point(12, 66);
            this.flpFormat.Name = "flpFormat";
            this.flpFormat.Size = new System.Drawing.Size(411, 26);
            this.flpFormat.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(267, 330);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblExtension
            // 
            this.lblExtension.AutoSize = true;
            this.lblExtension.Location = new System.Drawing.Point(12, 42);
            this.lblExtension.Name = "lblExtension";
            this.lblExtension.Size = new System.Drawing.Size(53, 13);
            this.lblExtension.TabIndex = 2;
            this.lblExtension.Text = "Extension";
            // 
            // cbxExtension
            // 
            this.cbxExtension.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxExtension.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxExtension.Location = new System.Drawing.Point(79, 39);
            this.cbxExtension.Name = "cbxExtension";
            this.cbxExtension.Size = new System.Drawing.Size(344, 21);
            this.cbxExtension.TabIndex = 3;
            this.cbxExtension.SelectedIndexChanged += new System.EventHandler(this.cbxExtension_SelectedIndexChanged);
            // 
            // frmBatchRename
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(435, 365);
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
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rename media files";
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
        private System.Windows.Forms.Label lblFormatHelp;
        private System.Windows.Forms.FlowLayoutPanel flpFormat;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblExtension;
        private System.Windows.Forms.ComboBox cbxExtension;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOriginalFilename;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNewFilename;
    }
}