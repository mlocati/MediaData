using System;
using System.Text.RegularExpressions;

namespace MLocati.MediaData
{
    public class ShowProcessingOutput
    {

        #region Types

        public enum When
        {
            Always,
            BiggerThan,
            Never,
        }

        #endregion


        #region Instance properties

        public readonly When Shown;

        public readonly UInt64 BiggerThanThisBytes;

        public UInt64 BiggerThanThisShownSize
        {
            get
            {
                UInt64 size;
                string unit;
                this.SplitSize(out size, out unit);
                return size;
            }
        }
        public string BiggerThanThisShownUnit
        {
            get
            {
                UInt64 size;
                string unit;
                this.SplitSize(out size, out unit);
                return unit;
            }
        }

        public readonly bool AutocloseOnSuccess;

        #endregion


        #region Constructors

        private ShowProcessingOutput(string serialized)
        {
            this.Shown = When.Never;
            this.BiggerThanThisBytes = 0L;
            if (!string.IsNullOrEmpty(serialized))
            {
                serialized = serialized.Trim();
                if (serialized.EndsWith("a", StringComparison.OrdinalIgnoreCase))
                {
                    this.AutocloseOnSuccess = true;
                    serialized = (serialized.Length == 1) ? "" : serialized.Substring(0, serialized.Length - 1);
                }
                if (serialized == "-1")
                {
                    this.Shown = When.Always;
                }
                else
                {
                    Match m = Regex.Match(serialized, @"^\s*(?<num>[1-9]\d*)\s*(?<unit>B|KB|MB|GB)\s*$", RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
                    if (m.Success)
                    {
                        this.Shown = When.BiggerThan;
                        this.BiggerThanThisBytes = UInt64.Parse(m.Groups["num"].Value);
                        switch (m.Groups["unit"].Value.ToUpperInvariant())
                        {
                            case "KB":
                                this.BiggerThanThisBytes = this.BiggerThanThisBytes << 10;
                                break;
                            case "MB":
                                this.BiggerThanThisBytes = this.BiggerThanThisBytes << 20;
                                break;
                            case "GB":
                                this.BiggerThanThisBytes = this.BiggerThanThisBytes << 30;
                                break;
                        }
                    }
                }
            }
        }

        #endregion


        #region Instance methods

        private void SplitSize(out UInt64 size, out string unit)
        {
            size = this.BiggerThanThisBytes;
            if (size < 1024)
            {
                unit = "B";
            }
            else
            {
                size = size >> 10;
                if (size < 1024)
                {
                    unit = "KB";
                }
                else
                {
                    size = size >> 10;
                    if (size < 1024)
                    {
                        unit = "MB";
                    }
                    else
                    {
                        size = size >> 10;
                        unit = "GB";
                    }
                }
            }
        }

        #endregion


        #region Static methods

        public static ShowProcessingOutput ShowProcessingOutput_GetForImages()
        {
            return new ShowProcessingOutput(MediaData.Properties.Settings.Default.ShowProgessingOutput_Images);
        }
        public static ShowProcessingOutput ShowProcessingOutput_GetForVideos()
        {
            return new ShowProcessingOutput(MediaData.Properties.Settings.Default.ShowProgessingOutput_Videos);
        }
        public static ShowProcessingOutput ShowProcessingOutput_GetForVideosTranscoding()
        {
            return new ShowProcessingOutput(MediaData.Properties.Settings.Default.ShowProgessingOutput_VideosTranscoding);
        }

        #endregion

    }
}
