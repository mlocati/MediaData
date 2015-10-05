using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MLocati.MediaData
{
    public partial class frmTool : Form
    {

        #region Types

        private enum RunStates : int
        {
            WarmUp = 0,
            Running = 1,
            Cancelled = 2,
            Failed = 3,
            Succeeded = 4,
        }

        #endregion


        #region Instance properties

        private Tool _tool;

        private bool _autoCloseOnSuccess;

        private RunStates _runState = RunStates.WarmUp;

        private RunStates RunState
        {
            get
            {
                return this._runState;
            }
            set
            {
                this._runState = value;
                List<Button> enabled = new List<Button>();
                Button cancelButton = null;
                Button acceptButton = null;
                switch (value)
                {
                    case RunStates.WarmUp:
                        break;
                    case RunStates.Running:
                        cancelButton = this.btnAbort;
                        enabled.Add(this.btnAbort);
                        break;
                    case RunStates.Cancelled:
                        cancelButton = this.btnClose;
                        acceptButton = this.btnClose;
                        enabled.Add(this.btnClose);
                        break;
                    case RunStates.Failed:
                        cancelButton = this.btnClose;
                        acceptButton = this.btnClose;
                        enabled.Add(this.btnClose);
                        break;
                    case RunStates.Succeeded:
                        cancelButton = this.btnClose;
                        acceptButton = this.btnAccept;
                        enabled.Add(this.btnCancel);
                        enabled.Add(this.btnAccept);
                        break;
                }
                foreach (Button button in new Button[] { this.btnAbort, this.btnClose, this.btnCancel, this.btnAccept })
                {
                    button.Enabled = enabled.Contains(button);
                    this.CancelButton = cancelButton;
                    this.AcceptButton = acceptButton;
                }
            }
        }

        private Font _logFontNormal = null;
        private Font _logFontBold = null;

        #endregion


        #region Constructors

        public frmTool(Tool tool, bool autoCloseOnSuccess)
        {
            InitializeComponent();
            this.Icon = Program.Icon;
            this._tool = tool;
            this._tool.Started += this.ToolStarted;
            this._tool.StdOutReceived += this.ToolStdOutReceived;
            this._tool.StdErrReceived += this.ToolStdErrReceived;
            this._tool.Completed += this.ToolCompleted;
            this.rtbOutput.WordWrap = this.chkWordWrap.Checked = MediaData.Properties.Settings.Default.ToolWordWrap;
            this.RunState = RunStates.WarmUp;
            this._autoCloseOnSuccess = autoCloseOnSuccess;
        }

        #endregion


        #region GUI events

        private void frmTool_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.RunState <= RunStates.Running)
            {
                e.Cancel = true;
            }
        }

        private void chkWordWrap_CheckedChanged(object sender, EventArgs e)
        {
            bool ww = this.chkWordWrap.Checked;
            if (this.rtbOutput.WordWrap != ww)
            {
                this.rtbOutput.WordWrap = ww;
            }
            try
            {
                if (MediaData.Properties.Settings.Default.ToolWordWrap != ww)
                {
                    MediaData.Properties.Settings.Default.ToolWordWrap = ww;
                    MediaData.Properties.Settings.Default.Save();
                }
            }
            catch
            { }
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            this._tool.Cancel();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        #endregion


        #region Instance methods

        private Font GetLogFont(bool bold)
        {
            if (this._logFontNormal == null)
            {
                this._logFontNormal = this.rtbOutput.SelectionFont;
            }
            if (bold && this._logFontBold == null)
            {
                this._logFontBold = new Font(this._logFontNormal, FontStyle.Bold);
            }
            return bold ? this._logFontBold : this._logFontNormal;
        }
        private void AddLogLine(string line)
        {
            this.AddLogLine(line, false, false);
        }
        private void AddLogLine(string line, bool err)
        {
            this.AddLogLine(line, err, false);
        }
        private void AddLogLine(string line, bool err, bool bold)
        {
            this.rtbOutput.SelectionStart = this.rtbOutput.TextLength;
            this.rtbOutput.SelectionLength = 0;
            this.rtbOutput.SelectionColor = err ? Color.Red : Color.Green;
            this.rtbOutput.SelectionFont = this.GetLogFont(bold);
            this.rtbOutput.AppendText(line + "\n");
        }

        public void ToolStarted(object sender)
        {
            this.RunState = RunStates.Running;
        }
        public void ToolStdOutReceived(object sender, Tool.OutputReceivedEventArgs e)
        {
            this.AddLogLine(e.Text);
        }
        public void ToolStdErrReceived(object sender, Tool.OutputReceivedEventArgs e)
        {
            this.AddLogLine(e.Text, true);
        }
        public void ToolCompleted(object sender, Tool.Result e)
        {
            if (e.Error == null)
            {
                this.RunState = RunStates.Succeeded;
                if (this._autoCloseOnSuccess && string.IsNullOrEmpty(e.StdErr))
                {
                    this.DialogResult = DialogResult.OK;
                }
            }
            else
            {
                this.AddLogLine("");
                this.AddLogLine(e.Error.Message, true, true);
                if (e.Cancelled)
                {
                    this.RunState = RunStates.Cancelled;
                }
                else
                {
                    this.RunState = RunStates.Failed;
                }
            }
        }

        #endregion

    }
}
