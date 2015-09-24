using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MLocati.MediaData
{
    public class ClipboardMonitor : IDisposable
    {

        #region Constants

        private const int WM_DRAWCLIPBOARD = 0x00000308;

        private const int WM_CHANGECBCHAIN = 0x0000030D;

        #endregion


        #region Types

        private class MyControl : Control
        {
            private ClipboardMonitor _monitor;
            public MyControl(ClipboardMonitor monitor)
            {
                this._monitor = monitor;
            }
            protected override void WndProc(ref Message m)
            {
                if (this._monitor == null || this._monitor.HandleWindowProc(ref m))
                {
                    base.WndProc(ref m);
                }
            }
        }

        #endregion


        #region Instance properties

        public event EventHandler ClipboardChanged;

        private MyControl _control = null;

        private Form _listener = null;

        private IntPtr? _nextClipboardViewer = null;

        #endregion


        #region Constructors

        public ClipboardMonitor(Form listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException("listener");
            }
            this._listener = listener;
            this._listener.Closed += this.ListenerClosed;
            this._control = new MyControl(this);
            this._nextClipboardViewer = ClipboardMonitor.SetClipboardViewer(this._control.Handle);
        }

        #endregion


        #region Instace methods

        private bool HandleWindowProc(ref Message m)
        {
            switch (m.Msg)
            {
                case ClipboardMonitor.WM_DRAWCLIPBOARD:
                    if (this._listener != null && this.ClipboardChanged != null)
                    {
                        this._listener.BeginInvoke(this.ClipboardChanged, new object[] { this, new EventArgs() });
                    }
                    if (this._nextClipboardViewer.HasValue)
                    {
                        ClipboardMonitor.SendMessage(this._nextClipboardViewer.Value, m.Msg, m.WParam, m.LParam);
                    }
                    return true;
                case ClipboardMonitor.WM_CHANGECBCHAIN:
                    if (this._nextClipboardViewer.HasValue)
                    {
                        if (m.WParam == this._nextClipboardViewer.Value)
                        {
                            this._nextClipboardViewer = m.LParam;
                        }
                        else
                        {
                            ClipboardMonitor.SendMessage(this._nextClipboardViewer.Value, m.Msg, m.WParam, m.LParam);
                        }
                    }
                    return false;
                default:
                    return true;
            }
        }

        private void ListenerClosed(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void Dispose()
        {
            if (this._nextClipboardViewer.HasValue)
            {
                try
                {
                    if (this._control != null)
                    {
                        ClipboardMonitor.ChangeClipboardChain(this._control.Handle, this._nextClipboardViewer.Value);
                    }
                }
                catch
                { }
                this._nextClipboardViewer = null;
            }
            if (this._control != null)
            {
                try
                { this._control.Dispose(); }
                catch
                { }
                this._control = null;
            }
            if (this._listener != null)
            {
                try
                {
                    this._listener.Closed -= this.ListenerClosed;
                }
                catch
                { }
                this._listener = null;
            }
        }

        #endregion


        #region Static methods

        [DllImport("User32.dll")]
        private static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        #endregion

    }
}
