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

        #region Instance properties

        private int _lastProcessedIndex;

        private List<BatchOperation> _operators;

        private bool _someChanged;
        public bool SomeChanged
        {
            get
            { return this._someChanged; }
        }

        #endregion


        #region Constructors

        public frmBatchDeltaTimestamp(List<BatchOperation> operators)
        {
            InitializeComponent();
            this.Icon = Program.Icon;
            this._someChanged = false;
            this._operators = operators;
            this.dgvProcessing.AutoGenerateColumns = false;
            this.dgvProcessing.DataSource = this._operators;
            this._lastProcessedIndex = -1;
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
            if (this._lastProcessedIndex < this._operators.Count)
            {
                this.dgvProcessing.Rows[this._lastProcessedIndex].Selected = true;
                this._operators[this._lastProcessedIndex].Process(this);
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
