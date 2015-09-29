﻿namespace MLocati.MediaData
{
    partial class frmPickDateTimeWithAlternatives
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPickDateTimeWithAlternatives));
            this.btnAccept = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dgvAlternatives = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTimestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.radUseAlternatives = new System.Windows.Forms.RadioButton();
            this.radUseCustom = new System.Windows.Forms.RadioButton();
            this.radUseNull = new System.Windows.Forms.RadioButton();
            this.dtpCustom = new MLocati.MediaData.ctxDateTime();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlternatives)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAccept
            // 
            resources.ApplyResources(this.btnAccept, "btnAccept");
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // dgvAlternatives
            // 
            this.dgvAlternatives.AllowUserToAddRows = false;
            this.dgvAlternatives.AllowUserToDeleteRows = false;
            this.dgvAlternatives.AllowUserToResizeColumns = false;
            this.dgvAlternatives.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgvAlternatives, "dgvAlternatives");
            this.dgvAlternatives.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAlternatives.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAlternatives.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colTimestamp});
            this.dgvAlternatives.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvAlternatives.MultiSelect = false;
            this.dgvAlternatives.Name = "dgvAlternatives";
            this.dgvAlternatives.ReadOnly = true;
            this.dgvAlternatives.RowHeadersVisible = false;
            this.dgvAlternatives.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAlternatives.SelectionChanged += new System.EventHandler(this.dgvAlternatives_SelectionChanged);
            // 
            // colName
            // 
            this.colName.DataPropertyName = "Name";
            resources.ApplyResources(this.colName, "colName");
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colTimestamp
            // 
            this.colTimestamp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colTimestamp.DataPropertyName = "TimestampStr";
            resources.ApplyResources(this.colTimestamp, "colTimestamp");
            this.colTimestamp.Name = "colTimestamp";
            this.colTimestamp.ReadOnly = true;
            // 
            // radUseAlternatives
            // 
            resources.ApplyResources(this.radUseAlternatives, "radUseAlternatives");
            this.radUseAlternatives.Name = "radUseAlternatives";
            this.radUseAlternatives.TabStop = true;
            this.radUseAlternatives.UseVisualStyleBackColor = true;
            this.radUseAlternatives.CheckedChanged += new System.EventHandler(this.radUseAlternatives_CheckedChanged);
            // 
            // radUseCustom
            // 
            resources.ApplyResources(this.radUseCustom, "radUseCustom");
            this.radUseCustom.Name = "radUseCustom";
            this.radUseCustom.TabStop = true;
            this.radUseCustom.UseVisualStyleBackColor = true;
            this.radUseCustom.CheckedChanged += new System.EventHandler(this.radUseCustom_CheckedChanged);
            // 
            // radUseNull
            // 
            resources.ApplyResources(this.radUseNull, "radUseNull");
            this.radUseNull.Name = "radUseNull";
            this.radUseNull.TabStop = true;
            this.radUseNull.UseVisualStyleBackColor = true;
            this.radUseNull.CheckedChanged += new System.EventHandler(this.radUseNull_CheckedChanged);
            // 
            // dtpCustom
            // 
            resources.ApplyResources(this.dtpCustom, "dtpCustom");
            this.dtpCustom.Name = "dtpCustom";
            this.dtpCustom.SelectedValue = new System.DateTime(2015, 9, 29, 16, 36, 33, 0);
            // 
            // frmPickDateTimeWithAlternatives
            // 
            this.AcceptButton = this.btnAccept;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.dtpCustom);
            this.Controls.Add(this.radUseNull);
            this.Controls.Add(this.radUseCustom);
            this.Controls.Add(this.radUseAlternatives);
            this.Controls.Add(this.dgvAlternatives);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPickDateTimeWithAlternatives";
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.frmPickDateTimeWithAlternatives_MouseClick);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlternatives)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView dgvAlternatives;
        private System.Windows.Forms.RadioButton radUseAlternatives;
        private System.Windows.Forms.RadioButton radUseCustom;
        private System.Windows.Forms.RadioButton radUseNull;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTimestamp;
        private ctxDateTime dtpCustom;
    }
}