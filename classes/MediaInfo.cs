using System;
using System.Collections.Generic;

namespace MLocati.MediaData
{
    public class MediaInfo
    {

        #region Types

        public class NameTimestamp
        {
            private string _name;
            public string Name
            {
                get { return this._name; }
            }
            private DateTime _timestamp;
            public DateTime Timestamp
            {
                get { return this._timestamp; }
            }
            public string TimestampStr
            {
                get
                {
                    return string.Format("{0} {1}", this._timestamp.ToShortDateString(), this._timestamp.ToLongTimeString());
                }
            }
            public NameTimestamp(string name, DateTime timestamp)
            {
                this._name = name;
                this._timestamp = timestamp;
            }
            public virtual void ShownTimeZoneChanged(TimeZoneInfo oldTZI, TimeZoneInfo newTZI)
            {
                this._timestamp = TimeZoneHandler.Convert(oldTZI, newTZI, this._timestamp);
            }
            public NameTimestamp Clone()
            {
                return new NameTimestamp(this._name, this._timestamp);
            }
        }

        #endregion


        #region Instance properties

        protected DateTime? _timestampMin;
        public DateTime? TimestampMin
        {
            get
            {
                return this._timestampMin;
            }
        }

        protected DateTime? _timestampMax;
        public DateTime? TimestampMax
        {
            get
            {
                return this._timestampMax;
            }
        }

        protected Position _position;
        public Position Position
        {
            get
            { return this._position; }
            set
            { this._position = value; }
        }

        public UInt64? TimestampMinMaxHalfDelta
        {
            get
            {
                if (this._timestampMin.HasValue)
                {
                    return Convert.ToUInt64(Math.Round((this._timestampMax.Value - this._timestampMin.Value).TotalSeconds)) >> 1;
                }
                else
                {
                    return null;
                }
            }
        }

        public DateTime? TimestampMean
        {
            get
            {
                UInt64? halfDelta = this.TimestampMinMaxHalfDelta;
                if (halfDelta.HasValue)
                {
                    if (halfDelta.Value == 0)
                    {
                        return this._timestampMin;
                    }
                    else
                    {
                        DateTime dt = this._timestampMin.Value.AddSeconds(halfDelta.Value);
                        return new DateTime?(dt);
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        private List<NameTimestamp> _alternativeMetadataTimestamps;

        #endregion


        #region Constructors

        public MediaInfo()
            : this(null, null)
        { }
        public MediaInfo(DateTime? timestamp)
            : this(timestamp, null)
        { }
        public MediaInfo(Position position)
            : this(null, position)
        { }
        public MediaInfo(DateTime? timestamp, Position position)
        {
            this._alternativeMetadataTimestamps = new List<NameTimestamp>();
            this._timestampMin = timestamp;
            this._timestampMax = timestamp;
            this._position = position;
        }

        #endregion


        #region Instance methods

        public List<NameTimestamp> GetAlternatives()
        {
            return this.GetAlternatives(false);
        }
        public List<NameTimestamp> GetAlternatives(bool evenIfNotNecessary)
        {
            List<NameTimestamp> result = new List<NameTimestamp>();
            if (this._alternativeMetadataTimestamps.Count > 1)
            {
                bool getThem;
                if (evenIfNotNecessary)
                {
                    getThem = true;
                }
                else
                {
                    UInt64? d = this.TimestampMinMaxHalfDelta;
                    getThem = (d.HasValue && d.Value > 0);
                }
                if (getThem)
                {
                    foreach (NameTimestamp nt in this._alternativeMetadataTimestamps)
                    {
                        result.Add(nt.Clone());
                    }
                    result.Sort(delegate (NameTimestamp a, NameTimestamp b)
                    {
                        if (a.Timestamp < b.Timestamp)
                        {
                            return -1;
                        }
                        if (a.Timestamp > b.Timestamp)
                        {
                            return 1;
                        }
                        return string.Compare(a.Name, b.Name, true);
                    });
                }
            }
            return result;
        }

        public void AddAlternativeMetadataTimestamp(string name, DateTime timestamp)
        {
            this._alternativeMetadataTimestamps.Add(new NameTimestamp(name, timestamp));
            if (this._timestampMin.HasValue)
            {
                if (this._timestampMin.Value > timestamp)
                {
                    this._timestampMin = new DateTime?(timestamp);
                }
                if (this._timestampMax.Value < timestamp)
                {
                    this._timestampMax = new DateTime?(timestamp);
                }
            }
            else
            {
                this._timestampMin = new DateTime?(timestamp);
                this._timestampMax = new DateTime?(timestamp);
            }
        }

        protected MediaInfo CloneX()
        {
            return new MediaInfo(this._timestampMin, (this._position == null) ? null : this._position.Clone());
        }

        public virtual MediaInfo CloneWithNewTimestamp(DateTime? timestamp)
        {
            MediaInfo clone = this.CloneX();
            clone._timestampMin = timestamp;
            clone._timestampMax = timestamp;
            return clone;
        }

        public virtual MediaInfo CloneWithNewPosition(Position position)
        {
            MediaInfo clone = this.CloneX();
            clone._position = position;
            return clone;
        }

        public virtual string CheckUpdatedInfo(MediaInfo original, MediaInfo wantedValues)
        {
            // "this" in this context represents the newly written data
            if (wantedValues._timestampMin.HasValue)
            {
                if (!this._timestampMin.HasValue)
                {
                    return i18n.Unable_to_set_metadata_timestamp;
                }
                if (wantedValues._timestampMin.Value != this._timestampMin.Value)
                {
                    return string.Format(i18n.Written_timestamp_X_is_different_from_wanted_timestamp_Y, this._timestampMin.Value, wantedValues._timestampMin.Value);
                }
                if (wantedValues._timestampMax.Value != this._timestampMax.Value)
                {
                    return string.Format(i18n.Written_timestamp_X_is_different_from_wanted_timestamp_Y, this._timestampMax.Value, wantedValues._timestampMax.Value);
                }
            }
            else
            {
                if (this._timestampMin.HasValue)
                {
                    return i18n.Unable_to_remove_metadata_timestamp;
                }
            }
            if (wantedValues._position == null)
            {
                if (this._position != null)
                {
                    return i18n.Unable_to_remove_metadata_position;
                }
            }
            else
            {
                if (this._position == null)
                {
                    return i18n.Unable_to_set_metadata_position;
                }
                double horizontalDistanceError = this._position.DistanceTo(wantedValues.Position);
                if (horizontalDistanceError > MediaData.Properties.Settings.Default.MaxAllowedDistanceError_Horizontal)
                {
                    return string.Format(i18n.Written_horizontal_position_more_distant_than_X_from_wanted__max_allowed_is_Y, horizontalDistanceError, MediaData.Properties.Settings.Default.MaxAllowedDistanceError_Horizontal);
                }
                if (this._position.Alt.HasValue)
                {
                    if (!wantedValues._position.Alt.HasValue)
                    {
                        return i18n.Unable_to_remove_metadata_altitude;
                    }
                    decimal verticalDistanceError = Math.Abs(wantedValues.Position.Alt.Value - this._position.Alt.Value);
                    if (verticalDistanceError > Convert.ToDecimal(MediaData.Properties.Settings.Default.MaxAllowedDistanceError_Vertical))
                    {
                        return string.Format(i18n.Written_vertical_position_more_distant_than_X_from_wanted__max_allowed_is_Y, verticalDistanceError, MediaData.Properties.Settings.Default.MaxAllowedDistanceError_Vertical);
                    }
                }
                else if (wantedValues._position.Alt.HasValue)
                {
                    return i18n.Unable_to_set_metadata_altitude;
                }
            }
            return null;
        }

        public virtual void ShownTimeZoneChanged(TimeZoneInfo oldTZI, TimeZoneInfo newTZI)
        {
            
            if (this._alternativeMetadataTimestamps != null && this._alternativeMetadataTimestamps.Count > 0)
            {
                this._timestampMin = null;
                this._timestampMax = null;
                foreach (NameTimestamp nt in this._alternativeMetadataTimestamps)
                {
                    nt.ShownTimeZoneChanged(oldTZI, newTZI);
                    if (this._timestampMin.HasValue)
                    {
                        if (this._timestampMin.Value > nt.Timestamp)
                        {
                            this._timestampMin = new DateTime?(nt.Timestamp);
                        }
                        if (this._timestampMax.Value < nt.Timestamp)
                        {
                            this._timestampMax = new DateTime?(nt.Timestamp);
                        }
                    }
                    else
                    {
                        this._timestampMin = new DateTime?(nt.Timestamp);
                        this._timestampMax = new DateTime?(nt.Timestamp);
                    }
                }
            }
            else
            {
                if (this._timestampMin.HasValue)
                {
                    this._timestampMin = TimeZoneHandler.Convert(oldTZI, newTZI, this._timestampMin.Value);
                }
                if (this._timestampMax.HasValue)
                {
                    this._timestampMax = TimeZoneHandler.Convert(oldTZI, newTZI, this._timestampMax.Value);
                }
            }
        }

        #endregion

    }
}
