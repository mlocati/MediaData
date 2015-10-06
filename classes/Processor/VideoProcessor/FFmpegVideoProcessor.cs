using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace MLocati.MediaData
{
    public class FFmpegVideoProcessor : MLocati.MediaData.VideoProcessor
    {

        #region Constants

        private const TimeZoneHandler.Zone CREATION_TIME_TAG_TIMEZONE = TimeZoneHandler.Zone.Utc;

        #endregion


        #region Instance properties

        public override bool CanBeNormalized
        {
            get
            {
                bool result = false;
                if (string.Compare(Path.GetExtension(this.Filename), ".mp4", true) != 0)
                {
                    if (((VideoInfo)this.Info).AudioTracks <= 1 && ((VideoInfo)this.Info).VideoTracks <= 1 && ((VideoInfo)this.Info).AudioTracks + ((VideoInfo)this.Info).VideoTracks > 0)
                    {
                        result = true;
                    }
                }
                return result;
            }
        }

        #endregion


        #region Constructors

        public FFmpegVideoProcessor(string fullFilename)
            : base(fullFilename)
        { }

        #endregion


        #region Instance methods

        protected override MediaInfo GetInfo(string fullFilename)
        {
            VideoInfo result = new VideoInfo();
            Tool tool = new Tool(Tool.Which.FFprobe, new string[] { "-hide_banner", "-print_format xml=fully_qualified=1", "-show_format", "-show_streams", "-show_error", Tool.Escape(fullFilename) }, new int[] { 0 });
            Tool.Result toolResult = tool.Run();
            if (toolResult.Error != null)
            {
                throw toolResult.Error;
            }
            FFprobe.ffprobeType ffp;
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(toolResult.StdOut);
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0L;
                using (XmlReader xr = XmlReader.Create(ms))
                {
                    ffp = (MediaData.FFprobe.ffprobeType)FFmpegVideoProcessor.FFProbeDeserializer.Deserialize(xr);
                }
            }
            if (ffp.error != null)
            {
                throw new Exception(ffp.error.@string);
            }
            if (ffp.format.tag != null)
            {
                foreach (FFprobe.tagType tag in ffp.format.tag)
                {
                    switch (tag.key)
                    {
                        case "creation_time":
                            DateTime? dt = FFmpegVideoProcessor.ParseCreationTimeTag(tag.value);
                            if (!dt.HasValue)
                            {
                                throw new Exception(string.Format(i18n.Invalid_tag_X_value_V, tag.key, tag.value));
                            }
                            result.AddAlternativeMetadataTimestamp(string.Format(i18n.GlobalTag_X, tag.key), dt.Value);
                            break;
                        case "location":
                        case "location-eng":
                            Position position = FFmpegVideoProcessor.ParseLocationTag(tag.value);
                            if (position == null)
                            {
                                throw new Exception(string.Format(i18n.Invalid_tag_X_value_V, tag.key, tag.value));
                            }
                            if (result.Position == null || (result.Position.Lat == 0M && result.Position.Lng == 0M))
                            {
                                result.Position = position;
                            }
                            else if (position.Lat != 0M && position.Lng != 0M && (position.Lat != result.Position.Lat || position.Lng != result.Position.Lng))
                            {
                                throw new Exception(i18n.Multiple_GPS_positions_found);
                            }
                            break;
                    }
                }
            }
            if (ffp.streams != null)
            {
                foreach (FFprobe.streamType stream in ffp.streams)
                {
                    switch (stream.codec_type)
                    {
                        case "audio":
                            result.AudioTracks++;
                            break;
                        case "video":
                            result.VideoTracks++;
                            break;
                        default:
                            throw new Exception(string.Format(i18n.Unsupported_stream_type_X, stream.codec_type));
                    }
                    if (stream.tag != null)
                    {
                        foreach (FFprobe.tagType tag in stream.tag)
                        {
                            switch (tag.key)
                            {
                                case "creation_time":
                                    DateTime? dt = FFmpegVideoProcessor.ParseCreationTimeTag(tag.value);
                                    if (!dt.HasValue)
                                    {
                                        throw new Exception(string.Format(i18n.Invalid_tag_X_value_V, tag.key, tag.value));
                                    }
                                    result.AddAlternativeMetadataTimestamp(string.Format(i18n.StreamTag_X, tag.key), dt.Value);
                                    break;
                                case "location":
                                case "location-eng":
                                    Position position = FFmpegVideoProcessor.ParseLocationTag(tag.value);
                                    if (position == null)
                                    {
                                        throw new Exception(string.Format(i18n.Invalid_tag_X_value_V, tag.key, tag.value));
                                    }
                                    if (result.Position == null || (result.Position.Lat == 0 && result.Position.Lng == 0))
                                    {
                                        result.Position = position;
                                    }
                                    else if (position.Lat != 0 && position.Lng != 0 && (position.Lat != result.Position.Lat || position.Lng != result.Position.Lng))
                                    {
                                        throw new Exception(i18n.Multiple_GPS_positions_found);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            if (result.VideoTracks == 0 && result.AudioTracks == 0)
            {
                throw new Exception(i18n.No_video_audio);
            }
            return result;
        }

        protected override Tool PrepareTool(MediaInfo newInfo, string tempFilename, bool normalize)
        {
            List<string> args = new List<string>();
            args.Add("-hide_banner");
            //args.Add("-nostats");
            args.Add("-loglevel info");
            args.Add("-i " + Tool.Escape(this.FullFilename));
            VideoInfo videoInfo = (VideoInfo)this.Info;
            string ext = Path.GetExtension(this.Filename).ToLower();
            if (ext == ".mov")
            {
                args.Add("-f mov");
            }
            else if (newInfo.Position != null)
            {
                if (normalize)
                {
                    args.Add("-f mov");
                }
                else
                {
                    switch (ext)
                    {
                        case ".mp4":
                        case ".mpeg4":
                            args.Add("-f mov");
                            break;
                    }
                }
            }
            if (videoInfo.VideoTracks == 0)
            {
                args.Add("-vn");
            }
            else if (normalize)
            {
                args.Add("-vcodec h264");
            }
            else
            {
                args.Add("-vcodec copy");
            }
            if (videoInfo.AudioTracks == 0)
            {
                args.Add("-an");
            }
            else if (normalize)
            {
                args.Add("-acodec libvo_aacenc");
            }
            else
            {
                args.Add("-acodec copy");
            }
            if (normalize)
            {
                args.Add("-pix_fmt yuv420p");
            }
            List<string> streamSpecs = new List<string>();
            streamSpecs.Add("g");
            streamSpecs.Add("s");
            DateTime? ts = newInfo.TimestampMean;
            if (ts.HasValue)
            {
                string creationtimeString = TimeZoneHandler.Convert(TimeZoneHandler.Zone.Shoot, FFmpegVideoProcessor.CREATION_TIME_TAG_TIMEZONE, ts.Value).ToString(@"yyyy-MM-dd HH\:mm\:ss", NumberFormatInfo.InvariantInfo);
                foreach (string streamSpec in streamSpecs)
                {
                    args.Add("-metadata:" + streamSpec);
                    args.Add("creation_time=\"" + creationtimeString + "\"");
                }
            }
            else
            {
                foreach (string streamSpec in streamSpecs)
                {
                    args.Add("-metadata:" + streamSpec);
                    args.Add("creation_time=\"\"");
                }
            }
            if (newInfo.Position == null)
            {
                foreach (string streamSpec in streamSpecs)
                {
                    args.Add("-metadata:" + streamSpec);
                    args.Add("location=\"\"");
                    args.Add("-metadata:" + streamSpec);
                    args.Add("location-eng=\"\"");
                }
            }
            else
            {
                var locString = "";
                locString += newInfo.Position.Lat.ToString("+00.0000;-00.0000", NumberFormatInfo.InvariantInfo);
                locString += newInfo.Position.Lng.ToString("+000.0000;-000.0000", NumberFormatInfo.InvariantInfo);
                if (newInfo.Position.Alt.HasValue)
                {
                    newInfo.Position.Alt.Value.ToString("+0.000000;-0.000000", NumberFormatInfo.InvariantInfo);
                }
                locString += "/";
                foreach (string streamSpec in streamSpecs)
                {
                    args.Add("-metadata:" + streamSpec);
                    args.Add("location=\"" + locString + "\"");
                    args.Add("-metadata:" + streamSpec);
                    args.Add("location-eng=\"" + locString + "\"");
                }
            }
            args.Add(Tool.Escape(tempFilename));
            return new Tool(Tool.Which.FFmpeg, args.ToArray(), new int[] { 0 });
        }

        #endregion


        #region Static properties

        protected static string[] HandledExtensions
        {
            get
            {
                return new string[]{
                    "3gp",
                    "avi",
                    "mp4", "mpeg4",
                    "mkv",
                    "mov",
                    "mpg",
                    "wmv",
                };
            }
        }

        protected static int Priority
        {
            get
            {
                return 0;
            }
        }

        private static XmlSerializer _ffProbeDeserializer = null;
        private static XmlSerializer FFProbeDeserializer
        {
            get
            {
                if (FFmpegVideoProcessor._ffProbeDeserializer == null)
                {
                    FFmpegVideoProcessor._ffProbeDeserializer = new XmlSerializer(typeof(MediaData.FFprobe.ffprobeType));
                }
                return FFmpegVideoProcessor._ffProbeDeserializer;
            }
        }

        private static Regex _rxCreationTimeTag = null;
        private static Regex RXCreationTimeTag
        {
            get
            {
                if (FFmpegVideoProcessor._rxCreationTimeTag == null)
                {
                    FFmpegVideoProcessor._rxCreationTimeTag = new Regex(@"^\s*(?<Y>\d{4})-(?<M>\d{2})-(?<D>\d{2})\s+(?<h>\d{2}):(?<m>\d{2}):(?<s>\d{2})\s*$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
                }
                return FFmpegVideoProcessor._rxCreationTimeTag;
            }
        }

        private static Regex _rxPositionTag = null;
        private static Regex RXPositionTag
        {
            get
            {
                if (FFmpegVideoProcessor._rxPositionTag == null)
                {
                    FFmpegVideoProcessor._rxPositionTag = new Regex(@"^\s*(?<lat>[+\-]\d+(\.\d*)?)(?<lng>[+\-]\d+(\.\d*)?)(?<alt>[+\-]\d+(\.\d*)?)?\s*\/\s*?$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
                }
                return FFmpegVideoProcessor._rxPositionTag;
            }
        }

        #endregion


        #region Static methods

        private static DateTime? ParseCreationTimeTag(string tagValue)
        {
            DateTime? result = null;
            Match match = FFmpegVideoProcessor.RXCreationTimeTag.Match(tagValue);
            if (match.Success)
            {
                int Y, M, D, h, m, s;
                if (
                   int.TryParse(match.Groups["Y"].Value, out Y)
                   &&
                   int.TryParse(match.Groups["M"].Value, out M)
                   &&
                   int.TryParse(match.Groups["D"].Value, out D)
                   &&
                   int.TryParse(match.Groups["h"].Value, out h)
                   &&
                   int.TryParse(match.Groups["m"].Value, out m)
                   &&
                   int.TryParse(match.Groups["s"].Value, out s)
                )
                {
                    result = TimeZoneHandler.ToShootZone(FFmpegVideoProcessor.CREATION_TIME_TAG_TIMEZONE, Y, M, D, h, m, s);
                }
            }
            return result;
        }

        private static Position ParseLocationTag(string tagValue)
        {
            Position result = null;
            Match match = FFmpegVideoProcessor.RXPositionTag.Match(tagValue);
            if (match.Success)
            {
                decimal lat, lng;
                decimal? alt;
                if (decimal.TryParse(match.Groups["lat"].Value, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, NumberFormatInfo.InvariantInfo, out lat) && decimal.TryParse(match.Groups["lng"].Value, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, NumberFormatInfo.InvariantInfo, out lng))
                {
                    alt = null;
                    decimal d;
                    if (!string.IsNullOrEmpty(match.Groups["alt"].Value) && decimal.TryParse(match.Groups["alt"].Value, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, NumberFormatInfo.InvariantInfo, out d))
                    {
                        alt = d;
                    }
                    result = new Position(lat, lng, alt);
                }
            }
            return result;
        }

        #endregion

    }
}
