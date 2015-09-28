using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MLocati.MediaData
{
    public partial class frmBatchRename : Form
    {

        #region Types

        private enum ExtensionFormats
        {
            [Localizer.Description("ExtensionFormats_KeepCase")]
            KeepCase,
            [Localizer.Description("ExtensionFormats_LowerCase")]
            LowerCase,
            [Localizer.Description("ExtensionFormats_UpperCase")]
            UpperCase,
        }

        private class ProcessorNamer
        {
            public readonly Processor Processor;
            private string _format;
            private ExtensionFormats _extensionFormat;
            private string _error;
            public string Error
            {
                get
                { return this._error; }
            }
            private string _newFilename;
            public string OriginalFilename
            {
                get
                {
                    return this.Processor.Filename;
                }
            }
            public string OriginalFullFilename
            {
                get
                {
                    return this.Processor.FullFilename;
                }
            }
            public string NewFilename
            {
                get
                {
                    return (this._error.Length == 0) ? this._newFilename : "";
                }
            }
            public string NewFullFilename
            {
                get
                {
                    return (this._error.Length == 0 && this._newFilename.Length > 0) ? Path.Combine(Path.GetDirectoryName(this.Processor.FullFilename), this._newFilename) : "";
                }
            }
            public string NewFilenameShown
            {
                get
                {
                    return (this._error.Length > 0) ? this._error : this.NewFilename;
                }
            }
            public bool HasError
            {
                get
                { return this._error.Length > 0; }
            }
            public ProcessorNamer(Processor processor)
            {
                this.Processor = processor;
                this._extensionFormat = ExtensionFormats.KeepCase;
                this._format = null;
                this._newFilename = "";
                this._error = i18n.Format_not_set;
            }
            public void SetFormat(string format, ExtensionFormats extensionFormat)
            {
                if (format == null)
                {
                    format = "";
                }
                if ((this._format != null) && string.Compare(format, this._format, false) == 0 && this._extensionFormat == extensionFormat)
                {
                    return;
                }
                this._error = "";
                this._extensionFormat = extensionFormat;
                this._format = format;
                this._newFilename = "";
                if (format.Length == 0)
                {
                    this._error = i18n.No_format_specified;
                }
                else
                {
                    string newFilename = format;
                    if (newFilename.IndexOf('<') >= 0 && newFilename.IndexOf('>') > newFilename.IndexOf('<'))
                    {
                        DateTime? timestamp = (this.Processor.Info == null) ? null : (DateTime?)this.Processor.Info.TimestampMean;
                        if (timestamp.HasValue)
                        {
                            newFilename = newFilename
                                .Replace("<Y>", timestamp.Value.Year.ToString("D4"))
                                .Replace("<M>", timestamp.Value.Month.ToString("D2"))
                                .Replace("<D>", timestamp.Value.Day.ToString("D2"))
                                .Replace("<h>", timestamp.Value.Hour.ToString("D2"))
                                .Replace("<m>", timestamp.Value.Minute.ToString("D2"))
                                .Replace("<s>", timestamp.Value.Second.ToString("D2"))
                            ;
                        }
                        else if (newFilename.Contains("<Y>") || newFilename.Contains("<M>") || newFilename.Contains("<D>") || newFilename.Contains("<h>") || newFilename.Contains("<m>") || newFilename.Contains("<s>"))
                        {
                            this._error = i18n.No_timestamp_in_metadata;
                        }
                    }
                    if (this._error.Length == 0)
                    {
                        char[] badChars = Path.GetInvalidFileNameChars();
                        int iBadChar = newFilename.IndexOfAny(badChars);
                        if (iBadChar >= 0)
                        {
                            this._error = string.Format(i18n.Invalid_char_in_file_name_X, badChars[Array.IndexOf(badChars, newFilename[iBadChar])]);
                        }
                    }
                    if (this._error.Length == 0)
                    {
                        string extension = Path.GetExtension(this.Processor.Filename);
                        switch (this._extensionFormat)
                        {
                            case ExtensionFormats.LowerCase:
                                extension = extension.ToLowerInvariant();
                                break;
                            case ExtensionFormats.UpperCase:
                                extension = extension.ToUpperInvariant();
                                break;
                        }
                        this._newFilename = newFilename + extension;
                    }
                }
            }
            public void SetError(string error)
            {
                this._error = error;
                this._newFilename = "";
                this._format = "";
            }
        }

        private class FileNamer
        {
            private List<string[]> _reverts;
            public FileNamer()
            {
                this._reverts = new List<string[]>();
            }
            public void Rename(string oldName, string newName)
            {
                if (string.Compare(oldName, newName, false) != 0)
                {
                    File.Move(oldName, newName);
                    this._reverts.Add(new string[] { oldName, newName });
                }
            }
            public void Revert()
            {
                int maxIndex = this._reverts.Count - 1;
                while (maxIndex >= 0)
                {
                    string[] revert = this._reverts[maxIndex];
                    File.Move(revert[1], revert[0]);
                    this._reverts.RemoveAt(maxIndex);
                    maxIndex--;
                }
            }
        }

        #endregion


        #region Instance properties

        private List<ProcessorNamer> _processorNamers;

        private CurrencyManager _processorNamersManager;

        private int[] _formatTextSelection = null;

        #endregion


        #region Constructors

        public frmBatchRename(List<Processor> processors)
        {
            InitializeComponent();
            this.Icon = Program.Icon;

            Dictionary<string, string> placeholders = new Dictionary<string, string>();
            placeholders.Add("<Y>", i18n.Year);
            placeholders.Add("<M>", i18n.Month);
            placeholders.Add("<D>", i18n.Day);
            placeholders.Add("<h>", i18n.Hours);
            placeholders.Add("<m>", i18n.Minutes);
            placeholders.Add("<s>", i18n.Seconds);
            foreach (KeyValuePair<string, string> ph in placeholders)
            {
                LinkLabel lnk = new LinkLabel();
                lnk.AutoSize = true;
                lnk.Text = string.Format("{0}: {1}", ph.Key, ph.Value);
                lnk.Click += delegate (object sender, EventArgs e)
                {
                    this.cbxFormat.SuspendLayout();
                    switch (this.cbxFormat.SelectedIndex)
                    {
                        case 0:
                            this.cbxFormat.SelectedIndex = -1;
                            this.cbxFormat.Text = ph.Key;
                            this._formatTextSelection = new int[] { this.cbxFormat.Text.Length, 0 };
                            this.cbxFormat.Focus();
                            break;
                        case -1:
                            string pre;
                            string post;
                            if (this._formatTextSelection != null)
                            {
                                pre = this.cbxFormat.Text.Substring(0, this._formatTextSelection[0]);
                                post = this.cbxFormat.Text.Substring(this._formatTextSelection[0] + this._formatTextSelection[1]);
                            }
                            else
                            {
                                pre = this.cbxFormat.Text;
                                post = "";
                            }
                            this.cbxFormat.Text = pre + ph.Key + post;
                            if (this._formatTextSelection != null && this._formatTextSelection[1] > 0)
                            {
                                this._formatTextSelection = new int[] { pre.Length, ph.Key.Length };
                            }
                            else
                            {
                                this._formatTextSelection = new int[] { pre.Length + ph.Key.Length, 0 };
                            }
                            this.cbxFormat.Focus();
                            break;
                        default:
                            string s = this.cbxFormat.Text;
                            this.cbxFormat.SelectedIndex = -1;
                            this.cbxFormat.Text = s + ph.Key;
                            this._formatTextSelection = new int[] { this.cbxFormat.Text.Length, 0 };
                            this.cbxFormat.Focus();
                            break;
                    }
                    this.cbxFormat.ResumeLayout();
                };
                this.flpFormat.Controls.Add(lnk);
            }

            List<string> formats = new List<string>();
            int selectedFormatIndex = 0;
            formats.Add(i18n.BatchRename_Format_Keep);
            if (!string.IsNullOrEmpty(MediaData.Properties.Settings.Default.BatchRenameFormats))
            {
                foreach (string s in MediaData.Properties.Settings.Default.BatchRenameFormats.Split('|'))
                {
                    string brf = s.Trim();
                    if (brf.Length > 0)
                    {
                        if (brf.Equals(MediaData.Properties.Settings.Default.BatchRenameLastFormat, StringComparison.Ordinal))
                        {
                            selectedFormatIndex = formats.Count;
                        }
                        formats.Add(brf);
                    }
                }
            }
            this.cbxFormat.Items.AddRange(formats.ToArray());
            this.cbxFormat.SelectedIndex = selectedFormatIndex;

            this._processorNamers = new List<ProcessorNamer>(processors.Count);
            foreach (Processor processor in processors)
            {
                this._processorNamers.Add(new ProcessorNamer(processor));
            }
            this.dgvRename.AutoGenerateColumns = false;
            this.dgvRename.DataSource = this._processorNamers;
            this._processorNamersManager = (CurrencyManager)this.dgvRename.BindingContext[this._processorNamers];
            UI.DescribedEnumToCombobox(typeof(ExtensionFormats), this.cbxExtension, 0);

            this.UpdateFormat();
        }

        #endregion


        #region GUI events

        private void cbxFormat_Enter(object sender, EventArgs e)
        {
            Timer tmr = new Timer();
            tmr.Enabled = false;
            tmr.Interval = 10;
            tmr.Tick += delegate (object s, EventArgs ea)
            {
                tmr.Stop();
                if (this._formatTextSelection != null)
                {
                    this.cbxFormat.Select(this._formatTextSelection[0], this._formatTextSelection[1]);
                }
                tmr.Dispose();
            };
            tmr.Start();
        }

        private void cbxFormat_KeyUp(object sender, KeyEventArgs e)
        {
            this._formatTextSelection = new int[] { this.cbxFormat.SelectionStart, this.cbxFormat.SelectionLength };
        }

        private void cbxFormat_MouseUp(object sender, MouseEventArgs e)
        {
            this._formatTextSelection = new int[] { this.cbxFormat.SelectionStart, this.cbxFormat.SelectionLength };
        }

        private void cbxFormat_TextChanged(object sender, EventArgs e)
        {
            if (this._processorNamers == null)
            {
                return;
            }
            if (this.cbxFormat.SelectedIndex < 0 && this.cbxFormat.Text.Equals(i18n.BatchRename_Format_Keep))
            {
                this.cbxFormat.SelectedIndex = 0;
                return;
            }
            this.UpdateFormat();
        }

        private void cbxExtension_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateFormat();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                FileNamer fileNamer = new FileNamer();
                try
                {
                    Dictionary<ProcessorNamer, string> tempFileNames = new Dictionary<ProcessorNamer, string>(this._processorNamers.Count);
                    foreach (ProcessorNamer pn in this._processorNamers)
                    {
                        if (pn.HasError)
                        {
                            throw new Exception(pn.Error);
                        }
                        string tempFilename = pn.OriginalFullFilename;
                        for (int i = 0; ; i++)
                        {
                            tempFilename += "-tmp-" + i.ToString();
                            if (!File.Exists(tempFilename))
                            {
                                break;
                            }
                        }
                        tempFileNames.Add(pn, tempFilename);
                        fileNamer.Rename(pn.OriginalFullFilename, tempFilename);
                    }
                    foreach (KeyValuePair<ProcessorNamer, string> tfn in tempFileNames)
                    {
                        fileNamer.Rename(tfn.Value, tfn.Key.NewFullFilename);
                    }
                }
                catch
                {
                    try
                    {
                        fileNamer.Revert();
                    }
                    catch
                    { }
                    throw;
                }
                foreach (ProcessorNamer pn in this._processorNamers)
                {
                    pn.Processor.SetFullFilename(pn.NewFullFilename);
                }
                if (this.cbxFormat.SelectedIndex == 0)
                {
                    MediaData.Properties.Settings.Default.BatchRenameLastFormat = "";
                }
                else
                {
                    List<string> rememberFormat = new List<string>();
                    rememberFormat.Add(this.cbxFormat.Text);
                    if (!string.IsNullOrEmpty(MediaData.Properties.Settings.Default.BatchRenameFormats))
                    {
                        foreach (string s in MediaData.Properties.Settings.Default.BatchRenameFormats.Split('|'))
                        {
                            string s2 = s.Trim();
                            if (s2.Length > 0 && !rememberFormat.Contains(s2))
                            {
                                rememberFormat.Add(s2);
                            }
                        }
                    }
                    MediaData.Properties.Settings.Default.BatchRenameFormats = string.Join("|", rememberFormat);
                    MediaData.Properties.Settings.Default.BatchRenameLastFormat = this.cbxFormat.Text;
                }
                try
                {
                    MediaData.Properties.Settings.Default.Save();
                }
                catch
                { }
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion


        #region Instance methods

        private void UpdateFormat()
        {
            if (this._processorNamers == null)
            {
                return;
            }
            string commonDirectoryNameLC = null;
            Dictionary<string, string> directoryContentsLC = new Dictionary<string, string>();
            ExtensionFormats extensionFormat = (ExtensionFormats)UI.GetDescribedEnumValueOfCombobox(this.cbxExtension);
            foreach (ProcessorNamer pn in this._processorNamers)
            {
                string pnDirectoryNameLC = Path.GetDirectoryName(pn.Processor.FullFilename).ToLower();
                if (commonDirectoryNameLC == null)
                {
                    commonDirectoryNameLC = pnDirectoryNameLC;
                    DirectoryInfo di = new DirectoryInfo(commonDirectoryNameLC);
                    foreach (FileSystemInfo fi in di.GetFileSystemInfos())
                    {
                        directoryContentsLC.Add(fi.Name.ToLower(), fi.Name.ToLower());
                    }
                }
                else if (pnDirectoryNameLC != commonDirectoryNameLC)
                {
                    pn.SetError(i18n.Multiple_directories_found);
                    continue;
                }
                string pnOriginalLC = pn.OriginalFilename.ToLower();
                if (!directoryContentsLC.ContainsKey(pnOriginalLC))
                {
                    pn.SetError(i18n.File_not_in_directory);
                    continue;
                }
                switch (this.cbxFormat.SelectedIndex)
                {
                    case 0:
                        if (pn.Processor.FilenameTimeStamper == null)
                        {
                            pn.SetError("FilenameTimeStamper is null??");
                        }
                        else
                        {
                            pn.SetFormat(pn.Processor.FilenameTimeStamper.Format, extensionFormat);
                        }
                        break;
                    default:
                        pn.SetFormat(this.cbxFormat.Text, extensionFormat);
                        break;
                }
                if (!pn.HasError)
                {
                    string pnNewnameLC = pn.NewFilename;
                    directoryContentsLC[pnOriginalLC] = "";
                    if (directoryContentsLC.ContainsValue(pnNewnameLC))
                    {
                        pn.SetError(i18n.Overlapping_file_names);
                    }
                    directoryContentsLC[pnOriginalLC] = pnNewnameLC;
                }
            }
            foreach (DataGridViewRow row in this.dgvRename.Rows)
            {
                ProcessorNamer pn = (ProcessorNamer)row.DataBoundItem;
                row.DefaultCellStyle.ForeColor = pn.HasError ? Color.Red : SystemColors.WindowText;
            }
            bool someFailed = false;
            foreach (ProcessorNamer pn in this._processorNamers)
            {
                if (pn.HasError)
                {
                    someFailed = true;
                    break;
                }
            }
            this.dgvRename.Invalidate();
            this.btnApply.Enabled = !someFailed;
        }

        #endregion

    }
}
