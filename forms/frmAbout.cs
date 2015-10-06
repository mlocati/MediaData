using LitJson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace MLocati.MediaData
{
    public partial class frmAbout : Form
    {

        #region Types

        private class ReleaseInfo
        {
            public readonly Version Version;
            public readonly bool PreRelase;
            public readonly DateTime Date;
            public readonly string ReleaseNotes;
            public readonly Uri SetupURL;
            public ReleaseInfo(JsonData json)
            {
                if (json == null || !json.IsObject)
                {
                    throw new Exception("Bad JSON: expecting an object...");
                }
                if (!json.Keys.Contains("name"))
                {
                    throw new Exception("Bad JSON: missing 'name' key");
                }
                if (!json["name"].IsString)
                {
                    throw new Exception("Bad JSON: bad 'name' type");
                }
                this.Version = new Version(((string)json["name"]).TrimStart('v').Trim(' ', '.', '-', '_'));
                int versionParts = this.Version.ToString().Split('.').Length;
                if (versionParts < 4)
                {
                    StringBuilder fixVersion = new StringBuilder(this.Version.ToString());
                    for (int i = versionParts; i < 4; i++)
                    {
                        fixVersion.Append(".0");
                    }
                    this.Version = new Version(fixVersion.ToString());
                }
                this.PreRelase = (json.Keys.Contains("prerelease") && json["prerelease"].IsBoolean) ? (bool)json["prerelease"] : false;
                if (!json.Keys.Contains("published_at"))
                {
                    throw new Exception("Bad JSON: missing 'published_at' key");
                }
                if (!json["published_at"].IsString)
                {
                    throw new Exception("Bad JSON: bad 'published_at' type");
                }
                this.Date = DateTime.Parse((string)json["published_at"]);
                this.ReleaseNotes = (json.Keys.Contains("body") && json["body"].IsString) ? (string)json["body"] : "";
                if (this.ReleaseNotes == null)
                {
                    this.ReleaseNotes = "";
                }
                else
                {
                    this.ReleaseNotes = this.ReleaseNotes.Replace("\r\n", "\n").Replace('\r', '\n').Replace("\n", Environment.NewLine);
                }
                this.SetupURL = null;
                if (json.Keys.Contains("assets") && json["assets"].IsArray)
                {
                    foreach (JsonData jsonAsset in json["assets"])
                    {
                        if (jsonAsset.Keys.Contains("name") && jsonAsset["name"].IsString && ((string)jsonAsset["name"]).EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                        {
                            if (this.SetupURL == null)
                            {
                                if (jsonAsset.Keys.Contains("browser_download_url") && jsonAsset["browser_download_url"].IsString)
                                {
                                    string du = (string)jsonAsset["browser_download_url"];
                                    if (du.Length > 0)
                                    {
                                        try
                                        {
                                            Uri url = new Uri(du);
                                            if (url.IsAbsoluteUri)
                                            {
                                                this.SetupURL = url;
                                            }
                                        }
                                        catch
                                        { }
                                    }
                                }
                            }
                            else
                            {
                                this.SetupURL = null;
                                break;
                            }
                        }

                    }
                }
            }
            public override string ToString()
            {
                return this.Version.ToString();
            }
        }

        private class ProgramUpdateInfo
        {
            public string Notes;
            public ReleaseInfo LatestVersion;
            public ProgramUpdateInfo()
            {
                this.Notes = "";
                this.LatestVersion = null;
            }
        }

        #endregion


        #region Constructors

        public frmAbout()
        {
            InitializeComponent();
            this.Icon = Program.Icon;
            this.Text = string.Format(i18n.About_X, Application.ProductName);
            this.lblProduct.Text = Application.ProductName;
            this.lblVersion.Text = string.Format(i18n.Version_X, Application.ProductVersion);
            this.lblCopyright.Text = "";
            try
            {
                AssemblyCopyrightAttribute[] crAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false) as AssemblyCopyrightAttribute[];
                if (crAttributes != null && crAttributes.Length > 0)
                {
                    this.lblCopyright.Text = crAttributes[0].Copyright;
                }
            }
            catch
            { }
        }

        #endregion


        #region GUI events

        private void lnkWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (BackgroundWorker bgw = new BackgroundWorker())
            {
                bgw.DoWork += delegate (object s, DoWorkEventArgs ea)
                {
                    using (Process process = new Process())
                    {
                        process.StartInfo.UseShellExecute = true;
                        process.StartInfo.FileName = "https://github.com/mlocati/MediaData/";
                        process.Start();
                    }
                };
                bgw.RunWorkerAsync();
            }
        }

        private void lnkLicenses_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string licensesDir = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "licenses");
            if (!Directory.Exists(licensesDir))
            {
                MessageBox.Show(i18n.Licenses_directory_not_found, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            using (BackgroundWorker bgw = new BackgroundWorker())
            {
                bgw.DoWork += delegate (object s, DoWorkEventArgs ea)
                {

                    using (Process process = new Process())
                    {
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.FileName = "explorer.exe";
                        process.StartInfo.Arguments = Tool.Escape(licensesDir);
                        process.Start();
                    }
                };
                bgw.RunWorkerAsync();
            }
        }

        private void lnkCheckUpdates_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.lnkCheckUpdates.Enabled = false;
            this.bgwCheckUpdates.RunWorkerAsync();
        }

        private void bgwCheckUpdates_DoWork(object sender, DoWorkEventArgs e)
        {
            Exception error = null;
            ProgramUpdateInfo result = new ProgramUpdateInfo();
            WebResponse response = null;
            try
            {
                string json;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.github.com/repos/mlocati/MediaData/releases");
                request.UserAgent = string.Format("{0} v{1}", Application.ProductName, Application.ProductVersion);
                request.Credentials = CredentialCache.DefaultCredentials;
                try
                {
                    response = request.GetResponse();
                }
                catch (WebException x)
                {
                    response = x.Response;
                    if (response == null)
                    {
                        throw x;
                    }
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
                Encoding encoding = null;
                try
                {
                    encoding = Encoding.GetEncoding(httpResponse.CharacterSet);
                }
                catch
                { }
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }
                using (Stream dataStream = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(dataStream, encoding))
                    {
                        json = sr.ReadToEnd();
                    }
                }
                Version installedVersion = new Version(Application.ProductVersion);
                JsonData data = JsonMapper.ToObject(json);
                if (!data.IsArray)
                {
                    throw new Exception("Bad JSON: expecting an array...");
                }
                ReleaseInfo currentRelease = null;
                List<ReleaseInfo> nextReleases = new List<ReleaseInfo>();
                foreach (JsonData jsonRelease in data)
                {
                    ReleaseInfo ri = new ReleaseInfo(jsonRelease);
                    int cmp = installedVersion.CompareTo(ri.Version);
                    if (cmp == 0)
                    {
                        currentRelease = ri;
                    }
                    else if (cmp < 0)
                    {
                        if (result.LatestVersion == null || result.LatestVersion.Version.CompareTo(ri.Version) < 0)
                        {
                            result.LatestVersion = ri;
                        }
                        nextReleases.Add(ri);
                    }
                }
                if (nextReleases.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    if (currentRelease != null)
                    {
                        sb.AppendLine(string.Format(i18n.Current_version_X, currentRelease.Version.ToString()));
                        sb.AppendLine(string.Format(i18n.Released_on_X, currentRelease.Date.ToString()));
                        sb.AppendLine();
                    }
                    nextReleases.Sort(delegate (ReleaseInfo a, ReleaseInfo b)
                    {
                        return b.Version.CompareTo(b.Version);
                    });
                    foreach (ReleaseInfo ri in nextReleases)
                    {
                        sb.AppendLine(string.Format(i18n.Version_X, ri.Version.ToString()));
                        sb.AppendLine(string.Format(i18n.Released_on_X, ri.Date.ToString()));
                        if (ri.ReleaseNotes.Length > 0)
                        {
                            sb.AppendLine(i18n.Release_notes_).AppendLine("\t" + ri.ReleaseNotes.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                        }
                        sb.AppendLine();
                    }
                    result.Notes = sb.ToString().TrimEnd();
                }
            }
            catch (Exception x)
            {
                error = x;
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
            if (error == null)
            {
                e.Result = result;
            }
            else
            {
                e.Result = error;
            }
        }

        private void bgwCheckUpdates_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.lnkCheckUpdates.Enabled = true;
            Exception error = e.Error;
            if (error == null)
            {
                error = e.Result as Exception;
            }
            if (error != null)
            {
                MessageBox.Show(error.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ProgramUpdateInfo update = (ProgramUpdateInfo)e.Result;
            if (update.LatestVersion == null)
            {
                MessageBox.Show(string.Format(i18n.Your_version_of_X_is_the_latest_one, Application.ProductName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            using (frmUpdateAvailable f = new frmUpdateAvailable(update.Notes, update.LatestVersion.SetupURL))
            {
                f.ShowDialog(this);
            }
        }

        private void frmAbout_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.bgwCheckUpdates.IsBusy)
            {
                e.Cancel = true;
            }
        }

        #endregion

    }
}
