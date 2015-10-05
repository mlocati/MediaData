using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MLocati.MediaData
{
    public partial class frmPickDateTimeWithAlternatives : Form
    {

        #region Instance properties

        private DateTime? _initialValue;

        private DateTime? _result;
        public DateTime? Result
        {
            get
            {
                return this._result;
            }
        }

        #endregion


        #region Constructors

        public frmPickDateTimeWithAlternatives(List<MediaInfo.NameTimestamp> alternatives)
            : this(alternatives, null)
        { }
        public frmPickDateTimeWithAlternatives(List<MediaInfo.NameTimestamp> alternatives, DateTime? initialValue)
        {
            InitializeComponent();
            this.Icon = Program.Icon;
            this._initialValue = initialValue;
            this._result = null;
            this.dgvAlternatives.AutoGenerateColumns = false;
            this.dgvAlternatives.DataSource = alternatives;
            this.dtpCustom.SelectedValue = initialValue.HasValue ? initialValue.Value : DateTime.Now;
            this.UpdateState();
        }

        #endregion


        #region GUI events

        private void radUseAlternatives_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateState();

        }

        private void radUseCustom_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateState();
        }

        private void radUseNull_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateState();
        }

        private void dgvAlternatives_SelectionChanged(object sender, EventArgs e)
        {
            this.UpdateState();
        }

        private void frmPickDateTimeWithAlternatives_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.dgvAlternatives.Bounds.Contains(e.Location))
            {
                this.radUseAlternatives.Checked = true;
                this.dgvAlternatives.Focus();
            }
            else if (this.dtpCustom.Bounds.Contains(e.Location))
            {
                this.radUseCustom.Checked = true;
                this.dtpCustom.Focus();
            }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            bool ok = false;
            if (this.radUseAlternatives.Checked)
            {
                if (this.dgvAlternatives.SelectedRows.Count == 1)
                {
                    MediaInfo.NameTimestamp alt = this.dgvAlternatives.SelectedRows[0].DataBoundItem as MediaInfo.NameTimestamp;
                    if (alt != null)
                    {
                        this._result = alt.Timestamp;
                        ok = true;
                    }
                }
            }
            else if (this.radUseCustom.Checked)
            {
                this._result = this.dtpCustom.SelectedValue;
                ok = true;
            }
            else if (this.radUseNull.Checked)
            {
                if (!this._initialValue.HasValue || MessageBox.Show(i18n.Confirm_remove_timestamp, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    this._result = null;
                    ok = true;
                }
            }
            if (ok)
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        #endregion


        #region Instance methods

        private void UpdateState()
        {
            this.dgvAlternatives.Enabled = this.radUseAlternatives.Checked;
            this.dgvAlternatives.DefaultCellStyle.SelectionBackColor = this.dgvAlternatives.Enabled ? SystemColors.Highlight : this.dgvAlternatives.DefaultCellStyle.BackColor;
            this.dgvAlternatives.DefaultCellStyle.SelectionForeColor = this.dgvAlternatives.Enabled ? SystemColors.HighlightText : SystemColors.ControlText;
            this.dtpCustom.Enabled = this.radUseCustom.Checked;
            if (this.radUseAlternatives.Checked)
            {
                this.btnAccept.Enabled = this.dgvAlternatives.SelectedRows.Count == 1;
            }
            else if (this.radUseCustom.Checked || this.radUseNull.Checked)
            {
                this.btnAccept.Enabled = true;
            }
            else
            {
                this.btnAccept.Enabled = false;
            }
        }

        #endregion

    }
}
