using System;

namespace MLocati.MediaData
{
    public static class TimeZoneHandler
    {

        #region Types

        public enum Zone
        {
            Utc,
            System,
            Shoot,
        }

        #endregion


        #region Static properties

        private static TimeZoneInfo _shootTimeZone;
        public static TimeZoneInfo ShootTimeZone
        {
            get
            {
                if (TimeZoneHandler._shootTimeZone == null)
                {
                    TimeZoneHandler._shootTimeZone = TimeZoneInfo.Local;
                }
                return TimeZoneHandler._shootTimeZone;
            }
            set
            {
                TimeZoneHandler._shootTimeZone = (value == null) ? TimeZoneInfo.Local : value;
            }
        }

        #endregion


        #region Static methods

        public static DateTime ToShootZone(Zone fromZone, DateTime dt)
        {
            return TimeZoneHandler.ToShootZone(fromZone, dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
        }
        public static DateTime ToShootZone(Zone fromZone, int year, int month, int day, int hour, int minute)
        {
            return TimeZoneHandler.ToShootZone(fromZone, year, month, day, hour, minute, 0, 0);
        }
        public static DateTime ToShootZone(Zone fromZone, int year, int month, int day, int hour, int minute, int second)
        {
            return TimeZoneHandler.ToShootZone(fromZone, year, month, day, hour, minute, second, 0);
        }
        public static DateTime ToShootZone(Zone fromZone, int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            return TimeZoneHandler.Convert(fromZone, Zone.Shoot, year, month, day, hour, minute, second, 0);
        }

        public static DateTime ToUTC(Zone fromZone, DateTime dt)
        {
            return TimeZoneHandler.ToUTC(fromZone, dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
        }
        public static DateTime ToUTC(Zone fromZone, int year, int month, int day, int hour, int minute)
        {
            return TimeZoneHandler.ToUTC(fromZone, year, month, day, hour, minute, 0, 0);
        }
        public static DateTime ToUTC(Zone fromZone, int year, int month, int day, int hour, int minute, int second)
        {
            return TimeZoneHandler.ToUTC(fromZone, year, month, day, hour, minute, second, 0);
        }
        public static DateTime ToUTC(Zone fromZone, int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            return TimeZoneHandler.Convert(fromZone, Zone.Utc, year, month, day, hour, minute, second, 0);
        }

        public static DateTime ToSystem(Zone fromZone, DateTime dt)
        {
            return TimeZoneHandler.ToSystem(fromZone, dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
        }
        public static DateTime ToSystem(Zone fromZone, int year, int month, int day, int hour, int minute)
        {
            return TimeZoneHandler.ToSystem(fromZone, year, month, day, hour, minute, 0, 0);
        }
        public static DateTime ToSystem(Zone fromZone, int year, int month, int day, int hour, int minute, int second)
        {
            return TimeZoneHandler.ToSystem(fromZone, year, month, day, hour, minute, second, 0);
        }
        public static DateTime ToSystem(Zone fromZone, int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            return TimeZoneHandler.Convert(fromZone, Zone.System, year, month, day, hour, minute, second, 0);
        }

        public static DateTime Convert(Zone fromZone, Zone toZone, DateTime dt)
        {
            return TimeZoneHandler.Convert(fromZone, toZone, dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
        }
        public static DateTime Convert(Zone fromZone, Zone toZone, int year, int month, int day, int hour, int minute)
        {
            return TimeZoneHandler.Convert(fromZone, toZone, year, month, day, hour, minute, 0, 0);
        }
        public static DateTime Convert(Zone fromZone, Zone toZone, int year, int month, int day, int hour, int minute, int second)
        {
            return TimeZoneHandler.Convert(fromZone, toZone, year, month, day, hour, minute, second, 0);
        }
        public static DateTime Convert(Zone fromZone, Zone toZone, int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            TimeZoneInfo fromZoneInfo;
            switch (fromZone)
            {
                case Zone.Utc:
                    fromZoneInfo = TimeZoneInfo.Utc;
                    break;
                case Zone.System:
                    fromZoneInfo = TimeZoneInfo.Local;
                    break;
                case Zone.Shoot:
                    fromZoneInfo = TimeZoneHandler.ShootTimeZone;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("fromZone");
            }
            TimeZoneInfo toZoneInfo;
            switch (toZone)
            {
                case Zone.Utc:
                    toZoneInfo = TimeZoneInfo.Utc;
                    break;
                case Zone.System:
                    toZoneInfo = TimeZoneInfo.Local;
                    break;
                case Zone.Shoot:
                    toZoneInfo = TimeZoneHandler.ShootTimeZone;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("toZone");
            }
            return TimeZoneHandler.Convert(fromZoneInfo, toZoneInfo, year, month, day, hour, minute, second, millisecond);
        }

        public static DateTime Convert(TimeZoneInfo fromZone, TimeZoneInfo toZone, DateTime dt)
        {
            return TimeZoneHandler.Convert(fromZone, toZone, dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
        }
        public static DateTime Convert(TimeZoneInfo fromZone, TimeZoneInfo toZone, int year, int month, int day, int hour, int minute)
        {
            return TimeZoneHandler.Convert(fromZone, toZone, year, month, day, hour, minute, 0, 0);
        }
        public static DateTime Convert(TimeZoneInfo fromZone, TimeZoneInfo toZone, int year, int month, int day, int hour, int minute, int second)
        {
            return TimeZoneHandler.Convert(fromZone, toZone, year, month, day, hour, minute, second, 0);
        }
        public static DateTime Convert(TimeZoneInfo fromZone, TimeZoneInfo toZone, int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            if (fromZone == null)
            {
                throw new ArgumentNullException("fromZone");
            }
            if (toZone == null)
            {
                throw new ArgumentNullException("toZone");
            }
            DateTime dt;
            DateTimeKind dtk;
            if (fromZone == toZone)
            {
                if (fromZone == TimeZoneInfo.Utc)
                {
                    dtk = DateTimeKind.Utc;
                }
                else if (fromZone == TimeZoneInfo.Local)
                {
                    dtk = DateTimeKind.Local;
                }
                else
                {
                    dtk = DateTimeKind.Unspecified;
                }
                return new DateTime(year, month, day, hour, minute, second, millisecond, dtk);
            }
            DateTime fromUTC;
            if (fromZone == TimeZoneInfo.Utc)
            {
                fromUTC = new DateTime(year, month, day, hour, minute, second, millisecond, DateTimeKind.Utc);
            }
            else
            {
                dt = new DateTime(year, month, day, hour, minute, second, millisecond, DateTimeKind.Unspecified);
                fromUTC = TimeZoneInfo.ConvertTimeToUtc(dt, fromZone);
            }
            return (toZone == TimeZoneInfo.Utc) ? fromUTC : TimeZoneInfo.ConvertTimeFromUtc(fromUTC, toZone);
        }

        #endregion

    }
}
