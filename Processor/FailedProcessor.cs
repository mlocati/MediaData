using System;
using System.Windows.Forms;

namespace MLocati.MediaData
{
    public class FailedProcessor : Processor
    {

        #region Instance properties

        public readonly Exception Error;

        public override string MetadataTimestampStr
        {
            get
            {
                return i18n._error_;
            }
        }

        #endregion


        #region Constructors

        public FailedProcessor(string fullFilename, Exception error)
            :base(fullFilename)
        {
            this.Error = error;
        }

        #endregion


        #region Instance methods

        protected override MediaInfo GetInfo(string fullFilename)
        {
            return new MediaInfo();
        }

        protected override void SetNewMetadata(MediaInfo newInfo, IWin32Window parentWindow)
        {
            throw new Exception(string.Format(i18n.Failed_process_file_X, this.FullFilename));
        }

        #endregion

    }
}
