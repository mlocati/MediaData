using GMap.NET.MapProviders;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MLocati.MediaData
{
    class MapProvider
    {

        #region Instance properties

        public readonly GMapProvider BaseProvider;

        private static Regex _rxSimplifyName = null;
        private static Regex RXSimplifyName
        {
            get
            {
                if (MapProvider._rxSimplifyName == null)
                {
                    MapProvider._rxSimplifyName = new Regex(@"[^a-z0-9_]+", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                }
                return MapProvider._rxSimplifyName;

            }
        }

        public string Name
        {
            get
            {
                return Localizer.GetString("GMapProvider_" + MapProvider.RXSimplifyName.Replace(this.BaseProvider.Name, "_"));
            }
        }

        public bool Enabled
        {
            get
            {
                if (string.IsNullOrEmpty(MediaData.Properties.Settings.Default.Maps_DisabledProviders))
                {
                    return false;
                }
                List<string> disabledNames = new List<string>(MediaData.Properties.Settings.Default.Maps_DisabledProviders.Split('|'));
                return disabledNames.Contains(this.BaseProvider.Name) ? false : true;
            }
        }

        #endregion


        #region Constructors

        private MapProvider(GMapProvider provider)
        {
            this.BaseProvider = provider;
        }

        #endregion


        #region Instance methods

        public override string ToString()
        {
            return this.Name;
        }

        #endregion


        #region Static properties

        private static MapProvider[] _all = null;
        public static MapProvider[] All
        {
            get
            {
                if (MapProvider._all == null)
                {
                    List<GMapProvider> original = GMapProviders.List;
                    List<MapProvider> all = new List<MapProvider>(original.Count);
                    foreach (GMapProvider o in original)
                    {
                        all.Add(new MapProvider(o));
                    }
                    all.Sort(delegate (MapProvider a, MapProvider b)
                    {
                        return string.Compare(a.Name, b.Name, true);
                    });
                    MapProvider._all = all.ToArray();
                }
                return MapProvider._all;
            }
        }

        #endregion


        #region Static methods

        public static MapProvider GetByBase(string baseProviderName)
        {
            if (!string.IsNullOrEmpty(baseProviderName))
            {
                foreach (MapProvider mp in MapProvider.All)
                {
                    if (string.Compare(baseProviderName, mp.BaseProvider.Name, true) == 0)
                    {
                        return mp;
                    }
                }
            }
            return null;
        }

        public static MapProvider GetByBase(GMapProvider baseProvider)
        {
            if (baseProvider != null)
            {
                foreach (MapProvider mp in MapProvider.All)
                {
                    if (mp.BaseProvider == baseProvider)
                    {
                        return mp;
                    }
                }
            }
            return null;
        }

        public static MapProvider[] GetEnabledProviders()
        {
            if (string.IsNullOrEmpty(MediaData.Properties.Settings.Default.Maps_DisabledProviders))
            {
                return MapProvider.All;
            }
            List<string> disabledNames = new List<string>(MediaData.Properties.Settings.Default.Maps_DisabledProviders.Split('|'));
            List<MapProvider> enabled = new List<MapProvider>(MapProvider.All.Length);
            foreach (MapProvider mp in MapProvider.All)
            {
                if (!disabledNames.Contains(mp.BaseProvider.Name))
                {
                    enabled.Add(mp);
                }
            }
            return (enabled.Count > 0) ? enabled.ToArray() : MapProvider.All;
        }

        #endregion

    }
}
