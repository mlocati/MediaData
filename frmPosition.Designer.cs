namespace MLocati.MediaData
{
    partial class frmPosition
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPosition));
            this.gmcMap = new GMap.NET.WindowsForms.GMapControl();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnEmpty = new System.Windows.Forms.Button();
            this.btnAccept = new System.Windows.Forms.Button();
            this.ssStatus = new System.Windows.Forms.StatusStrip();
            this.slHelp = new System.Windows.Forms.ToolStripStatusLabel();
            this.slCurrentPositionLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.slCurrentPosition = new System.Windows.Forms.ToolStripStatusLabel();
            this.vsbZoom = new System.Windows.Forms.VScrollBar();
            this.btnClipboardCopy = new System.Windows.Forms.Button();
            this.btnClipboardPaste = new System.Windows.Forms.Button();
            this.gbxLatLon = new System.Windows.Forms.GroupBox();
            this.cbxLatLonProvider = new System.Windows.Forms.ComboBox();
            this.lblLatLonProvider = new System.Windows.Forms.Label();
            this.tbxLatLonSearch = new System.Windows.Forms.TextBox();
            this.lblLatLonSearch = new System.Windows.Forms.Label();
            this.gbxAltitude = new System.Windows.Forms.GroupBox();
            this.lblAltAutoPrecision = new System.Windows.Forms.Label();
            this.lnkAltAuto = new System.Windows.Forms.LinkLabel();
            this.lblAlt = new System.Windows.Forms.Label();
            this.nudAlt = new System.Windows.Forms.NumericUpDown();
            this.chkAltSet = new System.Windows.Forms.CheckBox();
            this.ssStatus.SuspendLayout();
            this.gbxLatLon.SuspendLayout();
            this.gbxAltitude.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAlt)).BeginInit();
            this.SuspendLayout();
            // 
            // gmcMap
            // 
            resources.ApplyResources(this.gmcMap, "gmcMap");
            this.gmcMap.Bearing = 0F;
            this.gmcMap.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gmcMap.CanDragMap = true;
            this.gmcMap.EmptyTileColor = System.Drawing.Color.Navy;
            this.gmcMap.GrayScaleMode = false;
            this.gmcMap.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gmcMap.LevelsKeepInMemmory = 5;
            this.gmcMap.MarkersEnabled = true;
            this.gmcMap.MaxZoom = 28;
            this.gmcMap.MinZoom = 1;
            this.gmcMap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gmcMap.Name = "gmcMap";
            this.gmcMap.NegativeMode = false;
            this.gmcMap.PolygonsEnabled = true;
            this.gmcMap.RetryLoadTile = 0;
            this.gmcMap.RoutesEnabled = true;
            this.gmcMap.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gmcMap.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gmcMap.ShowTileGridLines = false;
            this.gmcMap.Zoom = 9D;
            this.gmcMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gmcMap_MouseDown);
            this.gmcMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gmcMap_MouseMove);
            this.gmcMap.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gmcMap_MouseUp);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnEmpty
            // 
            resources.ApplyResources(this.btnEmpty, "btnEmpty");
            this.btnEmpty.Name = "btnEmpty";
            this.btnEmpty.UseVisualStyleBackColor = true;
            this.btnEmpty.Click += new System.EventHandler(this.btnEmpty_Click);
            // 
            // btnAccept
            // 
            resources.ApplyResources(this.btnAccept, "btnAccept");
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // ssStatus
            // 
            this.ssStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slHelp,
            this.slCurrentPositionLabel,
            this.slCurrentPosition});
            resources.ApplyResources(this.ssStatus, "ssStatus");
            this.ssStatus.Name = "ssStatus";
            // 
            // slHelp
            // 
            this.slHelp.Name = "slHelp";
            resources.ApplyResources(this.slHelp, "slHelp");
            this.slHelp.Spring = true;
            // 
            // slCurrentPositionLabel
            // 
            this.slCurrentPositionLabel.Name = "slCurrentPositionLabel";
            resources.ApplyResources(this.slCurrentPositionLabel, "slCurrentPositionLabel");
            // 
            // slCurrentPosition
            // 
            resources.ApplyResources(this.slCurrentPosition, "slCurrentPosition");
            this.slCurrentPosition.Name = "slCurrentPosition";
            // 
            // vsbZoom
            // 
            resources.ApplyResources(this.vsbZoom, "vsbZoom");
            this.vsbZoom.Name = "vsbZoom";
            this.vsbZoom.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vsbZoom_Scroll);
            // 
            // btnClipboardCopy
            // 
            resources.ApplyResources(this.btnClipboardCopy, "btnClipboardCopy");
            this.btnClipboardCopy.Name = "btnClipboardCopy";
            this.btnClipboardCopy.UseVisualStyleBackColor = true;
            this.btnClipboardCopy.Click += new System.EventHandler(this.btnClipboardCopy_Click);
            // 
            // btnClipboardPaste
            // 
            resources.ApplyResources(this.btnClipboardPaste, "btnClipboardPaste");
            this.btnClipboardPaste.Name = "btnClipboardPaste";
            this.btnClipboardPaste.UseVisualStyleBackColor = true;
            this.btnClipboardPaste.Click += new System.EventHandler(this.btnClipboardPaste_Click);
            // 
            // gbxLatLon
            // 
            resources.ApplyResources(this.gbxLatLon, "gbxLatLon");
            this.gbxLatLon.Controls.Add(this.cbxLatLonProvider);
            this.gbxLatLon.Controls.Add(this.lblLatLonProvider);
            this.gbxLatLon.Controls.Add(this.tbxLatLonSearch);
            this.gbxLatLon.Controls.Add(this.lblLatLonSearch);
            this.gbxLatLon.Controls.Add(this.vsbZoom);
            this.gbxLatLon.Controls.Add(this.gmcMap);
            this.gbxLatLon.Name = "gbxLatLon";
            this.gbxLatLon.TabStop = false;
            // 
            // cbxLatLonProvider
            // 
            resources.ApplyResources(this.cbxLatLonProvider, "cbxLatLonProvider");
            this.cbxLatLonProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxLatLonProvider.FormattingEnabled = true;
            this.cbxLatLonProvider.Name = "cbxLatLonProvider";
            this.cbxLatLonProvider.SelectedIndexChanged += new System.EventHandler(this.cbxLatLonProvider_SelectedIndexChanged);
            // 
            // lblLatLonProvider
            // 
            resources.ApplyResources(this.lblLatLonProvider, "lblLatLonProvider");
            this.lblLatLonProvider.Name = "lblLatLonProvider";
            // 
            // tbxLatLonSearch
            // 
            resources.ApplyResources(this.tbxLatLonSearch, "tbxLatLonSearch");
            this.tbxLatLonSearch.Name = "tbxLatLonSearch";
            this.tbxLatLonSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbxLatLonSearch_KeyUp);
            // 
            // lblLatLonSearch
            // 
            resources.ApplyResources(this.lblLatLonSearch, "lblLatLonSearch");
            this.lblLatLonSearch.Name = "lblLatLonSearch";
            // 
            // gbxAltitude
            // 
            resources.ApplyResources(this.gbxAltitude, "gbxAltitude");
            this.gbxAltitude.Controls.Add(this.lblAltAutoPrecision);
            this.gbxAltitude.Controls.Add(this.lnkAltAuto);
            this.gbxAltitude.Controls.Add(this.lblAlt);
            this.gbxAltitude.Controls.Add(this.nudAlt);
            this.gbxAltitude.Controls.Add(this.chkAltSet);
            this.gbxAltitude.Name = "gbxAltitude";
            this.gbxAltitude.TabStop = false;
            // 
            // lblAltAutoPrecision
            // 
            resources.ApplyResources(this.lblAltAutoPrecision, "lblAltAutoPrecision");
            this.lblAltAutoPrecision.Name = "lblAltAutoPrecision";
            // 
            // lnkAltAuto
            // 
            resources.ApplyResources(this.lnkAltAuto, "lnkAltAuto");
            this.lnkAltAuto.Name = "lnkAltAuto";
            this.lnkAltAuto.TabStop = true;
            this.lnkAltAuto.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAltAuto_LinkClicked);
            // 
            // lblAlt
            // 
            resources.ApplyResources(this.lblAlt, "lblAlt");
            this.lblAlt.Name = "lblAlt";
            // 
            // nudAlt
            // 
            this.nudAlt.DecimalPlaces = 1;
            resources.ApplyResources(this.nudAlt, "nudAlt");
            this.nudAlt.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.nudAlt.Minimum = new decimal(new int[] {
            20000,
            0,
            0,
            -2147483648});
            this.nudAlt.Name = "nudAlt";
            this.nudAlt.ValueChanged += new System.EventHandler(this.nudAlt_ValueChanged);
            this.nudAlt.Leave += new System.EventHandler(this.nudAlt_Leave);
            // 
            // chkAltSet
            // 
            resources.ApplyResources(this.chkAltSet, "chkAltSet");
            this.chkAltSet.Name = "chkAltSet";
            this.chkAltSet.UseVisualStyleBackColor = true;
            this.chkAltSet.CheckedChanged += new System.EventHandler(this.chkAltSet_CheckedChanged);
            // 
            // frmPosition
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.gbxAltitude);
            this.Controls.Add(this.gbxLatLon);
            this.Controls.Add(this.btnClipboardPaste);
            this.Controls.Add(this.btnClipboardCopy);
            this.Controls.Add(this.ssStatus);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.btnEmpty);
            this.Controls.Add(this.btnCancel);
            this.KeyPreview = true;
            this.Name = "frmPosition";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPosition_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPosition_KeyDown);
            this.ssStatus.ResumeLayout(false);
            this.ssStatus.PerformLayout();
            this.gbxLatLon.ResumeLayout(false);
            this.gbxLatLon.PerformLayout();
            this.gbxAltitude.ResumeLayout(false);
            this.gbxAltitude.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAlt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GMap.NET.WindowsForms.GMapControl gmcMap;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnEmpty;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.StatusStrip ssStatus;
        private System.Windows.Forms.ToolStripStatusLabel slHelp;
        private System.Windows.Forms.ToolStripStatusLabel slCurrentPositionLabel;
        private System.Windows.Forms.ToolStripStatusLabel slCurrentPosition;
        private System.Windows.Forms.VScrollBar vsbZoom;
        private System.Windows.Forms.Button btnClipboardCopy;
        private System.Windows.Forms.Button btnClipboardPaste;
        private System.Windows.Forms.GroupBox gbxLatLon;
        private System.Windows.Forms.TextBox tbxLatLonSearch;
        private System.Windows.Forms.Label lblLatLonSearch;
        private System.Windows.Forms.ComboBox cbxLatLonProvider;
        private System.Windows.Forms.Label lblLatLonProvider;
        private System.Windows.Forms.GroupBox gbxAltitude;
        private System.Windows.Forms.CheckBox chkAltSet;
        private System.Windows.Forms.Label lblAlt;
        private System.Windows.Forms.NumericUpDown nudAlt;
        private System.Windows.Forms.LinkLabel lnkAltAuto;
        private System.Windows.Forms.Label lblAltAutoPrecision;
    }
}