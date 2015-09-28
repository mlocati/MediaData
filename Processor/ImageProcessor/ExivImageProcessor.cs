using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace MLocati.MediaData
{
    public class ExivImageProcessor : MLocati.MediaData.ImageProcessor
    {

        #region Constants

        private const TimeZoneHandler.Zone EXIF_IMAGEPHOTOXMP_TAGS_TIMEZONE = TimeZoneHandler.Zone.Shoot;

        private const TimeZoneHandler.Zone EXIF_GPS_TAGS_TIMEZONE = TimeZoneHandler.Zone.Utc;

        #endregion


        #region Types

        protected class ExivInfo : MediaInfo
        {
            public bool HasXmp = false;
            public bool HasGpsTimestamp = false;
        }

        #endregion


        #region Instance properties

        public override bool SupportsAltitude
        {
            get
            { return true; }
        }

        #endregion


        #region Constructors

        public ExivImageProcessor(string fullFilename)
            : base(fullFilename)
        { }

        #endregion


        #region Instance methods

        protected override MediaInfo GetInfo(string fullFilename)
        {
            ExivInfo result = new ExivInfo();
            Tool tool = new Tool(Tool.Which.Exiv2, new string[] { "-Pkv", Tool.Escape(fullFilename) }, new int[] { 0, 253 });
            Tool.Result toolOutput = tool.Run();
            decimal? lat = null;
            int? latSign = null;
            decimal? lng = null;
            int? lngSign = null;
            decimal? alt = null;
            int? altSign = null;
            DateTime dt;
            DateTime? gpsDate = null;
            TimeSpan? gpsTime = null;
            foreach (string line in toolOutput.StdOut.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n'))
            {
                string[] chunks = line.Split(new char[] { ' ' }, 2);
                if (chunks.Length == 2)
                {
                    chunks[1] = chunks[1].Trim();
                    if (chunks[1].Length > 0)
                    {
                        switch (chunks[0])
                        {
                            case "Exif.Image.DateTime":
                            case "Exif.Photo.DateTimeOriginal":
                            case "Exif.Photo.DateTimeDigitized":
                            case "Xmp.xmp.CreateDate":
                            case "Xmp.xmp.MetadataDate":
                            case "Xmp.xmp.ModifyDate":
                                if (chunks[0].StartsWith("Xmp.xmp."))
                                {
                                    result.HasXmp = true;
                                }
                                if (DateTime.TryParseExact(chunks[1], @"yyyy\:MM\:dd HH\:mm\:ss", NumberFormatInfo.InvariantInfo, DateTimeStyles.AssumeLocal, out dt))
                                {
                                    result.AddAlternativeMetadataTimestamp(string.Format(i18n.Tag_X, chunks[0]), TimeZoneHandler.ToShootZone(ExivImageProcessor.EXIF_IMAGEPHOTOXMP_TAGS_TIMEZONE, dt));
                                }
                                else if (DateTime.TryParseExact(chunks[1], @"yyyy-MM-dd\THH\:mm\:sszzz", NumberFormatInfo.InvariantInfo, DateTimeStyles.AssumeLocal, out dt))
                                {
                                    result.AddAlternativeMetadataTimestamp(string.Format(i18n.Tag_X, chunks[0]), TimeZoneHandler.ToShootZone(TimeZoneHandler.Zone.Utc, dt.ToUniversalTime()));
                                }
                                else
                                {
                                    throw new Exception(string.Format(i18n.Invalid_tag_X_value_V, chunks[0], chunks[1]));
                                }
                                break;
                            case "Exif.GPSInfo.GPSDateStamp":
                                result.HasGpsTimestamp = true;
                                if (!DateTime.TryParseExact(chunks[1], @"yyyy\:MM\:dd", NumberFormatInfo.InvariantInfo, DateTimeStyles.AssumeLocal, out dt))
                                {
                                    throw new Exception(string.Format(i18n.Invalid_tag_X_value_V, chunks[0], chunks[1]));
                                }
                                gpsDate = new DateTime?(dt);
                                break;
                            case "Exif.GPSInfo.GPSTimeStamp":
                                result.HasGpsTimestamp = true;
                                decimal dH, dM, dS;
                                if (!ExivImageProcessor.GpsTripleToDecimal(chunks[1], out dH, out dM, out dS))
                                {
                                    throw new Exception(string.Format(i18n.Invalid_tag_X_value_V, chunks[0], chunks[1]));
                                }
                                gpsTime = new TimeSpan(Convert.ToInt32(Math.Round(dH)), Convert.ToInt32(Math.Round(dM)), Convert.ToInt32(Math.Round(dS)));
                                break;
                            case "Exif.GPSInfo.GPSLatitudeRef":
                                if (latSign.HasValue)
                                {
                                    throw new Exception(string.Format(i18n.Multiple_tags_found, chunks[0]));
                                }
                                switch (chunks[1])
                                {
                                    case "N":
                                        latSign = +1;
                                        break;
                                    case "S":
                                        latSign = -1;
                                        break;
                                    default:
                                        throw new Exception(string.Format(i18n.Invalid_tag_X_value_V, chunks[0], chunks[1]));
                                }
                                break;
                            case "Exif.GPSInfo.GPSLatitude":
                                if (lat.HasValue)
                                {
                                    throw new Exception(string.Format(i18n.Multiple_tags_found, chunks[0]));
                                }
                                lat = ExivImageProcessor.gpsLLToDecimal(chunks[1]);
                                if (!lat.HasValue)
                                {
                                    throw new Exception(string.Format(i18n.Invalid_tag_X_value_V, chunks[0], chunks[1]));
                                }
                                break;
                            case "Exif.GPSInfo.GPSLongitudeRef":
                                if (lngSign.HasValue)
                                {
                                    throw new Exception(string.Format(i18n.Multiple_tags_found, chunks[0]));
                                }
                                switch (chunks[1])
                                {
                                    case "E":
                                        lngSign = +1;
                                        break;
                                    case "W":
                                        lngSign = -1;
                                        break;
                                    default:
                                        throw new Exception(string.Format(i18n.Invalid_tag_X_value_V, chunks[0], chunks[1]));
                                }
                                break;
                            case "Exif.GPSInfo.GPSLongitude":
                                if (lng.HasValue)
                                {
                                    throw new Exception(string.Format(i18n.Multiple_tags_found, chunks[0]));
                                }
                                lng = ExivImageProcessor.gpsLLToDecimal(chunks[1]);
                                if (!lng.HasValue)
                                {
                                    throw new Exception(string.Format(i18n.Invalid_tag_X_value_V, chunks[0], chunks[1]));
                                }
                                break;
                            case "Exif.GPSInfo.GPSAltitudeRef":
                                if (altSign.HasValue)
                                {
                                    throw new Exception(string.Format(i18n.Multiple_tags_found, chunks[0]));
                                }
                                switch (chunks[1])
                                {
                                    case "0":
                                        altSign = +1;
                                        break;
                                    case "1":
                                        altSign = -1;
                                        break;
                                    default:
                                        throw new Exception(string.Format(i18n.Invalid_tag_X_value_V, chunks[0], chunks[1]));
                                }
                                break;
                            case "Exif.GPSInfo.GPSAltitude":
                                if (alt.HasValue)
                                {
                                    throw new Exception(string.Format(i18n.Multiple_tags_found, chunks[0]));
                                }
                                alt = ExivImageProcessor.gpsAltToDecimal(chunks[1]);
                                if (!alt.HasValue)
                                {
                                    throw new Exception(string.Format(i18n.Invalid_tag_X_value_V, chunks[0], chunks[1]));
                                }
                                break;
                        }
                    }
                }
            }
            if (gpsDate.HasValue || gpsTime.HasValue)
            {
                if (!gpsTime.HasValue)
                {
                    throw new Exception(i18n.GPS_date_without_time);
                }
                if (!gpsDate.HasValue)
                {
                    throw new Exception(i18n.GPS_time_without_date);
                }
                result.AddAlternativeMetadataTimestamp(i18n.GPS_tag, TimeZoneHandler.ToShootZone(ExivImageProcessor.EXIF_GPS_TAGS_TIMEZONE, gpsDate.Value.Add(gpsTime.Value)));
            }
            if (lat.HasValue || latSign.HasValue || lng.HasValue || lngSign.HasValue || alt.HasValue || altSign.HasValue)
            {
                if (!lat.HasValue)
                {
                    throw new Exception(i18n.Missing_latitude);
                }
                if (!latSign.HasValue)
                {
                    throw new Exception(i18n.Missing_latituderef);
                }
                if (!lng.HasValue)
                {
                    throw new Exception(i18n.Missing_longitude);
                }
                if (!lngSign.HasValue)
                {
                    throw new Exception(i18n.Missing_longituderef);
                }

                result.Position = new Position(lat.Value * Convert.ToDecimal(latSign.Value), lng.Value * Convert.ToDecimal(lngSign.Value), (alt.HasValue && altSign.HasValue) ? alt.Value * Convert.ToDecimal(altSign.Value) : (decimal?)null);
            }
            return result;
        }

        protected override Tool PrepareTool(MediaInfo newInfo, string tempFilename)
        {
            DateTime? ts = newInfo.TimestampMean;
            List<string> args = new List<string>();
            args.Add("-M\"del Exif.Image.TimeZoneOffset\""); // Not supported by most software: let's remove it in order to avoid messing up data
            if (ts.HasValue)
            {
                string s = TimeZoneHandler.Convert(TimeZoneHandler.Zone.Shoot, ExivImageProcessor.EXIF_IMAGEPHOTOXMP_TAGS_TIMEZONE, ts.Value).ToString(@"yyyy\:MM\:dd HH\:mm\:ss", NumberFormatInfo.InvariantInfo);
                args.Add("-M\"set Exif.Image.DateTime " + s + "\"");
                args.Add("-M\"set Exif.Photo.DateTimeOriginal " + s + "\"");
                args.Add("-M\"set Exif.Photo.DateTimeDigitized " + s + "\"");
                if (((ExivInfo)this.Info).HasXmp)
                {
                    args.Add("-M\"set Xmp.xmp.CreateDate " + s + "\"");
                    args.Add("-M\"set Xmp.xmp.MetadataDate " + s + "\"");
                    args.Add("-M\"set Xmp.xmp.ModifyDate " + s + "\"");

                }
                if (((ExivInfo)this.Info).HasGpsTimestamp || newInfo.Position != null)
                {
                    DateTime gpsTime = TimeZoneHandler.Convert(TimeZoneHandler.Zone.Shoot, ExivImageProcessor.EXIF_GPS_TAGS_TIMEZONE, ts.Value);
                    args.Add("-M\"set Exif.GPSInfo.GPSDateStamp " + gpsTime.ToString(@"yyyy\:MM\:dd") + "\"");
                    args.Add("-M\"set Exif.GPSInfo.GPSTimeStamp " + ExivImageProcessor.numberToRational(gpsTime.Hour, 0) + " " + ExivImageProcessor.numberToRational(gpsTime.Minute, 0) + " " + ExivImageProcessor.numberToRational(gpsTime.Second, 0) + "\"");
                }
            }
            else
            {
                args.Add("-M\"del Exif.Image.DateTime\"");
                args.Add("-M\"del Exif.Photo.DateTimeOriginal\"");
                args.Add("-M\"del Exif.Photo.DateTimeDigitized\"");
                args.Add("-M\"del Xmp.xmp.CreateDate\"");
                args.Add("-M\"del Xmp.xmp.MetadataDate\"");
                args.Add("-M\"del Xmp.xmp.ModifyDate\"");
                args.Add("-M\"del Exif.GPSInfo.GPSDateStamp\"");
                args.Add("-M\"del Exif.GPSInfo.GPSTimeStamp\"");
            }
            if (newInfo.Position == null)
            {
                args.Add("-M\"del Exif.GPSInfo.GPSLatitudeRef\"");
                args.Add("-M\"del Exif.GPSInfo.GPSLatitude\"");
                args.Add("-M\"del Exif.GPSInfo.GPSLongitudeRef\"");
                args.Add("-M\"del Exif.GPSInfo.GPSLongitude\"");
                args.Add("-M\"del Exif.GPSInfo.GPSAltitudeRef\"");
                args.Add("-M\"del Exif.GPSInfo.GPSAltitude\"");
            }
            else
            {
                args.Add("-M\"set Exif.GPSInfo.GPSVersionID 2 2 0 0\"");
                args.Add("-M\"set Exif.GPSInfo.GPSLatitude " + ExivImageProcessor.degreesToRationalTriplet(Math.Abs(newInfo.Position.Lat)) + "\"");
                args.Add("-M\"set Exif.GPSInfo.GPSLatitudeRef " + ((newInfo.Position.Lat < 0) ? "S" : "N") + "\"");
                args.Add("-M\"set Exif.GPSInfo.GPSLongitude " + ExivImageProcessor.degreesToRationalTriplet(Math.Abs(newInfo.Position.Lng)) + "\"");
                args.Add("-M\"set Exif.GPSInfo.GPSLongitudeRef " + ((newInfo.Position.Lng < 0) ? "W" : "E") + "\"");
                if (newInfo.Position.Alt.HasValue)
                {
                    args.Add("-M\"set Exif.GPSInfo.GPSAltitudeRef " + ((newInfo.Position.Alt.Value < 0) ? "1" : "0") + "\"");
                    args.Add("-M\"set Exif.GPSInfo.GPSAltitude " + ExivImageProcessor.numberToRational(Math.Abs(newInfo.Position.Alt.Value), 2) + "\"");
                }
                else
                {
                    args.Add("-M\"del Exif.GPSInfo.GPSAltitudeRef\"");
                    args.Add("-M\"del Exif.GPSInfo.GPSAltitude\"");
                }
            }
            File.Copy(this.FullFilename, tempFilename);
            args.Add(Tool.Escape(tempFilename));
            return new Tool(Tool.Which.Exiv2, args.ToArray(), new int[] { 0 });
        }

        #endregion


        #region Static properties

        protected static string[] HandledExtensions
        {
            get
            {
                return new string[]{
                    "jpg", "jpeg",
                    "exv",
                    "cr2",
                    "crw",
                    "tif", "tiff",
                    "dng",
                    "nef",
                    "pef",
                    "srw",
                    "orf",
                    "png",
                    "pgf",
                    "psd",
                    "jp2",
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

        private static Regex _rxGpsTripleToDecimal = null;
        private static Regex RXGpsTripleToDecimal
        {
            get
            {
                if (ExivImageProcessor._rxGpsTripleToDecimal == null)
                {
                    ExivImageProcessor._rxGpsTripleToDecimal = new Regex(@"^\s*(?<num1>\d+)/(?<den1>\d+)\s+(?<num2>\d+)/(?<den2>\d+)\s+(?<num3>\d+)/(?<den3>\d+)\s*", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);
                }
                return ExivImageProcessor._rxGpsTripleToDecimal;
            }
        }

        private static Regex _rxGpsAltToDecimal = null;
        private static Regex RXGpsAltToDecimal
        {
            get
            {
                if (ExivImageProcessor._rxGpsAltToDecimal == null)
                {
                    ExivImageProcessor._rxGpsAltToDecimal = new Regex(@"^\s*(?<numerator>\d+)/(?<denominator>\d+)\s*", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);
                }
                return ExivImageProcessor._rxGpsAltToDecimal;
            }
        }

        #endregion


        #region Static methods

        private static bool GpsTripleToDecimal(string tagValue, out decimal number1, out decimal number2, out decimal number3)
        {
            if (!string.IsNullOrEmpty(tagValue))
            {
                Match m = ExivImageProcessor.RXGpsTripleToDecimal.Match(tagValue);
                if (m.Success)
                {
                    number1 = Convert.ToDecimal(UInt32.Parse(m.Groups["num1"].Value)) / Convert.ToDecimal(UInt32.Parse(m.Groups["den1"].Value));
                    number2 = Convert.ToDecimal(UInt32.Parse(m.Groups["num2"].Value)) / Convert.ToDecimal(UInt32.Parse(m.Groups["den2"].Value));
                    number3 = Convert.ToDecimal(UInt32.Parse(m.Groups["num3"].Value)) / Convert.ToDecimal(UInt32.Parse(m.Groups["den3"].Value));
                    return true;
                }
            }
            number3 = number2 = number1 = 0M;
            return false;
        }

        private static decimal? gpsLLToDecimal(string tagValue)
        {
            decimal? result = null;
            decimal deg, min, sec;
            if (ExivImageProcessor.GpsTripleToDecimal(tagValue, out deg, out min, out sec))
            {
                result = new decimal?(deg + (min + sec / 60M) / 60M);
            }
            else
            {
                result = null;
            }
            return result;
        }

        private static decimal? gpsAltToDecimal(string tagValue)
        {
            decimal? result = null;
            Match m = ExivImageProcessor.RXGpsAltToDecimal.Match(tagValue);
            if (m.Success)
            {
                result = new decimal?(Convert.ToDecimal(UInt32.Parse(m.Groups["numerator"].Value)) / Convert.ToDecimal(UInt32.Parse(m.Groups["denominator"].Value)));
            }
            return result;
        }

        private static string numberToRational(decimal number)
        {
            return ExivImageProcessor.numberToRational(number, null);
        }

        private static string numberToRational(decimal number, uint? fixedDecimals)
        {
            if (number < 0)
            {
                throw new ArgumentOutOfRangeException("number");
            }
            if (fixedDecimals.HasValue)
            {
                if (fixedDecimals.Value == 0)
                {
                    return Convert.ToUInt32(Math.Round(number)).ToString() + " / 1";
                }
                string sDen = "1" + new string('0', (int)fixedDecimals.Value);
                uint den = Convert.ToUInt32(sDen);
                uint num = Convert.ToUInt32(Math.Round(number * den));
                return num.ToString() + " / " + den.ToString();
            }
            string s = number.ToString(NumberFormatInfo.InvariantInfo);
            int p = s.IndexOf('.');
            if (p < 0)
            {
                return ExivImageProcessor.numberToRational(number, 0);
            }
            uint n = Convert.ToUInt32(s.Substring(p + 1).Length);
            return ExivImageProcessor.numberToRational(number, Math.Min(n, 15));
        }

        private static string degreesToRationalTriplet(decimal degrees)
        {
            if (degrees < 0)
            {
                throw new ArgumentOutOfRangeException("degrees");
            }
            uint degs;
            uint mins;
            decimal secs;
            bool foo;
            Position.ExplodeCoordinate(degrees, out foo, out degs, out mins, out secs);
            return string.Format(
                "{0} {1} {2}",
                ExivImageProcessor.numberToRational(degs, 0),
                ExivImageProcessor.numberToRational(mins, 0),
                ExivImageProcessor.numberToRational(secs, 4)
            );
        }

        #endregion

    }
}
