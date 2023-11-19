//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using WGetNET.Models;
using WGetNET.Exceptions;
using WGetNET.HelperClasses;
using System.IO;

namespace WGetNET
{
    /// <summary>
    /// The <see cref="WGetNET.WinGet"/> class offers informations about the installed winget version.
    /// </summary>
    public class WinGet
    {
        private const string _infoCmd = "--info";
        private const string _versionCmd = "--version";
        private const string _exportSettingsCmd = "settings export";

        private protected ProcessManager _processManager;
        private protected bool _isInstalled;

        private string _wingetExePath;
        private string _versionString;
        private Version _version;

        /// <summary>
        /// Gets if winget is installed on the system.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if winget is installed or <see langword="false"/> if not.
        /// </returns>
        public bool IsInstalled
        {
            get
            {
                return _isInstalled;
            }
        }

        /// <summary>
        /// Gets the version number of the winget installation.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> with the version number.
        /// </returns>
        public string VersionString
        {
            get
            {
                return _versionString;
            }
        }

        /// <summary>
        /// Gets the version number of the winget installation.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Version"/> object.
        /// </returns>
        public Version Version
        {
            get
            {
                return _version;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGet"/> class.
        /// </summary>
        public WinGet()
        {
            _wingetExePath = CheckInstallation();

            if (string.IsNullOrWhiteSpace(_wingetExePath))
            {
                _isInstalled = false;
                _processManager = new ProcessManager("winget");
            }
            else
            {
                _isInstalled = true;
                _processManager = new ProcessManager(_wingetExePath);
            }

            _versionString = CheckWinGetVersion();
            _version = VersionParser.Parse(_versionString);
        }

        /// <summary>
        /// Checks the system for a winget installation.
        /// </summary>
        /// <remarks>
        /// <para>Can be used to check if winget got installed will the installation is running without needing to restart it.</para>
        /// <para>This needs to be called manually, because a re-query for the installation will not be done automatically by the library.</para>
        /// </remarks>
        /// <returns>
        /// <see langword="true"/> if the installation was found and <see langword="false"/> if not.
        /// </returns>
        public bool ReQueryInstallation()
        {
            _wingetExePath = CheckInstallation();

            if (string.IsNullOrWhiteSpace(_wingetExePath))
            {
                _isInstalled = false;
                _processManager = new ProcessManager("winget");
            }
            else
            {
                _isInstalled = true;
                _processManager = new ProcessManager(_wingetExePath);
            }

            _versionString = CheckWinGetVersion();
            _version = VersionParser.Parse(_versionString);

            return _isInstalled;
        }

        /// <summary>
        /// Exports the WinGet settings to a json string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> containing the settings json.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public string ExportSettings()
        {
            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(_exportSettingsCmd);

                return ProcessOutputReader.ExportOutputToString(result);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Exporting sources failed.", _exportSettingsCmd, e);
            }
        }

        /// <summary>
        /// Asynchronous exports the WinGet settings to a json string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.String"/> containing the settings json.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public async Task<string> ExportSettingsAsync()
        {
            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(_exportSettingsCmd);

                return ProcessOutputReader.ExportOutputToString(result);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Exporting sources failed.", _exportSettingsCmd, e);
            }
        }

        /// <summary>
        /// Exports the WinGet settings to a json and writes them to the given file.
        /// </summary>
        /// <param name="file">
        /// The file for the export.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the action was succesfull, and <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// <para>A provided argument is empty.</para>
        /// <para>-or-</para>
        /// <para>The path contains one or more invalid characters as defined by <see cref="System.IO.Path.InvalidPathChars"/>.</para>
        /// </exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">
        /// The directory root does not exist.
        /// </exception>
        /// <exception cref="System.IO.PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// An I/O error occurred while opening the file
        /// </exception>
        /// <exception cref="System.NotSupportedException">
        /// Path is in an invalid format.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        public void ExportSettingsToFile(string file)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(file, "file");
            ArgsHelper.ThrowIfPathIsInvalid(file);

            ProcessResult result;

            try
            {
                result = _processManager.ExecuteWingetProcess(_exportSettingsCmd);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Exporting sources failed.", _exportSettingsCmd, e);
            }

            FileHandler.ExportOutputToFile(file, result);
        }

        /// <summary>
        /// Asynchronous exports the WinGet settings to a json and writes them to the given file.
        /// </summary>
        /// <param name="file">
        /// The file for the export.
        /// </param>
        /// <returns>
        /// The <see cref="System.Threading.Tasks.Task"/> for the action.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// <para>A provided argument is empty.</para>
        /// <para>-or-</para>
        /// <para>The path contains one or more invalid characters as defined by <see cref="System.IO.Path.InvalidPathChars"/>.</para>
        /// </exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">
        /// The directory root does not exist.
        /// </exception>
        /// <exception cref="System.IO.PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// An I/O error occurred while opening the file
        /// </exception>
        /// <exception cref="System.NotSupportedException">
        /// Path is in an invalid format.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        public async Task ExportSettingsToFileAsync(string file)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(file, "file");
            ArgsHelper.ThrowIfPathIsInvalid(file);

            ProcessResult result;

            try
            {
                result = await _processManager.ExecuteWingetProcessAsync(_exportSettingsCmd);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Exporting sources failed.", _exportSettingsCmd, e);
            }

            await FileHandler.ExportOutputToFileAsync(file, result);
        }

        /// <summary>
        /// Gets all WinGet related data provided by the WinGet info action.
        /// </summary>
        /// <returns>
        /// A <see cref="WGetNET.WinGetInfo"/> object containing winget related information.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public WinGetInfo GetInfo()
        {
            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(_infoCmd);

                InfoActionVersionId actionVersionId = InfoActionVersionId.VersionRange1;
                if (CheckWinGetVersion(new Version(1, 4, 3531), new Version(1, 5, 101)))
                {
                    actionVersionId = InfoActionVersionId.VersionRange2;
                }
                else if (CheckWinGetVersion(new Version(1, 5, 441), new Version(1, 5, 441)))
                {
                    actionVersionId = InfoActionVersionId.VersionRange3;
                }
                else if (CheckWinGetVersion(new Version(1, 5, 1081)))
                {
                    actionVersionId = InfoActionVersionId.VersionRange4;
                }

                return ProcessOutputReader.ToWingetInfo(result.Output, actionVersionId);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Getting data failed.", _infoCmd, e);
            }
        }

        /// <summary>
        /// Asynchronous gets all WinGet related data provided by the WinGet info action.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="WGetNET.WinGetInfo"/> object containing winget related information.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public async Task<WinGetInfo> GetInfoAsync()
        {
            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(_infoCmd);

                InfoActionVersionId actionVersionId = InfoActionVersionId.VersionRange1;
                if (CheckWinGetVersion(new Version(1, 4, 3531), new Version(1, 5, 101)))
                {
                    actionVersionId = InfoActionVersionId.VersionRange2;
                }
                else if (CheckWinGetVersion(new Version(1, 5, 441), new Version(1, 5, 441)))
                {
                    actionVersionId = InfoActionVersionId.VersionRange3;
                }
                else if (CheckWinGetVersion(new Version(1, 5, 1081)))
                {
                    actionVersionId = InfoActionVersionId.VersionRange4;
                }

                return ProcessOutputReader.ToWingetInfo(result.Output, actionVersionId);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Getting data failed.", _infoCmd, e);
            }
        }

        /// <summary>
        /// Checks if the installed WinGet version is between the given versions or the same.
        /// </summary>
        /// <remarks>
        /// If no max version is provided, no upper limit will be set.
        /// </remarks>
        /// <param name="minVersion">The min version for the check.</param>
        /// <param name="maxVersion">The max version for the check.</param>
        /// <returns>
        /// <see langword="true"/> if the installed WinGet version matches the check, or <see langword="false"/> if not.
        /// </returns>
        protected bool CheckWinGetVersion(Version minVersion, Version? maxVersion = null)
        {
            Version winGetVersion = Version;
            if ((winGetVersion.Major >= minVersion.Major && winGetVersion.Minor >= minVersion.Minor &&
                ((winGetVersion.Minor != minVersion.Minor) || (winGetVersion.Minor == minVersion.Minor && winGetVersion.Build >= minVersion.Build))) &&
                ((maxVersion == null) || (winGetVersion.Major <= maxVersion.Major && winGetVersion.Minor <= maxVersion.Minor &&
                ((winGetVersion.Minor != maxVersion.Minor) || (winGetVersion.Minor == maxVersion.Minor && winGetVersion.Build <= maxVersion.Build)))))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks the winget version and returns it as a <see cref="System.String"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> containing the winget version.
        /// </returns>
        private string CheckWinGetVersion()
        {
            if (!_isInstalled)
            {
                return string.Empty;
            }

            ProcessResult result =
                _processManager.ExecuteWingetProcess(_versionCmd);

            for (int i = 0; i < result.Output.Length; i++)
            {
#if NETCOREAPP3_1_OR_GREATER
                if (result.Output[i].StartsWith('v') && result.Output[i].Length >= 2)
                {
                    // Return output without the 'v' at the start.
                    return result.Output[i].Trim()[1..];
                }
#elif NETSTANDARD2_0
                if (result.Output[i].StartsWith("v") && result.Output[i].Length >= 2)
                {
                // Return output without the 'v' at the start.
                    return result.Output[i].Trim().Substring(1);
                }
#endif
            }

            return string.Empty;
        }

        /// <summary>
        /// Checks if winget is installed on the system and returns the path to the executable.
        /// </summary>
        /// <returns>
        /// <see cref="System.String"/> containing the executable path if it was found or <see cref="System.String.Empty"/> if not. 
        /// </returns>
        private string CheckInstallation()
        {
            string? pathEnvVar = Environment.GetEnvironmentVariable("Path");
            if (string.IsNullOrWhiteSpace(pathEnvVar))
            {
                return string.Empty;
            }

            string[] paths = pathEnvVar.Split(';');

            string exePath;
            for (int i = 0; i < paths.Length; i++)
            {
                exePath = Path.Combine(paths[i], "winget.exe");
                if (File.Exists(exePath))
                {
                    return exePath;
                }
            }

            return string.Empty;
        }
    }
}