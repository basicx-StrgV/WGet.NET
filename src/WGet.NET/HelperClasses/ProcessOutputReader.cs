//--------------------------------------------------//
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
            var labelLine = ArrayManager.GetEntryContains(output, "------") - 1;

            if (labelLine < 0)
            {
                return new List<WinGetPackage>();
            }

            var columns = GetColumnInformation(output[labelLine]);

            //Remove unneeded output Lines
            output = ArrayManager.RemoveRange(output, 0, labelLine + 2);

            return CreatePackageListFromOutput(output, columns);
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
            var labelLine = ArrayManager.GetEntryContains(output, "------") - 1;

            if (labelLine < 0)
            {
                // this will happen, when there are no updates
                return new List<WinGetSource>();
            }

            var columns = GetColumnInformation(output[labelLine]);

            //Remove unneeded output Lines
            output = ArrayManager.RemoveRange(output, 0, labelLine + 2);

            return CreateSourceListFromOutput(output, columns);
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
            if (!result.Success)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            foreach (var line in result.Output)
            {
                sb.Append(line);
            }

            return sb.ToString().Trim();
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
        /// Creates a source list from output.
        /// </summary>
        /// <param name="output">
        /// The <see langword="array"/> containing the output.
        /// </param>
        /// <param name="columns">
        /// The <see cref="ColumnInfoList"/> representing an IList{<see cref="ColumnInfo"/>}
        /// </param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WinGetSource"/>'s.
        /// </returns>
        private static List<WinGetSource> CreateSourceListFromOutput(string[] output, ColumnInfoList columns)
        {
            var sources = new List<WinGetSource>();

            for (var line = 0; line < output.Length; line++)
            {
                try
                {
                    var source = new WinGetSource()
                    {
                        SourceName = output[line][columns[0].StartIndex..columns[0].End].Trim(),
                        SourceUrl = output[line][columns[1].StartIndex..].Trim()
                    };

                    if (!source.IsEmpty)
                    {
                        sources.Add(source);
                    }
                }
                catch
                {
                    //Invalid entrys can be skiped
                }
            }

            return sources;
        }

        /// <summary>
        /// Splits a line into columns and returns the columns found in a <see cref="ColumnInfoList"/>
        /// </summary>
        /// <param name="line">the line to analyze</param>
        /// <returns>A <see cref="ColumnInfoList"/> representing an <see cref="IList{T}"/> where T : <see cref="ColumnInfo"/></returns>
        private static ColumnInfoList GetColumnInformation(string line)
        {
            var columns = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var infos = new ColumnInfoList();

            for (var columnIndex = 0; columnIndex < columns.Length; columnIndex++)
            {
                var column = columns[columnIndex];
                var start = line.IndexOf(column);
                var next = columnIndex + 1 == columns.Length ? line.Length : line.IndexOf(columns[columnIndex + 1]);

                infos.Add(new ColumnInfo()
                {
                    Name = column,
                    StartIndex = start,
                    End = next
                });
            }

            return infos;
        }

    }
}
