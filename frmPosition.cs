using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MLocati.MediaData
{
    public partial class frmPosition : Form
    {

        #region Instance properties

        private Label _altitudeNotSupported = null;

        private Position _originalPosition;

        private bool _supportAltitude;

        private Position _selectedPosition = null;
        public Position SelectedPosition
        {
            get
            {
                return this._selectedPosition;
            }
        }

        private GMapOverlay _overlay;

        private GMarkerGoogle _marker;
        private GMarkerGoogle Marker
        {
            get
            {
                return this._marker;
            }
            set
            {
                if (this._marker != value)
                {
                    if (this._marker != null)
                    {
                        try
                        {
                            this._overlay.Markers.Remove(this._marker);
                        }
                        catch
                        { }
                        try
                        {
                            this._marker.Dispose();
                        }
                        catch
                        { }
                    }
                    this._marker = value;
                    if (this._marker != null)
                    {
                        this._overlay.Markers.Add(this._marker);
                    }
                }
                this.btnClipboardCopy.Enabled = this.btnAccept.Enabled = (this._marker != null);
                this.UpdateAltState();
            }
        }

        private PointLatLng? MarkerPosition
        {
            get
            {
                return (this._marker == null) ? (PointLatLng?)null : this._marker.Position;
            }
            set
            {
                if (value.HasValue)
                {
                    if (this.Marker == null)
                    {
                        this.Marker = new GMarkerGoogle(value.Value, GMarkerGoogleType.arrow);
                    }
                    else
                    {
                        this.Marker.Position = value.Value;
                    }
                    this.slCurrentPosition.Text = this.MarkerPositionMD.ToString(true);
                }
                else
                {
                    this.Marker = null;
                    this.slCurrentPosition.Text = "";
                }
            }
        }
        private Position MarkerPositionMD
        {
            get
            {
                PointLatLng? p = this.MarkerPosition;
                return p.HasValue ? new Position(Convert.ToDecimal(p.Value.Lat), Convert.ToDecimal(p.Value.Lng)) : null;
            }
            set
            {
                if (value == null)
                {
                    this.MarkerPosition = null;
                }
                else
                {
                    this.MarkerPosition = new PointLatLng(Convert.ToDouble(value.Lat), Convert.ToDouble(value.Lng));
                }
            }
        }

        private bool _draggingMarker;

        #endregion


        #region Constructors

        public frmPosition(Position position, bool supportAltitude)
        {
            InitializeComponent();
            this.Icon = Program.Icon;
            this._originalPosition = position;
            this._supportAltitude = supportAltitude;
            this.gmcMap.Manager.Mode = MediaData.Properties.Settings.Default.Maps_EnableCache ? AccessMode.ServerAndCache : AccessMode.ServerOnly;
            this.gmcMap.ShowCenter = false;
            List<MapProvider> providers = new List<MapProvider>(MapProvider.GetEnabledProviders());
            MapProvider selectedProvider;
            selectedProvider = MapProvider.GetByBase(MediaData.Properties.Settings.Default.Maps_LastProvider);
            if (selectedProvider == null || !providers.Contains(selectedProvider))
            {
                selectedProvider = MapProvider.GetByBase(OpenStreetMapProvider.Instance);
                if (selectedProvider == null || !providers.Contains(selectedProvider))
                {
                    selectedProvider = (providers.Count > 0) ? providers[0] : null;
                }
            }
            this.cbxLatLonProvider.DataSource = providers;
            this.cbxLatLonProvider.SelectedItem = selectedProvider;
            this._overlay = new GMapOverlay();
            this.gmcMap.Overlays.Add(_overlay);
            this.gmcMap.MapScaleInfoEnabled = true;
            this.MarkerPositionMD = position;
            this.vsbZoom.Scroll -= this.vsbZoom_Scroll;
            this.vsbZoom.Minimum = int.MinValue;
            this.vsbZoom.Maximum = int.MaxValue;
            this.vsbZoom.Value = Convert.ToInt32(this.gmcMap.Zoom);
            this.vsbZoom.Minimum = Convert.ToInt32(this.gmcMap.MinZoom);
            this.vsbZoom.Maximum = Convert.ToInt32(this.gmcMap.MaxZoom);
            this.vsbZoom.Scroll += this.vsbZoom_Scroll;
            if (position == null)
            {
                Position prepos;
                try
                {
                    prepos = Position.Unserialize(MediaData.Properties.Settings.Default.Maps_LastPosition);
                }
                catch
                {
                    prepos = null;
                }
                if (prepos == null)
                {
                    this.gmcMap.Position = new PointLatLng(0D, 0D);
                    this.SetZoom(1D);
                }
                else
                {
                    this.gmcMap.Position = new PointLatLng(Convert.ToDouble(prepos.Lat), Convert.ToDouble(prepos.Lng));
                    this.SetZoom(12D);
                }
            }
            else
            {
                MediaData.Properties.Settings.Default.Maps_LastPosition = position.Serialize();
                try
                {
                    MediaData.Properties.Settings.Default.Save();
                }
                catch
                { }
                this.gmcMap.Position = this.MarkerPosition.Value;
                this.SetZoom(15D);
            }
            this.gmcMap.OnMapZoomChanged += this.gmcMap_OnMapZoomChanged;
            this._draggingMarker = false;
            this.CheckClipboard();
            (new ClipboardMonitor(this)).ClipboardChanged += delegate (object sender, EventArgs e) { this.CheckClipboard(); };
            if (this._supportAltitude)
            {
                if (position != null && position.Alt.HasValue)
                {
                    this.chkAltSet.Checked = true;
                    this.SetAltValue(position.Alt.Value);
                }
            }
            else
            {
                foreach (Control c in this.gbxAltitude.Controls)
                {
                    c.Visible = false;
                }
                this._altitudeNotSupported = new Label();
                this._altitudeNotSupported.Text = i18n.Filetype_dont_support_altitude;
                this._altitudeNotSupported.TextAlign = ContentAlignment.MiddleCenter;
                this._altitudeNotSupported.AutoSize = true;
                this._altitudeNotSupported.Anchor = AnchorStyles.Top;
                this.gbxAltitude.Controls.Add(this._altitudeNotSupported);
                this._altitudeNotSupported.Location = new Point((this.gbxAltitude.ClientRectangle.Width - this._altitudeNotSupported.Width) >> 1, (this.gbxAltitude.ClientRectangle.Height - this._altitudeNotSupported.Height) >> 1);
                this.gbxAltitude.Enabled = false;
            }
            this.UpdateAltState();
        }

        #endregion


        #region GUI events

        private void frmPosition_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Modifiers)
            {
                case Keys.None:
                    switch (e.KeyCode)
                    {
                        case Keys.F3:
                            if (this.tbxLatLonSearch.Enabled)
                            {
                                this.tbxLatLonSearch.Focus();
                            }
                            break;
                    }
                    break;
                case Keys.Control:
                    bool enableClipboardShortcurs;
                    if (
                        this.ActiveControl is TextBox
                        || this.ActiveControl is NumericUpDown
                        || (this.ActiveControl is ComboBox && ((ComboBox)this.ActiveControl).DropDownStyle != ComboBoxStyle.DropDownList)
                    )
                    {
                        enableClipboardShortcurs = false;
                    } else
                    {
                        enableClipboardShortcurs = true;
                    }
                    switch (e.KeyCode)
                    {
                        case Keys.C:
                            if (enableClipboardShortcurs)
                            {
                                this.CopyPositionToClipboard();
                            }
                            break;
                        case Keys.V:
                            if (enableClipboardShortcurs)
                            {
                                this.PastePositionFromClipboard();
                            }
                            break;
                    }
                    break;
            }
        }

        private void tbxLatLonSearch_Enter(object sender, EventArgs e)
        {
            this.AcceptButton = null;
        }

        private void tbxLatLonSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.tbxLatLonSearch.Enabled)
            {
                if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter)
                {
                    this.SearchPlace(this.tbxLatLonSearch.Text);
                }
            }
        }

        private void tbxLatLonSearch_Leave(object sender, EventArgs e)
        {
            this.AcceptButton = this.btnAccept;
        }

        private void cbxLatLonProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            MapProvider provider = this.cbxLatLonProvider.SelectedItem as MapProvider;
            if (provider != null)
            {
                PointLatLng? prevPosition = null;
                try
                { prevPosition = this.gmcMap.Position; }
                catch
                { }
                double? prevZoom = null;
                try
                { prevZoom = this.gmcMap.Zoom; }
                catch
                { }
                this.gmcMap.MapProvider = provider.BaseProvider;
                this.gmcMap.Refresh();
                if (prevPosition.HasValue)
                {
                    try
                    { this.gmcMap.Position = prevPosition.Value; }
                    catch
                    { }
                }
                if (prevZoom.HasValue)
                {
                    try
                    { this.SetZoom(prevZoom.Value); }
                    catch
                    { }
                }
                try
                {
                    MediaData.Properties.Settings.Default.Maps_LastProvider = provider.BaseProvider.Name;
                    MediaData.Properties.Settings.Default.Save();
                }
                catch
                { }
            }
        }

        private void gmcMap_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.MarkerPosition = this.gmcMap.FromLocalToLatLng(e.X, e.Y);
                this._draggingMarker = true;
            }
        }

        private void gmcMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (this._draggingMarker)
            {
                this.MarkerPosition = this.gmcMap.FromLocalToLatLng(e.X, e.Y);
            }
        }

        private void gmcMap_MouseUp(object sender, MouseEventArgs e)
        {
            this._draggingMarker = false;
        }

        private void gmcMap_OnMapZoomChanged()
        {
            this.SetZoom(this.gmcMap.Zoom, false, true);
        }

        private void vsbZoom_Scroll(object sender, ScrollEventArgs e)
        {
            this.SetZoom(e.NewValue, true, false);
        }

        private void chkAltSet_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateAltState();
            this.lblAltAutoPrecision.Visible = false;
        }

        private void nudAlt_Enter(object sender, EventArgs e)
        {
            this.AcceptButton = null;
        }

        private void nudAlt_ValueChanged(object sender, EventArgs e)
        {
            this.lblAltAutoPrecision.Visible = false;
        }

        private void nudAlt_Leave(object sender, EventArgs e)
        {
            this.SetAltValue(this.nudAlt.Value);
            this.AcceptButton = this.btnAccept;
        }

        private void lnkAltAuto_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.lblAltAutoPrecision.Visible = false;
            Position pos = this.MarkerPositionMD;
            if (pos == null)
            {
                this.UpdateAltState();
                return;
            }
            if (string.IsNullOrEmpty(MediaData.Properties.Settings.Default.GoogleAPI_Elevation))
            {
                if (MessageBox.Show(i18n.Missing_Google_Elevation_API_Key_Ask_open_options, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
                {
                    return;
                }
                using (frmOptions f = new frmOptions())
                {
                    if (f.ShowDialog(this) == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                if (string.IsNullOrEmpty(MediaData.Properties.Settings.Default.GoogleAPI_Elevation))
                {
                    return;
                }
            }
            decimal alt;
            decimal? prec;
            try
            {
                alt = pos.QueryElevation(out prec);
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.SetAltValue(alt);
            this.lblAltAutoPrecision.Text = prec.HasValue ? string.Format(i18n._precision_is_X, Localizer.FormatMeters(prec.Value)) : i18n._precision_not_available;
            this.lblAltAutoPrecision.Visible = true;
        }

        private void btnClipboardCopy_Click(object sender, EventArgs e)
        {
            this.CopyPositionToClipboard();
        }

        private void btnClipboardPaste_Click(object sender, EventArgs e)
        {
            this.PastePositionFromClipboard();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            Position position = this.MarkerPositionMD;
            if (position != null)
            {
                MediaData.Properties.Settings.Default.Maps_LastPosition = position.Serialize();
                if (this._supportAltitude && this.chkAltSet.Checked)
                {
                    position = new Position(position.Lat, position.Lng, this.nudAlt.Value);
                }
                try
                {
                    MediaData.Properties.Settings.Default.Save();
                }
                catch
                { }
                this._selectedPosition = position;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnEmpty_Click(object sender, EventArgs e)
        {
            if (this._originalPosition == null || MessageBox.Show(i18n.Confirm_remove_position, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                this._selectedPosition = null;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void frmPosition_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.gmcMap.Manager.CancelTileCaching();
        }

        #endregion


        #region Instance methods

        private Position CheckClipboard()
        {
            Position found = null;
            try
            {
                if (Clipboard.ContainsText())
                {
                    Position p = Position.Unserialize(Clipboard.GetText(TextDataFormat.Text));
                    if (p != null)
                    {
                        found = p;
                    }
                }
            }
            catch
            { }
            this.btnClipboardPaste.Enabled = (found != null);
            return found;
        }

        private bool SearchPlace(string placeKeywords)
        {
            if (placeKeywords != null)
            {
                placeKeywords = placeKeywords.Trim();
            }
            if (string.IsNullOrEmpty(placeKeywords))
            {
                return false;
            }
            GeoCoderStatusCode status;
            PointLatLng? pos = GMapProviders.GoogleMap.GetPoint(placeKeywords, out status);
            if (pos != null && status == GeoCoderStatusCode.G_GEO_SUCCESS)
            {
                this.gmcMap.Position = pos.Value;
                if (this.gmcMap.Zoom < 9D)
                {
                    this.SetZoom(9D);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private void UpdateAltState()
        {
            if (!this._supportAltitude)
            {
                return;
            }
            this.nudAlt.Enabled = this.chkAltSet.Checked;
            this.lnkAltAuto.Enabled = this.chkAltSet.Checked && this.Marker != null;
        }

        private void SetZoom(double zoom)
        {
            this.SetZoom(zoom, true, true);
        }
        private void SetZoom(double zoom, bool setMap, bool setVBar)
        {
            if (setMap)
            {
                double mZoom = Math.Round(zoom);
                if (mZoom < this.gmcMap.MinZoom)
                {
                    mZoom = this.gmcMap.MinZoom;
                }
                if (mZoom > this.gmcMap.MaxZoom)
                {
                    mZoom = this.gmcMap.MaxZoom;
                }
                if (this.gmcMap.Zoom != mZoom)
                {
                    this.gmcMap.Zoom = mZoom;
                }
            }
            if (setVBar)
            {
                int vZoom = Convert.ToInt32(Math.Round(zoom));
                if (vZoom < this.vsbZoom.Minimum)
                {
                    vZoom = this.vsbZoom.Minimum;
                }
                if (vZoom > this.vsbZoom.Maximum)
                {
                    vZoom = this.vsbZoom.Maximum;
                }
                if (this.vsbZoom.Value != vZoom)
                {
                    this.vsbZoom.Value = vZoom;
                }
            }
        }

        private void SetAltValue(decimal value)
        {
            if (this._supportAltitude)
            {
                value = Math.Round(value, 1);
                if (this.nudAlt.Minimum > value)
                {
                    this.nudAlt.Minimum = value - 1000M;
                }
                if (this.nudAlt.Maximum <= value)
                {
                    this.nudAlt.Maximum = value + 1000M;
                }
                this.nudAlt.Value = value;
            }
        }

        private void CopyPositionToClipboard()
        {
            if (this.btnClipboardCopy.Enabled)
            {
                Position p = this.MarkerPositionMD;
                if (p != null)
                {
                    try
                    {
                        if (this._supportAltitude && this.chkAltSet.Checked)
                        {
                            p = new Position(p.Lat, p.Lng, this.nudAlt.Value);
                        }
                        Clipboard.SetText(p.Serialize(), TextDataFormat.Text);
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void PastePositionFromClipboard()
        {
            if (this.btnClipboardPaste.Enabled)
            {
                Position found = this.CheckClipboard();
                if (found != null)
                {
                    this.MarkerPositionMD = found;
                    if (this.gmcMap.Zoom < 15D)
                    {
                        this.SetZoom(15D);
                    }
                    if (!this.gmcMap.ViewArea.Contains(this.MarkerPosition.Value))
                    {
                        this.gmcMap.Position = this.MarkerPosition.Value;
                    }
                    if (this._supportAltitude)
                    {
                        this.chkAltSet.Checked = found.Alt.HasValue;
                        if (found.Alt.HasValue)
                        {
                            this.SetAltValue(found.Alt.Value);
                        }
                    }
                }
            }
        }

        #endregion

    }
}
