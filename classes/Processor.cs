using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace MLocati.MediaData
{
    public abstract class Processor
    {

        #region Types

        public class UnableToSetNewMetadataException : Exception
        {
            public UnableToSetNewMetadataException(string why)
                : base(string.Format(i18n.Unable_to_set_new_metadata_because_X, why))
            { }
        }

        [Serializable]
        public enum SetFileDates
        {
            [Localizer.Description("SetFileDates_Never")]
            Never,
            [Localizer.Description("SetFileDates_Writing")]
            Writing,
            [Localizer.Description("SetFileDates_ReadingWriting")]
            ReadingWriting,
        }

        #endregion


        #region Instance properties

        private MediaInfo _info;
        public MediaInfo Info
        {
            get
            {
                return this._info;
            }
        }

        private string _fullFilename;
        public string FullFilename
        {
            get
            { return this._fullFilename; }
        }

        private string _filename;
        public string Filename
        {
            get
            { return this._filename; }
        }

        private FilenameTimeStamper _filenameTimeStamper = null;
        public FilenameTimeStamper FilenameTimeStamper
        {
            get
            {
                return this._filenameTimeStamper;
            }
        }

        public DateTime? FilenameTimestamp
        {
            get
            {
                return (this._filenameTimeStamper == null) ? null : (DateTime?)this._filenameTimeStamper.Timestamp;
            }
        }

        public string FilenameTimestampStr
        {
            get
            {
                string result = "";
                if (this._filenameTimeStamper != null)
                {
                    result = this._filenameTimeStamper.Timestamp.ToString();
                    if (result == this.MetadataTimestampStr)
                    {
                        result = "=";
                    }
                    else
                    {
                        DateTime? ts = this._info.TimestampMean;
                        if (ts.HasValue)
                        {
                            TimeSpan delta = this.FilenameTimestamp.Value - ts.Value;
                            result += " (" + delta.ToString() + ")";
                        }
                    }
                }
                return result;
            }
        }

        public virtual string MetadataTimestampStr
        {
            get
            {
                if (this._info.TimestampMin.HasValue)
                {
                    UInt64 halfDelta = this._info.TimestampMinMaxHalfDelta.Value;
                    if (halfDelta == 0)
                    {
                        return this._info.TimestampMin.ToString();
                    }
                    if (halfDelta < 100)
                    {
                        return string.Format("{0} ± {1:N0} {2}", this._info.TimestampMean.ToString(), halfDelta, i18n.ShortTime_seconds);
                    }
                    halfDelta = halfDelta / 60;
                    if (halfDelta < 100)
                    {
                        return string.Format("{0} ± {1:N0} {2}", this._info.TimestampMean.ToString(), halfDelta, i18n.ShortTime_minutes);
                    }
                    halfDelta = halfDelta / 60;
                    if (halfDelta < 100)
                    {
                        return string.Format("{0} ± {1:N0} {2}", this._info.TimestampMean.ToString(), halfDelta, i18n.ShortTime_hours);
                    }
                    halfDelta = halfDelta / 24;
                    return string.Format("{0} ± {1:N0} {2}", this._info.TimestampMean.ToString(), halfDelta, i18n.ShortTime_days);
                }
                else
                {
                    return "";
                }
            }
        }

        public string MetadataPositionStr
        {
            get
            {
                return ((this._info == null) || (this._info.Position == null)) ? "" : this._info.Position.ToGridString();
            }
        }

        public virtual bool SupportsAltitude
        {
            get { return false; }
        }

        #endregion


        #region Constructors

        protected Processor(string fullFilename)
        {
            if (!File.Exists(fullFilename))
            {
                throw new FileNotFoundException(fullFilename);
            }
            this.SetFullFilename(fullFilename);
            this._info = this.GetInfo(fullFilename);
        }

        #endregion


        #region Instance methods

        public void ChangeMetadataTimestamp(DateTime? newTimestamp)
        {
            this.ChangeMetadataTimestamp(newTimestamp, null);
        }
        public void ChangeMetadataTimestamp(DateTime? newTimestamp, IWin32Window parentWindow)
        {
            MediaInfo newInfo = this.Info.CloneWithNewTimestamp(newTimestamp);
            this.ChangeMetadata(newInfo, parentWindow);
            try
            {
                switch (MediaData.Properties.Settings.Default.SetFileDates)
                {
                    case SetFileDates.Writing:
                    case SetFileDates.ReadingWriting:
                        this.UpdateFileTimestamp();
                        break;
                }
            }
            catch
            { }
        }

        public void ChangeMetadataPosition(Position newPosition)
        {
            this.ChangeMetadataPosition(newPosition, null);
        }
        public void ChangeMetadataPosition(Position newPosition, IWin32Window parentWindow)
        {
            MediaInfo newInfo = this.Info.CloneWithNewPosition(newPosition);
            this.ChangeMetadata(newInfo, parentWindow);
        }

        private void ChangeMetadata(MediaInfo newInfo, IWin32Window parentWindow)
        {
            FileAttributes fa = File.GetAttributes(this.FullFilename);
            if ((fa & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                if (MessageBox.Show(parentWindow, string.Format(i18n.Ask_remove_readonly_file_X, this.Filename), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
                {
                    throw new OperationCanceledException();
                }
                File.SetAttributes(this.FullFilename, fa & ~FileAttributes.ReadOnly);
            }
            this.SetNewMetadata(newInfo, parentWindow);
        }
        protected abstract void SetNewMetadata(MediaInfo newInfo, IWin32Window parentWindow);

        protected Exception RunTool(MediaInfo newInfo, Tool tool, ShowProcessingOutput spo, string tempFilename, IWin32Window parentWindow)
        {
            return this.RunTool(newInfo, tool, spo, tempFilename, parentWindow, this.FullFilename);
        }
        protected Exception RunTool(MediaInfo newInfo, Tool tool, ShowProcessingOutput spo, string tempFilename, IWin32Window parentWindow, string finalFilename)
        {
            Exception result = null;
            try
            {
                bool gui;
                switch (spo.Shown)
                {
                    case ShowProcessingOutput.When.Always:
                        gui = true;
                        break;
                    case ShowProcessingOutput.When.BiggerThan:
                        gui = (Convert.ToUInt64((new FileInfo(this.FullFilename)).Length) >= spo.BiggerThanThisBytes) ? true : false;
                        break;
                    case ShowProcessingOutput.When.Never:
                    default:
                        gui = false;
                        break;
                }
                if (gui)
                {
                    result = tool.RunGUI(parentWindow, spo.AutocloseOnSuccess).Error;
                }
                else
                {
                    result = tool.Run().Error;
                }
                if (result == null)
                {
                    MediaInfo updatedInfo = this.GetInfo(tempFilename);
                    string updateError = updatedInfo.CheckUpdatedInfo(this.Info, newInfo);
                    if (updateError != null)
                    {
                        result = new UnableToSetNewMetadataException(updateError);
                    }
                    else
                    {
                        if (MediaData.Properties.Settings.Default.DeleteToTrash)
                        {
                            MyIO.DeleteToTrash(this.FullFilename);
                        }
                        else
                        {
                            File.Delete(this.FullFilename);
                        }
                        File.Move(tempFilename, finalFilename);
                        this._info = updatedInfo;
                    }
                }
            }
            catch (Exception x)
            {
                result = x;
            }
            return result;
        }

        protected abstract MediaInfo GetInfo(string fullFilename);

        public virtual void ShownTimeZoneChanged(TimeZoneInfo oldTZI, TimeZoneInfo newTZI)
        {
            if (this._info != null)
            {
                this._info.ShownTimeZoneChanged(oldTZI, newTZI);
            }
        }

        internal void SetFullFilename(string fullFilename)
        {
            this._fullFilename = fullFilename;
            this._filename = Path.GetFileName(fullFilename);
            this._filenameTimeStamper = FilenameTimeStamper.Get(this._filename);
        }

        protected string GetNewTempFilename()
        {
            return this.GetNewTempFilename(Path.GetExtension(this.Filename));
        }
        protected string GetNewTempFilename(string extension)
        {
            string baseFilename = Path.GetFileNameWithoutExtension(this.Filename);

            string dir = Path.GetDirectoryName(this.FullFilename);
            for (int i = 0; ; i++)
            {
                string tempFilename = Path.Combine(dir, string.Format("{0}-tmp{1:N0}{2}", baseFilename, i, extension));
                if (!(File.Exists(tempFilename) || Directory.Exists(tempFilename)))
                {
                    return tempFilename;
                }
            }
        }

        private void UpdateFileTimestamp()
        {
            if (this.Info == null)
            {
                return;
            }
            DateTime? newFileTimestamp = this.Info.TimestampMean;
            if (!newFileTimestamp.HasValue)
            {
                return;
            }
            DateTime? oldFileTimestamp;

            try
            {
                oldFileTimestamp = File.GetCreationTime(this.FullFilename);
            }
            catch
            {
                oldFileTimestamp = null;
            }
            if ((!oldFileTimestamp.HasValue) || Math.Round((oldFileTimestamp.Value - newFileTimestamp.Value).TotalSeconds) != 0)
            {
                File.SetCreationTime(this.FullFilename, newFileTimestamp.Value);
            }

            try
            {
                oldFileTimestamp = File.GetLastWriteTime(this.FullFilename);
            }
            catch
            {
                oldFileTimestamp = null;
            }
            if ((!oldFileTimestamp.HasValue) || Math.Round((oldFileTimestamp.Value - newFileTimestamp.Value).TotalSeconds) != 0)
            {
                File.SetLastWriteTime(this.FullFilename, newFileTimestamp.Value);
            }
        }

        #endregion


        #region Static properties

        private static Type[] _all = null;
        private static Type[] All
        {
            get
            {
                if (Processor._all == null)
                {
                    List<Type> all = new List<Type>();
                    Type baseType = typeof(Processor);
                    Type[] skipTypes = new Type[] { typeof(UnhandledFileTypeProcessor), typeof(FailedProcessor) };
                    foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        foreach (Type type in assembly.GetTypes())
                        {
                            if (Array.IndexOf(skipTypes, type) >= 0)
                            {
                                continue;
                            }
                            if (type.IsAbstract)
                            {
                                continue;
                            }
                            for (Type parentType = type.BaseType; parentType != null; parentType = parentType.BaseType)
                            {
                                if (parentType == baseType)
                                {
                                    all.Add(type);
                                    break;
                                }
                            }
                        }
                    }
                    Processor._all = all.ToArray();
                }
                return Processor._all;
            }
        }

        private static Dictionary<string, List<Type>> _processorsPerExtension = null;
        private static Dictionary<string, List<Type>> ProcessorsPerExtension
        {
            get
            {
                if (Processor._processorsPerExtension == null)
                {
                    Dictionary<string, List<Type>> processorsPerExtension = new Dictionary<string, List<Type>>();
                    foreach (Type type in Processor.All)
                    {
                        foreach (string extension in (string[])type.GetProperty("HandledExtensions", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null, null))
                        {
                            string lowerExtension = extension.ToLowerInvariant();
                            if (!processorsPerExtension.ContainsKey(lowerExtension))
                            {
                                processorsPerExtension.Add(lowerExtension, new List<Type>());
                            }
                            processorsPerExtension[lowerExtension].Add(type);
                        }
                    }
                    BindingFlags priorityBindingFlags = BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetProperty;
                    foreach (KeyValuePair<string, List<Type>> kv in processorsPerExtension)
                    {

                        kv.Value.Sort(delegate (Type a, Type b)
                        {

                            return (int)b.GetProperty("Priority", priorityBindingFlags).GetValue(null, null) - (int)a.GetProperty("Priority", priorityBindingFlags).GetValue(null, null);
                        });
                    }
                    Processor._processorsPerExtension = processorsPerExtension;
                }
                return Processor._processorsPerExtension;
            }
        }

        #endregion


        #region Static methods

        public static Processor Get(string fullFilename)
        {
            Processor result = null;
            Exception error = null;
            string lowerExtension = Path.GetExtension(fullFilename).TrimStart('.').ToLower();
            if (Processor.ProcessorsPerExtension.ContainsKey(lowerExtension))
            {
                foreach (Type processorType in Processor.ProcessorsPerExtension[lowerExtension])
                {
                    try
                    {
                        result = (Processor)processorType.GetConstructor(new Type[] { typeof(string) }).Invoke(new object[] { fullFilename });
                        if (result._info.TimestampMin.HasValue)
                        {
                            break;
                        }
                    }
                    catch (Exception x)
                    {
                        if (error == null)
                        {
                            error = (x is TargetInvocationException && x.InnerException != null && !string.IsNullOrEmpty(x.InnerException.Message)) ? x.InnerException : x;
                        }
                    }
                }
            }
            if (result != null)
            {
                try
                {
                    switch (MediaData.Properties.Settings.Default.SetFileDates)
                    {
                        case SetFileDates.ReadingWriting:
                            result.UpdateFileTimestamp();
                            break;
                    }
                }
                catch
                { }
                return result;
            }
            else if (error != null)
            {
                return new FailedProcessor(fullFilename, error);
            }
            else
            {
                return new UnhandledFileTypeProcessor(fullFilename);
            }
        }

        #endregion

    }
}
