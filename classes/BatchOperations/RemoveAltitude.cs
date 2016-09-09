using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MLocati.MediaData.BatchOperations
{
    class SetTimeStamp : MLocati.MediaData.BatchOperation
    {
        private DateTime? _newDateTime;

        protected override string IdleStateName
        {
            get
            {
                if (this._newDateTime.HasValue)
                {
                    return string.Format(i18n.Going_to_set_metadata_timestamp_to_X, this._newDateTime.Value.ToString());
                }
                else
                {
                    return i18n.Going_to_remove_metadata_timestamp;
                }
            }
        }

        protected override string ProcessedStateName
        {
            get
            {
                if (this._newDateTime.HasValue)
                {
                    return string.Format(i18n.Metadata_timestamp_set_to_X, this._newDateTime.Value.ToString());
                }
                else
                {
                    return i18n.Metadata_timestamp_removed;
                }
            }
        }

        protected override void ProcessReal(IWin32Window parentWindow)
        {
            this._processor.ChangeMetadataTimestamp(this._newDateTime, parentWindow);
        }

        public SetTimeStamp(Processor processor, DateTime? newDateTime)
            : base(processor)
        {
            if (processor.Info == null)
            {
                this._error = i18n.No_timestamp_in_metadata;
                this._state = States.Failed;
            }
            else
            {
                if (newDateTime.HasValue)
                {
                    this._newDateTime = newDateTime.Value;
                }
                else
                {
                    this._newDateTime = null;
                }
            }
        }
    }
}
