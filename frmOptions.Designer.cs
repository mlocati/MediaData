namespace MLocati.MediaData
{
    partial class frmOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOptions));
            this.lblNormalizeVideo = new System.Windows.Forms.Label();
            this.cbxNormalizeVideo = new System.Windows.Forms.ComboBox();
            this.lblSetFiledateOnMetadata = new System.Windows.Forms.Label();
            this.cbxSetFiledateOnMetadata = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAccept = new System.Windows.Forms.Button();
            this.lblProcessingOutput = new System.Windows.Forms.Label();
            this.chkDeleteToTrash = new System.Windows.Forms.CheckBox();
            this.tcPages = new System.Windows.Forms.TabControl();
            this.tpGeneral = new System.Windows.Forms.TabPage();
            this.cbxLanguage = new System.Windows.Forms.ComboBox();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.tpProcessingOutput = new System.Windows.Forms.TabPage();
            this.ctxSPOOVideosTranscoding = new MLocati.MediaData.ctxShowProcessingOutputOptions();
            this.ctxSPOOImages = new MLocati.MediaData.ctxShowProcessingOutputOptions();
            this.ctxSPOOVideos = new MLocati.MediaData.ctxShowProcessingOutputOptions();
            this.tpMaps = new System.Windows.Forms.TabPage();
            this.lnkEnabledMapProviders_None = new System.Windows.Forms.LinkLabel();
            this.lnkEnabledMapProviders_Default = new System.Windows.Forms.LinkLabel();
            this.lnkEnabledMapProviders_All = new System.Windows.Forms.LinkLabel();
            this.clbEnabledMapProviders = new System.Windows.Forms.CheckedListBox();
            this.lblEnabledMapProviders = new System.Windows.Forms.Label();
            this.lnkGoogleAPI_Elevation_Get = new System.Windows.Forms.LinkLabel();
            this.lnkGoogleAPI_Elevation_Check = new System.Windows.Forms.LinkLabel();
            this.tbxGoogleAPI_Elevation = new System.Windows.Forms.TextBox();
            this.lblGoogleAPI_Elevation = new System.Windows.Forms.Label();
            this.lblMapsCacheSize = new System.Windows.Forms.Label();
            this.lnkMaps_ClearCache = new System.Windows.Forms.LinkLabel();
            this.chkMaps_Cache = new System.Windows.Forms.CheckBox();
            this.tcPages.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            this.tpProcessingOutput.SuspendLayout();
            this.tpMaps.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblNormalizeVideo
            // 
            resources.ApplyResources(this.lblNormalizeVideo, "lblNormalizeVideo");
            this.lblNormalizeVideo.Name = "lblNormalizeVideo";
            // 
            // cbxNormalizeVideo
            // 
            resources.ApplyResources(this.cbxNormalizeVideo, "cbxNormalizeVideo");
            this.cbxNormalizeVideo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxNormalizeVideo.FormattingEnabled = true;
            this.cbxNormalizeVideo.Name = "cbxNormalizeVideo";
            // 
            // lblSetFiledateOnMetadata
            // 
            resources.ApplyResources(this.lblSetFiledateOnMetadata, "lblSetFiledateOnMetadata");
            this.lblSetFiledateOnMetadata.Name = "lblSetFiledateOnMetadata";
            // 
            // cbxSetFiledateOnMetadata
            // 
            resources.ApplyResources(this.cbxSetFiledateOnMetadata, "cbxSetFiledateOnMetadata");
            this.cbxSetFiledateOnMetadata.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSetFiledateOnMetadata.FormattingEnabled = true;
            this.cbxSetFiledateOnMetadata.Name = "cbxSetFiledateOnMetadata";
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
            // lblProcessingOutput
            // 
            resources.ApplyResources(this.lblProcessingOutput, "lblProcessingOutput");
            this.lblProcessingOutput.Name = "lblProcessingOutput";
            // 
            // chkDeleteToTrash
            // 
            resources.ApplyResources(this.chkDeleteToTrash, "chkDeleteToTrash");
            this.chkDeleteToTrash.Name = "chkDeleteToTrash";
            this.chkDeleteToTrash.UseVisualStyleBackColor = true;
            // 
            // tcPages
            // 
            resources.ApplyResources(this.tcPages, "tcPages");
            this.tcPages.Controls.Add(this.tpGeneral);
            this.tcPages.Controls.Add(this.tpProcessingOutput);
            this.tcPages.Controls.Add(this.tpMaps);
            this.tcPages.Name = "tcPages";
            this.tcPages.SelectedIndex = 0;
            // 
            // tpGeneral
            // 
            resources.ApplyResources(this.tpGeneral, "tpGeneral");
            this.tpGeneral.Controls.Add(this.cbxLanguage);
            this.tpGeneral.Controls.Add(this.lblLanguage);
            this.tpGeneral.Controls.Add(this.lblNormalizeVideo);
            this.tpGeneral.Controls.Add(this.lblSetFiledateOnMetadata);
            this.tpGeneral.Controls.Add(this.chkDeleteToTrash);
            this.tpGeneral.Controls.Add(this.cbxNormalizeVideo);
            this.tpGeneral.Controls.Add(this.cbxSetFiledateOnMetadata);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.UseVisualStyleBackColor = true;
            // 
            // cbxLanguage
            // 
            resources.ApplyResources(this.cbxLanguage, "cbxLanguage");
            this.cbxLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxLanguage.FormattingEnabled = true;
            this.cbxLanguage.Name = "cbxLanguage";
            // 
            // lblLanguage
            // 
            resources.ApplyResources(this.lblLanguage, "lblLanguage");
            this.lblLanguage.Name = "lblLanguage";
            // 
            // tpProcessingOutput
            // 
            resources.ApplyResources(this.tpProcessingOutput, "tpProcessingOutput");
            this.tpProcessingOutput.Controls.Add(this.lblProcessingOutput);
            this.tpProcessingOutput.Controls.Add(this.ctxSPOOVideosTranscoding);
            this.tpProcessingOutput.Controls.Add(this.ctxSPOOImages);
            this.tpProcessingOutput.Controls.Add(this.ctxSPOOVideos);
            this.tpProcessingOutput.Name = "tpProcessingOutput";
            this.tpProcessingOutput.UseVisualStyleBackColor = true;
            // 
            // ctxSPOOVideosTranscoding
            // 
            resources.ApplyResources(this.ctxSPOOVideosTranscoding, "ctxSPOOVideosTranscoding");
            this.ctxSPOOVideosTranscoding.Name = "ctxSPOOVideosTranscoding";
            // 
            // ctxSPOOImages
            // 
            resources.ApplyResources(this.ctxSPOOImages, "ctxSPOOImages");
            this.ctxSPOOImages.Name = "ctxSPOOImages";
            // 
            // ctxSPOOVideos
            // 
            resources.ApplyResources(this.ctxSPOOVideos, "ctxSPOOVideos");
            this.ctxSPOOVideos.Name = "ctxSPOOVideos";
            // 
            // tpMaps
            // 
            resources.ApplyResources(this.tpMaps, "tpMaps");
            this.tpMaps.Controls.Add(this.lnkEnabledMapProviders_None);
            this.tpMaps.Controls.Add(this.lnkEnabledMapProviders_Default);
            this.tpMaps.Controls.Add(this.lnkEnabledMapProviders_All);
            this.tpMaps.Controls.Add(this.clbEnabledMapProviders);
            this.tpMaps.Controls.Add(this.lblEnabledMapProviders);
            this.tpMaps.Controls.Add(this.lnkGoogleAPI_Elevation_Get);
            this.tpMaps.Controls.Add(this.lnkGoogleAPI_Elevation_Check);
            this.tpMaps.Controls.Add(this.tbxGoogleAPI_Elevation);
            this.tpMaps.Controls.Add(this.lblGoogleAPI_Elevation);
            this.tpMaps.Controls.Add(this.lblMapsCacheSize);
            this.tpMaps.Controls.Add(this.lnkMaps_ClearCache);
            this.tpMaps.Controls.Add(this.chkMaps_Cache);
            this.tpMaps.Name = "tpMaps";
            this.tpMaps.UseVisualStyleBackColor = true;
            // 
            // lnkEnabledMapProviders_None
            // 
            resources.ApplyResources(this.lnkEnabledMapProviders_None, "lnkEnabledMapProviders_None");
            this.lnkEnabledMapProviders_None.Name = "lnkEnabledMapProviders_None";
            this.lnkEnabledMapProviders_None.TabStop = true;
            this.lnkEnabledMapProviders_None.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkEnabledMapProviders_None_LinkClicked);
            // 
            // lnkEnabledMapProviders_Default
            // 
            resources.ApplyResources(this.lnkEnabledMapProviders_Default, "lnkEnabledMapProviders_Default");
            this.lnkEnabledMapProviders_Default.Name = "lnkEnabledMapProviders_Default";
            this.lnkEnabledMapProviders_Default.TabStop = true;
            this.lnkEnabledMapProviders_Default.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkEnabledMapProviders_Default_LinkClicked);
            // 
            // lnkEnabledMapProviders_All
            // 
            resources.ApplyResources(this.lnkEnabledMapProviders_All, "lnkEnabledMapProviders_All");
            this.lnkEnabledMapProviders_All.Name = "lnkEnabledMapProviders_All";
            this.lnkEnabledMapProviders_All.TabStop = true;
            this.lnkEnabledMapProviders_All.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkEnabledMapProviders_All_LinkClicked);
            // 
            // clbEnabledMapProviders
            // 
            resources.ApplyResources(this.clbEnabledMapProviders, "clbEnabledMapProviders");
            this.clbEnabledMapProviders.CheckOnClick = true;
            this.clbEnabledMapProviders.Name = "clbEnabledMapProviders";
            // 
            // lblEnabledMapProviders
            // 
            resources.ApplyResources(this.lblEnabledMapProviders, "lblEnabledMapProviders");
            this.lblEnabledMapProviders.Name = "lblEnabledMapProviders";
            // 
            // lnkGoogleAPI_Elevation_Get
            // 
            resources.ApplyResources(this.lnkGoogleAPI_Elevation_Get, "lnkGoogleAPI_Elevation_Get");
            this.lnkGoogleAPI_Elevation_Get.Name = "lnkGoogleAPI_Elevation_Get";
            this.lnkGoogleAPI_Elevation_Get.TabStop = true;
            this.lnkGoogleAPI_Elevation_Get.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGoogleAPI_Elevation_Get_LinkClicked);
            // 
            // lnkGoogleAPI_Elevation_Check
            // 
            resources.ApplyResources(this.lnkGoogleAPI_Elevation_Check, "lnkGoogleAPI_Elevation_Check");
            this.lnkGoogleAPI_Elevation_Check.Name = "lnkGoogleAPI_Elevation_Check";
            this.lnkGoogleAPI_Elevation_Check.TabStop = true;
            this.lnkGoogleAPI_Elevation_Check.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGoogleAPI_Elevation_Check_LinkClicked);
            // 
            // tbxGoogleAPI_Elevation
            // 
            resources.ApplyResources(this.tbxGoogleAPI_Elevation, "tbxGoogleAPI_Elevation");
            this.tbxGoogleAPI_Elevation.Name = "tbxGoogleAPI_Elevation";
            this.tbxGoogleAPI_Elevation.TextChanged += new System.EventHandler(this.tbxGoogleAPI_Elevation_TextChanged);
            // 
            // lblGoogleAPI_Elevation
            // 
            resources.ApplyResources(this.lblGoogleAPI_Elevation, "lblGoogleAPI_Elevation");
            this.lblGoogleAPI_Elevation.Name = "lblGoogleAPI_Elevation";
            // 
            // lblMapsCacheSize
            // 
            resources.ApplyResources(this.lblMapsCacheSize, "lblMapsCacheSize");
            this.lblMapsCacheSize.Name = "lblMapsCacheSize";
            // 
            // lnkMaps_ClearCache
            // 
            resources.ApplyResources(this.lnkMaps_ClearCache, "lnkMaps_ClearCache");
            this.lnkMaps_ClearCache.Name = "lnkMaps_ClearCache";
            this.lnkMaps_ClearCache.TabStop = true;
            this.lnkMaps_ClearCache.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkMaps_ClearCache_LinkClicked);
            // 
            // chkMaps_Cache
            // 
            resources.ApplyResources(this.chkMaps_Cache, "chkMaps_Cache");
            this.chkMaps_Cache.Name = "chkMaps_Cache";
            this.chkMaps_Cache.UseVisualStyleBackColor = true;
            // 
            // frmOptions
            // 
            this.AcceptButton = this.btnAccept;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.tcPages);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOptions";
            this.tcPages.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            this.tpGeneral.PerformLayout();
            this.tpProcessingOutput.ResumeLayout(false);
            this.tpProcessingOutput.PerformLayout();
            this.tpMaps.ResumeLayout(false);
            this.tpMaps.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblNormalizeVideo;
        private System.Windows.Forms.ComboBox cbxNormalizeVideo;
        private System.Windows.Forms.Label lblSetFiledateOnMetadata;
        private System.Windows.Forms.ComboBox cbxSetFiledateOnMetadata;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Label lblProcessingOutput;
        private System.Windows.Forms.CheckBox chkDeleteToTrash;
        private ctxShowProcessingOutputOptions ctxSPOOImages;
        private ctxShowProcessingOutputOptions ctxSPOOVideos;
        private ctxShowProcessingOutputOptions ctxSPOOVideosTranscoding;
        private System.Windows.Forms.TabControl tcPages;
        private System.Windows.Forms.TabPage tpGeneral;
        private System.Windows.Forms.TabPage tpProcessingOutput;
        private System.Windows.Forms.TabPage tpMaps;
        private System.Windows.Forms.Label lblMapsCacheSize;
        private System.Windows.Forms.LinkLabel lnkMaps_ClearCache;
        private System.Windows.Forms.CheckBox chkMaps_Cache;
        private System.Windows.Forms.LinkLabel lnkGoogleAPI_Elevation_Get;
        private System.Windows.Forms.LinkLabel lnkGoogleAPI_Elevation_Check;
        private System.Windows.Forms.TextBox tbxGoogleAPI_Elevation;
        private System.Windows.Forms.Label lblGoogleAPI_Elevation;
        private System.Windows.Forms.Label lblEnabledMapProviders;
        private System.Windows.Forms.CheckedListBox clbEnabledMapProviders;
        private System.Windows.Forms.LinkLabel lnkEnabledMapProviders_None;
        private System.Windows.Forms.LinkLabel lnkEnabledMapProviders_All;
        private System.Windows.Forms.LinkLabel lnkEnabledMapProviders_Default;
        private System.Windows.Forms.ComboBox cbxLanguage;
        private System.Windows.Forms.Label lblLanguage;
    }
}