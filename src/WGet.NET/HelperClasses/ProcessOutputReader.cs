//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
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
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/>'s.
        /// </returns>
        public static List<WinGetPackage> ToPackageList(string[] output)
        {
            //Get top line index
            int labelLine = 0;
            for (int i = 0; i < output.Length; i++)
            {
                if (output[i].Contains("------") && i > 0)
                {
                    labelLine = i - 1;
                    break;
                }
            }

            //Get start indexes of each tabel colum
            int nameStartIndex = 0;

            int idStartIndex = -1;

            int versionStartIndex = -1;

            int extraInfoStartIndex = -1;

            bool checkForChar = false;
            for (int i = 0; i < output[labelLine].Length; i++)
            {
                if (output[labelLine][i] != ' ' && checkForChar)
                {
                    if (idStartIndex < 0)
                    {
                        idStartIndex = i;
                        checkForChar = false;
                    }
                    else if (versionStartIndex < 0)
                    {
                        versionStartIndex = i;
                        checkForChar = false;
                    }
                    else if (extraInfoStartIndex < 0)
                    {
                        extraInfoStartIndex = i;
                        checkForChar = false;
                    }
                    else if (idStartIndex >= 0 && versionStartIndex >= 0 && extraInfoStartIndex >= 0)
                    {
                        //Breake the loop if all indexes are set
                        break;
                    }
                }
                else if (output[labelLine][i] == ' ')
                {
                    checkForChar = true;
                }
            }

            //Remove unneeded output Lines
            output = ArrayManager.RemoveRange(output, 0, labelLine + 2);

            List<WinGetPackage> resultList = new List<WinGetPackage>();

            for (int i = 0; i < output.Length; i++)
            {
                // [var1..var2] : selects the index range from var1 to var2
                // (eg. if var1 is 2 and var2 is 5, the selectet index range will be [2, 3, 4])
                resultList.Add(
                    new WinGetPackage() 
                    { 
                        PackageName = output[i][nameStartIndex..idStartIndex].Trim(), 
                        PackageId = output[i][idStartIndex..versionStartIndex].Trim(), 
                        PackageVersion = output[i][versionStartIndex..extraInfoStartIndex].Trim()
                    });
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
            //Get top line index
            int labelLine = 0;
            for (int i = 0; i < output.Length; i++)
            {
                if (output[i].Contains("------") && i > 0)
                {
                    labelLine = i - 1;
                    break;
                }
            }

            //Get start indexes of each tabel colum
            int nameStartIndex = 0;

            int urlStartIndex = -1;

            bool checkForChar = false;
            for (int i = 0; i < output[labelLine].Length; i++)
            {
                if (output[labelLine][i] != ' ' && checkForChar)
                {
                    urlStartIndex = i;
                    break;
                }
                else if (output[labelLine][i] == ' ')
                {
                    checkForChar = true;
                }
            }

            //Remove unneeded output Lines
            output = ArrayManager.RemoveRange(output, 0, labelLine + 2);

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
                        SourceName = output[i][nameStartIndex..urlStartIndex].Trim(),
                        SourceUrl = output[i][urlStartIndex..].Trim()
                    });
            }

            return resultList;
        }
    }
}
