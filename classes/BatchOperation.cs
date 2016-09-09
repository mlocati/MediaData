using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MLocati.MediaData
{
    public abstract class BatchOperation
    {
        #region Types

        public enum States
        {
            Failed,
            Idle,
            Processing,
            Updated,
        }

        #endregion

        #region Instance properties

        protected readonly Processor _processor;

        protected string _error;

        protected States _state;

        protected abstract string IdleStateName
        {
            get;
        }

        protected abstract string ProcessedStateName
        {
            get;
        }

        public string Filename
        {
            get
            {
                return this._processor.Filename;
            }
        }

        public string State
        {
            get
            {
                switch (this._state)
                {
                    case States.Idle:
                        return this.IdleStateName;
                    case States.Processing:
                        return i18n.Processing;
                    case States.Updated:
                        return this.ProcessedStateName;
                    default:
                    case States.Failed:
                        return this._error;
                }
            }
        }

        #endregion

        #region Constructors

        protected BatchOperation(Processor processor)
        {
            this._processor = processor;
            this._error = "";
            this._state = States.Idle;
        }

        #endregion

        #region Instance methods

        protected abstract void ProcessReal(IWin32Window parentWindow);

        public void Process(IWin32Window parentWindow)
        {
            if (this._state == States.Idle)
            {
                try
                {
                    this.ProcessReal(parentWindow);
                    this._state = States.Updated;
                }
                catch (Exception x)
                {
                    this._state = States.Failed;
                    this._error = x.Message;
                }
            }
        }

        #endregion
    }
}
