using System;
using System.IO;
using System.Windows.Forms;

namespace MLocati.MediaData
{
    public abstract class ImageProcessor : Processor
    {

        #region Constructors

        protected ImageProcessor(string fullFilename)
            :base(fullFilename)
        { }

        #endregion


        #region Instance methods

        protected override void SetNewMetadata(MediaInfo newInfo, IWin32Window parentWindow)
        {
            string tempFilename = this.GetNewTempFilename();
            try
            {
                ShowProcessingOutput spo = ShowProcessingOutput.ShowProcessingOutput_GetForImages();
                Tool tool = this.PrepareTool(newInfo, tempFilename);
                Exception error = this.RunTool(newInfo, tool, spo, tempFilename, parentWindow);
                if (error != null)
                {
                    throw error;
                }
            }
            catch
            {
                try
                {
                    if (File.Exists(tempFilename))
                    {
                        File.Delete(tempFilename);
                    }
                }
                catch
                { }
                throw;
            }
        }

        protected abstract Tool PrepareTool(MediaInfo newInfo, string tempFilename);

        #endregion

    }
}
