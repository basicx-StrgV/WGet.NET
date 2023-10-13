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
        /// <param name="sourceName">
        /// Name of the source used in the search or list by source action.
        /// </param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/>'s.
        /// </returns>
        public static List<WinGetPackage> ToPackageList(string[] output, PackageAction action = PackageAction.Default, string? sourceName = null)
        {
            //Get top line index.
            //The array should always contain this line.
            //If it dose not contain this line the resulting out of range exception,
            //that will be thrown later, will be catched in the calling method.
            int labelLine = ArrayManager.GetEntryContains(output, "------") - 1;

            int[] columnList = GetColumnList(output[labelLine]);

            //Remove unneeded output Lines
            output = ArrayManager.RemoveRange(output, 0, labelLine + 2);

            return CreatePackageListFromOutput(output, columnList, action, sourceName);
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
        /// <param name="sourceName">
        /// Name of the source used in the search or list by source action.
        /// </param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/>'s.
        /// </returns>
        private static List<WinGetPackage> CreatePackageListFromOutput(string[] output, int[] columnList, PackageAction action = PackageAction.Default, string? sourceName = null)
        {
            List<WinGetPackage> resultList = new();

            if (columnList.Length < 3)
            {
                return resultList;
            }

            for (int i = 0; i < output.Length; i++)
            {
                // Stop parsing the output when the end of the list is reached.
#if NETCOREAPP3_1_OR_GREATER
                if (string.IsNullOrWhiteSpace(output[i]) || output[i].Length < columnList[^1])
                {
                    break;
                }
#elif NETSTANDARD2_0
                if (string.IsNullOrWhiteSpace(output[i]) || output[i].Length < columnList[columnList.Length-1])
                {
                    break;
                }
#endif

                WinGetPackage package = new()
                {
#if NETCOREAPP3_1_OR_GREATER
                    PackageName = output[i][columnList[0]..columnList[1]].Trim(),
                    PackageId = output[i][columnList[1]..columnList[2]].Trim()
#elif NETSTANDARD2_0
                    PackageName = output[i].Substring(columnList[0], (columnList[1] - columnList[0])).Trim(),
                    PackageId = output[i].Substring(columnList[1], (columnList[2] - columnList[1])).Trim()
#endif
                };

                //Set version info depending on the column count.
                if (columnList.Length > 3)
                {
#if NETCOREAPP3_1_OR_GREATER
                    package.PackageVersion = output[i][columnList[2]..columnList[3]].Trim();
                    package.PackageAvailableVersion = output[i][columnList[2]..columnList[3]].Trim();
#elif NETSTANDARD2_0
                    package.PackageVersion = output[i].Substring(columnList[2], (columnList[3] - columnList[2])).Trim();
                    package.PackageAvailableVersion = output[i].Substring(columnList[2], (columnList[3] - columnList[2])).Trim();
#endif
                }
                else
                {
#if NETCOREAPP3_1_OR_GREATER
                    package.PackageVersion = output[i][columnList[2]..].Trim();
                    package.PackageAvailableVersion = output[i][columnList[2]..].Trim();
#elif NETSTANDARD2_0
                    package.PackageVersion = output[i].Substring(columnList[2]).Trim();
                    package.PackageAvailableVersion = output[i].Substring(columnList[2]).Trim();
#endif
                }

                //Set information, depending on the action and the column count.
                if ((action == PackageAction.UpgradeList || action == PackageAction.InstalledList || action == PackageAction.Search) && columnList.Length >= 5)
                {
#if NETCOREAPP3_1_OR_GREATER
                    string availableVersion = output[i][columnList[3]..columnList[4]].Trim();
#elif NETSTANDARD2_0
                    string availableVersion = output[i].Substring(columnList[3], (columnList[4] - columnList[3])).Trim();
#endif
                    if (!string.IsNullOrWhiteSpace(availableVersion) && action != PackageAction.Search)
                    {
                        package.PackageAvailableVersion = availableVersion;
                    }

#if NETCOREAPP3_1_OR_GREATER
                    package.PackageSourceName = output[i][columnList[4]..].Trim();
#elif NETSTANDARD2_0
                    package.PackageSourceName = output[i].Substring(columnList[4]).Trim();
#endif
                }
                else if((action == PackageAction.InstalledList || action == PackageAction.Search) && columnList.Length == 4)
                {
#if NETCOREAPP3_1_OR_GREATER
                    package.PackageSourceName = output[i][columnList[3]..].Trim();
#elif NETSTANDARD2_0
                    package.PackageSourceName = output[i].Substring(columnList[3]).Trim();
#endif
                }
                else if ((action == PackageAction.SearchBySource || action == PackageAction.InstalledListBySource) && !string.IsNullOrWhiteSpace(sourceName))
                {
#pragma warning disable IDE0079 
                    // Remove info about the unnecessary suppression, that is shown by .NET Core 3.1, because .Net Standard 2.0 needs to know that not me.
#pragma warning disable CS8601
                    // "sourceName" source name cant't be null here because of the following check "!string.IsNullOrWhiteSpace(sourceName)".
                    // But .NET Standard 2.0 thinks it knows better. (Or I'm stupid)
                    package.PackageSourceName = sourceName;
#pragma warning restore CS8601
#pragma warning restore IDE0079
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
                StringBuilder outputBuilder = new();
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
            List<WinGetSource> resultList = new();

            if (columnList.Length < 2)
            {
                return resultList;
            }

            for (int i = 0; i < output.Length; i++)
            {
                resultList.Add(
                    new WinGetSource()
                    {
#if NETCOREAPP3_1_OR_GREATER
                        SourceName = output[i][columnList[0]..columnList[1]].Trim(),
                        SourceUrl = output[i][columnList[1]..].Trim(),
#elif NETSTANDARD2_0
                        SourceName = output[i].Substring(columnList[0], (columnList[1] - columnList[0])).Trim(),
                        SourceUrl = output[i].Substring(columnList[1]).Trim(),
#endif
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
