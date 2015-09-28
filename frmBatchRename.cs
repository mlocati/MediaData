using System;
using System.Collections.Generic;
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
            private string _newFilename;
            public string OriginalFilename
            {
                get
                {
                    return this.Processor.Filename;
                }
            }
            public string NewFilename
            {
                get
                {
                    return (this._error.Length == 0) ? this._newFilename : "";
                }
            }
            public string NewFilenameShown
            {
                get
                {
                    return (this._error.Length > 0) ? this._error : this.NewFilename;
                }
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
                    this._error = "i18n.No format specified";
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
                            this._error = "i18n.No date/time in metadata";
                        }
                    }
                    if (this._error.Length == 0)
                    {
                        char[] badChars = Path.GetInvalidFileNameChars();
                        int iBadChar = newFilename.IndexOfAny(badChars);
                        if (iBadChar >= 0)
                        {
                            this._error = string.Format("i18n.Invalid char in file name: '{0}'", badChars[Array.IndexOf(badChars, newFilename[iBadChar])]);
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

        #endregion


        #region Instance properties

        private List<ProcessorNamer> _processorNamers;

        private CurrencyManager _processorNamersManager;

        #endregion


        #region Constructors

        public frmBatchRename(List<Processor> processors)
        {
            InitializeComponent();
            this.Icon = Program.Icon;

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

        #endregion


        #region Instance methods

        private void UpdateFormat()
        {
            if (this._processorNamers == null)
            {
                return;
            }
            ExtensionFormats extensionFormat = (ExtensionFormats)UI.GetDescribedEnumValueOfCombobox(this.cbxExtension);
            foreach (ProcessorNamer pn in this._processorNamers)
            {
                switch (this.cbxFormat.SelectedIndex)
                {
                    case 0:
                        if (pn.Processor.FilenameTimeStamper == null)
                        {
                            pn.SetError("FilenameTimeStamper is null");
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
            }
            this.dgvRename.Invalidate();
        }

        #endregion

    }
}
