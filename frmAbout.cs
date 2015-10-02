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

        #endregion

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
                /*
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
                */
                json = "[{\"url\":\"https://api.github.com/repos/mlocati/MediaData/releases/1897561\",\"assets_url\":\"https://api.github.com/repos/mlocati/MediaData/releases/1897561/assets\",\"upload_url\":\"https://uploads.github.com/repos/mlocati/MediaData/releases/1897561/assets{?name,label}\",\"html_url\":\"https://github.com/mlocati/MediaData/releases/tag/1.1.4\",\"id\":1897561,\"tag_name\":\"1.1.4\",\"target_commitish\":\"master\",\"name\":\"v1.1.4\",\"draft\":false,\"author\":{\"login\":\"mlocati\",\"id\":928116,\"avatar_url\":\"https://avatars.githubusercontent.com/u/928116?v=3\",\"gravatar_id\":\"\",\"url\":\"https://api.github.com/users/mlocati\",\"html_url\":\"https://github.com/mlocati\",\"followers_url\":\"https://api.github.com/users/mlocati/followers\",\"following_url\":\"https://api.github.com/users/mlocati/following{/other_user}\",\"gists_url\":\"https://api.github.com/users/mlocati/gists{/gist_id}\",\"starred_url\":\"https://api.github.com/users/mlocati/starred{/owner}{/repo}\",\"subscriptions_url\":\"https://api.github.com/users/mlocati/subscriptions\",\"organizations_url\":\"https://api.github.com/users/mlocati/orgs\",\"repos_url\":\"https://api.github.com/users/mlocati/repos\",\"events_url\":\"https://api.github.com/users/mlocati/events{/privacy}\",\"received_events_url\":\"https://api.github.com/users/mlocati/received_events\",\"type\":\"User\",\"site_admin\":false},\"prerelease\":false,\"created_at\":\"2015-10-01T16:07:04Z\",\"published_at\":\"2015-10-01T16:09:33Z\",\"assets\":[{\"url\":\"https://api.github.com/repos/mlocati/MediaData/releases/assets/908531\",\"id\":908531,\"name\":\"MediaData-setup.exe\",\"label\":null,\"uploader\":{\"login\":\"mlocati\",\"id\":928116,\"avatar_url\":\"https://avatars.githubusercontent.com/u/928116?v=3\",\"gravatar_id\":\"\",\"url\":\"https://api.github.com/users/mlocati\",\"html_url\":\"https://github.com/mlocati\",\"followers_url\":\"https://api.github.com/users/mlocati/followers\",\"following_url\":\"https://api.github.com/users/mlocati/following{/other_user}\",\"gists_url\":\"https://api.github.com/users/mlocati/gists{/gist_id}\",\"starred_url\":\"https://api.github.com/users/mlocati/starred{/owner}{/repo}\",\"subscriptions_url\":\"https://api.github.com/users/mlocati/subscriptions\",\"organizations_url\":\"https://api.github.com/users/mlocati/orgs\",\"repos_url\":\"https://api.github.com/users/mlocati/repos\",\"events_url\":\"https://api.github.com/users/mlocati/events{/privacy}\",\"received_events_url\":\"https://api.github.com/users/mlocati/received_events\",\"type\":\"User\",\"site_admin\":false},\"content_type\":\"application/x-msdownload\",\"state\":\"uploaded\",\"size\":15241160,\"download_count\":0,\"created_at\":\"2015-10-01T16:08:24Z\",\"updated_at\":\"2015-10-01T16:08:39Z\",\"browser_download_url\":\"https://github.com/mlocati/MediaData/releases/download/1.1.4/MediaData-setup.exe\"}],\"tarball_url\":\"https://api.github.com/repos/mlocati/MediaData/tarball/1.1.4\",\"zipball_url\":\"https://api.github.com/repos/mlocati/MediaData/zipball/1.1.4\",\"body\":\"- CTRL-C, CTRL-V in position form to copy/paste position\\r\\n- Allow slightly bigger error when saving position (5 meters instead of 1 meter)\"},{\"url\":\"https://api.github.com/repos/mlocati/MediaData/releases/1896582\",\"assets_url\":\"https://api.github.com/repos/mlocati/MediaData/releases/1896582/assets\",\"upload_url\":\"https://uploads.github.com/repos/mlocati/MediaData/releases/1896582/assets{?name,label}\",\"html_url\":\"https://github.com/mlocati/MediaData/releases/tag/1.1.3\",\"id\":1896582,\"tag_name\":\"1.1.3\",\"target_commitish\":\"master\",\"name\":\"v1.1.3\",\"draft\":false,\"author\":{\"login\":\"mlocati\",\"id\":928116,\"avatar_url\":\"https://avatars.githubusercontent.com/u/928116?v=3\",\"gravatar_id\":\"\",\"url\":\"https://api.github.com/users/mlocati\",\"html_url\":\"https://github.com/mlocati\",\"followers_url\":\"https://api.github.com/users/mlocati/followers\",\"following_url\":\"https://api.github.com/users/mlocati/following{/other_user}\",\"gists_url\":\"https://api.github.com/users/mlocati/gists{/gist_id}\",\"starred_url\":\"https://api.github.com/users/mlocati/starred{/owner}{/repo}\",\"subscriptions_url\":\"https://api.github.com/users/mlocati/subscriptions\",\"organizations_url\":\"https://api.github.com/users/mlocati/orgs\",\"repos_url\":\"https://api.github.com/users/mlocati/repos\",\"events_url\":\"https://api.github.com/users/mlocati/events{/privacy}\",\"received_events_url\":\"https://api.github.com/users/mlocati/received_events\",\"type\":\"User\",\"site_admin\":false},\"prerelease\":false,\"created_at\":\"2015-10-01T13:51:46Z\",\"published_at\":\"2015-10-01T13:54:23Z\",\"assets\":[{\"url\":\"https://api.github.com/repos/mlocati/MediaData/releases/assets/908162\",\"id\":908162,\"name\":\"MediaData-setup.exe\",\"label\":null,\"uploader\":{\"login\":\"mlocati\",\"id\":928116,\"avatar_url\":\"https://avatars.githubusercontent.com/u/928116?v=3\",\"gravatar_id\":\"\",\"url\":\"https://api.github.com/users/mlocati\",\"html_url\":\"https://github.com/mlocati\",\"followers_url\":\"https://api.github.com/users/mlocati/followers\",\"following_url\":\"https://api.github.com/users/mlocati/following{/other_user}\",\"gists_url\":\"https://api.github.com/users/mlocati/gists{/gist_id}\",\"starred_url\":\"https://api.github.com/users/mlocati/starred{/owner}{/repo}\",\"subscriptions_url\":\"https://api.github.com/users/mlocati/subscriptions\",\"organizations_url\":\"https://api.github.com/users/mlocati/orgs\",\"repos_url\":\"https://api.github.com/users/mlocati/repos\",\"events_url\":\"https://api.github.com/users/mlocati/events{/privacy}\",\"received_events_url\":\"https://api.github.com/users/mlocati/received_events\",\"type\":\"User\",\"site_admin\":false},\"content_type\":\"application/x-msdownload\",\"state\":\"uploaded\",\"size\":15242624,\"download_count\":1,\"created_at\":\"2015-10-01T13:53:50Z\",\"updated_at\":\"2015-10-01T13:54:08Z\",\"browser_download_url\":\"https://github.com/mlocati/MediaData/releases/download/1.1.3/MediaData-setup.exe\"}],\"tarball_url\":\"https://api.github.com/repos/mlocati/MediaData/tarball/1.1.3\",\"zipball_url\":\"https://api.github.com/repos/mlocati/MediaData/zipball/1.1.3\",\"body\":\"- Fix normalization to MP4\\r\\n  Before the process failed because the codec used is not included in the ffmpeg version that comes with MediaData.\"},{\"url\":\"https://api.github.com/repos/mlocati/MediaData/releases/1896121\",\"assets_url\":\"https://api.github.com/repos/mlocati/MediaData/releases/1896121/assets\",\"upload_url\":\"https://uploads.github.com/repos/mlocati/MediaData/releases/1896121/assets{?name,label}\",\"html_url\":\"https://github.com/mlocati/MediaData/releases/tag/1.1.2\",\"id\":1896121,\"tag_name\":\"1.1.2\",\"target_commitish\":\"master\",\"name\":\"v1.1.2\",\"draft\":false,\"author\":{\"login\":\"mlocati\",\"id\":928116,\"avatar_url\":\"https://avatars.githubusercontent.com/u/928116?v=3\",\"gravatar_id\":\"\",\"url\":\"https://api.github.com/users/mlocati\",\"html_url\":\"https://github.com/mlocati\",\"followers_url\":\"https://api.github.com/users/mlocati/followers\",\"following_url\":\"https://api.github.com/users/mlocati/following{/other_user}\",\"gists_url\":\"https://api.github.com/users/mlocati/gists{/gist_id}\",\"starred_url\":\"https://api.github.com/users/mlocati/starred{/owner}{/repo}\",\"subscriptions_url\":\"https://api.github.com/users/mlocati/subscriptions\",\"organizations_url\":\"https://api.github.com/users/mlocati/orgs\",\"repos_url\":\"https://api.github.com/users/mlocati/repos\",\"events_url\":\"https://api.github.com/users/mlocati/events{/privacy}\",\"received_events_url\":\"https://api.github.com/users/mlocati/received_events\",\"type\":\"User\",\"site_admin\":false},\"prerelease\":false,\"created_at\":\"2015-10-01T12:25:30Z\",\"published_at\":\"2015-10-01T12:28:29Z\",\"assets\":[{\"url\":\"https://api.github.com/repos/mlocati/MediaData/releases/assets/907973\",\"id\":907973,\"name\":\"MediaData-setup.exe\",\"label\":null,\"uploader\":{\"login\":\"mlocati\",\"id\":928116,\"avatar_url\":\"https://avatars.githubusercontent.com/u/928116?v=3\",\"gravatar_id\":\"\",\"url\":\"https://api.github.com/users/mlocati\",\"html_url\":\"https://github.com/mlocati\",\"followers_url\":\"https://api.github.com/users/mlocati/followers\",\"following_url\":\"https://api.github.com/users/mlocati/following{/other_user}\",\"gists_url\":\"https://api.github.com/users/mlocati/gists{/gist_id}\",\"starred_url\":\"https://api.github.com/users/mlocati/starred{/owner}{/repo}\",\"subscriptions_url\":\"https://api.github.com/users/mlocati/subscriptions\",\"organizations_url\":\"https://api.github.com/users/mlocati/orgs\",\"repos_url\":\"https://api.github.com/users/mlocati/repos\",\"events_url\":\"https://api.github.com/users/mlocati/events{/privacy}\",\"received_events_url\":\"https://api.github.com/users/mlocati/received_events\",\"type\":\"User\",\"site_admin\":false},\"content_type\":\"application/x-msdownload\",\"state\":\"uploaded\",\"size\":15244320,\"download_count\":5,\"created_at\":\"2015-10-01T12:28:10Z\",\"updated_at\":\"2015-10-01T12:28:27Z\",\"browser_download_url\":\"https://github.com/mlocati/MediaData/releases/download/1.1.2/MediaData-setup.exe\"}],\"tarball_url\":\"https://api.github.com/repos/mlocati/MediaData/tarball/1.1.2\",\"zipball_url\":\"https://api.github.com/repos/mlocati/MediaData/zipball/1.1.2\",\"body\":\"Digitally sign main program file, installer and uninstaller\"},{\"url\":\"https://api.github.com/repos/mlocati/MediaData/releases/1883965\",\"assets_url\":\"https://api.github.com/repos/mlocati/MediaData/releases/1883965/assets\",\"upload_url\":\"https://uploads.github.com/repos/mlocati/MediaData/releases/1883965/assets{?name,label}\",\"html_url\":\"https://github.com/mlocati/MediaData/releases/tag/1.1.1\",\"id\":1883965,\"tag_name\":\"1.1.1\",\"target_commitish\":\"master\",\"name\":\"v1.1.1\",\"draft\":false,\"author\":{\"login\":\"mlocati\",\"id\":928116,\"avatar_url\":\"https://avatars.githubusercontent.com/u/928116?v=3\",\"gravatar_id\":\"\",\"url\":\"https://api.github.com/users/mlocati\",\"html_url\":\"https://github.com/mlocati\",\"followers_url\":\"https://api.github.com/users/mlocati/followers\",\"following_url\":\"https://api.github.com/users/mlocati/following{/other_user}\",\"gists_url\":\"https://api.github.com/users/mlocati/gists{/gist_id}\",\"starred_url\":\"https://api.github.com/users/mlocati/starred{/owner}{/repo}\",\"subscriptions_url\":\"https://api.github.com/users/mlocati/subscriptions\",\"organizations_url\":\"https://api.github.com/users/mlocati/orgs\",\"repos_url\":\"https://api.github.com/users/mlocati/repos\",\"events_url\":\"https://api.github.com/users/mlocati/events{/privacy}\",\"received_events_url\":\"https://api.github.com/users/mlocati/received_events\",\"type\":\"User\",\"site_admin\":false},\"prerelease\":false,\"created_at\":\"2015-09-29T14:44:06Z\",\"published_at\":\"2015-09-29T14:46:24Z\",\"assets\":[{\"url\":\"https://api.github.com/repos/mlocati/MediaData/releases/assets/902305\",\"id\":902305,\"name\":\"MediaData-setup.exe\",\"label\":null,\"uploader\":{\"login\":\"mlocati\",\"id\":928116,\"avatar_url\":\"https://avatars.githubusercontent.com/u/928116?v=3\",\"gravatar_id\":\"\",\"url\":\"https://api.github.com/users/mlocati\",\"html_url\":\"https://github.com/mlocati\",\"followers_url\":\"https://api.github.com/users/mlocati/followers\",\"following_url\":\"https://api.github.com/users/mlocati/following{/other_user}\",\"gists_url\":\"https://api.github.com/users/mlocati/gists{/gist_id}\",\"starred_url\":\"https://api.github.com/users/mlocati/starred{/owner}{/repo}\",\"subscriptions_url\":\"https://api.github.com/users/mlocati/subscriptions\",\"organizations_url\":\"https://api.github.com/users/mlocati/orgs\",\"repos_url\":\"https://api.github.com/users/mlocati/repos\",\"events_url\":\"https://api.github.com/users/mlocati/events{/privacy}\",\"received_events_url\":\"https://api.github.com/users/mlocati/received_events\",\"type\":\"User\",\"site_admin\":false},\"content_type\":\"application/x-msdownload\",\"state\":\"uploaded\",\"size\":15229133,\"download_count\":4,\"created_at\":\"2015-09-29T14:46:05Z\",\"updated_at\":\"2015-09-29T14:46:22Z\",\"browser_download_url\":\"https://github.com/mlocati/MediaData/releases/download/1.1.1/MediaData-setup.exe\"}],\"tarball_url\":\"https://api.github.com/repos/mlocati/MediaData/tarball/1.1.1\",\"zipball_url\":\"https://api.github.com/repos/mlocati/MediaData/zipball/1.1.1\",\"body\":\"- Allow view/edit AM/PM with 12-hour times\\r\\n- Fix processing all files when updating metadata times in batch operation\\r\\n- Some better error messages\"},{\"url\":\"https://api.github.com/repos/mlocati/MediaData/releases/1882049\",\"assets_url\":\"https://api.github.com/repos/mlocati/MediaData/releases/1882049/assets\",\"upload_url\":\"https://uploads.github.com/repos/mlocati/MediaData/releases/1882049/assets{?name,label}\",\"html_url\":\"https://github.com/mlocati/MediaData/releases/tag/1.1.0\",\"id\":1882049,\"tag_name\":\"1.1.0\",\"target_commitish\":\"master\",\"name\":\"v1.1.0\",\"draft\":false,\"author\":{\"login\":\"mlocati\",\"id\":928116,\"avatar_url\":\"https://avatars.githubusercontent.com/u/928116?v=3\",\"gravatar_id\":\"\",\"url\":\"https://api.github.com/users/mlocati\",\"html_url\":\"https://github.com/mlocati\",\"followers_url\":\"https://api.github.com/users/mlocati/followers\",\"following_url\":\"https://api.github.com/users/mlocati/following{/other_user}\",\"gists_url\":\"https://api.github.com/users/mlocati/gists{/gist_id}\",\"starred_url\":\"https://api.github.com/users/mlocati/starred{/owner}{/repo}\",\"subscriptions_url\":\"https://api.github.com/users/mlocati/subscriptions\",\"organizations_url\":\"https://api.github.com/users/mlocati/orgs\",\"repos_url\":\"https://api.github.com/users/mlocati/repos\",\"events_url\":\"https://api.github.com/users/mlocati/events{/privacy}\",\"received_events_url\":\"https://api.github.com/users/mlocati/received_events\",\"type\":\"User\",\"site_admin\":false},\"prerelease\":false,\"created_at\":\"2015-09-29T09:03:40Z\",\"published_at\":\"2015-09-29T09:15:40Z\",\"assets\":[{\"url\":\"https://api.github.com/repos/mlocati/MediaData/releases/assets/901595\",\"id\":901595,\"name\":\"MediaData-setup.exe\",\"label\":null,\"uploader\":{\"login\":\"mlocati\",\"id\":928116,\"avatar_url\":\"https://avatars.githubusercontent.com/u/928116?v=3\",\"gravatar_id\":\"\",\"url\":\"https://api.github.com/users/mlocati\",\"html_url\":\"https://github.com/mlocati\",\"followers_url\":\"https://api.github.com/users/mlocati/followers\",\"following_url\":\"https://api.github.com/users/mlocati/following{/other_user}\",\"gists_url\":\"https://api.github.com/users/mlocati/gists{/gist_id}\",\"starred_url\":\"https://api.github.com/users/mlocati/starred{/owner}{/repo}\",\"subscriptions_url\":\"https://api.github.com/users/mlocati/subscriptions\",\"organizations_url\":\"https://api.github.com/users/mlocati/orgs\",\"repos_url\":\"https://api.github.com/users/mlocati/repos\",\"events_url\":\"https://api.github.com/users/mlocati/events{/privacy}\",\"received_events_url\":\"https://api.github.com/users/mlocati/received_events\",\"type\":\"User\",\"site_admin\":false},\"content_type\":\"application/x-msdownload\",\"state\":\"uploaded\",\"size\":15229854,\"download_count\":1,\"created_at\":\"2015-09-29T09:09:27Z\",\"updated_at\":\"2015-09-29T09:09:42Z\",\"browser_download_url\":\"https://github.com/mlocati/MediaData/releases/download/1.1.0/MediaData-setup.exe\"}],\"tarball_url\":\"https://api.github.com/repos/mlocati/MediaData/tarball/1.1.0\",\"zipball_url\":\"https://api.github.com/repos/mlocati/MediaData/zipball/1.1.0\",\"body\":\"- Fix saving GPS position\\r\\n- More date/time formats supported in Exif metatags\\r\\n- Batch rename media files\\r\\n- Add/substract time to many media files at once\\r\\n- File list is now sortable\\r\\n- Add about dialog\\r\\n- Add function to check for program updates\\r\\n- Updated FFmpeg to 2015-09-28 git-1d0487f\"},{\"url\":\"https://api.github.com/repos/mlocati/MediaData/releases/1864708\",\"assets_url\":\"https://api.github.com/repos/mlocati/MediaData/releases/1864708/assets\",\"upload_url\":\"https://uploads.github.com/repos/mlocati/MediaData/releases/1864708/assets{?name,label}\",\"html_url\":\"https://github.com/mlocati/MediaData/releases/tag/1.0.0\",\"id\":1864708,\"tag_name\":\"1.0.0\",\"target_commitish\":\"master\",\"name\":\"v1.0.0\",\"draft\":false,\"author\":{\"login\":\"mlocati\",\"id\":928116,\"avatar_url\":\"https://avatars.githubusercontent.com/u/928116?v=3\",\"gravatar_id\":\"\",\"url\":\"https://api.github.com/users/mlocati\",\"html_url\":\"https://github.com/mlocati\",\"followers_url\":\"https://api.github.com/users/mlocati/followers\",\"following_url\":\"https://api.github.com/users/mlocati/following{/other_user}\",\"gists_url\":\"https://api.github.com/users/mlocati/gists{/gist_id}\",\"starred_url\":\"https://api.github.com/users/mlocati/starred{/owner}{/repo}\",\"subscriptions_url\":\"https://api.github.com/users/mlocati/subscriptions\",\"organizations_url\":\"https://api.github.com/users/mlocati/orgs\",\"repos_url\":\"https://api.github.com/users/mlocati/repos\",\"events_url\":\"https://api.github.com/users/mlocati/events{/privacy}\",\"received_events_url\":\"https://api.github.com/users/mlocati/received_events\",\"type\":\"User\",\"site_admin\":false},\"prerelease\":false,\"created_at\":\"2015-09-24T20:02:48Z\",\"published_at\":\"2015-09-24T21:58:24Z\",\"assets\":[{\"url\":\"https://api.github.com/repos/mlocati/MediaData/releases/assets/891686\",\"id\":891686,\"name\":\"MediaData-setup.exe\",\"label\":null,\"uploader\":{\"login\":\"mlocati\",\"id\":928116,\"avatar_url\":\"https://avatars.githubusercontent.com/u/928116?v=3\",\"gravatar_id\":\"\",\"url\":\"https://api.github.com/users/mlocati\",\"html_url\":\"https://github.com/mlocati\",\"followers_url\":\"https://api.github.com/users/mlocati/followers\",\"following_url\":\"https://api.github.com/users/mlocati/following{/other_user}\",\"gists_url\":\"https://api.github.com/users/mlocati/gists{/gist_id}\",\"starred_url\":\"https://api.github.com/users/mlocati/starred{/owner}{/repo}\",\"subscriptions_url\":\"https://api.github.com/users/mlocati/subscriptions\",\"organizations_url\":\"https://api.github.com/users/mlocati/orgs\",\"repos_url\":\"https://api.github.com/users/mlocati/repos\",\"events_url\":\"https://api.github.com/users/mlocati/events{/privacy}\",\"received_events_url\":\"https://api.github.com/users/mlocati/received_events\",\"type\":\"User\",\"site_admin\":false},\"content_type\":\"application/x-msdownload\",\"state\":\"uploaded\",\"size\":16581080,\"download_count\":1,\"created_at\":\"2015-09-24T21:55:36Z\",\"updated_at\":\"2015-09-24T21:58:20Z\",\"browser_download_url\":\"https://github.com/mlocati/MediaData/releases/download/1.0.0/MediaData-setup.exe\"}],\"tarball_url\":\"https://api.github.com/repos/mlocati/MediaData/tarball/1.0.0\",\"zipball_url\":\"https://api.github.com/repos/mlocati/MediaData/zipball/1.0.0\",\"body\":\"First public release\"}]";
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
    }
}
