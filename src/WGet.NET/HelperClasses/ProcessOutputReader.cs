//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System.Text;
using System.Collections.Generic;

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
        /// <param name="action">
        /// Sets info about the action that is executet.
        /// </param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/>'s.
        /// </returns>
        public static List<WinGetPackage> ToPackageList(string[] output, PackageAction action = PackageAction.Default)
        {
            //Get top line index.
            //The array should always contain this line.
            //If it dose not contain this line the resulting out of range exception,
            //that will be thrown later, will be catched in the calling method.
            int labelLine = ArrayManager.GetEntryContains(output, "------") - 1;

            int[] columnList = GetColumnList(output[labelLine]);

            //Remove unneeded output Lines
            output = ArrayManager.RemoveRange(output, 0, labelLine + 2);

            return CreatePackageListFromOutput(output, columnList, action);
        }

        /// <summary>
        /// Creates a package list from output.
        /// </summary>
        /// <param name="output">
        /// The <see langword="array"/> containing the output.
        /// </param>
        /// <param name="columnList">
        /// A <see cref="System.Int32"/> <see langword="array"/> containing the column start indexes.
        /// </param>
        /// <param name="action">
        /// Sets info about the action that is executet.
        /// </param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/>'s.
        /// </returns>
        private static List<WinGetPackage> CreatePackageListFromOutput(string[] output, int[] columnList, PackageAction action = PackageAction.Default)
        {
            List<WinGetPackage> resultList = new List<WinGetPackage>();

            if (columnList.Length < 4)
            {
                return resultList;
            }

            for (int i = 0; i < output.Length; i++)
            {
                // Stop parsing the output when the end of the list is reached.
                if (string.IsNullOrWhiteSpace(output[i]) || output[i].Length < columnList[^1])
                {
                    break;
                }

                // [var1..var2] : selects the index range from var1 to var2
                // (eg. if var1 is 2 and var2 is 5, the selectet index range will be [2, 3, 4])
                WinGetPackage package = new WinGetPackage()
                {
                    PackageName = output[i][columnList[0]..columnList[1]].Trim(),
                    PackageId = output[i][columnList[1]..columnList[2]].Trim(),
                    PackageVersion = output[i][columnList[2]..columnList[3]].Trim(),
                    PackageAvailableVersion = output[i][columnList[2]..columnList[3]].Trim()
                };

                if ((action == PackageAction.UpgradeList || action == PackageAction.InstalledList) && columnList.Length >= 5)
                {
                    string availableVersion = output[i][columnList[3]..columnList[4]].Trim();
                    if (!string.IsNullOrWhiteSpace(availableVersion))
                    {
                        package.PackageAvailableVersion = availableVersion;
                    }
                }

                resultList.Add(package);
            }

            // Check for secondery list in output.
            if (ArrayManager.GetEntryContains(output, "------") != -1)
            {
                List<WinGetPackage> seconderyList = ToPackageList(output, action);
                resultList.AddRange(seconderyList);
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

            int[] columnList = GetColumnList(output[labelLine]);

            //Remove unneeded output Lines
            output = ArrayManager.RemoveRange(output, 0, labelLine + 2);

            return CreateSourceListFromOutput(output, columnList);
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
        /// <param name="columnList">
        /// A <see cref="System.Int32"/> <see langword="array"/> containing the column start indexes.
        /// </param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/>'s.
        /// </returns>
        private static List<WinGetSource> CreateSourceListFromOutput(string[] output, int[] columnList)
        {
            List<WinGetSource> resultList = new List<WinGetSource>();

            if (columnList.Length < 2)
            {
                return resultList;
            }

            for (int i = 0; i < output.Length; i++)
            {
                // [var1..var2] : selects the index range from var1 to var2.
                // (eg. if var1 is 2 and var2 is 5, the selectet index range will be [2, 3, 4])
                // [var1..] : selects the index range from var1 to the end.
                // (eg. if var1 is 2 and the last index is 5, the selectet index range will be[2, 3, 4, 5])
                resultList.Add(
                    new WinGetSource()
                    {
                        SourceName = output[i][columnList[0]..columnList[1]].Trim(),
                        SourceUrl = output[i][columnList[1]..].Trim(),
                        SourceType = string.Empty
                    });
            }

            return resultList;
        }
    
        /// <summary>
        /// Gets all column start indexes from the input line.
        /// </summary>
        /// <param name="line">
        /// A <see cref="System.String"/> containing the header, for column calculation.
        /// </param>
        /// <returns>
        /// A <see cref="System.Int32"/> <see langword="array"/> containing the column start indexes.
        /// </returns>
        private static int[] GetColumnList(string line)
        {
            int[] columns = new int[0];
            
            bool checkForColumn = true;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] != ((char)32) && checkForColumn)
                {
                    columns = ArrayManager.Add(columns, i);
                    checkForColumn = false;
                }
                else if (line[i] == ((char)32))
                {
                    checkForColumn = true;
                }
            }

            return columns;
        }
    }
}
