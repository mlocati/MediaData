using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace MLocati.MediaData
{
    public abstract class VideoProcessor : Processor
    {

        #region Types

        protected class VideoInfo : MediaInfo
        {
            public int AudioTracks = 0;
            public int VideoTracks = 0;
            public override bool CheckUpdatedInfo(MediaInfo original, MediaInfo newValues)
            {
                if (!base.CheckUpdatedInfo(original, newValues))
                {
                    return false;
                }
                VideoInfo orig = (VideoInfo)original;
                if (this.AudioTracks != orig.AudioTracks || this.VideoTracks != orig.VideoTracks)
                {
                    return false;
                }
                return true;
            }
        }

        [Serializable]
        public enum NormalizeCase
        {
            [Localizer.Description("NormalizeCase_Never")]
            Never,
            [Localizer.Description("NormalizeCase_AskOnError")]
            AskOnError,
            [Localizer.Description("NormalizeCase_AlwaysAsk")]
            AlwaysAsk,
            [Localizer.Description("NormalizeCase_AlwaysOnError")]
            AlwaysOnError,
            [Localizer.Description("NormalizeCase_Always")]
            Always,

        }

        #endregion


        #region Instance properties

        public abstract bool CanBeNormalized
        {
            get;
        }

        #endregion


        #region Constructors

        protected VideoProcessor(string fullFilename)
            : base(fullFilename)
        {
        }

        #endregion


        #region Instance Methods

        protected abstract Tool PrepareTool(MediaInfo newInfo, string tempFilename, bool normalize);

        protected override void SetNewMetadata(MediaInfo newInfo, IWin32Window parentWindow)
        {
            List<bool?> normalizes = new List<bool?>();
            if (this.CanBeNormalized)
            {
                switch (MediaData.Properties.Settings.Default.VideoNormalization)
                {
                    case NormalizeCase.Always:
                        normalizes.Add(true);
                        break;
                    case NormalizeCase.AlwaysAsk:
                        normalizes.Add(null);
                        break;
                    case NormalizeCase.AlwaysOnError:
                        normalizes.Add(false);
                        normalizes.Add(true);
                        break;
                    case NormalizeCase.AskOnError:
                        normalizes.Add(false);
                        normalizes.Add(null);
                        break;
                }
            }
            if (normalizes.Count == 0)
            {
                normalizes.Add(false);
            }
            Exception error = null;
            foreach (bool? normalize in normalizes)
            {
                bool norm;
                if (normalize.HasValue)
                {
                    norm = normalize.Value;
                }
                else
                {
                    if (error == null)
                    {
                        switch (MessageBox.Show(i18n.Confirm_video_normalization, Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                        {
                            case DialogResult.Yes:
                                norm = true;
                                break;
                            case DialogResult.No:
                                norm = false;
                                break;
                            default:
                                throw new OperationCanceledException();
                        }
                    }
                    else
                    {
                        if (MessageBox.Show(string.Format("{0}\n\n{1}", string.Format(i18n.Following_error_occurred_X, error.Message), i18n.Confirm_video_normalization), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            norm = true;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                error = this.SetNewMetadata(newInfo, parentWindow, norm);
                if (error == null || !(error is Processor.UnableToSetNewMetadataException))
                {
                    break;
                }
            }
            if (error != null)
            {
                throw error;
            }
        }

        private Exception SetNewMetadata(MediaInfo newInfo, IWin32Window parentWindow, bool normalize)
        {
            string finalFilename;
            string tempFilename;
            ShowProcessingOutput spo;
            if (normalize)
            {
                finalFilename = Path.ChangeExtension(this.FullFilename, ".mp4");
                tempFilename = this.GetNewTempFilename(".mp4");
                spo = ShowProcessingOutput.ShowProcessingOutput_GetForVideosTranscoding();
            }
            else
            {
                finalFilename = this.FullFilename;
                tempFilename = this.GetNewTempFilename();
                spo = ShowProcessingOutput.ShowProcessingOutput_GetForVideos();
            }
            Exception result;
            try
            {
                Tool tool = this.PrepareTool(newInfo, tempFilename, normalize);
                result = this.RunTool(newInfo, tool, spo, tempFilename, parentWindow, finalFilename);
            }
            catch (Exception error)
            {
                result = error;
            }
            if (result != null)
            { 
                try
                {
                    if (File.Exists(tempFilename))
                    {
                        File.Delete(tempFilename);
                    }
                }
                catch
                { }
            }
            return result;
        }

        #endregion

    }
}
