//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System.IO;

namespace WGetNET.HelperClasses
{
    internal static class FileHandler
    {
        /// <summary>
        /// Writes the export result to a file.
        /// </summary>
        /// <param name="result">
        /// The <see cref="WGetNET.ProcessResult"/> object containing the export data.
        /// </param>
        /// <param name="file">
        /// A <see cref="System.String"/> containing the file path and name.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the action was successfull and <see langword="false"/> if it failed.
        /// </returns>
        public static bool ExportOutputToFile(ProcessResult result, string file)
        {
            if (result.Success)
            {
                string outputString = ProcessOutputReader.ExportOutputToString(result);

                File.WriteAllText(
                    file,
                    outputString);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
