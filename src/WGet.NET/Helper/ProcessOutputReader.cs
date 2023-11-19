//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System.Text;
using System.Collections.Generic;
using WGetNET.Models;
using WGetNET.Builder;
using WGetNET.Extensions;

namespace WGetNET.Helper
{
    /// <summary>
    /// The <see langword="static"/> <see cref="WGetNET.Helper.ProcessOutputReader"/> class,
    /// provieds <see langword="static"/> methodes to process the output of the winget processes.
    /// </summary>
    internal static class ProcessOutputReader
    {
        //---To Package List------------------------------------------------------------------------------------------
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
            int labelLine = output.GetEntryContains("------") - 1;

            if (labelLine < 0)
            {
                // Output does not contain any entries
                return new List<WinGetPackage>();
            }

            int[] columnList = GetColumnList(output[labelLine]);

            //Remove unneeded output Lines
            output = output.RemoveRange(0, labelLine + 2);

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

            PackageBuilder builder = new();

            for (int i = 0; i < output.Length; i++)
            {
                builder.Clear();

                // Stop parsing the output when the end of the list is reached.
#if NETCOREAPP3_1_OR_GREATER
                if (string.IsNullOrWhiteSpace(output[i]) || output[i].Length < columnList[^1])
                {
                    break;
                }
#elif NETSTANDARD2_0
                if (string.IsNullOrWhiteSpace(output[i]) || output[i].Length < columnList[columnList.Length - 1])
                {
                    break;
                }
#endif

#if NETCOREAPP3_1_OR_GREATER
                builder.AddId(output[i][columnList[1]..columnList[2]].Trim());
#elif NETSTANDARD2_0
                builder.AddId(output[i].Substring(columnList[1], (columnList[2] - columnList[1])).Trim());
#endif

#if NETCOREAPP3_1_OR_GREATER
                builder.AddName(output[i][columnList[0]..columnList[1]].Trim());
#elif NETSTANDARD2_0
                builder.AddName(output[i].Substring(columnList[0], (columnList[1] - columnList[0])).Trim());
#endif

                //Set version info depending on the column count.
                if (columnList.Length > 3)
                {
#if NETCOREAPP3_1_OR_GREATER
                    builder.AddVersion(output[i][columnList[2]..columnList[3]].Trim());
                    builder.AddAvailableVersion(output[i][columnList[2]..columnList[3]].Trim());
#elif NETSTANDARD2_0
                    builder.AddVersion(output[i].Substring(columnList[2], (columnList[3] - columnList[2])).Trim());
                    builder.AddAvailableVersion(output[i].Substring(columnList[2], (columnList[3] - columnList[2])).Trim());
#endif
                }
                else
                {
#if NETCOREAPP3_1_OR_GREATER
                    builder.AddVersion(output[i][columnList[2]..].Trim());
                    builder.AddAvailableVersion(output[i][columnList[2]..].Trim());
#elif NETSTANDARD2_0
                    builder.AddVersion(output[i].Substring(columnList[2]).Trim());
                    builder.AddAvailableVersion(output[i].Substring(columnList[2]).Trim());
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
                        builder.AddAvailableVersion(availableVersion);
                    }

#if NETCOREAPP3_1_OR_GREATER
                    builder.AddSourceName(output[i][columnList[4]..].Trim());
#elif NETSTANDARD2_0
                    builder.AddSourceName(output[i].Substring(columnList[4]).Trim());
#endif
                }
                else if ((action == PackageAction.InstalledList || action == PackageAction.Search) && columnList.Length == 4)
                {
#if NETCOREAPP3_1_OR_GREATER
                    builder.AddSourceName(output[i][columnList[3]..].Trim());
#elif NETSTANDARD2_0
                    builder.AddSourceName(output[i].Substring(columnList[3]).Trim());
#endif
                }
                else if ((action == PackageAction.SearchBySource || action == PackageAction.InstalledListBySource)
                    && !string.IsNullOrWhiteSpace(sourceName) && sourceName != null)
                {
                    // "sourceName" source name cant't be null here because of the following check "!string.IsNullOrWhiteSpace(sourceName)".
                    // But .NET Standard 2.0 thinks it knows better (Or I'm stupid). Therefore a second null check comes after it.
                    builder.AddSourceName(sourceName);
                }

                resultList.Add(builder.GetInstance());
            }

            // Check for secondery list in output.
            if (output.GetEntryContains("------") != -1)
            {
                List<WinGetPackage> seconderyList = ToPackageList(output, action);
                resultList.AddRange(seconderyList);
            }

            return resultList;
        }
        //------------------------------------------------------------------------------------------------------------

        //---To Pinned Package List-----------------------------------------------------------------------------------
        /// <summary>
        /// Converts a <see cref="System.Collections.Generic.List{T}"/> 
        /// of output lines from a winget process to a list of <see cref="WGetNET.WinGetPinnedPackage"/>'s.
        /// </summary>
        /// <param name="output">
        /// The <see langword="array"/> of output lines from a winget process.
        /// </param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPinnedPackage"/>'s.
        /// </returns>
        public static List<WinGetPinnedPackage> ToPinnedPackageList(string[] output)
        {
            //Get top line index.
            //The array should always contain this line.
            //If it dose not contain this line the resulting out of range exception,
            //that will be thrown later, will be catched in the calling method.
            int labelLine = output.GetEntryContains("------") - 1;

            if (labelLine < 0)
            {
                // Output does not contain any entries
                return new List<WinGetPinnedPackage>();
            }

            int[] columnList = GetColumnList(output[labelLine], true);

            //Remove unneeded output Lines
            output = output.RemoveRange(0, labelLine + 2);

            return CreatePinnedPackageListFromOutput(output, columnList);
        }

        /// <summary>
        /// Creates a pinned package list from output.
        /// </summary>
        /// <param name="output">
        /// The <see langword="array"/> containing the output.
        /// </param>
        /// <param name="columnList">
        /// A <see cref="System.Int32"/> <see langword="array"/> containing the column start indexes.
        /// </param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPinnedPackage"/>'s.
        /// </returns>
        private static List<WinGetPinnedPackage> CreatePinnedPackageListFromOutput(string[] output, int[] columnList)
        {
            List<WinGetPinnedPackage> resultList = new();

            if (columnList.Length < 3)
            {
                return resultList;
            }

            PinnedPackageBuilder builder = new();

            for (int i = 0; i < output.Length; i++)
            {
                builder.Clear();

                // Stop parsing the output when the end of the list is reached.
#if NETCOREAPP3_1_OR_GREATER
                if (string.IsNullOrWhiteSpace(output[i]) || output[i].Length < columnList[^1])
                {
                    break;
                }
#elif NETSTANDARD2_0
                if (string.IsNullOrWhiteSpace(output[i]) || output[i].Length < columnList[columnList.Length - 1])
                {
                    break;
                }
#endif

#if NETCOREAPP3_1_OR_GREATER
                builder.AddName(output[i][columnList[0]..columnList[1]].Trim());
                builder.AddId(output[i][columnList[1]..columnList[2]].Trim());
                builder.AddVersion(output[i][columnList[2]..columnList[3]].Trim());
                builder.AddSourceName(output[i][columnList[3]..columnList[4]].Trim());

#elif NETSTANDARD2_0
                builder.AddName(output[i].Substring(columnList[0], (columnList[1] - columnList[0])).Trim());
                builder.AddId(output[i].Substring(columnList[1], (columnList[2] - columnList[1])).Trim());
                builder.AddVersion(output[i].Substring(columnList[2], (columnList[3] - columnList[2])).Trim());
                builder.AddSourceName(output[i].Substring(columnList[3], (columnList[4] - columnList[3])).Trim());
#endif

                // Workaround for getting the pin data from the output.
                // The normal method will not work, because "Pin type" and "Pinned version" contain spaces.
#if NETCOREAPP3_1_OR_GREATER
                string pinInfoString = output[i][columnList[4]..].TrimStart();
#elif NETSTANDARD2_0
                string pinInfoString = output[i].Substring(columnList[4]).TrimStart();
#endif
                int endOfTypeIndex = -1;
                for (int j = 0; j < pinInfoString.Length; j++)
                {
                    if (pinInfoString[j] == (char)32)
                    {
                        endOfTypeIndex = j;
                        break;
                    }
                }

#if NETCOREAPP3_1_OR_GREATER
                if (endOfTypeIndex == -1)
                {
                    builder.AddPinType(pinInfoString.Trim());
                }
                else
                {
                    builder.AddPinType(pinInfoString[0..endOfTypeIndex].Trim());
                    builder.AddPinnedVersion(pinInfoString[endOfTypeIndex..].Trim());
                }
#elif NETSTANDARD2_0
                if (endOfTypeIndex == -1)
                {
                    builder.AddPinType(pinInfoString.Trim());
                }
                else
                {
                    builder.AddPinType(pinInfoString.Substring(0, endOfTypeIndex).Trim());
                    builder.AddPinnedVersion(pinInfoString.Substring(endOfTypeIndex));
                }
#endif
                // End of workaround

                resultList.Add(builder.GetInstance());
            }

            // Check for secondery list in output.
            if (output.GetEntryContains("------") != -1)
            {
                List<WinGetPinnedPackage> seconderyList = ToPinnedPackageList(output);
                resultList.AddRange(seconderyList);
            }

            return resultList;
        }
        //------------------------------------------------------------------------------------------------------------

        //---To Source List-------------------------------------------------------------------------------------------
        /// <summary>
        /// Converts a <see cref="System.Collections.Generic.List{T}"/> 
        /// of output lines from a winget process, that contains the sources in json format, to a list of <see cref="WGetNET.WinGetSource"/>'s.
        /// </summary>
        /// <param name="output">
        /// The <see langword="array"/> of output lines from a winget process.
        /// </param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/>'s.
        /// </returns>
        public static List<WinGetSource> ToSourceList(string[] output)
        {
            List<WinGetSource> sourceList = new();

            for (int i = 0; i < output.Length; i++)
            {
                SourceModel? source =
                    JsonHelper.StringToObject<SourceModel>(output[i]);

                if (source != null)
                {
                    sourceList.Add(WinGetSource.FromSourceModel(source));
                }
            }

            return sourceList;
        }
        //------------------------------------------------------------------------------------------------------------

        //---To WinGet info-------------------------------------------------------------------------------------------
        /// <summary>
        /// Creates a <see cref="WGetNET.WinGetInfo"/> object from the winget output.
        /// </summary>
        /// <param name="output">The <see langword="array"/> containing the winget output lines.</param>
        /// <param name="actionVersionId">Containes info about the winget version range for the output.</param>
        /// <returns>
        /// The <see cref="WGetNET.WinGetInfo"/> object created from the output.
        /// </returns>
        public static WinGetInfo ToWingetInfo(string[] output, InfoActionVersionId actionVersionId)
        {
            if (output.Length <= 0)
            {
                return WinGetInfo.Empty;
            }

            return ReadDataByRange(output, actionVersionId);
        }

        /// <summary>
        /// Reads the version number from the winget info output.
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        private static string ReadVersionFromData(string[] output)
        {
            string version = string.Empty;

            string[] versionLineContent = output[0].Split((char)32);
            for (int i = 0; i < versionLineContent.Length; i++)
            {
#if NETCOREAPP3_1_OR_GREATER
                if (versionLineContent[i].Trim().StartsWith('v'))
                {
                    version = versionLineContent[i].Trim()[1..];
                    break;
                }
#elif NETSTANDARD2_0
                if (versionLineContent[i].Trim().StartsWith("v"))
                {
                    version = versionLineContent[i].Trim().Substring(1);
                    break;
                }
#endif
            }

            return version;
        }

        /// <summary>
        /// Initializes the reading of the data for the specific version range.
        /// </summary>
        /// <param name="output">The <see langword="array"/> containing the winget output lines.</param>
        /// <param name="actionVersionId">Containes info about the winget version range for the output.</param>
        /// <returns>
        /// The <see cref="WGetNET.WinGetInfo"/> object created from the output.
        /// </returns>
        private static WinGetInfo ReadDataByRange(string[] output, InfoActionVersionId actionVersionId)
        {
            return actionVersionId switch
            {
                InfoActionVersionId.VersionRange1 => ReadDataForRange1(output),
                InfoActionVersionId.VersionRange2 => ReadDataForRange2(output),
                InfoActionVersionId.VersionRange3 => ReadDataForRange3(output),
                InfoActionVersionId.VersionRange4 => ReadDataForRange4(output),
                _ => WinGetInfo.Empty,
            };
        }

        /// <summary>
        /// Reads the data from the winget info output for the version range 1.
        /// </summary>
        /// <param name="output">The <see langword="array"/> containing the winget output lines.</param>
        /// <returns>
        /// The <see cref="WGetNET.WinGetInfo"/> object created from the output.
        /// </returns>
        private static WinGetInfo ReadDataForRange1(string[] output)
        {
            InfoSetBuilder builder = new();

            string version = ReadVersionFromData(output);

            if (string.IsNullOrWhiteSpace(version))
            {
                // Return if version number could not be determined, because the outupt is probably not correct.
                return WinGetInfo.Empty;
            }

            builder.AddVersion(version);

            // Logs directory
            builder.AddDirectory(ReadSingleDirectoryEntry(output, 7));

            // Remove unnasesary range from output
            output = output.RemoveRange(0, 11);

            builder.AddLinks(ReadLinks(output));

            return builder.GetInstance();
        }

        /// <summary>
        /// Reads the data from the winget info output for the version range 1.
        /// </summary>
        /// <param name="output">The <see langword="array"/> containing the winget output lines.</param>
        /// <returns>
        /// The <see cref="WGetNET.WinGetInfo"/> object created from the output.
        /// </returns>
        private static WinGetInfo ReadDataForRange2(string[] output)
        {
            InfoSetBuilder builder = new();

            string version = ReadVersionFromData(output);

            if (string.IsNullOrWhiteSpace(version))
            {
                // Return if version number could not be determined, because the outupt is probably not correct.
                return WinGetInfo.Empty;
            }

            builder.AddVersion(version);

            // Logs directory
            builder.AddDirectory(ReadSingleDirectoryEntry(output, 7));

            // User Settings directory
            builder.AddDirectory(ReadSingleDirectoryEntry(output, 9));

            // Remove unnasesary range from output
            output = output.RemoveRange(0, 13);

            builder.AddLinks(ReadLinks(output));

            return builder.GetInstance();
        }

        /// <summary>
        /// Reads the data from the winget info output for the version range 1.
        /// </summary>
        /// <param name="output">The <see langword="array"/> containing the winget output lines.</param>
        /// <returns>
        /// The <see cref="WGetNET.WinGetInfo"/> object created from the output.
        /// </returns>
        private static WinGetInfo ReadDataForRange3(string[] output)
        {
            InfoSetBuilder builder = new();

            string version = ReadVersionFromData(output);

            if (string.IsNullOrWhiteSpace(version))
            {
                // Return if version number could not be determined, because the outupt is probably not correct.
                return WinGetInfo.Empty;
            }

            builder.AddVersion(version);

            // Logs directory
            builder.AddDirectory(ReadSingleDirectoryEntry(output, 7));

            // User Settings directory
            builder.AddDirectory(ReadSingleDirectoryEntry(output, 9));

            // Remove unnasesary range from output
            output = output.RemoveRange(0, 13);

            builder.AddLinks(ReadLinks(output));

            // Remove links area and admin settings header range from output
            output = output.RemoveRange(0, output.GetEntryContains("----") + 1);

            builder.AddAdminOptions(ReadAdminSettings(output));

            return builder.GetInstance();
        }

        /// <summary>
        /// Reads the data from the winget info output for the version range 1.
        /// </summary>
        /// <param name="output">The <see langword="array"/> containing the winget output lines.</param>
        /// <returns>
        /// The <see cref="WGetNET.WinGetInfo"/> object created from the output.
        /// </returns>
        private static WinGetInfo ReadDataForRange4(string[] output)
        {
            InfoSetBuilder builder = new();

            string version = ReadVersionFromData(output);

            if (string.IsNullOrWhiteSpace(version))
            {
                // Return if version number could not be determined, because the outupt is probably not correct.
                return WinGetInfo.Empty;
            }

            builder.AddVersion(version);

            // Remove unnasesary range from output
            output = output.RemoveRange(0, 9);

            builder.AddDirectories(ReadDirectories(output));

            // Remove directories area and links header range from output
            output = output.RemoveRange(0, output.GetEntryContains("----") + 1);

            builder.AddLinks(ReadLinks(output));

            // Remove links area and admin settings header range from output
            output = output.RemoveRange(0, output.GetEntryContains("----") + 1);

            builder.AddAdminOptions(ReadAdminSettings(output));

            return builder.GetInstance();
        }

        /// <summary>
        /// Reads a single directory entry from the winget info output.
        /// </summary>
        /// <param name="output">The <see langword="array"/> containing the winget output lines.</param>
        /// <param name="index">The index of the directory entry.</param>
        /// <returns>
        /// A <see cref="WinGetDirectory"/> instance if the action was successful, or null if it failed. 
        /// </returns>
        private static WinGetDirectory? ReadSingleDirectoryEntry(string[] output, int index)
        {
            string[] entry = output[index].Split(':');
            if (entry.Length == 2)
            {
                DirectoryBuilder builder = new();

                builder.AddEntryName(entry[0].Trim());
                builder.AddRawContent(entry[1].Trim());

                return builder.GetInstance();
            }
            return null;
        }

        /// <summary>
        /// Reads all directories from the winget info output.
        /// </summary>
        /// <remarks>
        /// The first output entry needs to be the first directory entry.
        /// </remarks>
        /// <param name="output">The <see langword="array"/> containing the winget output lines.</param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetDirectory"/> objects.
        /// </returns>
        private static List<WinGetDirectory> ReadDirectories(string[] output)
        {
            List<WinGetDirectory> directories = new();

            StringBuilder nameBuilder = new();
            StringBuilder contentBuilder = new();

            DirectoryBuilder directoryBuilder = new();

            for (int i = 0; i < output.Length; i++)
            {
                directoryBuilder.Clear();

                if (string.IsNullOrWhiteSpace(output[i]))
                {
                    break;
                }

                string[] directoryEntry = output[i].Split((char)32).RemoveEmptyEntries();

                nameBuilder.Clear();
                int startOfDirectory = 0;
                for (int j = 0; j < directoryEntry.Length; j++)
                {
#if NETCOREAPP3_1_OR_GREATER
                    if (directoryEntry[j].StartsWith('%') || directoryEntry[j].Contains(":\\"))
                    {
                        // Start of the directory reached, stop building name.
                        startOfDirectory = j;
                        break;
                    }
#elif NETSTANDARD2_0
                    if (directoryEntry[j].StartsWith("%") || directoryEntry[j].Contains(":\\"))
                    {
                        // Start of the directory reached, stop building name.
                        startOfDirectory = j;
                        break;
                    }
#endif

                    if (j > 0)
                    {
                        // Add a space in front of every part of the name that comes after the first on.
                        nameBuilder.Append((char)32);
                    }

                    nameBuilder.Append(directoryEntry[j].Trim());
                }

                directoryBuilder.AddEntryName(nameBuilder.ToString());

                directoryEntry = directoryEntry.RemoveRange(0, startOfDirectory);

                contentBuilder.Clear();
                for (int j = 0; j < directoryEntry.Length; j++)
                {
                    if (j > 0)
                    {
                        // Add a space in front of every part of the directory that comes after the first on.
                        contentBuilder.Append((char)32);
                    }

                    contentBuilder.Append(directoryEntry[j].Trim());
                }

                directoryBuilder.AddRawContent(contentBuilder.ToString());

                WinGetDirectory? winGetDirectory = directoryBuilder.GetInstance();
                if (winGetDirectory != null)
                {
                    directories.Add(winGetDirectory);
                }
            }

            return directories;
        }

        /// <summary>
        /// Reads all links from the winget info output.
        /// </summary>
        /// <remarks>
        /// The first output entry needs to be the first link entry.
        /// </remarks>
        /// <param name="output">The <see langword="array"/> containing the winget output lines.</param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetLink"/> objects.
        /// </returns>
        private static List<WinGetLink> ReadLinks(string[] output)
        {
            List<WinGetLink> links = new();

            StringBuilder nameBuilder = new();

            LinkBuilder linkBuilder = new();

            for (int i = 0; i < output.Length; i++)
            {
                linkBuilder.Clear();

                if (string.IsNullOrWhiteSpace(output[i]))
                {
                    break;
                }

                string[] linksEntry = output[i].Split((char)32).RemoveEmptyEntries();

#if NETCOREAPP3_1_OR_GREATER
                linkBuilder.AddRawContent(linksEntry[^1].Trim());
#elif NETSTANDARD2_0
                linkBuilder.AddRawContent(linksEntry[linksEntry.Length - 1].Trim());
#endif

                // Remove link from entry, so it only contains the name
                linksEntry = linksEntry.RemoveRange(linksEntry.Length - 1, 1);

                nameBuilder.Clear();
                for (int j = 0; j < linksEntry.Length; j++)
                {
                    if (j > 0)
                    {
                        // Add a space in front of every part of the name that comes after the first on.
                        nameBuilder.Append((char)32);
                    }
                    nameBuilder.Append(linksEntry[j].Trim());
                }

                linkBuilder.AddEntryName(nameBuilder.ToString());

                WinGetLink? winGetLink = linkBuilder.GetInstance();
                if (winGetLink != null)
                {
                    links.Add(winGetLink);
                }
            }

            return links;
        }

        /// <summary>
        /// Reads all admin options from the winget info output.
        /// </summary>
        /// <remarks>
        /// The first output entry needs to be the first admin option entry.
        /// </remarks>
        /// <param name="output">The <see langword="array"/> containing the winget output lines.</param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetAdminOption"/> objects.
        /// </returns>
        private static List<WinGetAdminOption> ReadAdminSettings(string[] output)
        {
            List<WinGetAdminOption> adminSetting = new();

            AdminOptionBuilder adminOptionBuilder = new();

            for (int i = 0; i < output.Length; i++)
            {
                adminOptionBuilder.Clear();

                if (string.IsNullOrWhiteSpace(output[i]))
                {
                    break;
                }

                string[] settingsEntry = output[i].Split((char)32).RemoveEmptyEntries();

                if (settingsEntry.Length == 2)
                {
                    adminOptionBuilder.AddEntryName(settingsEntry[0].Trim());
                    adminOptionBuilder.AddRawContent(settingsEntry[1].Trim());

                    WinGetAdminOption? adminOption = adminOptionBuilder.GetInstance();
                    if (adminOption != null)
                    {
                        adminSetting.Add(adminOption);
                    }
                }
            }

            return adminSetting;
        }
        //------------------------------------------------------------------------------------------------------------

        //---To Hash--------------------------------------------------------------------------------------------------
        /// <summary>
        /// Reads the hash from the winget hash action result.
        /// </summary>
        /// <param name="result">
        /// The <see cref="WGetNET.Models.ProcessResult"/> object of the action.
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/> containing the hash value.
        /// </returns>
        public static string ResultToHash(ProcessResult result)
        {
            string hash = "";
            if (result.Output.Length > 0 && result.Output[0].Contains(":"))
            {
                string[] splitOutput = result.Output[0].Split(':');
                if (splitOutput.Length >= 2)
                {
                    hash = splitOutput[1].Trim();
                }
            }

            return hash;
        }
        //------------------------------------------------------------------------------------------------------------

        //---Other----------------------------------------------------------------------------------------------------
        /// <summary>
        /// Writes the export result to a <see cref="System.String"/>.
        /// </summary>
        /// <param name="result">
        /// The <see cref="WGetNET.Models.ProcessResult"/> object containing the export data.
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
        //------------------------------------------------------------------------------------------------------------

        //---Helper---------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets all column start indexes from the input line.
        /// </summary>
        /// <param name="line">
        /// A <see cref="System.String"/> containing the header, for column calculation.
        /// </param>
        /// <param name="isPinnedPackageTable">Activate workaround for the pinned package list.</param>
        /// <returns>
        /// A <see cref="System.Int32"/> <see langword="array"/> containing the column start indexes.
        /// </returns>
        private static int[] GetColumnList(string line, bool isPinnedPackageTable = false)
        {
            int[] columns = new int[0];

            bool checkForColumn = true;
            for (int i = 0; i < line.Length; i++)
            {
                if (isPinnedPackageTable && columns.Length >= 5)
                {
                    // Workaround for the pinned package table
                    break;
                }

                if (line[i] != ((char)32) && checkForColumn)
                {
                    columns = columns.Add(i);
                    checkForColumn = false;
                }
                else if (line[i] == ((char)32))
                {
                    checkForColumn = true;
                }
            }

            return columns;
        }
        //------------------------------------------------------------------------------------------------------------
    }
}
