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
        private List<Processor> _processors;
        public frmBatchRename(List<Processor> processors)
        {
            InitializeComponent();
            this.Icon = Program.Icon;
            this._processors = processors;
        }
    }
}
