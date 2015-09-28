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
    public partial class frmBatchDeltaTimestamp : Form
    {

        #region Types

        private class ProcessorTimestamper
        {
            public enum States
            {
                Failed,
                Idle,
                Processing,
                Updated,
            }
            private readonly Processor _processor;
            public string Filename
            {
                get
                {
                    return this._processor.Filename;
                }
            }
            public string State
            {
                get
                {
                    switch(this._state)
                    {
                        case States.Idle:
                            return string.Format(i18n.Going_to_set_metadata_timestamp_to_X, this._newDateTime.Value.ToString());
                        case States.Processing:
                            return i18n.Processing;
                        case States.Updated:
                            return string.Format(i18n.Metadata_timestamp_set_to_X, this._newDateTime.Value.ToString());
                        default:
                        case States.Failed:
                            return this._error;
                    }
                }
            }
            private string _error;
            private States _state;
            private DateTime? _newDateTime;
            public ProcessorTimestamper(Processor processor, TimeSpan timespan)
            {
                this._processor = processor;
                if (processor.Info == null || !processor.Info.TimestampMean.HasValue)
                {
                    this._error = i18n.No_timestamp_in_metadata;
                    this._newDateTime = null;
                    this._state = States.Failed;
                }
                else
                {
                    this._error = "";
                    this._newDateTime = processor.Info.TimestampMean.Value.Add(timespan);
                    this._state = States.Idle;
                }
            }
            public void Process(IWin32Window parentWindow)
            {
                if (this._state == States.Idle)
                {
                    try
                    {
                        this._processor.ChangeMetadataTimestamp(this._newDateTime.Value, parentWindow);
                        this._state = States.Updated;
                    }
                    catch (Exception x)
                    {
                        this._state = States.Failed;
                        this._error = x.Message;
                    }
                }
            }
        }

        #endregion


        #region Instance properties

        private int _lastProcessedIndex;

        private List<ProcessorTimestamper> _processors;

        private bool _someChanged;
        public bool SomeChanged
        {
            get
            { return this._someChanged; }
        }

        #endregion


        #region Constructors

        public frmBatchDeltaTimestamp(List<Processor> processors, TimeSpan deltaTimestamp)
        {
            InitializeComponent();
            this.Icon = Program.Icon;
            this._someChanged = false;
            this._processors = new List<ProcessorTimestamper>(processors.Count);
            foreach (Processor processor in processors)
            {
                this._processors.Add(new ProcessorTimestamper(processor, deltaTimestamp));
            }
            this.dgvProcessing.AutoGenerateColumns = false;
            this.dgvProcessing.DataSource = this._processors;
        }

        #endregion


        #region GUI events

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.btnStart.Enabled = false;
            this.Refresh();
            this.ProcessNext();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmBatchDeltaTimestamp_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.btnClose.Enabled)
            {
                e.Cancel = true;
            }
        }

        #endregion


        #region Instance methods

        private void ProcessNext()
        {
            this._lastProcessedIndex++;
            if (this._lastProcessedIndex < this._processors.Count)
            {
                this.dgvProcessing.Rows[this._lastProcessedIndex].Selected = true;
                this._processors[this._lastProcessedIndex].Process(this);
                this.dgvProcessing.InvalidateRow(this._lastProcessedIndex);
                this.Refresh();
                this.ProcessNext();
                this._someChanged = true;
            }
            else
            {
                this.btnClose.Enabled = true;
                this.CancelButton = this.AcceptButton = this.btnClose;
            }
        }

        #endregion

    }
}
