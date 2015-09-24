using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace MLocati.MediaData
{
    public class Position
    {

        #region Instance properties

        private decimal _lat;
        public decimal Lat
        {
            get
            {
                return this._lat;
            }
        }

        private decimal _lng;
        public decimal Lng
        {
            get
            {
                return this._lng;
            }
        }

        private decimal? _alt;
        public decimal? Alt
        {
            get
            {
                return this._alt;
            }
        }

        #endregion


        #region Constructors

        public Position(decimal lat, decimal lng)
            : this(lat, lng, null)
        { }

        public Position(decimal lat, decimal lng, decimal? alt)
        {
            this._lat = lat;
            this._lng = lng;
            this._alt = alt;
        }

        #endregion


        #region Instance methods

        public Position Clone()
        {
            return new Position(this._lat, this._lng, this._alt);
        }

        public string ToGridString()
        {
            string coo = this.ToString(false);
            return this._alt.HasValue ? string.Format("{0} ↕ {1}", coo, Localizer.FormatMeters(this._alt.Value, false)) : coo;
        }
        public override string ToString()
        {
            return this.ToString(false);
        }
        public string ToString(bool precise)
        {
            bool neg;
            uint deg;
            uint min;
            decimal sec;
            string secFormat = precise ? "00.000" : "00";
            StringBuilder sb = new StringBuilder();
            Position.ExplodeCoordinate(this._lat, out neg, out deg, out min, out sec);
            sb.Append(neg ? '-' : '+');
            sb.Append(deg.ToString("0")).Append('°');
            sb.Append(min.ToString("00")).Append('\'');
            sb.Append(sec.ToString(secFormat)).Append('"');
            sb.Append(' ');
            Position.ExplodeCoordinate(this._lng, out neg, out deg, out min, out sec);
            sb.Append(neg ? '-' : '+');
            sb.Append(deg.ToString()).Append('°');
            sb.Append(min.ToString("00")).Append('\'');
            sb.Append(sec.ToString(secFormat)).Append('"');
            return sb.ToString();
        }

        public string Serialize()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this._lat.ToString(NumberFormatInfo.InvariantInfo));
            sb.Append(',');
            sb.Append(this._lng.ToString(NumberFormatInfo.InvariantInfo));
            if (this._alt.HasValue)
            {
                sb.Append(',');
                sb.Append(this._alt.Value.ToString(NumberFormatInfo.InvariantInfo));
            }
            return sb.ToString();
        }

        public decimal QueryElevation()
        {
            decimal? resolution;
            return this.QueryElevation(MediaData.Properties.Settings.Default.GoogleAPI_Elevation, out resolution);
        }
        public decimal QueryElevation(out decimal? resolution)
        {
            return this.QueryElevation(MediaData.Properties.Settings.Default.GoogleAPI_Elevation, out resolution);
        }
        public decimal QueryElevation(string googleApiElevationKey)
        {
            decimal? resolution;
            return this.QueryElevation(googleApiElevationKey, out resolution);
        }
        public decimal QueryElevation(string googleApiElevationKey, out decimal? resolution)
        {
            resolution = null;
            if (string.IsNullOrEmpty(googleApiElevationKey))
            {
                throw new Exception(i18n.Missing_Google_Elevation_API_Key);
            }
            if (!Regex.IsMatch(googleApiElevationKey, @"^[a-z0-9\-]+$", RegexOptions.IgnoreCase))
            {
                throw new Exception(i18n.Invalid_Google_Elevation_API_Key);
            }
            WebRequest request = WebRequest.Create(string.Format("https://maps.googleapis.com/maps/api/elevation/xml?key={0}&locations={1},{2}", googleApiElevationKey, this._lat.ToString(NumberFormatInfo.InvariantInfo), this._lng.ToString(NumberFormatInfo.InvariantInfo)));
            request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response = null;
            XmlReaderSettings xSettings = new XmlReaderSettings();
            xSettings.ConformanceLevel = ConformanceLevel.Document;
            xSettings.IgnoreComments = true;
            xSettings.IgnoreProcessingInstructions = true;
            xSettings.IgnoreWhitespace = true;
            XPathNavigator xRoot;
            try
            {
                try
                {
                    response = request.GetResponse();
                }
                catch (WebException x)
                {
                    response = x.Response;
                }
                HttpWebResponse httpResponse = response as HttpWebResponse;
                if (httpResponse == null)
                {
                    throw new Exception(i18n.No_server_response);
                }
                if ((int)httpResponse.StatusCode < 200 || (int)httpResponse.StatusCode >= 300)
                {
                    throw new Exception(httpResponse.StatusDescription);
                }
                using (Stream dataStream = response.GetResponseStream())
                {
                    using (XmlReader xReader = XmlReader.Create(dataStream, xSettings))
                    {
                        xRoot = (new XPathDocument(xReader)).CreateNavigator();
                    }
                }
            }
            finally
            {
                if (response != null)
                {
                    try
                    {
                        response.Close();
                    }
                    catch
                    { }
                }
            }
            XPathNodeIterator xNodes;
            string nodeName;

            nodeName = "/ElevationResponse/status";
            xNodes = xRoot.Select(nodeName);
            switch (xNodes.Count)
            {
                case 0:
                    throw new Exception(string.Format(i18n.Invalid_response_missing_node_X, nodeName));
                case 1:
                    xNodes.MoveNext();
                    switch (xNodes.Current.InnerXml.Trim())
                    {
                        case "OK":
                            break;
                        case "INVALID_REQUEST":
                            throw new Exception(i18n.GoogleElevationRequest_malformed);
                        case "OVER_QUERY_LIMIT":
                            throw new Exception(i18n.GoogleElevationRequest_overquota);
                        case "REQUEST_DENIED":
                            throw new Exception(i18n.GoogleElevationRequest_denied);
                        default:
                            throw new Exception(xNodes.Current.InnerXml);
                    }
                    break;
                default:
                    throw new Exception(string.Format(i18n.Invalid_response_too_many_X_nodes, nodeName));
            }
            decimal elevation;
            nodeName = "/ElevationResponse/result/elevation";
            xNodes = xRoot.Select(nodeName);
            switch (xNodes.Count)
            {
                case 0:
                    throw new Exception(string.Format(i18n.Invalid_response_missing_node_X, nodeName));
                case 1:
                    xNodes.MoveNext();
                    if (!decimal.TryParse(xNodes.Current.InnerXml.Trim(), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out elevation))
                    {
                        throw new Exception(string.Format(i18n.Invalid_response_node_X_bad_value_Y, nodeName, xNodes.Current.InnerXml.Trim()));
                    }
                    break;
                default:
                    throw new Exception(string.Format(i18n.Invalid_response_too_many_X_nodes, nodeName));
            }
            nodeName = "/ElevationResponse/result/resolution";
            xNodes = xRoot.Select(nodeName);
            switch (xNodes.Count)
            {
                case 0:
                    break;
                case 1:
                    xNodes.MoveNext();
                    decimal d;
                    if (!decimal.TryParse(xNodes.Current.InnerXml.Trim(), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out d))
                    {
                        throw new Exception(string.Format(i18n.Invalid_response_node_X_bad_value_Y, nodeName, xNodes.Current.InnerXml.Trim()));
                    }
                    resolution = d;
                    break;
                default:
                    throw new Exception(string.Format(i18n.Invalid_response_too_many_X_nodes, nodeName));
            }
            return elevation;
        }

        #endregion


        #region Static methods

        public static void ExplodeCoordinate(decimal value, out bool negative, out UInt32 degrees, out UInt32 minutes, out decimal seconds)
        {
            if (value < 0)
            {
                negative = true;
                value = Math.Abs(value);
            }
            else
            {
                negative = false;
            }
            degrees = Convert.ToUInt32(Math.Floor(value));
            value = (value - Convert.ToDecimal(degrees)) * 60M;
            minutes = Convert.ToUInt32(Math.Floor(value));
            value = (value - Convert.ToDecimal(minutes)) * 60M;
            seconds = value;
        }

        public static Position Unserialize(string serialized)
        {
            Position result = null;
            if (!string.IsNullOrEmpty(serialized))
            {
                string[] chunks = serialized.Split(',');
                decimal lat, lng;
                if (
                    chunks.Length >= 2 && chunks.Length <= 3
                    && decimal.TryParse(chunks[0], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out lat)
                    && decimal.TryParse(chunks[1], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out lng)
                )
                {
                    bool ok;
                    decimal? alt = null;
                    if (chunks.Length < 3)
                    {
                        ok = true;
                    }
                    else
                    {
                        decimal d;
                        ok = decimal.TryParse(chunks[2], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out d);
                        if (ok)
                        {
                            alt = d;
                        }
                    }
                    if (ok)
                    {
                        result = new Position(lat, lng, alt);
                    }
                }
                if (result == null)
                {
                    throw new Exception(string.Format(i18n.Unable_to_unserialize_position_from_X, serialized));
                }
            }
            return result;
        }

        #endregion

    }
}
