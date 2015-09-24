using System;
using System.Runtime.InteropServices;

namespace MLocati.MediaData
{
    public class MyIO
    {
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
    }
}
