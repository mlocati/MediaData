using System;
using System.Windows.Forms;

namespace MLocati.MediaData
{
    public partial class ctxDateTime : UserControl
    {

        public DateTime SelectedValue
        {
            get
            {
                return new DateTime(
                    this.dtpDate.Value.Year,
                    this.dtpDate.Value.Month,
                    this.dtpDate.Value.Day,
                    this.dtpTime.Value.Hour,
                    this.dtpTime.Value.Minute,
                    this.dtpTime.Value.Second
                );
            }
            set
            {
                this.dtpDate.Value = value;
                this.dtpTime.Value = value;
            }
        }

        public ctxDateTime()
        {
            InitializeComponent();
            this.SelectedValue = DateTime.Now;
        }

    }
}
