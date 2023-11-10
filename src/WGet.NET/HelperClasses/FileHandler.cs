//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System.IO;
using System.Threading.Tasks;
using WGetNET.Models;

namespace WGetNET.HelperClasses
{
    internal static class FileHandler
    {
        /// <summary>
        /// Writes the export result to a file.
        /// </summary>
        /// <param name="result">
        /// The <see cref="WGetNET.Models.ProcessResult"/> object containing the export data.
        /// </param>
        /// <param name="file">
        /// A <see cref="System.String"/> containing the file path and name.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the action was successful and <see langword="false"/> if it failed.
        /// </returns>
        public static bool ExportOutputToFile(ProcessResult result, string file)
        {
            if (result.Success)
            {
                string outputString = ProcessOutputReader.ExportOutputToString(result);

                File.WriteAllText(file, outputString);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Asynchronous writes the export result to a file.
        /// </summary>
        /// <param name="result">
        /// The <see cref="WGetNET.Models.ProcessResult"/> object containing the export data.
        /// </param>
        /// <param name="file">
        /// A <see cref="System.String"/> containing the file path and name.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the action was successful and <see langword="false"/> if it failed.
        /// </returns>
        public static async Task<bool> ExportOutputToFileAsync(ProcessResult result, string file)
        {
            if (result.Success)
            {
                string outputString = ProcessOutputReader.ExportOutputToString(result);

#if NETCOREAPP3_1_OR_GREATER
                await File.WriteAllTextAsync(file, outputString);
#elif NETSTANDARD2_0
                // Delete file to recreate it
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
                // Create file and write the string to the stream
                using StreamWriter fileStream = File.CreateText(file);
                await fileStream.WriteAsync(outputString);
                fileStream.Close();
#endif

                return true;
            }
            else
            {
                return false;
            }
        }

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
        public static void WriteTextToFile(string path, string text)
        {
            string? directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
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
        public static async Task WriteTextToFileAsync(string path, string text)
        {
            string? directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

#if NETCOREAPP3_1_OR_GREATER
            await File.WriteAllTextAsync(path, text);
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
