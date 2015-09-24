using System;
using System.IO;
using System.Windows.Forms;

namespace MLocati.MediaData
{
    public class UnhandledFileTypeProcessor : Processor
    {

        #region Constructors

        public UnhandledFileTypeProcessor(string fullFilename)
            : base(fullFilename)
        { }

        #endregion


        #region Instance methods

        protected override MediaInfo GetInfo(string fullFilename)
        {
            return new MediaInfo();
        }

        protected override void SetNewMetadata(MediaInfo newInfo, IWin32Window parentWindow)
        {
            throw new Exception(string.Format(i18n.Unsupported_file_type_X, Path.GetExtension(this.FullFilename).ToLowerInvariant()));
        }

        #endregion

    }
}
