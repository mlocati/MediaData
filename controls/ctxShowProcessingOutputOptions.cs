using System;
using System.Text;
using System.Windows.Forms;

namespace MLocati.MediaData
{
    public partial class ctxShowProcessingOutputOptions : UserControl
    {

        #region Instance properties

        public string SerializedValue
        {
            get
            {
                StringBuilder r = new StringBuilder();
                if (this.rbAlways.Checked)
                {
                    r.Append("-1");
                }
                else if (this.rbBigger.Checked)
                {
                    r.Append(Convert.ToUInt64(Math.Round(this.nudBigger.Value)));
                    r.Append(this.cbxBigger.SelectedItem);
                }
                if (this.chkAutoclose.Checked)
                {
                    r.Append('a');
                }
                return r.ToString();
            }
        }

        #endregion


        #region Constructors

        public ctxShowProcessingOutputOptions()
        {
            InitializeComponent();
        }

        #endregion


        #region Public methods

        public void Setup(string name, ShowProcessingOutput spo, ulong defaultBiggerThanSize, string defaultBiggerThanUnit)
        {
            this.gbxContainer.Text = name;
            this.nudBigger.Value = (spo.Shown == ShowProcessingOutput.When.BiggerThan) ? spo.BiggerThanThisShownSize : defaultBiggerThanSize;
            this.cbxBigger.SelectedItem = (spo.Shown == ShowProcessingOutput.When.BiggerThan) ? spo.BiggerThanThisShownUnit : defaultBiggerThanUnit;
            this.chkAutoclose.Checked = spo.AutocloseOnSuccess;
            switch (spo.Shown)
            {
                case ShowProcessingOutput.When.Always:
                    this.rbAlways.Checked = true;
                    break;
                case ShowProcessingOutput.When.BiggerThan:
                    this.rbBigger.Checked = true;
                    break;
                case ShowProcessingOutput.When.Never:
                default:
                    this.rbNever.Checked = true;
                    break;
            }
        }

        #endregion


        #region GUI events

        private void nudBigger_ValueChanged(object sender, EventArgs e)
        {
            this.rbBigger.Checked = true;
        }

        private void cbxBigger_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.rbBigger.Checked = true;
        }

        #endregion

    }
}
