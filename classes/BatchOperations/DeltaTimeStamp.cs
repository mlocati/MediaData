using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MLocati.MediaData.BatchOperations
{
    class DeltaTimeStamp : MLocati.MediaData.BatchOperation
    {
        private DateTime? _newDateTime;

        protected override string IdleStateName
        {
            get
            {
                return string.Format(i18n.Going_to_set_metadata_timestamp_to_X, this._newDateTime.Value.ToString());
            }
        }

        protected override string ProcessedStateName
        {
            get
            {
                return string.Format(i18n.Metadata_timestamp_set_to_X, this._newDateTime.Value.ToString());
            }
        }

        protected override void ProcessReal(IWin32Window parentWindow)
        {
            this._processor.ChangeMetadataTimestamp(this._newDateTime.Value, parentWindow);
        }

        public DeltaTimeStamp(Processor processor, TimeSpan timespan)
            : base(processor)
        {

            if (processor.Info == null || !processor.Info.TimestampMean.HasValue)
            {
                this._error = i18n.No_timestamp_in_metadata;
                this._state = States.Failed;
            }
            else
            {
                this._newDateTime = processor.Info.TimestampMean.Value.Add(timespan);
            }
        }
    }
}
