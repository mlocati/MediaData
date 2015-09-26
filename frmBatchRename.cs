using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
            public string OriginalFilename
            {
                get
                {
                    return this.Processor.Filename;
                }
            }
            public ProcessorNamer(Processor processor)
            {
                this.Processor = processor;
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
            formats.Add(i18n.BatchFormat_Keep);
            if (!string.IsNullOrEmpty(MediaData.Properties.Settings.Default.BatchRenameFormats))
            {
                foreach (string s in MediaData.Properties.Settings.Default.BatchRenameFormats.Split('|'))
                {
                    string brf = s.Trim();
                    if (brf.Length > 0)
                    {
                        formats.Add(brf);
                    }
                }
            }
            this.cbxFormat.Items.AddRange(formats.ToArray());
            this.cbxFormat.SelectedIndex = 0;

            this._processorNamers = new List<ProcessorNamer>(processors.Count);
            foreach (Processor processor in processors)
            {
                this._processorNamers.Add(new ProcessorNamer(processor));
            }
            this.dgvRename.AutoGenerateColumns = false;
            this.dgvRename.DataSource = this._processorNamers;
            this._processorNamersManager = (CurrencyManager)this.dgvRename.BindingContext[this._processorNamers];
            UI.DescribedEnumToCombobox(typeof(ExtensionFormats), this.cbxExtension, 0);
        }

        #endregion

    }
}
