using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace MLocati.MediaData
{
    public static class Localizer
    {

        #region Types

        [AttributeUsage(AttributeTargets.All)]
        public class DescriptionAttribute : System.ComponentModel.DescriptionAttribute
        {
            public DescriptionAttribute(string key)
            : base(Localizer.GetString(key))
            { }
        }

        public class CultureChangedEventArgs : EventArgs
        {
            public readonly CultureInfo NewCulture;
            public CultureChangedEventArgs(CultureInfo newCulture)
            {
                this.NewCulture = newCulture;
            }
        }

        public delegate void CultureChangedEventHandler(object sender, CultureChangedEventArgs e);

        #endregion


        #region Constants

        private const string NEUTRAL_CULTURE_ID = "en-US";

        #endregion


        #region Events

        public static event CultureChangedEventHandler CultureChanged;

        #endregion


        #region Static properties

        private static List<CultureInfo> _availableCultures = null;
        public static List<CultureInfo> AvailableCultures
        {
            get
            {
                if (Localizer._availableCultures == null)
                {
                    Dictionary<string, CultureInfo> availableCulturesDictionary = new Dictionary<string, CultureInfo>();
                    availableCulturesDictionary.Add(Localizer.NeutralCulture.Name, Localizer.NeutralCulture);
                    ResourceManager rm = new ResourceManager(typeof(MediaData.i18n));
                    foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
                    {
                        if (!availableCulturesDictionary.ContainsKey(ci.Name) && !ci.Equals(CultureInfo.InvariantCulture))
                        {
                            ResourceSet rs;
                            try { rs = rm.GetResourceSet(ci, true, false); }
                            catch
                            { rs = null; }
                            if (rs != null)
                            {
                                availableCulturesDictionary.Add(ci.Name, ci);
                            }
                        }
                    }
                    Localizer._availableCultures = new List<CultureInfo>(availableCulturesDictionary.Values);
                }
                Localizer._availableCultures.Sort(delegate (CultureInfo a, CultureInfo b)
                {
                    return string.Compare(a.DisplayName, b.DisplayName);
                });
                return Localizer._availableCultures;
            }
        }

        private static CultureInfo _neutralCulture = null;
        public static CultureInfo NeutralCulture
        {
            get
            {
                if (Localizer._neutralCulture == null)
                {
                    Localizer._neutralCulture = new CultureInfo("en-US");
                }
                return Localizer._neutralCulture;
            }
        }

        private static CultureInfo _defaultCulture = null;
        public static CultureInfo DefaultCulture
        {
            get
            {
                if (Localizer._defaultCulture == null)
                {
                    CultureInfo searchCulture = CultureInfo.InstalledUICulture;
                    while (Localizer._defaultCulture == null && searchCulture != null && !string.IsNullOrEmpty(searchCulture.Name))
                    {
                        foreach (CultureInfo ci in Localizer.AvailableCultures)
                        {
                            if (ci.Name == searchCulture.Name)
                            {
                                Localizer._defaultCulture = ci;
                                break;
                            }
                        }
                        searchCulture = searchCulture.Parent;
                    }
                    if (Localizer._defaultCulture == null)
                    {
                        Localizer._defaultCulture = Localizer.NeutralCulture;
                    }
                }
                return Localizer._defaultCulture;
            }
        }

        private static CultureInfo _currentCulture = null;
        public static CultureInfo CurrentCulture
        {
            get
            {
                if (Localizer._currentCulture == null)
                {
                    Localizer._currentCulture = Localizer.DefaultCulture;
                }
                return Localizer._currentCulture;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("CurrentCulture");
                }
                CultureInfo ci = null;
                foreach (CultureInfo available in Localizer.AvailableCultures)
                {
                    if (available.Name == value.Name)
                    {
                        ci = available;
                        break;
                    }
                }
                if (ci == null)
                {
                    throw new ArgumentOutOfRangeException("CurrentCulture");
                }
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture = Application.CurrentCulture = Localizer._currentCulture = ci;
                foreach (Form form in Application.OpenForms)
                {
                    try
                    {
                        ComponentResourceManager resources = new ComponentResourceManager(form.GetType());
                        form.SuspendLayout();
                        try
                        {
                            foreach (FieldInfo field in form.GetType().GetFields(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.NonPublic))
                            {
                                IComponent component = field.GetValue(form) as IComponent;
                                if (component != null)
                                {
                                    resources.ApplyResources(component, field.Name);
                                }
                            }
                            resources.ApplyResources(form, "$this");
                        }
                        catch
                        { }
                        finally
                        {
                            form.ResumeLayout(false);
                            form.PerformLayout();
                        }
                    }
                    catch
                    { }

                }
                if (Localizer.CultureChanged != null)
                {
                    Localizer.CultureChanged(null, new CultureChangedEventArgs(ci));
                }
            }
        }

        #endregion


        #region Static methods

        public static string GetString(string key)
        {
            try
            {
                string value = i18n.ResourceManager.GetString(key);
                if (value != null)
                {
                    return value;
                }
            }
            catch
            { }
            return key;
        }

        public static string FormatSize(decimal size)
        {
            if (Math.Abs(size) < 1000)
            {
                return string.Format(i18n.Size_bytes_X, size.ToString("0"));
            }
            size /= 1024M;
            if (Math.Abs(size) < 1000)
            {
                return string.Format(i18n.Size_KB_X, size.ToString("0.0"));
            }
            size /= 1024M;
            if (Math.Abs(size) < 1000)
            {
                return string.Format(i18n.Size_MB_X, size.ToString("0.0"));
            }
            size /= 1024M;
            if (Math.Abs(size) < 1000)
            {
                return string.Format(i18n.Size_GB_X, size.ToString("0.0"));
            }
            size /= 1024M;
            return string.Format(i18n.Size_TB_X, size.ToString("0.0"));
        }

        public static string FormatMeters(decimal meters)
        {
            return Localizer.FormatMeters(meters, true);
        }
        public static string FormatMeters(decimal meters, bool precise)
        {
            return string.Format("{0} m.", meters.ToString(precise ? "0.0" : "0"));
        }

        private static List<Control> GetAllFormControls(Form form)
        {
            List<Control> controls = new List<Control>();
            GetAllFormControls(form, controls);
            return controls;
        }
        private static void GetAllFormControls(Control parent, List<Control> controls)
        {
            foreach (Control child in parent.Controls)
            {
                controls.Add(child);
                GetAllFormControls(child, controls);
            }
        }
        #endregion

    }
}
