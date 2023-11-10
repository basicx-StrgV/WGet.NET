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
                // Deleate file to recreate it
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
                // Create file and write the string to the stream
                using StreamWriter fileStream = File.CreateText(file);
                await fileStream.WriteAsync(outputString);
#endif

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
