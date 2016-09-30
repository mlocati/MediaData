namespace MLocati.MediaData
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ssStatus = new System.Windows.Forms.StatusStrip();
            this.ssStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ssStatusProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.tsTools = new System.Windows.Forms.ToolStrip();
            this.tslSrcDir = new System.Windows.Forms.ToolStripLabel();
            this.tstSrcDir = new System.Windows.Forms.ToolStripTextBox();
            this.tsbSrcDir = new System.Windows.Forms.ToolStripButton();
            this.tsSep0 = new System.Windows.Forms.ToolStripSeparator();
            this.tslTimeZone = new System.Windows.Forms.ToolStripLabel();
            this.tscbxTimeZone = new System.Windows.Forms.ToolStripComboBox();
            this.tsSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbOptions = new System.Windows.Forms.ToolStripButton();
            this.tsbAbout = new System.Windows.Forms.ToolStripButton();
            this.dgvFiles = new System.Windows.Forms.DataGridView();
            this.colBrowseTo = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colFilename = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colFilenameDatetime = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colMetadataDatetime = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colMetadataPosition = new System.Windows.Forms.DataGridViewLinkColumn();
            this.gbxSelection = new System.Windows.Forms.GroupBox();
            this.flpSelection = new System.Windows.Forms.FlowLayoutPanel();
            this.lnkSelectAll = new System.Windows.Forms.LinkLabel();
            this.lnkSelectNone = new System.Windows.Forms.LinkLabel();
            this.cbxSelection = new System.Windows.Forms.ComboBox();
            this.lblSelectionDeltaTime = new System.Windows.Forms.Label();
            this.nudSelectionDeltaTimeValue = new System.Windows.Forms.NumericUpDown();
            this.cbxSelectionDeltaTimeUnit = new System.Windows.Forms.ComboBox();
            this.nudSelectionSetTimestampDate = new System.Windows.Forms.DateTimePicker();
            this.nudSelectionSetTimestampTime = new System.Windows.Forms.DateTimePicker();
            this.tbxBatchSetPosition = new System.Windows.Forms.TextBox();
            this.btnBatchSetPosition = new System.Windows.Forms.Button();
            this.btnSelectionApply = new System.Windows.Forms.Button();
            this.chkSelection = new System.Windows.Forms.CheckBox();
            this.ctxProcessor = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxProcessorCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxProcessorPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.ssStatus.SuspendLayout();
            this.tsTools.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).BeginInit();
            this.gbxSelection.SuspendLayout();
            this.flpSelection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSelectionDeltaTimeValue)).BeginInit();
            this.ctxProcessor.SuspendLayout();
            this.SuspendLayout();
            // 
            // ssStatus
            // 
            this.ssStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ssStatusLabel,
            this.ssStatusProgress});
            resources.ApplyResources(this.ssStatus, "ssStatus");
            this.ssStatus.Name = "ssStatus";
            // 
            // ssStatusLabel
            // 
            this.ssStatusLabel.Name = "ssStatusLabel";
            resources.ApplyResources(this.ssStatusLabel, "ssStatusLabel");
            this.ssStatusLabel.Spring = true;
            // 
            // ssStatusProgress
            // 
            this.ssStatusProgress.Name = "ssStatusProgress";
            resources.ApplyResources(this.ssStatusProgress, "ssStatusProgress");
            // 
            // tsTools
            // 
            this.tsTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslSrcDir,
            this.tstSrcDir,
            this.tsbSrcDir,
            this.tsSep0,
            this.tslTimeZone,
            this.tscbxTimeZone,
            this.tsSep1,
            this.tsbOptions,
            this.tsbAbout});
            resources.ApplyResources(this.tsTools, "tsTools");
            this.tsTools.Name = "tsTools";
            // 
            // tslSrcDir
            // 
            this.tslSrcDir.Name = "tslSrcDir";
            resources.ApplyResources(this.tslSrcDir, "tslSrcDir");
            // 
            // tstSrcDir
            // 
            this.tstSrcDir.Name = "tstSrcDir";
            resources.ApplyResources(this.tstSrcDir, "tstSrcDir");
            this.tstSrcDir.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tstSrcDir_KeyPress);
            // 
            // tsbSrcDir
            // 
            this.tsbSrcDir.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.tsbSrcDir, "tsbSrcDir");
            this.tsbSrcDir.Name = "tsbSrcDir";
            this.tsbSrcDir.Click += new System.EventHandler(this.tsbSrcDir_Click);
            // 
            // tsSep0
            // 
            this.tsSep0.Name = "tsSep0";
            resources.ApplyResources(this.tsSep0, "tsSep0");
            // 
            // tslTimeZone
            // 
            this.tslTimeZone.Name = "tslTimeZone";
            resources.ApplyResources(this.tslTimeZone, "tslTimeZone");
            // 
            // tscbxTimeZone
            // 
            this.tscbxTimeZone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tscbxTimeZone.DropDownWidth = 400;
            this.tscbxTimeZone.Name = "tscbxTimeZone";
            resources.ApplyResources(this.tscbxTimeZone, "tscbxTimeZone");
            this.tscbxTimeZone.SelectedIndexChanged += new System.EventHandler(this.tscbxTimeZone_SelectedIndexChanged);
            // 
            // tsSep1
            // 
            this.tsSep1.Name = "tsSep1";
            resources.ApplyResources(this.tsSep1, "tsSep1");
            // 
            // tsbOptions
            // 
            this.tsbOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.tsbOptions, "tsbOptions");
            this.tsbOptions.Name = "tsbOptions";
            this.tsbOptions.Click += new System.EventHandler(this.tsbOptions_Click);
            // 
            // tsbAbout
            // 
            this.tsbAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.tsbAbout, "tsbAbout");
            this.tsbAbout.Name = "tsbAbout";
            this.tsbAbout.Click += new System.EventHandler(this.tsbAbout_Click);
            // 
            // dgvFiles
            // 
            this.dgvFiles.AllowUserToAddRows = false;
            this.dgvFiles.AllowUserToDeleteRows = false;
            this.dgvFiles.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgvFiles, "dgvFiles");
            this.dgvFiles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFiles.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBrowseTo,
            this.colFilename,
            this.colFilenameDatetime,
            this.colMetadataDatetime,
            this.colMetadataPosition});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFiles.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvFiles.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvFiles.Name = "dgvFiles";
            this.dgvFiles.ReadOnly = true;
            this.dgvFiles.RowHeadersVisible = false;
            this.dgvFiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFiles.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFiles_CellClick);
            this.dgvFiles.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFiles_CellContentClick);
            this.dgvFiles.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvFiles_MouseDown);
            // 
            // colBrowseTo
            // 
            this.colBrowseTo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.colBrowseTo, "colBrowseTo");
            this.colBrowseTo.Name = "colBrowseTo";
            this.colBrowseTo.ReadOnly = true;
            this.colBrowseTo.Text = "...";
            this.colBrowseTo.UseColumnTextForButtonValue = true;
            // 
            // colFilename
            // 
            this.colFilename.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colFilename.DataPropertyName = "Filename";
            this.colFilename.FillWeight = 17.54385F;
            resources.ApplyResources(this.colFilename, "colFilename");
            this.colFilename.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.colFilename.LinkColor = System.Drawing.Color.Blue;
            this.colFilename.Name = "colFilename";
            this.colFilename.ReadOnly = true;
            this.colFilename.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colFilename.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colFilename.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // colFilenameDatetime
            // 
            this.colFilenameDatetime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colFilenameDatetime.DataPropertyName = "FilenameTimestampStr";
            resources.ApplyResources(this.colFilenameDatetime, "colFilenameDatetime");
            this.colFilenameDatetime.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.colFilenameDatetime.LinkColor = System.Drawing.Color.Blue;
            this.colFilenameDatetime.Name = "colFilenameDatetime";
            this.colFilenameDatetime.ReadOnly = true;
            this.colFilenameDatetime.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colFilenameDatetime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colFilenameDatetime.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // colMetadataDatetime
            // 
            this.colMetadataDatetime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colMetadataDatetime.DataPropertyName = "MetadataTimestampStr";
            resources.ApplyResources(this.colMetadataDatetime, "colMetadataDatetime");
            this.colMetadataDatetime.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.colMetadataDatetime.LinkColor = System.Drawing.Color.Blue;
            this.colMetadataDatetime.Name = "colMetadataDatetime";
            this.colMetadataDatetime.ReadOnly = true;
            this.colMetadataDatetime.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colMetadataDatetime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colMetadataDatetime.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // colMetadataPosition
            // 
            this.colMetadataPosition.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colMetadataPosition.DataPropertyName = "MetadataPositionStr";
            resources.ApplyResources(this.colMetadataPosition, "colMetadataPosition");
            this.colMetadataPosition.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.colMetadataPosition.LinkColor = System.Drawing.Color.Blue;
            this.colMetadataPosition.Name = "colMetadataPosition";
            this.colMetadataPosition.ReadOnly = true;
            this.colMetadataPosition.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colMetadataPosition.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // gbxSelection
            // 
            resources.ApplyResources(this.gbxSelection, "gbxSelection");
            this.gbxSelection.Controls.Add(this.flpSelection);
            this.gbxSelection.Name = "gbxSelection";
            this.gbxSelection.TabStop = false;
            // 
            // flpSelection
            // 
            this.flpSelection.Controls.Add(this.lnkSelectAll);
            this.flpSelection.Controls.Add(this.lnkSelectNone);
            this.flpSelection.Controls.Add(this.cbxSelection);
            this.flpSelection.Controls.Add(this.lblSelectionDeltaTime);
            this.flpSelection.Controls.Add(this.nudSelectionDeltaTimeValue);
            this.flpSelection.Controls.Add(this.cbxSelectionDeltaTimeUnit);
            this.flpSelection.Controls.Add(this.nudSelectionSetTimestampDate);
            this.flpSelection.Controls.Add(this.nudSelectionSetTimestampTime);
            this.flpSelection.Controls.Add(this.tbxBatchSetPosition);
            this.flpSelection.Controls.Add(this.btnBatchSetPosition);
            this.flpSelection.Controls.Add(this.btnSelectionApply);
            resources.ApplyResources(this.flpSelection, "flpSelection");
            this.flpSelection.Name = "flpSelection";
            // 
            // lnkSelectAll
            // 
            resources.ApplyResources(this.lnkSelectAll, "lnkSelectAll");
            this.lnkSelectAll.Name = "lnkSelectAll";
            this.lnkSelectAll.TabStop = true;
            this.lnkSelectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSelectAll_LinkClicked);
            // 
            // lnkSelectNone
            // 
            resources.ApplyResources(this.lnkSelectNone, "lnkSelectNone");
            this.lnkSelectNone.Name = "lnkSelectNone";
            this.lnkSelectNone.TabStop = true;
            this.lnkSelectNone.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSelectNone_LinkClicked);
            // 
            // cbxSelection
            // 
            this.cbxSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSelection.FormattingEnabled = true;
            resources.ApplyResources(this.cbxSelection, "cbxSelection");
            this.cbxSelection.Name = "cbxSelection";
            this.cbxSelection.SelectedIndexChanged += new System.EventHandler(this.cbxSelection_SelectedIndexChanged);
            // 
            // lblSelectionDeltaTime
            // 
            resources.ApplyResources(this.lblSelectionDeltaTime, "lblSelectionDeltaTime");
            this.lblSelectionDeltaTime.Name = "lblSelectionDeltaTime";
            // 
            // nudSelectionDeltaTimeValue
            // 
            resources.ApplyResources(this.nudSelectionDeltaTimeValue, "nudSelectionDeltaTimeValue");
            this.nudSelectionDeltaTimeValue.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudSelectionDeltaTimeValue.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.nudSelectionDeltaTimeValue.Name = "nudSelectionDeltaTimeValue";
            // 
            // cbxSelectionDeltaTimeUnit
            // 
            this.cbxSelectionDeltaTimeUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSelectionDeltaTimeUnit.FormattingEnabled = true;
            resources.ApplyResources(this.cbxSelectionDeltaTimeUnit, "cbxSelectionDeltaTimeUnit");
            this.cbxSelectionDeltaTimeUnit.Name = "cbxSelectionDeltaTimeUnit";
            // 
            // nudSelectionSetTimestampDate
            // 
            this.nudSelectionSetTimestampDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.nudSelectionSetTimestampDate, "nudSelectionSetTimestampDate");
            this.nudSelectionSetTimestampDate.Name = "nudSelectionSetTimestampDate";
            this.nudSelectionSetTimestampDate.ShowCheckBox = true;
            this.nudSelectionSetTimestampDate.ValueChanged += new System.EventHandler(this.nudSelectionSetTimestampDate_ValueChanged);
            // 
            // nudSelectionSetTimestampTime
            // 
            this.nudSelectionSetTimestampTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            resources.ApplyResources(this.nudSelectionSetTimestampTime, "nudSelectionSetTimestampTime");
            this.nudSelectionSetTimestampTime.Name = "nudSelectionSetTimestampTime";
            this.nudSelectionSetTimestampTime.ShowUpDown = true;
            // 
            // tbxBatchSetPosition
            // 
            resources.ApplyResources(this.tbxBatchSetPosition, "tbxBatchSetPosition");
            this.tbxBatchSetPosition.Name = "tbxBatchSetPosition";
            // 
            // btnBatchSetPosition
            // 
            resources.ApplyResources(this.btnBatchSetPosition, "btnBatchSetPosition");
            this.btnBatchSetPosition.Name = "btnBatchSetPosition";
            this.btnBatchSetPosition.UseVisualStyleBackColor = true;
            this.btnBatchSetPosition.Click += new System.EventHandler(this.btnBatchSetPosition_Click);
            // 
            // btnSelectionApply
            // 
            resources.ApplyResources(this.btnSelectionApply, "btnSelectionApply");
            this.btnSelectionApply.Name = "btnSelectionApply";
            this.btnSelectionApply.UseVisualStyleBackColor = true;
            this.btnSelectionApply.Click += new System.EventHandler(this.btnSelectionApply_Click);
            // 
            // chkSelection
            // 
            resources.ApplyResources(this.chkSelection, "chkSelection");
            this.chkSelection.Name = "chkSelection";
            this.chkSelection.UseVisualStyleBackColor = true;
            this.chkSelection.CheckedChanged += new System.EventHandler(this.chkSelection_CheckedChanged);
            // 
            // ctxProcessor
            // 
            this.ctxProcessor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxProcessorCopy,
            this.ctxProcessorPaste});
            this.ctxProcessor.Name = "ctxProcessor";
            this.ctxProcessor.ShowImageMargin = false;
            resources.ApplyResources(this.ctxProcessor, "ctxProcessor");
            // 
            // ctxProcessorCopy
            // 
            this.ctxProcessorCopy.Name = "ctxProcessorCopy";
            resources.ApplyResources(this.ctxProcessorCopy, "ctxProcessorCopy");
            this.ctxProcessorCopy.Click += new System.EventHandler(this.ctxProcessorCopy_Click);
            // 
            // ctxProcessorPaste
            // 
            this.ctxProcessorPaste.Name = "ctxProcessorPaste";
            resources.ApplyResources(this.ctxProcessorPaste, "ctxProcessorPaste");
            this.ctxProcessorPaste.Click += new System.EventHandler(this.ctxProcessorPaste_Click);
            // 
            // frmMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkSelection);
            this.Controls.Add(this.gbxSelection);
            this.Controls.Add(this.dgvFiles);
            this.Controls.Add(this.tsTools);
            this.Controls.Add(this.ssStatus);
            this.KeyPreview = true;
            this.Name = "frmMain";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            this.ssStatus.ResumeLayout(false);
            this.ssStatus.PerformLayout();
            this.tsTools.ResumeLayout(false);
            this.tsTools.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).EndInit();
            this.gbxSelection.ResumeLayout(false);
            this.flpSelection.ResumeLayout(false);
            this.flpSelection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSelectionDeltaTimeValue)).EndInit();
            this.ctxProcessor.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip ssStatus;
        private System.Windows.Forms.ToolStripStatusLabel ssStatusLabel;
        private System.Windows.Forms.ToolStripProgressBar ssStatusProgress;
        private System.Windows.Forms.ToolStrip tsTools;
        private System.Windows.Forms.ToolStripLabel tslSrcDir;
        private System.Windows.Forms.ToolStripTextBox tstSrcDir;
        private System.Windows.Forms.ToolStripButton tsbSrcDir;
        private System.Windows.Forms.DataGridView dgvFiles;
        private System.Windows.Forms.ToolStripSeparator tsSep0;
        private System.Windows.Forms.ToolStripButton tsbOptions;
        private System.Windows.Forms.ToolStripLabel tslTimeZone;
        private System.Windows.Forms.ToolStripComboBox tscbxTimeZone;
        private System.Windows.Forms.ToolStripSeparator tsSep1;
        private System.Windows.Forms.DataGridViewButtonColumn colBrowseTo;
        private System.Windows.Forms.DataGridViewLinkColumn colFilename;
        private System.Windows.Forms.DataGridViewLinkColumn colFilenameDatetime;
        private System.Windows.Forms.DataGridViewLinkColumn colMetadataDatetime;
        private System.Windows.Forms.DataGridViewLinkColumn colMetadataPosition;
        private System.Windows.Forms.GroupBox gbxSelection;
        private System.Windows.Forms.CheckBox chkSelection;
        private System.Windows.Forms.FlowLayoutPanel flpSelection;
        private System.Windows.Forms.LinkLabel lnkSelectAll;
        private System.Windows.Forms.LinkLabel lnkSelectNone;
        private System.Windows.Forms.ComboBox cbxSelection;
        private System.Windows.Forms.Label lblSelectionDeltaTime;
        private System.Windows.Forms.NumericUpDown nudSelectionDeltaTimeValue;
        private System.Windows.Forms.ComboBox cbxSelectionDeltaTimeUnit;
        private System.Windows.Forms.Button btnSelectionApply;
        private System.Windows.Forms.ToolStripButton tsbAbout;
        private System.Windows.Forms.ContextMenuStrip ctxProcessor;
        private System.Windows.Forms.ToolStripMenuItem ctxProcessorCopy;
        private System.Windows.Forms.ToolStripMenuItem ctxProcessorPaste;
        private System.Windows.Forms.DateTimePicker nudSelectionSetTimestampDate;
        private System.Windows.Forms.DateTimePicker nudSelectionSetTimestampTime;
        private System.Windows.Forms.TextBox tbxBatchSetPosition;
        private System.Windows.Forms.Button btnBatchSetPosition;
    }
}