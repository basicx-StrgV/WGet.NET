//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WGetNET.Helper
{
    /// <summary>
    /// The <see langword="static"/> <see cref="WGetNET.Helper.FileHelper"/> class provides methods for working with files.
    /// </summary>
    internal static class FileHelper
    {
        /// <summary>
        /// Writes the provided text to the given file.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <param name="text">The text to write to the file.</param>
        /// <exception cref="System.ArgumentException">
        /// Path contains one or more invalid characters as defined by <see cref="System.IO.Path.InvalidPathChars"/>.
        /// </exception>
        /// <exception cref="System.IO.PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">
        /// The root of the specified path is invalid.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// An I/O error occurred while opening the file
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// Path specified a file that is read-only. 
        /// Or Path specified a file that is hidden.
        /// Or This operation is not supported on the current platform. 
        /// Or Path specified a directory. 
        /// Or The caller does not have the required permission.
        /// </exception>
        /// <exception cref="System.NotSupportedException">
        /// Path is in an invalid format.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        public static void WriteTextToFile(string path, string text)
        {
            string? directory = Path.GetDirectoryName(path);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(path, text);
        }

        /// <summary>
        /// Asyncounes writes the provided text to the given file.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <param name="text">The text to write to the file.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <exception cref="System.ArgumentException">
        /// Path contains one or more invalid characters as defined by <see cref="System.IO.Path.InvalidPathChars"/>.
        /// </exception>
        /// <exception cref="System.IO.PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">
        /// The specified path is invalid (for example, it is on an unmapped drive).
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// An I/O error occurred while opening the file
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// Path specified a file that is read-only. 
        /// Or Path specified a file that is hidden.
        /// Or This operation is not supported on the current platform. 
        /// Or Path specified a directory. 
        /// Or The caller does not have the required permission.
        /// </exception>
        /// <exception cref="System.NotSupportedException">
        /// Path is in an invalid format.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        public static async Task WriteTextToFileAsync(string path, string text, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            string? directory = Path.GetDirectoryName(path);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

#if NETCOREAPP3_1_OR_GREATER
            await File.WriteAllTextAsync(path, text, cancellationToken);
#elif NETSTANDARD2_0
            // Delete file to recreate it
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            // Create file and write the string to the stream
            using StreamWriter fileStream = File.CreateText(path);
            await fileStream.WriteAsync(text);
            fileStream.Close();
#endif
        }
    }
}
