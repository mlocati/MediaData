using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MLocati.MediaData.BatchOperations
{
    class SetPosition : MLocati.MediaData.BatchOperation
    {
        private Position _newPosition;

        protected override string IdleStateName
        {
            get
            {
                return (this._newPosition == null) ? i18n.Going_to_remove_position : i18n.Going_to_update_position;
            }
        }

        protected override string ProcessedStateName
        {
            get
            {
                return (this._newPosition == null) ? i18n.Position_removed : i18n.Position_updated;
            }
        }

        protected override void ProcessReal(IWin32Window parentWindow)
        {
            this._processor.ChangeMetadataPosition(this._newPosition, parentWindow);
        }

        public SetPosition(Processor processor, Position newPosition)
            : base(processor)
        {
            if (newPosition == null && (processor.Info == null || processor.Info.Position == null))
            {
                this._error = i18n.Already_without_position;
                this._state = States.Failed;
            }
            else if (newPosition != null && processor.Info != null && processor.Info.Position != null && newPosition.Alt.HasValue == processor.Info.Position.Alt.HasValue && newPosition.DistanceTo(processor.Info.Position) < 0.5)
            {
            } else {
                this._newPosition = (newPosition == null) ? null : newPosition.Clone();
            }
        }
    }
}
