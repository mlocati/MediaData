using System;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MLocati.MediaData
{
    public partial class frmOptions : Form
    {

        #region Types

        private class Language
        {
            public readonly string Code;
            public readonly string DisplayName;
            public readonly CultureInfo CultureInfo;
            public Language()
            {
                this.CultureInfo = null;
                this.Code = "";
                this.DisplayName = string.Format(i18n.AutomaticallyChooseLanguage_X, string.Format("{0} [{1}]", Localizer.DefaultCulture.DisplayName, Localizer.DefaultCulture.EnglishName));
            }
            public Language(CultureInfo ci)
            {
                this.CultureInfo = ci;
                this.DisplayName = string.Format("{0} [{1}]", ci.DisplayName, ci.EnglishName);
                this.Code = ci.Name;
            }
            public override string ToString()
            {
                return this.DisplayName;
            }
        }

        #endregion


        #region Constructors

        public frmOptions()
        {
            InitializeComponent();
            this.Icon = Program.Icon;

            List<Language> languages = new List<Language>();
            Language language;
            languages.Add(language = new Language());
            Language selectedLanguage = language;
            foreach (CultureInfo ci in Localizer.AvailableCultures)
            {
                languages.Add(language = new Language(ci));
                if (!string.IsNullOrEmpty(MediaData.Properties.Settings.Default.AppCulture) && ci.Name == MediaData.Properties.Settings.Default.AppCulture)
                {
                    selectedLanguage = language;
                }
            }
            this.cbxLanguage.DataSource = languages;
            this.cbxLanguage.SelectedItem = selectedLanguage;

            Type descriptionAttributeType = typeof(DescriptionAttribute);

            UI.DescribedEnumToCombobox(typeof(VideoProcessor.NormalizeCase), this.cbxNormalizeVideo, MediaData.Properties.Settings.Default.VideoNormalization);

            UI.DescribedEnumToCombobox(typeof(Processor.SetFileDates), this.cbxSetFiledateOnMetadata, MediaData.Properties.Settings.Default.SetFileDates);

            double maxErr;

            maxErr = MediaData.Properties.Settings.Default.MaxAllowedDistanceError_Horizontal;
            if (maxErr < 0D)
            {
                maxErr = 0D;
            }
            if (this.nudMaxErrHorizontal.Maximum < Convert.ToDecimal(maxErr))
            {
                this.nudMaxErrHorizontal.Maximum = Convert.ToDecimal(maxErr);
            }
            this.nudMaxErrHorizontal.Value = Convert.ToDecimal(maxErr);
            maxErr = -1D;
            try
            {
                SettingsProperty prop = MediaData.Properties.Settings.Default.Properties["MaxAllowedDistanceError_Horizontal"];
                if (prop != null)
                {
                    maxErr = Convert.ToDouble(prop.DefaultValue);
                }
            }
            catch
            { }
            this.lblMaxErrDefHorizontal.Text = (maxErr < 0D) ? "?" : maxErr.ToString();
            
            maxErr = MediaData.Properties.Settings.Default.MaxAllowedDistanceError_Vertical;
            if (maxErr < 0D)
            {
                maxErr = 0D;
            }
            if (this.nudMaxErrVertical.Maximum < Convert.ToDecimal(maxErr))
            {
                this.nudMaxErrVertical.Maximum = Convert.ToDecimal(maxErr);
            }
            this.nudMaxErrVertical.Value = Convert.ToDecimal(maxErr);
            maxErr = -1D;
            try
            {
                SettingsProperty prop = MediaData.Properties.Settings.Default.Properties["MaxAllowedDistanceError_Vertical"];
                if (prop != null)
                {
                    maxErr = Convert.ToDouble(prop.DefaultValue);
                }
            }
            catch
            { }
            this.lblMaxErrDefVertical.Text = (maxErr < 0D) ? "?" : maxErr.ToString();

            this.chkDeleteToTrash.Checked = MediaData.Properties.Settings.Default.DeleteToTrash;
            this.chkMaps_Cache.Checked = MediaData.Properties.Settings.Default.Maps_EnableCache;

            this.ctxSPOOImages.Setup(i18n.Images, ShowProcessingOutput.ShowProcessingOutput_GetForImages(), 5, "MB");
            this.ctxSPOOVideos.Setup(i18n.Videos, ShowProcessingOutput.ShowProcessingOutput_GetForVideos(), 10, "MB");
            this.ctxSPOOVideosTranscoding.Setup(i18n.Videos_when_transcoding, ShowProcessingOutput.ShowProcessingOutput_GetForVideosTranscoding(), 1, "MB");

            this.tbxGoogleAPI_Elevation.Text = string.IsNullOrEmpty(MediaData.Properties.Settings.Default.GoogleAPI_Elevation) ? "" : MediaData.Properties.Settings.Default.GoogleAPI_Elevation;
            this.tbxGoogleAPI_Elevation_TextChanged(null, null);

            this.clbEnabledMapProviders.Items.AddRange(MapProvider.All);
            this.SetDisabledMapProviders(MediaData.Properties.Settings.Default.Maps_DisabledProviders);

            this.RefreshShownMapsCacheSize();
        }

        #endregion


        #region GUI events

        private void lnkMaps_ClearCache_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                using (GMap.NET.WindowsForms.GMapControl c = new GMap.NET.WindowsForms.GMapControl())
                {
                    c.Manager.PrimaryCache.DeleteOlderThan(DateTime.Now, null);
                    GMap.NET.CacheProviders.SQLitePureImageCache slCache = c.Manager.PrimaryCache as GMap.NET.CacheProviders.SQLitePureImageCache;
                    if (slCache != null)
                    {
                        DirectoryInfo di = new DirectoryInfo(slCache.GtileCache);
                        foreach (DirectoryInfo di2 in di.GetDirectories())
                        {
                            foreach (FileInfo fi in di2.GetFiles())
                            {
                                try
                                {
                                    GMap.NET.CacheProviders.SQLitePureImageCache.VacuumDb(fi.FullName); ;
                                }
                                catch
                                { }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.RefreshShownMapsCacheSize();
        }

        private void tbxGoogleAPI_Elevation_TextChanged(object sender, EventArgs e)
        {
            this.lnkGoogleAPI_Elevation_Check.Enabled = this.tbxGoogleAPI_Elevation.Text.Trim().Length > 0;
        }

        private void lnkGoogleAPI_Elevation_Get_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                BackgroundWorker bgw = new BackgroundWorker();
                bgw.DoWork += delegate (object bgwSender, DoWorkEventArgs bgwEvent)
                {
                    using (Process process = new Process())
                    {
                        process.StartInfo.UseShellExecute = true;
                        process.StartInfo.FileName = "https://developers.google.com/maps/documentation/elevation/get-api-key";
                        process.Start();
                    }
                };
                bgw.RunWorkerCompleted += delegate (object bgwSender, RunWorkerCompletedEventArgs bgwEvent)
                {
                    bgw.Dispose();
                };
                bgw.RunWorkerAsync();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lnkGoogleAPI_Elevation_Check_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Position position = new Position(45.815363432981M, 9.08027529716492M);
                decimal elevation = position.QueryElevation(this.tbxGoogleAPI_Elevation.Text.Trim());
                if (elevation < 50M || elevation > 500M)
                {
                    throw new Exception(i18n.Retrieved_elevation_wrong);
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show(i18n.This_Google_Elevation_API_Key_good, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void lnkEnabledMapProviders_Default_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string def = null;
            SettingsProperty prop = MediaData.Properties.Settings.Default.Properties["Maps_DisabledProviders"];
            if (prop != null)
            {
                def = prop.DefaultValue as string;
            }
            if (def != null)
            {
                this.SetDisabledMapProviders(def);
            }
        }

        private void lnkEnabledMapProviders_All_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.SetAllMapProviders(true);
        }

        private void lnkEnabledMapProviders_None_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.SetAllMapProviders(false);
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            bool someEnabledMapProvider = false;
            StringBuilder disabledMapProviders = null;
            for (int i = 0; i < this.clbEnabledMapProviders.Items.Count; i++)
            {
                if (this.clbEnabledMapProviders.GetItemChecked(i))
                {
                    someEnabledMapProvider = true;
                }
                else
                {
                    if (disabledMapProviders == null)
                    {
                        disabledMapProviders = new StringBuilder();
                    }
                    else
                    {
                        disabledMapProviders.Append('|');
                    }
                    disabledMapProviders.Append(((MapProvider)this.clbEnabledMapProviders.Items[i]).BaseProvider.Name);
                }
            }
            if (!someEnabledMapProvider)
            {
                this.tcPages.SelectedTab = this.tpMaps;
                MessageBox.Show(i18n.Select_at_least_one_map_provider, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.clbEnabledMapProviders.Focus();
                return;
            }
            try
            {
                object o;
                Language selectedLanguage = (Language)this.cbxLanguage.SelectedItem;
                MediaData.Properties.Settings.Default.AppCulture = selectedLanguage.Code;
                o = UI.GetDescribedEnumValueOfCombobox(this.cbxNormalizeVideo);
                MediaData.Properties.Settings.Default.VideoNormalization = (o == null) ? VideoProcessor.NormalizeCase.Never : (VideoProcessor.NormalizeCase)o;

                o = UI.GetDescribedEnumValueOfCombobox(this.cbxSetFiledateOnMetadata);
                MediaData.Properties.Settings.Default.SetFileDates = (o == null) ? Processor.SetFileDates.Never : (Processor.SetFileDates)o;

                MediaData.Properties.Settings.Default.MaxAllowedDistanceError_Horizontal = Convert.ToDouble(this.nudMaxErrHorizontal.Value);
                MediaData.Properties.Settings.Default.MaxAllowedDistanceError_Vertical = Convert.ToDouble(this.nudMaxErrVertical.Value);

                MediaData.Properties.Settings.Default.DeleteToTrash = this.chkDeleteToTrash.Checked;
                MediaData.Properties.Settings.Default.Maps_EnableCache = this.chkMaps_Cache.Checked;
                MediaData.Properties.Settings.Default.ShowProgessingOutput_Images = this.ctxSPOOImages.SerializedValue;
                MediaData.Properties.Settings.Default.ShowProgessingOutput_Videos = this.ctxSPOOVideos.SerializedValue;
                MediaData.Properties.Settings.Default.ShowProgessingOutput_VideosTranscoding = this.ctxSPOOVideosTranscoding.SerializedValue;
                MediaData.Properties.Settings.Default.GoogleAPI_Elevation = this.tbxGoogleAPI_Elevation.Text.Trim();
                MediaData.Properties.Settings.Default.Maps_DisabledProviders = (disabledMapProviders == null) ? "" : disabledMapProviders.ToString();
                MediaData.Properties.Settings.Default.Save();
                if (selectedLanguage.CultureInfo == null)
                {
                    Localizer.CurrentCulture = Localizer.DefaultCulture;
                }
                else
                {
                    Localizer.CurrentCulture = selectedLanguage.CultureInfo;
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion


        #region Instance methods

        private void RefreshShownMapsCacheSize()
        {
            try
            {
                decimal totalSize = 0M;
                using (GMap.NET.WindowsForms.GMapControl c = new GMap.NET.WindowsForms.GMapControl())
                {
                    GMap.NET.CacheProviders.SQLitePureImageCache slCache = c.Manager.PrimaryCache as GMap.NET.CacheProviders.SQLitePureImageCache;
                    if (slCache != null)
                    {
                        DirectoryInfo di = new DirectoryInfo(slCache.GtileCache);
                        foreach (DirectoryInfo di2 in di.GetDirectories())
                        {
                            foreach (FileInfo fi in di2.GetFiles())
                            {
                                totalSize += Convert.ToDecimal(fi.Length);
                            }
                        }
                    }
                }
                this.lblMapsCacheSize.Text = string.Format(i18n._cache_size_X_, Localizer.FormatSize(totalSize));
                this.lblMapsCacheSize.Visible = true;
            }
            catch
            {
                this.lblMapsCacheSize.Visible = false;
            }
        }

        private void SetAllMapProviders(bool chk)
        {
            for (int i = 1; i < this.clbEnabledMapProviders.Items.Count; i++)
            {
                this.clbEnabledMapProviders.SetItemChecked(i, chk);
            }
        }

        private void SetDisabledMapProviders(string serializedList)
        {
            this.SetAllMapProviders(true);
            if (!string.IsNullOrEmpty(serializedList))
            {
                foreach (string baseName in serializedList.Split('|'))
                {
                    MapProvider mp = MapProvider.GetByBase(baseName);
                    if (mp != null)
                    {
                        int itemIndex = this.clbEnabledMapProviders.Items.IndexOf(mp);
                        if (itemIndex >= 0)
                        {
                            this.clbEnabledMapProviders.SetItemChecked(itemIndex, false);
                        }
                    }
                }
            }
        }

        #endregion

    }
}
