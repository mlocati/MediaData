using System;
using System.Windows.Forms;

namespace MLocati.MediaData
{
    public partial class frmPickDateTime : Form
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

        public frmPickDateTime()
            : this(null)
        { }
        public frmPickDateTime(DateTime? initialValue)
        {
            InitializeComponent();
            this.Icon = Program.Icon;
            this._result = null;
            this._initialValue = initialValue;
            DateTime dt = initialValue.HasValue ? initialValue.Value : DateTime.Now;
            this.dtpDate.Value = dt;
            this.dtpTime.Value = dt;
        }

        #endregion


        #region GUI events

        private void btnNone_Click(object sender, EventArgs e)
        {
            if (!this._initialValue.HasValue || MessageBox.Show(i18n.Confirm_remove_timestamp, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                this._result = null;
                this.DialogResult = DialogResult.OK;
            }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            this._result = new DateTime(
                this.dtpDate.Value.Year,
                this.dtpDate.Value.Month,
                this.dtpDate.Value.Day,
                this.dtpTime.Value.Hour,
                this.dtpTime.Value.Minute,
                this.dtpTime.Value.Second
            );
            this.DialogResult = DialogResult.OK;
        }

        #endregion

    }
}
