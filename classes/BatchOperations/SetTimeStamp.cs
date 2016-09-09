using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MLocati.MediaData.BatchOperations
{
    class RemoveAltitude : MLocati.MediaData.BatchOperation
    {
        private Position _newPosition;

        protected override string IdleStateName
        {
            get
            {
                return i18n.Going_to_remove_altitude;
            }
        }

        protected override string ProcessedStateName
        {
            get
            {
                return i18n.Altitude_removed;
            }
        }

        protected override void ProcessReal(IWin32Window parentWindow)
        {
            this._processor.ChangeMetadataPosition(this._newPosition, parentWindow);
        }

        public RemoveAltitude(Processor processor)
            : base(processor)
        {
            if (processor.Info == null || processor.Info.Position == null || !processor.Info.Position.Alt.HasValue)
            {
                this._error = i18n.Already_without_altitude;
                this._state = States.Failed;
            }
            else
            {
                this._newPosition = new Position(processor.Info.Position.Lat, processor.Info.Position.Lng);
            }
        }
    }
}
