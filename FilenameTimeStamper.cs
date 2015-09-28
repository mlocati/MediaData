using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MLocati.MediaData
{
    public class FilenameTimeStamper
    {

        public readonly string Format;
        public readonly string Extension;
        private DateTime _timestamp;
        public DateTime Timestamp
        {
            get
            { return this._timestamp; }
        }

        private FilenameTimeStamper(string format, string extension, DateTime timestamp)
        {
            this.Format = format;
            this.Extension = extension;
            this._timestamp = timestamp;
        }

        private static Regex _rxTimestamp = null;
        private static Regex RXTimestamp
        {
            get
            {
                if (FilenameTimeStamper._rxTimestamp == null)
                {
                    FilenameTimeStamper._rxTimestamp = new Regex(@"^(?<pre_Y>.*?(\D|^))(?<Y>\d{4})(?<pre_M>\D?)(?<M>\d{2})(?<pre_D>\D?)(?<D>\d{2})(?<pre_h>\D*?)(?<h>\d{2})(?<pre_m>\D?)(?<m>\d{2})((?<pre_s>\D?)(?<s>\d{2}))?(?<final>(\D.*))?$", RegexOptions.Compiled | RegexOptions.ExplicitCapture);
                }
                return FilenameTimeStamper._rxTimestamp;
            }
        }

        private static Regex _rxExtension = null;
        private static Regex RXExtension
        {
            get
            {
                if (FilenameTimeStamper._rxExtension == null)
                {
                    FilenameTimeStamper._rxExtension = new Regex(@"^(?<pre>.*)(?<ext>\.[a-zA-Z0-9_\-]+)$", RegexOptions.Compiled | RegexOptions.ExplicitCapture);
                }
                return FilenameTimeStamper._rxExtension;
            }
        }

        public static FilenameTimeStamper Get(string filename)
        {
            Match match;
            int Y, M, D, h, m, s;
            StringBuilder sb;
            match = FilenameTimeStamper.RXTimestamp.Match(filename);
            if (match.Success)
            {
                Y = int.Parse(match.Groups["Y"].Value);
                M = int.Parse(match.Groups["M"].Value);
                D = int.Parse(match.Groups["D"].Value);
                if (Y > 1000 && Y <= DateTime.Now.Year + 1 && M >= 1 && M <= 12 && D >= 1 && D <= 31 && match.Groups["pre_M"].Value == match.Groups["pre_D"].Value)
                {
                    sb = new StringBuilder();
                    sb.Append(match.Groups["pre_Y"].Value).Append("<Y>").Append(match.Groups["pre_M"].Value).Append("<M>").Append(match.Groups["pre_D"].Value).Append("<D>").Append(match.Groups["pre_h"].Value).Append("<h>").Append(match.Groups["pre_m"].Value).Append("<m>");
                    h = int.Parse(match.Groups["h"].Value);
                    m = int.Parse(match.Groups["m"].Value);

                    bool ok;
                    if (match.Groups["s"].Value == "")
                    {
                        ok = true;
                        s = 0;
                    }
                    else
                    {
                        s = int.Parse(match.Groups["s"].Value);
                        ok = (match.Groups["pre_m"].Value == match.Groups["pre_s"].Value) ? true : false;
                        sb.Append(match.Groups["pre_s"].Value).Append("<s>");
                    }
                    if (ok && h >= 0 && h <= 23 && m >= 0 && m <= 59 && s >= 0 && s <= 59)
                    {
                        string final = match.Groups["final"].Value;
                        string extension = "";
                        match = FilenameTimeStamper.RXExtension.Match(final);
                        if (match.Success)
                        {
                            final = match.Groups["pre"].Value;
                            extension = match.Groups["ext"].Value;
                        }
                        sb.Append(final);
                        return new FilenameTimeStamper(
                            sb.ToString(),
                            extension,
                            new DateTime(Y, M, D, h, m, s)
                        );
                    }

                }
            }
            return null;
        }
    }
}
