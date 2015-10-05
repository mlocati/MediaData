using System;
using System.IO;
using System.Runtime.InteropServices;

namespace MLocati.MediaData
{
    public class MyIO
    {
        /// <summary>
        /// GUID of the current user Downloads folder (default is %USERPROFILE%\Downloads, but may be different)
        /// </summary>
        private static readonly Guid FOLDERID_DOWNLOADS = new Guid("{374DE290-123F-4565-9164-39C4925E467B}");

        /// <summary>
        /// Possible values of SHFILEOPSTRUCT.wFunc
        /// </summary>
        private enum FileOperation : uint
        {
            /// <summary>
            /// Copy the files specified in the pFrom member to the location specified in the pTo member.
            /// </summary>
            Copy = 0x00000002,
            /// <summary>
            /// Delete the files specified in pFrom.
            /// </summary>
            Delete = 0x00000003,
            /// <summary>
            /// Move the files specified in pFrom to the location specified in pTo.
            /// </summary>
            Move = 0x00000001,
            /// <summary>
            /// Rename the file specified in pFrom.
            /// You cannot use this flag to rename multiple files with a single function call. Use Move instead.
            /// </summary>
            Rename = 0x00000004,
        }

        /// <summary>
        /// Possible values of SHFILEOPSTRUCT.fFlags
        /// </summary>
        [Flags]
        private enum FileOperationFlags : ushort
        {
            /// <summary>
            /// Preserve undo information, if possible.
            /// </summary>
            AllowUndo = 0x0040,
            /// <summary>
            /// Perform the operation only on files (not on folders) if a wildcard file name (*.*) is specified.
            /// </summary>
            FilesOnly = 0x0080,
            /// <summary>
            /// The pTo member specifies multiple destination files (one for each source file in pFrom) rather than one directory where all source files are to be deposited.
            /// </summary>
            MultiDestFiles = 0x0001,
            /// <summary>
            /// Respond with Yes to All for any dialog box that is displayed.
            /// </summary>
            NoConfirmation = 0x0010,
            /// <summary>
            /// Do not ask the user to confirm the creation of a new directory if the operation requires one to be created.
            /// </summary>
            NoConfirmMkDir = 0x0200,
            /// <summary>
            /// Do not move connected files as a group. Only move the specified files.
            /// Version 5.0
            /// </summary>
            NoConnectedElements = 0x2000,
            /// <summary>
            /// Do not copy the security attributes of the file.
            /// The destination file receives the security attributes of its new folder.
            /// Version 4.71
            /// </summary>
            NoCopySecurityAttribs = 0x0800,
            /// <summary>
            /// Do not display a dialog to the user if an error occurs.
            /// </summary>
            NoErrorUI = 0x0400,
            /// <summary>
            /// Only perform the operation in the local directory.
            /// Do not operate recursively into subdirectories, which is the default behavior.
            /// </summary>
            NoRecursion = 0x1000,
            /// <summary>
            /// Perform the operation silently, presenting no UI to the user.
            /// This is equivalent to Silent | NoConfirmation | NoErrorUI | NoConfirmMkDir.
            /// </summary>
            NoUI = FileOperationFlags.Silent | FileOperationFlags.NoConfirmation | FileOperationFlags.NoErrorUI | FileOperationFlags.NoConfirmMkDir,
            /// <summary>
            /// Give the file being operated on a new name in a move, copy, or rename operation if a file with the target name already exists at the destination.
            /// </summary>
            RenameOnCollision = 0x0008,
            /// <summary>
            /// Do not display a progress dialog box.
            /// </summary>
            Silent = 0x0004,
            /// <summary>
            /// Display a progress dialog box but do not show individual file names as they are operated on.
            /// </summary>
            SimpleProgress = 0x0100,
            /// <summary>
            /// If RenameOnCollision is specified and any files were renamed, assign a name mapping object that contains their old and new names to the hNameMappings member.
            /// This object must be freed using SHFreeNameMappings when it is no longer needed.
            /// </summary>
            WantMappingHandle = 0x0020,
            /// <summary>
            /// Send a warning if a file is being permanently destroyed during a delete operation rather than recycled.
            /// This flag partially overrides NoConfirmation.
            /// Version 5.0
            /// </summary>
            WantNukeWarning = 0x4000,
        }

        /// <summary>
        /// Specify special retrieval options for known folders.
        /// </summary>
        [Flags]
        public enum KnownFolderFlag : UInt32
        {
            None = 0x00000000,
            /// <summary>
            /// Build a simple IDList (PIDL).
            /// This value can be used when you want to retrieve the file system path but do not specify this value if you are retrieving the localized display name of the folder because it might not resolve correctly.
            /// </summary>
            SimpleIDList = 0x00000100,
            /// <summary>
            /// Gets the folder's default path independent of the current location of its parent.
            /// KnownFolderFlag.DefaultPath must also be set.
            /// </summary>
            NotParentRelative = 0x00000200,
            /// <summary>
            /// Gets the default path for a known folder.
            /// If this flag is not set, the function retrieves the current—and possibly redirected—path of the folder.
            /// The execution of this flag includes a verification of the folder's existence unless KnownFolderFlag.DontVerify is set.
            /// </summary>
            DefaultPath = 0x00000400,
            /// <summary>
            /// Initializes the folder using its Desktop.ini settings.
            /// If the folder cannot be initialized, the function returns a failure code and no path is returned.
            /// This flag should always be combined with KnownFolderFlag.Create.
            /// If the folder is located on a network, the function might take a longer time to execute.
            /// </summary>
            Init = 0x00000800,
            /// <summary>
            /// Gets the true system path for the folder, free of any aliased placeholders such as %USERPROFILE%, returned by SHGetKnownFolderIDList and IKnownFolder::GetIDList.
            /// This flag has no effect on paths returned by SHGetKnownFolderPath and IKnownFolder::GetPath.
            /// By default, known folder retrieval functions and methods return the aliased path if an alias exists.
            /// </summary>
            NoAlias = 0x00001000,
            /// <summary>
            /// Stores the full path in the registry without using environment strings.
            /// If this flag is not set, portions of the path may be represented by environment strings such as %USERPROFILE%.
            /// This flag can only be used with SHSetKnownFolderPath and IKnownFolder::SetPath.
            /// </summary>
            DontUnexpand = 0x00002000,
            /// <summary>
            /// Do not verify the folder's existence before attempting to retrieve the path or IDList.
            /// If this flag is not set, an attempt is made to verify that the folder is truly present at the path.
            /// If that verification fails due to the folder being absent or inaccessible, the function returns a failure code and no path is returned.
            /// If the folder is located on a network, the function might take a longer time to execute.
            /// Setting this flag can reduce that lag time.
            /// </summary>
            DontVerify = 0x00004000,
            /// <summary>
            /// Forces the creation of the specified folder if that folder does not already exist.
            /// The security provisions predefined for that folder are applied.
            /// If the folder does not exist and cannot be created, the function returns a failure code and no path is returned.
            /// This value can be used only with the following functions and methods:
            /// - SHGetKnownFolderPath
            /// - SHGetKnownFolderIDList
            /// - IKnownFolder::GetIDList
            /// - IKnownFolder::GetPath
            /// - IKnownFolder::GetShellItem
            /// </summary>
            Create = 0x00008000,
            /// <summary>
            /// When running inside an app container, or when providing an app container token, this flag prevents redirection to app container folders.
            /// Instead, it retrieves the path that would be returned where it not running inside an app container.
            /// </summary>
            /// <remarks>
            /// Introduced in Windows 7
            /// </remarks>
            NoAppcontainerRedirection = 0x00010000,
            /// <summary>
            /// Return only aliased PIDLs.
            /// Do not use the file system path.
            /// </summary>
            /// <remarks>
            /// Introduced in Windows 7
            /// </remarks>
            AliasOnly = 0x80000000,
        }


        /// <summary>
        /// Contains information that the SHFileOperation function uses to perform file operations.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SHFILEOPSTRUCT
        {
            /// <summary>
            /// A window handle to the dialog box to display information about the status of the file operation.
            /// </summary>
            public IntPtr hwnd;
            /// <summary>
            /// A value that indicates which operation to perform.
            /// </summary>
            [MarshalAs(UnmanagedType.U4)]
            public FileOperation wFunc;
            /// <summary>
            /// A pointer to one or more source file names.
            /// These names should be fully qualified paths to prevent unexpected results.
            /// Standard MS-DOS wildcard characters, such as "*", are permitted only in the file-name position. Using a wildcard character elsewhere in the string will lead to unpredictable results.
            /// It is a buffer that can hold multiple null-delimited file names.
            /// Each file name is terminated by a single NULL character.
            /// The last file name is terminated with a double NULL character ("\0\0") to indicate the end of the buffer.
            /// </summary>
            public string pFrom;
            /// <summary>
            /// A pointer to the destination file or directory name.
            /// This parameter must be set to NULL if it is not used.
            /// Wildcard characters are not allowed. Their use will lead to unpredictable results.
            /// </summary>
            public string pTo;
            /// <summary>
            /// Flags that control the file operation.
            /// </summary>
            public FileOperationFlags fFlags;
            /// <summary>
            /// When the function returns, this member contains TRUE if any file operations were aborted before they were completed; otherwise, FALSE.
            /// An operation can be manually aborted by the user through UI or it can be silently aborted by the system if the NoErrorUI or NoConfirmation flags were set.
            /// </summary>
            [MarshalAs(UnmanagedType.Bool)]
            public bool fAnyOperationsAborted;
            /// <summary>
            /// When the function returns, this member contains a handle to a name mapping object that contains the old and new names of the renamed files.
            /// This member is used only if the fFlags member includes the WantMappingHandle flag.
            /// </summary>
            public IntPtr hNameMappings;
            /// <summary>
            /// A pointer to the title of a progress dialog box.
            /// This is a null-terminated string.
            /// This member is used only if fFlags includes the SimpleProgress flag.
            /// </summary>
            public string lpszProgressTitle;
        }

        /// <summary>
        /// Copies, moves, renames, or deletes a file system object.
        /// </summary>
        /// <param name="lpFileOp">An SHFILEOPSTRUCT structure that contains information this function needs to carry out the specified operation.</param>
        /// <returns>Returns zero if successful; otherwise nonzero.</returns>
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SHFileOperation(ref SHFILEOPSTRUCT lpFileOp);

        /// <summary>
        /// Retrieves the full path of a known folder identified by the folder's KNOWNFOLDERID.
        /// </summary>
        /// <param name="rfid">A reference to the KNOWNFOLDERID that identifies the folder.</param>
        /// <param name="dwFlags">Flags that specify special retrieval options. This value can be one or more of the KnownFolderFlag values.</param>
        /// <param name="hToken">An access token that represents a particular user. If this parameter is IntPtr.Zero, which is the most common usage, the function requests the known folder for the current user.</param>
        /// <param name="ppszPath">When this method returns, contains the address of a pointer to a null-terminated Unicode string that specifies the path of the known folder. The calling process is responsible for freeing this resource once it is no longer needed by calling CoTaskMemFree. The returned path does not include a trailing backslash. For example, "C:\Users" is returned rather than "C:\Users\".</param>
        /// <returns></returns>
        [DllImport("shell32.dll")]
        private static extern Int32 SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, KnownFolderFlag dwFlags, IntPtr hToken, out IntPtr ppszPath);

        private static void CallSHFileOperation(string pFrom, FileOperation operation, FileOperationFlags flags)
        {
            SHFILEOPSTRUCT sfos = new SHFILEOPSTRUCT();
            sfos.wFunc = operation;
            sfos.pFrom = pFrom + "\0\0";
            sfos.fFlags = flags;
            if (SHFileOperation(ref sfos) != 0)
            {
                throw new Exception(string.Format(i18n.System_function_X_failed, "SHFileOperation"));
            }
            if (sfos.fAnyOperationsAborted)
            {
                throw new OperationCanceledException();
            }
        }

        public static void DeleteToTrash(string filename)
        {
            MyIO.CallSHFileOperation(
                filename,
                FileOperation.Delete,
                FileOperationFlags.AllowUndo | FileOperationFlags.NoConfirmation | FileOperationFlags.WantNukeWarning | FileOperationFlags.Silent
            );
        }

        public static string GetDownloadsFolder()
        {
            string path = null;
            try
            {
                IntPtr pPath = IntPtr.Zero;
                try
                {
                    int hr = MyIO.SHGetKnownFolderPath(
                        MyIO.FOLDERID_DOWNLOADS,
                        KnownFolderFlag.NoAlias,
                        IntPtr.Zero,
                        out pPath
                    );
                    if (hr != 0)
                    {
                        throw Marshal.GetExceptionForHR(hr);
                    }
                    path = Marshal.PtrToStringUni(pPath);
                }
                finally
                {
                    if (pPath != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(pPath);
                        pPath = IntPtr.Zero;
                    }
                }
            }
            catch
            { }
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
            {
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                if (!Directory.Exists(path))
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    if (!Directory.Exists(path))
                    {
                        path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                        if (!Directory.Exists(path))
                        {
                            throw new DirectoryNotFoundException("Unable to find the Downloads folder");
                        }
                    }
                }
            }
            return path;
        }
    }
}
