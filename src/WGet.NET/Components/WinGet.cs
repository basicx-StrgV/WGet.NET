//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using WGetNET.Builder;
using WGetNET.Components.Internal;
using WGetNET.Exceptions;
using WGetNET.Helper;
using WGetNET.Models;
using WGetNET.Parser;

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
        private const string _settingsEnableCmd = "settings --enable \"{0}\"";
        private const string _settingsDisableCmd = "settings --disable \"{0}\"";

        private ProcessManager _processManager;
        private string _wingetExePath;
        private DateTime _wingetExeModificationDate;
        private string _versionString;
        private Version _version;
        private bool _isPreview;

        private readonly bool _administratorPrivileges;

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
                // Check if the winget executable still exist to ensure a correct result,
                // even if winget gets removed while the application is running.
                if (File.Exists(_wingetExePath))
                {
                    return true;
                }
                else
                {
                    // Re-query installation and return the result to allow instalation of winget while the application is running.
                    return QueryInstallation();
                }
            }
        }

        /// <summary>
        /// Gets if the version of winget is a preview version.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the winget version is a preview version or <see langword="false"/> if not.
        /// </returns>
        public bool IsPreview
        {
            get
            {
                // Check if winget got modefied or removed while the application is running.
                if (_wingetExeModificationDate != GetLastModificationData())
                {
                    // Re-query installation if a change was detected.
                    QueryInstallation();
                }

                return _isPreview;
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
                // Check if winget got modefied or removed while the application is running.
                if (_wingetExeModificationDate != GetLastModificationData())
                {
                    // Re-query installation if a change was detected.
                    QueryInstallation();
                }

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
                // Check if winget got modefied or removed while the application is running.
                if (_wingetExeModificationDate != GetLastModificationData())
                {
                    // Re-query installation if a change was detected.
                    QueryInstallation();
                }

                return _version;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGet"/> class.
        /// </summary>
        public WinGet()
        {
            // Check if the current process has administrator privileges
            _administratorPrivileges = SystemHelper.CheckAdministratorPrivileges();

            // Set inital values
            _processManager = new ProcessManager("winget");
            _wingetExePath = string.Empty;
            _wingetExeModificationDate = DateTime.MinValue;
            _versionString = string.Empty;
            _version = new Version(0, 0);

            QueryInstallation();
        }

        //---Settings Export---------------------------------------------------------------------------
        /// <summary>
        /// Exports the WinGet settings to a json string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> containing the settings json.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        public string ExportSettings()
        {
            ProcessResult result = Execute(_exportSettingsCmd);

            return ProcessOutputReader.ExportOutputToString(result);
        }

        /// <summary>
        /// Asynchronous exports the WinGet settings to a json string.
        /// </summary>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.String"/> containing the settings json.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        public async Task<string> ExportSettingsAsync(CancellationToken cancellationToken = default)
        {
            ProcessResult result = await ExecuteAsync(_exportSettingsCmd, false, cancellationToken);

            return ProcessOutputReader.ExportOutputToString(result);
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

            ProcessResult result = Execute(_exportSettingsCmd);

            FileHelper.WriteTextToFile(file, ProcessOutputReader.ExportOutputToString(result));
        }

        /// <summary>
        /// Asynchronous exports the WinGet settings to a json and writes them to the given file.
        /// </summary>
        /// <param name="file">
        /// The file for the export.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// The <see cref="System.Threading.Tasks.Task"/> for the action.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
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
        public async Task ExportSettingsToFileAsync(string file, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(file, "file");
            ArgsHelper.ThrowIfPathIsInvalid(file);

            ProcessResult result = await ExecuteAsync(_exportSettingsCmd, false, cancellationToken);

            await FileHelper.WriteTextToFileAsync(file, ProcessOutputReader.ExportOutputToString(result), cancellationToken);
        }
        //---------------------------------------------------------------------------------------------

        //---Admin Settings----------------------------------------------------------------------------
        /// <summary>
        /// Gets all winget admin settings.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetAdminSetting"/> object.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        public List<WinGetAdminSetting> GetAdminSettings()
        {
            List<WinGetAdminSetting> adminSettings = new();

            string settingsJson = ExportSettings();

            SettingsModel settings = JsonHelper.StringToObject<SettingsModel>(settingsJson);
            if (settings == null)
            {
                return adminSettings;
            }

            WinGetAdminSettingBuilder builder = new();
            foreach (KeyValuePair<string, bool> entry in settings.AdminSettings)
            {
                builder.Clear();

                builder.AddEntryName(entry.Key);
                builder.AddStatus(entry.Value);

                WinGetAdminSetting? adminSetting = builder.GetInstance();
                if (adminSetting != null)
                {
                    adminSettings.Add(adminSetting);
                }
            }

            return adminSettings;
        }

        /// <summary>
        /// Asynchronously gets all winget admin settings.
        /// </summary>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetAdminSetting"/> object.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        public async Task<List<WinGetAdminSetting>> GetAdminSettingsAsync(CancellationToken cancellationToken = default)
        {
            List<WinGetAdminSetting> adminSettings = new();

            string settingsJson = await ExportSettingsAsync(cancellationToken);

#if NETCOREAPP3_1_OR_GREATER
            SettingsModel settings = await JsonHelper.StringToObjectAsync<SettingsModel>(settingsJson, cancellationToken);
#elif NETSTANDARD2_0
            SettingsModel settings = JsonHelper.StringToObject<SettingsModel>(settingsJson);
#endif
            if (settings == null)
            {
                return adminSettings;
            }

            WinGetAdminSettingBuilder builder = new();
            foreach (KeyValuePair<string, bool> entry in settings.AdminSettings)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return adminSettings;
                }

                builder.Clear();

                builder.AddEntryName(entry.Key);
                builder.AddStatus(entry.Value);

                WinGetAdminSetting? adminSetting = builder.GetInstance();
                if (adminSetting != null)
                {
                    adminSettings.Add(adminSetting);
                }
            }

            return adminSettings;
        }

        /// <summary>
        /// Enables the provided admin setting (Needs administrator rights).
        /// </summary>
        /// <param name="settingName">
        /// Name of the admin setting to enable.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the action was succesfull and <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public bool EnableAdminSetting(string settingName)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(settingName, "settingName");

            string cmd = string.Format(_settingsEnableCmd, settingName);

            ProcessResult result = Execute(cmd, true);

            return result.Success;
        }

        /// <summary>
        /// Enables the provided admin setting (Needs administrator rights).
        /// </summary>
        /// <param name="setting">
        /// The <see cref="WGetNET.WinGetAdminSetting"/> to enable.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the action was succesfull and <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public bool EnableAdminSetting(WinGetAdminSetting setting)
        {
            ArgsHelper.ThrowIfObjectIsNull(setting, "setting");

            return EnableAdminSetting(setting.EntryName);
        }

        /// <summary>
        /// Asynchronously enables the provided admin setting (Needs administrator rights).
        /// </summary>
        /// <param name="settingName">
        /// Name of the admin setting to enable.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the action was succesfull and <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public async Task<bool> EnableAdminSettingAsync(string settingName, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(settingName, "settingName");

            string cmd = string.Format(_settingsEnableCmd, settingName);

            ProcessResult result = await ExecuteAsync(cmd, true, cancellationToken);

            return result.Success;
        }

        /// <summary>
        /// Asynchronously enables the provided admin setting (Needs administrator rights).
        /// </summary>
        /// <param name="setting">
        /// The <see cref="WGetNET.WinGetAdminSetting"/> to enable.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the action was succesfull and <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public async Task<bool> EnableAdminSettingAsync(WinGetAdminSetting setting, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfObjectIsNull(setting, "setting");

            return await EnableAdminSettingAsync(setting.EntryName, cancellationToken);
        }

        /// <summary>
        /// Disables the provided admin setting (Needs administrator rights).
        /// </summary>
        /// <param name="settingName">
        /// Name of the admin setting to disable.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the action was succesfull and <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public bool DisableAdminSetting(string settingName)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(settingName, "settingName");

            string cmd = string.Format(_settingsDisableCmd, settingName);

            ProcessResult result = Execute(cmd, true);

            return result.Success;
        }

        /// <summary>
        /// Disables the provided admin setting (Needs administrator rights).
        /// </summary>
        /// <param name="setting">
        /// The <see cref="WGetNET.WinGetAdminSetting"/> to disable.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the action was succesfull and <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public bool DisableAdminSetting(WinGetAdminSetting setting)
        {
            ArgsHelper.ThrowIfObjectIsNull(setting, "setting");

            return DisableAdminSetting(setting.EntryName);
        }

        /// <summary>
        /// Asynchronously disables the provided admin setting (Needs administrator rights).
        /// </summary>
        /// <param name="settingName">
        /// Name of the admin setting to disable.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the action was succesfull and <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public async Task<bool> DisableAdminSettingAsync(string settingName, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(settingName, "settingName");

            string cmd = string.Format(_settingsDisableCmd, settingName);

            ProcessResult result = await ExecuteAsync(cmd, true, cancellationToken);

            return result.Success;
        }

        /// <summary>
        /// Asynchronously disables the provided admin setting (Needs administrator rights).
        /// </summary>
        /// <param name="setting">
        /// The <see cref="WGetNET.WinGetAdminSetting"/> to disable.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the action was succesfull and <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public async Task<bool> DisableAdminSettingAsync(WinGetAdminSetting setting, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfObjectIsNull(setting, "setting");

            return await DisableAdminSettingAsync(setting.EntryName, cancellationToken);
        }
        //---------------------------------------------------------------------------------------------

        //---Info--------------------------------------------------------------------------------------
        /// <summary>
        /// Gets all WinGet related data provided by the WinGet info action.
        /// </summary>
        /// <returns>
        /// A <see cref="WGetNET.WinGetInfo"/> object containing winget related information.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        public WinGetInfo GetInfo()
        {
            ProcessResult result = Execute(_infoCmd);

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

        /// <summary>
        /// Asynchronous gets all WinGet related data provided by the WinGet info action.
        /// </summary>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="WGetNET.WinGetInfo"/> object containing winget related information.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        public async Task<WinGetInfo> GetInfoAsync(CancellationToken cancellationToken = default)
        {
            ProcessResult result = await ExecuteAsync(_infoCmd, false, cancellationToken);

            // Check the version range the action should be performed for
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

            // Return empty data if the task was canceled
            if (cancellationToken.IsCancellationRequested)
            {
                return WinGetInfo.Empty;
            }

            return ProcessOutputReader.ToWingetInfo(result.Output, actionVersionId);
        }
        //---------------------------------------------------------------------------------------------

        //---Protected Functions-----------------------------------------------------------------------
        // \cond PRIVATE
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
        private protected bool CheckWinGetVersion(Version minVersion, Version? maxVersion = null)
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
        /// Exectutes a WinGet action from the given cmd.
        /// </summary>
        /// <param name="args">
        /// A <see cref="System.String"/> containing the arguments for the WinGet process.
        /// </param>
        /// <param name="needsAdminRights">
        /// Sets if the process that should be executed needs administrator privileges.
        /// </param>
        /// <returns>
        /// The <see cref="WGetNET.Models.ProcessResult"/> for the process.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current process does not have administrator privileges. 
        /// (Only if <paramref name="needsAdminRights"/> is set to <see langword="true"/>)
        /// </exception>
        private protected ProcessResult Execute(string args, bool needsAdminRights = false)
        {
            ThrowIfNotInstalled();

            if (needsAdminRights)
            {
                ThrowIfNotAdmin();
            }

            return _processManager.ExecuteWingetProcess(args);
        }

        /// <summary>
        /// Asynchronously exectutes a WinGet action from the given cmd.
        /// </summary>
        /// <param name="args">
        /// A <see cref="System.String"/> containing the arguments for the WinGet process.
        /// </param>
        /// <param name="needsAdminRights">
        /// Sets if the process that should be executed needs administrator privileges.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is the <see cref="WGetNET.Models.ProcessResult"/> for the process.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current process does not have administrator privileges.
        /// (Only if <paramref name="needsAdminRights"/> is set to <see langword="true"/>)
        /// </exception>
        private protected async Task<ProcessResult> ExecuteAsync(string args, bool needsAdminRights = false, CancellationToken cancellationToken = default)
        {
            ThrowIfNotInstalled();

            if (needsAdminRights)
            {
                ThrowIfNotAdmin();
            }

            return await _processManager.ExecuteWingetProcessAsync(args, cancellationToken);
        }
        // \endcond
        //---------------------------------------------------------------------------------------------

        //---Other-------------------------------------------------------------------------------------
        /// <summary>
        /// Throws a <see cref="WGetNET.Exceptions.WinGetNotInstalledException"/> if winget installation could not be found.
        /// </summary>
        /// <exception cref="WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        private void ThrowIfNotInstalled()
        {
            if (!IsInstalled)
            {
                throw new WinGetNotInstalledException();
            }
        }

        /// <summary>
        /// Throws a <see cref="System.Security.SecurityException"/> if the current process does not have administrator privileges.
        /// </summary>
        /// <exception cref="System.Security.SecurityException">
        /// The current process does not have administrator privileges.
        /// </exception>
        private void ThrowIfNotAdmin()
        {
            if (!_administratorPrivileges)
            {
                throw new SecurityException("Administrator privileges are missing.");
            }
        }

        /// <summary>
        /// Checks the winget version and returns it as a <see cref="System.String"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> containing the winget version.
        /// </returns>
        private string CheckWinGetVersion()
        {
            if (!IsInstalled)
            {
                return string.Empty;
            }

            ProcessResult result = Execute(_versionCmd);

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
        /// Gets the last modification date (UTC) of the currently set winget executable.
        /// </summary>
        /// <returns>
        /// <see cref="System.DateTime"/> object of the last modification date (UTC).
        /// </returns>
        private DateTime GetLastModificationData()
        {
            if (string.IsNullOrWhiteSpace(_wingetExePath) || !File.Exists(_wingetExePath))
            {
                return DateTime.MinValue;
            }

            return File.GetLastWriteTimeUtc(_wingetExePath);
        }

        /// <summary>
        /// Checks the system for a winget installation.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the installation was found and <see langword="false"/> if not.
        /// </returns>
        private bool QueryInstallation()
        {
            bool isInstalled;

            _wingetExePath = SystemHelper.CheckWingetInstallation();

            if (string.IsNullOrWhiteSpace(_wingetExePath))
            {
                isInstalled = false;
                _processManager = new ProcessManager("winget");
            }
            else
            {
                isInstalled = true;
                _processManager = new ProcessManager(_wingetExePath);
            }

            _wingetExeModificationDate = GetLastModificationData();

            if (isInstalled)
            {
                _versionString = CheckWinGetVersion();
            }
            else
            {
                _versionString = string.Empty;
            }

            _version = VersionParser.Parse(_versionString);

            _isPreview = VersionParser.CheckPreviewStatus(_versionString);

            return isInstalled;
        }
        //---------------------------------------------------------------------------------------------
    }
}