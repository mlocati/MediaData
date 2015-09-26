using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MLocati.MediaData
{
    public partial class frmMain : Form
    {

        #region Types

        private enum SelectionOperations
        {
            [Localizer.Description("SelectionOperations_PleaseSelect")]
            PleaseSelect,
            [Localizer.Description("SelectionOperations_RenameFiles")]
            RenameFiles,
            [Localizer.Description("SelectionOperations_DeltaTimestamp")]
            DeltaTimestamp,
        }

        private enum DeltaOperationUnit
        {
            [Localizer.Description("DeltaOperationUnit_seconds")]
            Seconds,
            [Localizer.Description("DeltaOperationUnit_minutes")]
            Minutes,
            [Localizer.Description("DeltaOperationUnit_hours")]
            Hours,
            [Localizer.Description("DeltaOperationUnit_days")]
            Days,
        }

        #endregion


        #region Instance Properties

        private List<Processor> _processors;

        private CurrencyManager _processorsManager;

        private Processor SelectedProcessor
        {
            get
            {
                return this._processorsManager.Current as Processor;
            }
        }

        private bool _working = false;
        private bool Working
        {
            get
            {
                return this._working;
            }
            set
            {
                this._working = value;
                this.tsTools.Enabled = !value;
                this.chkSelection.Enabled = !value;
                this.gbxSelection.Enabled = (!value) && this.SelectionEnabled;
                if (!value)
                {
                    this.ssStatusProgress.Visible = false;
                    this.ssStatusLabel.Text = "";
                }
            }
        }


        private bool SelectionEnabled
        {
            get
            {
                return this.chkSelection.Checked;
            }
            set
            {
                this.chkSelection.Checked = value;
                this.gbxSelection.Enabled = value && !this._working;
                if (value)
                {
                    this.dgvFiles.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
                    this.dgvFiles.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                }
                else
                {
                    this.dgvFiles.DefaultCellStyle.SelectionForeColor = this.dgvFiles.DefaultCellStyle.ForeColor;
                    this.dgvFiles.DefaultCellStyle.SelectionBackColor = this.dgvFiles.DefaultCellStyle.BackColor;
                }
                DataGridViewLinkColumn linkCol;
                foreach (DataGridViewColumn col in this.dgvFiles.Columns)
                {
                    linkCol = col as DataGridViewLinkColumn;
                    if (linkCol != null)
                    {
                        linkCol.ActiveLinkColor = value ? SystemColors.ControlText : Color.Red;
                        linkCol.VisitedLinkColor = linkCol.LinkColor = value ? SystemColors.ControlText : Color.Blue;
                    }
                }
            }
        }

        #endregion


        #region Constructors

        public frmMain()
        {
            InitializeComponent();
            this.Icon = Program.Icon;
            Localizer.CultureChanged += this.Localizer_CultureChanged;
            this.SelectionEnabled = false;
            this.tstSrcDir.Text = "";
            this._processors = new List<Processor>();
            this.dgvFiles.AutoGenerateColumns = false;
            this.dgvFiles.DataSource = this._processors;
            this._processorsManager = (CurrencyManager)this.dgvFiles.BindingContext[this._processors];
            Type descriptionAttributeType = typeof(DescriptionAttribute);
            this.tscbxTimeZone.SelectedIndexChanged -= this.tscbxTimeZone_SelectedIndexChanged;
            ReadOnlyCollection<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones();
            ((ComboBox)this.tscbxTimeZone.Control).DataSource = TimeZoneInfo.GetSystemTimeZones();
            this.tscbxTimeZone.SelectedItem = TimeZoneHandler.ShootTimeZone;
            this.tscbxTimeZone.SelectedIndexChanged += this.tscbxTimeZone_SelectedIndexChanged;
            this.UpdateSelectionOperation(true);
            this.UpdateSourceFolder();
            try
            {
                string dir = MediaData.Properties.Settings.Default.LastMediaDirectory;
                while (!string.IsNullOrEmpty(dir))
                {
                    if (Directory.Exists(dir))
                    {
                        this.tstSrcDir.Text = dir;
                        break;
                    }
                    dir = Path.GetDirectoryName(dir);
                }
            }
            catch
            { }
        }

        #endregion


        #region GUI events

        private void tsbSrcDir_Click(object sender, EventArgs e)
        {
            if (this._working)
            {
                return;
            }
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = i18n.Select_media_source_dir;
                try
                {
                    fbd.SelectedPath = this.tstSrcDir.Text;
                }
                catch
                { }
                fbd.ShowNewFolderButton = false;
                if (fbd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    this.tstSrcDir.Text = fbd.SelectedPath;
                    this.UpdateSourceFolder();
                }
            }
        }

        private void tstSrcDir_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this._working)
            {
                return;
            }
            if (e.KeyChar == '\r')
            {
                this.UpdateSourceFolder();
            }
        }

        private void tscbxTimeZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            TimeZoneInfo newTZI = (TimeZoneInfo)this.tscbxTimeZone.SelectedItem as TimeZoneInfo;
            if (TimeZoneHandler.ShootTimeZone != newTZI)
            {
                TimeZoneInfo oldTZI = TimeZoneHandler.ShootTimeZone;
                TimeZoneHandler.ShootTimeZone = newTZI;
                try
                {

                    foreach (Processor processor in this._processors)
                    {
                        processor.ShownTimeZoneChanged(oldTZI, newTZI);
                    }
                }
                catch
                {
                    this._processors.Clear();
                }
                this._processorsManager.Refresh();
                this.dgvFiles.Invalidate();
            }
        }

        private void tsbOptions_Click(object sender, EventArgs e)
        {
            this.ShowOptions();
        }

        private void dgvFiles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.IsFullColumnClick(e.ColumnIndex))
            {
                this.HandleGridClick(e);
            }
        }

        private void dgvFiles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.IsFullColumnClick(e.ColumnIndex))
            {
                this.HandleGridClick(e);
            }

        }

        private void chkSelection_CheckedChanged(object sender, EventArgs e)
        {
            this.SelectionEnabled = this.chkSelection.Checked;
        }

        private void lnkSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.dgvFiles.SelectAll();
        }

        private void lnkSelectNone_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.dgvFiles.ClearSelection();
        }

        private void cbxSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateSelectionOperation(false);
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (this._working)
            {
                return;
            }
            if (e.Modifiers == Keys.None)
            {
                switch (e.KeyCode)
                {
                    case Keys.F5:
                        e.SuppressKeyPress = true;
                        this.UpdateSourceFolder();
                        break;
                    case Keys.F4:
                        e.SuppressKeyPress = true;
                        this.ShowOptions();
                        break;
                }
            }
            else if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.L:
                        e.SuppressKeyPress = true;
                        this.tstSrcDir.Focus();
                        this.tstSrcDir.SelectionStart = 0;
                        this.tstSrcDir.SelectionLength = this.tstSrcDir.Text.Length;
                        break;
                }
            }
        }

        #endregion


        #region Instance methods

        private void UpdateSourceFolder()
        {
            this.Working = true;
            this.Refresh();
            string sf = this.tstSrcDir.Text;
            this.ssStatusLabel.Text = string.Format(i18n.Parsing_folder_X, sf);
            this._processors.Clear();
            this._processorsManager.Refresh();
            this.dgvFiles.Invalidate();

            FileInfo[] files = null;
            if (Directory.Exists(sf))
            {
                DirectoryInfo di = new DirectoryInfo(sf);
                files = di.GetFiles();
            }
            if (files != null && files.Length > 0)
            {
                BackgroundWorker bgw = new BackgroundWorker();
                bgw.WorkerSupportsCancellation = false;
                bgw.DoWork += this.AnalyzeSourceFolderFiles;
                bgw.RunWorkerCompleted += this.AnalysisDone;
                bgw.ProgressChanged += this.AnalysisInProgress;
                bgw.WorkerReportsProgress = true;
                this.ssStatusProgress.Minimum = 0;
                this.ssStatusProgress.Value = 0;
                this.ssStatusProgress.Maximum = files.Length;
                this.ssStatusProgress.Visible = true;
                this.ssStatus.Refresh();
                bgw.RunWorkerAsync(files);
            }
            else
            {
                this.Working = false;
            }
        }

        private void AnalyzeSourceFolderFiles(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bgw = (BackgroundWorker)sender;
            try
            {
                FileInfo[] files = (FileInfo[])e.Argument;

                foreach (FileInfo fi in files)
                {
                    if (bgw.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                    Processor processor = Processor.Get(fi.FullName);
                    this._processors.Add(processor);
                    bgw.ReportProgress(0, fi.Name);
                }
            }
            catch (Exception x)
            {
                e.Result = x;
            }
        }

        private void AnalysisInProgress(object sender, ProgressChangedEventArgs e)
        {
            this.ssStatusProgress.Value++;
            this.ssStatusLabel.Text = string.Format(i18n.Processing_file_X, e.UserState);
            this._processorsManager.Refresh();
            this.dgvFiles.Invalidate();
        }

        private void AnalysisDone(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker bgw = (BackgroundWorker)sender;
            Exception error = e.Result as Exception;
            if (error == null)
            {
                if (e.Cancelled)
                {
                    error = new OperationCanceledException();
                }
            }
            bgw.Dispose();
            this.Working = false;
            if (error != null)
            {
                MessageBox.Show(this, error.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MediaData.Properties.Settings.Default.LastMediaDirectory = this.tstSrcDir.Text;
                MediaData.Properties.Settings.Default.Save();
            }
        }

        private void ShowOptions()
        {
            using (frmOptions f = new frmOptions())
            {
                f.ShowDialog(this);
            }
        }

        private bool IsFullColumnClick(int columnIndex)
        {
            return (columnIndex == this.colMetadataDatetime.DisplayIndex || columnIndex == this.colMetadataPosition.DisplayIndex);
        }

        private void HandleGridClick(DataGridViewCellEventArgs e)
        {
            if (this._working)
            {
                return;
            }
            if (e.RowIndex < 0)
            {
                return;
            }
            Processor processor = this.SelectedProcessor;
            if (processor == null)
            {
                return;
            }
            if (e.ColumnIndex == this.colBrowseTo.DisplayIndex)
            {
                if (File.Exists(processor.FullFilename))
                {
                    try
                    {
                        using (Process process = new Process())
                        {
                            process.StartInfo.UseShellExecute = false;
                            process.StartInfo.FileName = "explorer.exe";
                            process.StartInfo.Arguments = "/select," + Tool.Escape(processor.FullFilename);
                            process.Start();
                        }
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    MessageBox.Show(string.Format(i18n.File_not_found_X, processor.FullFilename), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (this.SelectionEnabled)
            {
                return;
            }
            else if (e.ColumnIndex == this.colFilename.DisplayIndex)
            {
                if (File.Exists(processor.FullFilename))
                {
                    try
                    {
                        using (Process process = new Process())
                        {
                            process.StartInfo.UseShellExecute = true;
                            process.StartInfo.FileName = processor.FullFilename;
                            process.Start();
                        }
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    MessageBox.Show(string.Format(i18n.File_not_found_X, processor.FullFilename), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (processor is UnhandledFileTypeProcessor)
            {
                MessageBox.Show(string.Format(i18n.Unsupported_file_type_X, Path.GetExtension(processor.Filename).ToLowerInvariant()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (processor is FailedProcessor)
            {
                MessageBox.Show(((FailedProcessor)processor).Error.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (e.ColumnIndex == this.colFilenameDatetime.DisplayIndex)
                {
                    if (processor.FilenameTimestamp.HasValue)
                    {
                        DateTime dt = processor.FilenameTimestamp.Value;
                        if (MessageBox.Show(string.Format(i18n.Confirm_new_metadata_timestamp, dt.ToString()), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                        {
                            try
                            {
                                processor.ChangeMetadataTimestamp(dt, this);
                            }
                            catch (OperationCanceledException)
                            {
                                return;
                            }
                            catch (Exception x)
                            {
                                MessageBox.Show(x.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                            this.dgvFiles.InvalidateRow(e.RowIndex);
                        }
                    }
                }
                else if (e.ColumnIndex == this.colMetadataDatetime.DisplayIndex)
                {
                    bool setNewValue = false;
                    DateTime? newValue = null;
                    List<MediaInfo.NameTimestamp> alternatives = processor.Info.GetAlternatives(false);
                    if (alternatives.Count > 0)
                    {
                        using (frmPickDateTimeWithAlternatives f = new frmPickDateTimeWithAlternatives(processor.Info.GetAlternatives(false), processor.Info.TimestampMin))
                        {
                            if (f.ShowDialog(this) == DialogResult.OK)
                            {
                                newValue = f.Result;
                                setNewValue = true;
                            }
                        }
                    }
                    else
                    {
                        using (frmPickDateTime f = new frmPickDateTime(processor.Info.TimestampMin))
                        {
                            if (f.ShowDialog(this) == DialogResult.OK)
                            {
                                newValue = f.Result;
                                setNewValue = true;
                            }
                        }
                    }
                    if (setNewValue)
                    {
                        try
                        {
                            processor.ChangeMetadataTimestamp(newValue, this);
                        }
                        catch (OperationCanceledException)
                        {
                            return;
                        }
                        catch (Exception x)
                        {
                            MessageBox.Show(x.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        this.dgvFiles.InvalidateRow(e.RowIndex);
                    }
                }
                else if (e.ColumnIndex == this.colMetadataPosition.DisplayIndex)
                {
                    using (frmPosition f = new frmPosition(processor.Info.Position, processor.SupportsAltitude))
                    {
                        if (f.ShowDialog(this) == DialogResult.OK)
                        {
                            try
                            {
                                processor.ChangeMetadataPosition(f.SelectedPosition, this);
                            }
                            catch (OperationCanceledException)
                            {
                                return;
                            }
                            catch (Exception x)
                            {
                                MessageBox.Show(x.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                            this.dgvFiles.InvalidateRow(e.RowIndex);
                        }
                    }
                }
            }
        }

        private void Localizer_CultureChanged(object sender, Localizer.CultureChangedEventArgs e)
        {
            this.dgvFiles.Invalidate();
            this.UpdateSelectionOperation(true);
        }

        private void UpdateSelectionOperation(bool repopulate)
        {
            if (repopulate || this.cbxSelection.Items.Count == 0)
            {
                UI.DescribedEnumToCombobox(typeof(SelectionOperations), this.cbxSelection, UI.GetDescribedEnumValueOfCombobox(this.cbxSelection));
            }
            if (this.cbxSelection.SelectedItem == null)
            {
                this.cbxSelection.SelectedIndex = 0;
            }
            if (repopulate || this.cbxSelectionDeltaTimeUnit.Items.Count == 0)
            {
                UI.DescribedEnumToCombobox(typeof(DeltaOperationUnit), this.cbxSelectionDeltaTimeUnit, UI.GetDescribedEnumValueOfCombobox(this.cbxSelectionDeltaTimeUnit));
            }
            if (this.cbxSelectionDeltaTimeUnit.SelectedItem == null)
            {
                this.cbxSelectionDeltaTimeUnit.SelectedIndex = 0;
            }
            SelectionOperations operation = (SelectionOperations)UI.GetDescribedEnumValueOfCombobox(this.cbxSelection);
            this.lblSelectionDeltaTime.Visible = operation == SelectionOperations.DeltaTimestamp;
            this.cbxSelectionDeltaTimeUnit.Visible = this.nudSelectionDeltaTimeValue.Visible = operation == SelectionOperations.DeltaTimestamp;
            this.btnSelectionApply.Visible = (operation != SelectionOperations.PleaseSelect);
        }

        private void btnSelectionApply_Click(object sender, EventArgs e)
        {
            List<Processor> processors = new List<Processor>(this.dgvFiles.SelectedRows.Count);
            foreach (DataGridViewRow row in this.dgvFiles.SelectedRows)
            {
                Processor processor = row.DataBoundItem as Processor;
                if (processor != null)
                {
                    processors.Add(processor);
                }
            }
            if (processors.Count == 0)
            {
                MessageBox.Show(i18n.Please_select_at_least_one_file, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            switch ((SelectionOperations)UI.GetDescribedEnumValueOfCombobox(this.cbxSelection))
            {
                case SelectionOperations.DeltaTimestamp:
                    TimeSpan? ts = null;
                    int deltaValue = Convert.ToInt32(Math.Round(this.nudSelectionDeltaTimeValue.Value));
                    if (deltaValue != 0)
                    {
                        switch ((DeltaOperationUnit)UI.GetDescribedEnumValueOfCombobox(this.cbxSelectionDeltaTimeUnit))
                        {
                            case DeltaOperationUnit.Days:
                                ts = new TimeSpan(deltaValue, 0, 0, 0, 0);
                                break;
                            case DeltaOperationUnit.Hours:
                                ts = new TimeSpan(0, deltaValue, 0, 0, 0);
                                break;
                            case DeltaOperationUnit.Minutes:
                                ts = new TimeSpan(0, 0, deltaValue, 0, 0);
                                break;
                            case DeltaOperationUnit.Seconds:
                                ts = new TimeSpan(0, 0, 0, deltaValue, 0);
                                break;
                        }
                    }
                    if (!ts.HasValue)
                    {
                        MessageBox.Show(i18n.Please_specify_delta_time, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(ts.Value.ToString());
                    }
                    break;
                case SelectionOperations.RenameFiles:
                    using (frmBatchRename f = new frmBatchRename(processors))
                    {
                        if (f.ShowDialog(this) == DialogResult.OK)
                        {
                            this.dgvFiles.Invalidate();
                        }
                    }
                    break;
            }
        }

        #endregion

    }
}