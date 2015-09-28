using System;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

namespace MLocati.MediaData
{
    public static class Program
    {

        #region Static properties

        private static Icon _icon = null;
        public static Icon Icon
        {
            get
            {
                if (Program._icon == null)
                {
                    Program._icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location); ;
                }
                return Program._icon;
            }
        }

        #endregion


        #region Static methods

        [STAThread]
        public static int Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                if (MediaData.Properties.Settings.Default.DoUpgradeSettings)
                {
                    MediaData.Properties.Settings.Default.Upgrade();
                    MediaData.Properties.Settings.Default.DoUpgradeSettings = true;
                    MediaData.Properties.Settings.Default.Save();
                }
            }
            catch
            { }
            try
            {
                if (string.IsNullOrEmpty(MediaData.Properties.Settings.Default.AppCulture))
                {
                    Localizer.CurrentCulture = Localizer.DefaultCulture;
                }
                else
                {
                    Localizer.CurrentCulture = new CultureInfo(MediaData.Properties.Settings.Default.AppCulture);
                }
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }
            if (!Tool.Check())
            {
                return 1;
            }
            using (frmMain fm = new frmMain())
            {
                Application.Run(fm);
            }
            return 0;
        }

        #endregion

    }
}
