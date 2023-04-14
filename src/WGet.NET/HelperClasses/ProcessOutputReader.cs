﻿//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

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
            //Get top line start.
            //The array should always contain this line.
            //If it dose not contain this line the resulting out of range exception,
            //that will be thrown later, will be catched in the calling method.
            var labelLine = ArrayManager.GetEntryContains(output, "------") - 1;

            if(labelLine == -1)
            {
                return new List<WinGetPackage>();
            }

            var columns = GetColumnInformation(output[labelLine]);

            //Remove unneeded output Lines
            output = ArrayManager.RemoveRange(output, 0, labelLine + 2);

            return CreatePackageListFromOutput(output, columns);
        }

        /// <summary>
        /// Creates a package list from output.
        /// </summary>
        /// <param name="output">
        /// The <see langword="array"/> containing the output.
        /// </param>
        /// <param name="columns">
        /// The <see cref="ColumnInfoList"/> representing an IList{<see cref="ColumnInfo"/>}
        /// </param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/>'s.
        /// </returns>
        private static List<WinGetPackage> CreatePackageListFromOutput(string[] output, ColumnInfoList columns)
        {
            var resultList = new List<WinGetPackage>();

            for (var line = 0; line < output.Length; line++)
            {
                try
                {
                    var package = new WinGetPackage()
                    {
                        PackageName = output[line][columns[0].StartIndex..columns[0].End].Trim(),
                        PackageId = output[line][columns[1].StartIndex..columns[1].End].Trim(),
                        PackageVersion = output[line][columns[2].StartIndex..columns[2].End].Trim(),
                        Source = output[line][columns[^1].StartIndex..].Trim()
                    };

                    if (!package.IsEmpty)
                    {
                        resultList.Add(package);
                    }
                }
                catch
                {
                    //Invalid entrys can be skiped
                }
            }

            return resultList;
        }

        /// <summary>
        /// Converts a <see cref="System.Collections.Generic.List{T}"/> 
        /// of output lines from a winget process to a list of <see cref="WinGetUpdatePackage"/>'s.
        /// </summary>
        /// <param name="output">
        /// A <see cref="System.Collections.Generic.List{T}"/> of output lines from a winget process.
        /// </param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WinGetUpdatePackage"/>'s.
        /// </returns>
        public static List<WinGetUpdatePackage> ToUpdatePackageList(string[] output)
        {
            //Get top line start.
            var labelLine = ArrayManager.GetEntryContains(output, "------") - 1;

            if (labelLine < 0)
            {
                // this will happen, when there are no updates
                return new List<WinGetUpdatePackage>();
            }

            var columns = GetColumnInformation(output[labelLine]);

            //Remove unneeded output Lines
            output = ArrayManager.RemoveRange(output, 0, labelLine + 2);

            return CreateUpdatePackageListFromOutput(output, columns);
        }

        /// <summary>
        /// Creates an updatable package list from output.
        /// </summary>
        /// <param name="output">
        /// The <see langword="array"/> containing the output.
        /// </param>
        /// <param name="columns">
        /// The <see cref="ColumnInfoList"/> representing an IList{<see cref="ColumnInfo"/>}
        /// </param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WinGetUpdatePackage"/>'s.
        /// </returns>
        private static List<WinGetUpdatePackage> CreateUpdatePackageListFromOutput(string[] output, ColumnInfoList columns)
        {
            var resultList = new List<WinGetUpdatePackage>();

            for (var line = 0; line < output.Length; line++)
            {
                try
                {
                    var update = new WinGetUpdatePackage()
                    {
                        PackageName = output[line][columns[0].StartIndex..columns[0].End].Trim(),
                        PackageId = output[line][columns[1].StartIndex..columns[1].End].Trim(),
                        PackageVersion = output[line][columns[2].StartIndex..columns[2].End].Trim(),
                        UpdateVersion = output[line][columns[3].StartIndex..columns[3].End].Trim(),
                        Source = output[line][columns[4].StartIndex..].Trim()
                    };

                    if (!update.IsEmpty)
                    {
                        resultList.Add(update);
                    }
                }
                catch
                {
                    //Invalid entrys can be skiped
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
            //Get top line start.
            //The array should always contain this line.
            //If it dose not contain this line the resulting out of range exception,
            //that will be thrown later, will be catched in the calling method.
            int labelLine = ArrayManager.GetEntryContains(output, "------") - 1;

            //Get start indexes of each tabel colum
            //(The line starts wich the name followed by the id.
            //The start for the name is always 0 an therfor it is not configured here.)
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
        /// The <see cref="System.Int32"/> representing the start start of the url.
        /// </param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/>'s.
        /// </returns>
        private static List<WinGetSource> CreateSourceListFromOutput(string[] output, int urlStartIndex)
        {
            List<WinGetSource> resultList = new List<WinGetSource>();

            for (int i = 0; i < output.Length; i++)
            {
                // [var1..var2] : selects the start range from var1 to var2.
                // (eg. if var1 is 2 and var2 is 5, the selectet start range will be [2, 3, 4])
                // [var1..] : selects the start range from var1 to the end.
                // (eg. if var1 is 2 and the last start is 5, the selectet start range will be[2, 3, 4, 5])
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

        private static ColumnInfoList GetColumnInformation(string line)
        {
            var columns = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var infos = new ColumnInfoList();

            for (var columnIndex = 0; columnIndex < columns.Length; columnIndex++)
            {
                var column = columns[columnIndex];
                var start = line.IndexOf(column);

                if (start >= 0)
                {
                    var next = columnIndex + 1 == columns.Length ? line.Length : line.IndexOf(columns[columnIndex + 1]);

                    infos.Add(new ColumnInfo()
                    {
                        Name = column,
                        StartIndex = start,
                        End = next
                    });
                }
            }

            return infos;
        }

        /// <summary>
        /// Gets the start start of a column in the given line.
        /// </summary>
        /// <param name="line">
        /// A <see cref="System.String"/> representing the line that contains the column names.
        /// </param>
        /// <param name="column">
        /// A <see cref="System.Int32"/> representing the column to look for.
        /// </param>
        /// <returns>
        /// A <see cref="System.Int32"/> representing the start start of the column 
        /// or -1 if the column was not found.
        /// </returns>
        private static int GetColumnStartIndex(string line, int column)
        {
            var columns = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

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
