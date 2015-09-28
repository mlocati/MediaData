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
        private List<Processor> _processors;
        private TimeSpan _deltaTimestamp;
        private bool _someChanged;
        public bool SomeChanged
        {
            get
            { return this._someChanged; }
        }
        public frmBatchDeltaTimestamp(List<Processor> processors, TimeSpan deltaTimestamp)
        {
            InitializeComponent();
            this.Icon = Program.Icon;
            this._processors = processors;
            this._deltaTimestamp = deltaTimestamp;
            this._someChanged = false;
        }
    }
}
