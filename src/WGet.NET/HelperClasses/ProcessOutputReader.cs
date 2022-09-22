//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System.Collections.Generic;
using System.Text;

namespace WGetNET.HelperClasses
{
    /// <summary>
    /// The <see langword="static"/> <see cref="WGetNET.HelperClasses.ProcessOutputReader"/> class,
    /// provieds <see langword="static"/> methodes to process the output of the winget processes.
    /// </summary>
    internal static class ProcessOutputReader
    {
        /// <summary>
        /// Converts a <see cref="System.Collections.Generic.List{T}"/> 
        /// of output lines from a winget process to a list of <see cref="WGetNET.WinGetPackage"/>'s.
        /// </summary>
        /// <param name="output">
        /// A <see cref="System.Collections.Generic.List{T}"/> of output lines from a winget process.
        /// </param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/>'s.
        /// </returns>
        public static List<WinGetPackage> ToPackageList(string[] output)
        {
            //Get top line index.
            //The array should always contain this line.
            //If it dose not contain this line the resulting out of range exception,
            //that will be thrown later, will be catched in the calling method.
            int labelLine = ArrayManager.GetEntryContains(output, "------") - 1;

            //Get start indexes of each tabel colum
            //(The line starts wich the name followed by the id.
            //The index for the name is always 0 an therfor it is not configured here.)
            int idStartIndex = GetColumnStartIndex(output[labelLine], 2);
            int versionStartIndex = GetColumnStartIndex(output[labelLine], 3);
            int extraInfoStartIndex = GetColumnStartIndex(output[labelLine], 4);

            //Remove unneeded output Lines
            output = ArrayManager.RemoveRange(output, 0, labelLine + 2);

            return CreatePackageListFromOutput(output, idStartIndex, versionStartIndex, extraInfoStartIndex);
        }

        /// <summary>
        /// Creates a package list from output.
        /// </summary>
        /// <param name="output">
        /// The <see langword="array"/> containing the output.
        /// </param>
        /// <param name="idStartIndex">
        /// The <see cref="System.Int32"/> representing the start index of the id.
        /// </param>
        /// <param name="versionStartIndex">
        /// The <see cref="System.Int32"/> representing the start index of the version.
        /// </param>
        /// <param name="extraInfoStartIndex">
        /// The <see cref="System.Int32"/> representing the start index of the extra information.
        /// </param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/>'s.
        /// </returns>
        private static List<WinGetPackage> CreatePackageListFromOutput(string[] output, int idStartIndex, int versionStartIndex, int extraInfoStartIndex)
        {
            List<WinGetPackage> resultList = new List<WinGetPackage>();

            for (int i = 0; i < output.Length; i++)
            {
                // [var1..var2] : selects the index range from var1 to var2
                // (eg. if var1 is 2 and var2 is 5, the selectet index range will be [2, 3, 4])
                if (output[i].Length >= extraInfoStartIndex && !string.IsNullOrWhiteSpace(output[i])) {
                    resultList.Add(
                        new WinGetPackage()
                        {
                            PackageName = output[i][0..idStartIndex].Trim(),
                            PackageId = output[i][idStartIndex..versionStartIndex].Trim(),
                            PackageVersion = output[i][versionStartIndex..extraInfoStartIndex].Trim()
                        });
                }
            }

            return resultList;
        }

        /// <summary>
        /// Converts a <see cref="System.Collections.Generic.List{T}"/> 
        /// of output lines from a winget process to a list of <see cref="WGetNET.WinGetSource"/>'s.
        /// </summary>
        /// <param name="output">
        /// A <see cref="System.Collections.Generic.List{T}"/> of output lines from a winget process.
        /// </param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/>'s.
        /// </returns>
        public static List<WinGetSource> ToSourceList(string[] output)
        {
            //Get top line index.
            //The array should always contain this line.
            //If it dose not contain this line the resulting out of range exception,
            //that will be thrown later, will be catched in the calling method.
            int labelLine = ArrayManager.GetEntryContains(output, "------") - 1;

            //Get start indexes of each tabel colum
            //(The line starts wich the name followed by the id.
            //The index for the name is always 0 an therfor it is not configured here.)
            int urlStartIndex = GetColumnStartIndex(output[labelLine], 2);

            //Remove unneeded output Lines
            output = ArrayManager.RemoveRange(output, 0, labelLine + 2);

            return CreateSourceListFromOutput(output, urlStartIndex);
        }

        /// <summary>
        /// Writes the export result to a <see cref="System.String"/>.
        /// </summary>
        /// <param name="result">
        /// The <see cref="WGetNET.ProcessResult"/> object containing the export data.
        /// </param>
        /// <returns>
        /// The <see cref="System.String"/> containing the export result.
        /// </returns>
        public static string ExportOutputToString(ProcessResult result)
        {
            if (result.Success)
            {
                StringBuilder outputBuilder = new StringBuilder();
                foreach (string line in result.Output)
                {
                    outputBuilder.Append(line);
                }

                return outputBuilder.ToString().Trim();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Creates a source list from output.
        /// </summary>
        /// <param name="output">
        /// The <see langword="array"/> containing the output.
        /// </param>
        /// <param name="urlStartIndex">
        /// The <see cref="System.Int32"/> representing the start index of the url.
        /// </param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/>'s.
        /// </returns>
        private static List<WinGetSource> CreateSourceListFromOutput(string[] output, int urlStartIndex)
        {
            List<WinGetSource> resultList = new List<WinGetSource>();

            for (int i = 0; i < output.Length; i++)
            {
                // [var1..var2] : selects the index range from var1 to var2.
                // (eg. if var1 is 2 and var2 is 5, the selectet index range will be [2, 3, 4])
                // [var1..] : selects the index range from var1 to the end.
                // (eg. if var1 is 2 and the last index is 5, the selectet index range will be[2, 3, 4, 5])
                resultList.Add(
                    new WinGetSource()
                    {
                        SourceName = output[i][0..urlStartIndex].Trim(),
                        SourceUrl = output[i][urlStartIndex..].Trim(),
                        SourceType = string.Empty
                    });
            }

            return resultList;
        }
    
        /// <summary>
        /// Gets the start index of a column in the given line.
        /// </summary>
        /// <param name="line">
        /// A <see cref="System.String"/> representing the line that contains the column names.
        /// </param>
        /// <param name="column">
        /// A <see cref="System.Int32"/> representing the column to look for.
        /// </param>
        /// <returns>
        /// A <see cref="System.Int32"/> representing the start index of the column 
        /// or -1 if the column was not found.
        /// </returns>
        private static int GetColumnStartIndex(string line, int column)
        {
            int currentColumn = 0;
            bool checkForColumn = true;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] != ((char)32) && checkForColumn)
                {
                    currentColumn++;
                    if (currentColumn == column)
                    {
                        return i;
                    }
                    checkForColumn = false;
                }
                else if (line[i] == ((char)32))
                {
                    checkForColumn = true;
                }
            }
            return -1;
        }
    }
}
