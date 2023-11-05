//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.IO;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using WGetNET.HelperClasses;

namespace WGetNET
{
    /// <summary>
    /// The <see cref="WGetNET.WinGetPackageManager"/> class offers methods to manage packages with winget.
    /// </summary>
    public class WinGetPackageManager : WinGetInfo
    {
        private const string _listCmd = "list";
        private const string _searchInstalledCmd = "list \"{0}\"";
        private const string _searchInstalledBySourceCmd = "list \"{0}\" --source {1}";
        private const string _searchCmd = "search \"{0}\" --accept-source-agreements";
        private const string _searchBySourceCmd = "search \"{0}\" --source {1} --accept-source-agreements";
        private const string _installCmd = "install \"{0}\"";
        private const string _upgradeCmd = "upgrade \"{0}\"";
        private const string _upgradeAllCmd = "upgrade --all";
        private const string _getUpgradeableCmd = "upgrade";
        private const string _includeUnknown = "--include-unknown";
        private const string _uninstallCmd = "uninstall \"{0}\"";
        private const string _exportCmd = "export -o {0}";
        private const string _importCmd = "import -i {0} --ignore-unavailable";
        private const string _hashCmd = "hash {0}";
        private const string _downloadCmd = "download {0} --download-directory {1}";
        private const string _pinListCmd = "pin list";
        private const string _pinAddCmd = "pin add \"{0}\"";
        private const string _pinAddByVersionCmd = "pin add \"{0}\" --version \"{1}\"";
        private const string _pinRemoveCmd = "pin remove \"{0}\"";
        private const string _pinAddInstalledCmd = "pin add \"{0}\" --installed";
        private const string _pinAddInstalledByVersionCmd = "pin add \"{0}\" --installed --version \"{1}\"";
        private const string _pinRemoveInstalledCmd = "pin remove \"{0}\" --installed";
        private const string _pinResetCmd = "pin reset --force";

        private readonly Version _downloadMinVersion = new(1, 6, 0);
        private readonly Version _pinMinVersion = new(1, 5, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetPackageManager"/> class.
        /// </summary>
        public WinGetPackageManager()
        {
            // Provide empty constructor for xlm docs
        }

        //---Search------------------------------------------------------------------------------------
        /// <summary>
        /// Uses the winget search function to search for a package that maches the given name.
        /// </summary>
        /// <param name="packageId">
        /// The id or name of the package for the search.
        /// </param>
        /// <param name="exact">Use exact match.</param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public List<WinGetPackage> SearchPackage(string packageId, bool exact = false)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            string cmd = string.Format(_searchCmd, packageId);
            
            if (exact)
            {
                cmd += " --exact";
            }

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(cmd);

                return ProcessOutputReader.ToPackageList(result.Output, PackageAction.Search);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The package search failed.", cmd, e);
            }
        }

        /// <summary>
        /// Uses the winget search function to search for a package that maches the given name.
        /// </summary>
        /// <param name="packageId">
        /// The id or name of the package for the search.
        /// </param>
        /// <param name="sourceName">
        /// The name of the source for the search.
        /// </param>
        /// <param name="exact">Use exact match.</param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public List<WinGetPackage> SearchPackage(string packageId, string sourceName, bool exact = false)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(sourceName, "sourceName");

            string cmd = string.Format(_searchBySourceCmd, packageId, sourceName);

            if (exact)
            {
                cmd += " --exact";
            }

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(cmd);

                return ProcessOutputReader.ToPackageList(result.Output, PackageAction.SearchBySource, sourceName);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The package search failed.", cmd, e);
            }
        }

        /// <summary>
        /// Uses the winget search function to asynchronously search for a package that maches the given name.
        /// </summary>
        /// <param name="packageId">
        /// The id or name of the package for the search.
        /// </param>
        /// <param name="exact">Use exact match.</param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<List<WinGetPackage>> SearchPackageAsync(string packageId, bool exact = false)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            string cmd = string.Format(_searchCmd, packageId);

            if (exact)
            {
                cmd += " --exact";
            }

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(cmd);

                return ProcessOutputReader.ToPackageList(result.Output, PackageAction.Search);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The package search failed.", cmd, e);
            }
        }

        /// <summary>
        /// Uses the winget search function to asynchronously search for a package that maches the given name.
        /// </summary>
        /// <param name="packageId">
        /// The id or name of the package for the search.
        /// </param>
        /// <param name="sourceName">
        /// The name of the source for the search.
        /// </param>
        /// <param name="exact">Use exact match.</param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<List<WinGetPackage>> SearchPackageAsync(string packageId, string sourceName, bool exact = false)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(sourceName, "sourceName");

            string cmd = string.Format(_searchBySourceCmd, packageId, sourceName);

            if (exact)
            {
                cmd += " --exact";
            }

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(cmd);

                return ProcessOutputReader.ToPackageList(result.Output, PackageAction.SearchBySource, sourceName);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The package search failed.", cmd, e);
            }
        }
        //---------------------------------------------------------------------------------------------

        //---Install-----------------------------------------------------------------------------------
        /// <summary>
        /// Gets a list of all installed packages.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public List<WinGetPackage> GetInstalledPackages()
        {
            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(_listCmd);

                return ProcessOutputReader.ToPackageList(result.Output, PackageAction.InstalledList);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The search of installed packages failed.", _listCmd, e);
            }
        }

        /// <summary>
        /// Gets a list of all installed packages. That match the provided name.
        /// </summary>
        /// <param name="packageId">
        /// The id or name of the package for the search.
        /// </param>
        /// <param name="exact">Use exact match.</param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public List<WinGetPackage> GetInstalledPackages(string packageId, bool exact = false)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            string cmd = string.Format(_searchInstalledCmd, packageId);

            if (exact)
            {
                cmd += " --exact";
            }

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(cmd);

                return ProcessOutputReader.ToPackageList(result.Output, PackageAction.InstalledList);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The search of installed packages failed.", cmd, e);
            }
        }

        /// <summary>
        /// Gets a list of all installed packages. That match the provided name.
        /// </summary>
        /// <param name="packageId">
        /// The id or name of the package for the search.
        /// </param>
        /// <param name="sourceName">
        /// The name of the source for the search.
        /// </param>
        /// <param name="exact">Use exact match.</param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public List<WinGetPackage> GetInstalledPackages(string packageId, string sourceName, bool exact = false)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(sourceName, "sourceName");

            string cmd = string.Format(_searchInstalledBySourceCmd, packageId, sourceName);

            if (exact)
            {
                cmd += " --exact";
            }

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(cmd);

                return ProcessOutputReader.ToPackageList(result.Output, PackageAction.InstalledListBySource, sourceName);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The search of installed packages failed.", cmd, e);
            }
        }

        /// <summary>
        /// Asynchronously gets a list of all installed packages.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public async Task<List<WinGetPackage>> GetInstalledPackagesAsync()
        {
            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(_listCmd);

                return ProcessOutputReader.ToPackageList(result.Output, PackageAction.InstalledList);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The search of installed packages failed.", _listCmd, e);
            }
        }

        /// <summary>
        /// Asynchronously gets a list of all installed packages. That match the provided name.
        /// </summary>
        /// <param name="packageId">
        /// The id or name of the package for the search.
        /// </param>
        /// <param name="exact">Use exact match.</param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<List<WinGetPackage>> GetInstalledPackagesAsync(string packageId, bool exact = false)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            string cmd = string.Format(_searchInstalledCmd, packageId);

            if (exact)
            {
                cmd += " --exact";
            }

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(cmd);

                return ProcessOutputReader.ToPackageList(result.Output, PackageAction.InstalledList);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The search of installed packages failed.", cmd, e);
            }
        }

        /// <summary>
        /// Asynchronously gets a list of all installed packages. That match the provided name.
        /// </summary>
        /// <param name="packageId">
        /// The id or name of the package for the search.
        /// </param>
        /// <param name="sourceName">
        /// The name of the source for the search.
        /// </param>
        /// <param name="exact">Use exact match.</param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<List<WinGetPackage>> GetInstalledPackagesAsync(string packageId, string sourceName, bool exact = false)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(sourceName, "sourceName");

            string cmd = string.Format(_searchInstalledBySourceCmd, packageId, sourceName);

            if (exact)
            {
                cmd += " --exact";
            }

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(cmd);

                return ProcessOutputReader.ToPackageList(result.Output, PackageAction.InstalledListBySource, sourceName);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The search of installed packages failed.", cmd, e);
            }
        }

        /// <summary>
        /// Install a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package for the installation.</param>
        /// <returns>
        /// <see langword="true"/> if the installation was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public bool InstallPackage(string packageId)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            string cmd = string.Format(_installCmd, packageId);

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(cmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The package installtion failed.", cmd, e);
            }
        }

        /// <summary>
        /// Install a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> for the installation.</param>
        /// <returns>
        /// <see langword="true"/> if the installation was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public bool InstallPackage(WinGetPackage package)
        {
            ArgsHelper.ThrowIfObjectIsNull(package, "package");

            if (package.HasShortenedId)
            {
                return InstallPackage(package.Name);
            }

            return InstallPackage(package.Id);
        }

        /// <summary>
        /// Asynchronously install a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package for the installation.</param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the installation was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<bool> InstallPackageAsync(string packageId)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            string cmd = string.Format(_installCmd, packageId);

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(cmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The package installtion failed.", cmd, e);
            }
        }

        /// <summary>
        /// Asynchronously install a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> for the installation.</param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the installation was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<bool> InstallPackageAsync(WinGetPackage package)
        {
            ArgsHelper.ThrowIfObjectIsNull(package, "package");

            if (package.HasShortenedId)
            {
                return await InstallPackageAsync(package.Name);
            }

            return await InstallPackageAsync(package.Id);
        }
        //---------------------------------------------------------------------------------------------

        //---Uninstall---------------------------------------------------------------------------------
        /// <summary>
        /// Uninsatll a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package for uninstallation.</param>
        /// <returns>
        /// <see langword="true"/> if the uninstallation was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public bool UninstallPackage(string packageId)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            string cmd = string.Format(_uninstallCmd, packageId);

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(cmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The package uninstalltion failed.", cmd, e);
            }
        }

        /// <summary>
        /// Uninstall a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> for the uninstallation.</param>
        /// <returns>
        /// <see langword="true"/> if the uninstallation was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public bool UninstallPackage(WinGetPackage package)
        {
            ArgsHelper.ThrowIfObjectIsNull(package, "package");

            if (package.HasShortenedId)
            {
                return UninstallPackage(package.Name);
            }

            return UninstallPackage(package.Id);
        }

        /// <summary>
        /// Asynchronously uninsatll a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package for uninstallation.</param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the uninstallation was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<bool> UninstallPackageAsync(string packageId)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            string cmd = string.Format(_uninstallCmd, packageId);

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(cmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The package uninstalltion failed.", cmd, e);
            }
        }

        /// <summary>
        /// Asynchronously uninstall a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> for the uninstallation.</param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the uninstallation was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<bool> UninstallPackageAsync(WinGetPackage package)
        {
            ArgsHelper.ThrowIfObjectIsNull(package, "package");

            if (package.HasShortenedId)
            {
                return await UninstallPackageAsync(package.Name);
            }

            return await UninstallPackageAsync(package.Id);
        }
        //---------------------------------------------------------------------------------------------

        //---Upgrade-----------------------------------------------------------------------------------
        /// <summary>
        /// Get all upgradeable packages.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public List<WinGetPackage> GetUpgradeablePackages()
        {
            string cmd = AddArgumentByVersion(_getUpgradeableCmd);

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(cmd);

                return ProcessOutputReader.ToPackageList(result.Output, PackageAction.UpgradeList);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Getting updateable packages failed.", cmd, e);
            }
        }

        /// <summary>
        /// Asynchronously get all upgradeable packages.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public async Task<List<WinGetPackage>> GetUpgradeablePackagesAsync()
        {
            string cmd = AddArgumentByVersion(_getUpgradeableCmd);

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(cmd);

                return ProcessOutputReader.ToPackageList(result.Output, PackageAction.UpgradeList);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Getting updateable packages failed.", cmd, e);
            }
        }

        /// <summary>
        /// Upgrades a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package for upgrade.</param>
        /// <returns>
        /// <see langword="true"/> if the upgrade was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public bool UpgradePackage(string packageId)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            string cmd = string.Format(_upgradeCmd, packageId);

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(cmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Upgrading the package failed.", cmd, e);
            }
        }

        /// <summary>
        /// Upgrades a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> that for the upgrade</param>
        /// <returns>
        /// <see langword="true"/> if the upgrade was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public bool UpgradePackage(WinGetPackage package)
        {
            ArgsHelper.ThrowIfObjectIsNull(package, "package");

            if (package.HasShortenedId)
            {
                return UpgradePackage(package.Name);
            }

            return UpgradePackage(package.Id);
        }

        /// <summary>
        /// Asynchronously upgrades a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package for upgrade.</param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the upgrade was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<bool> UpgradePackageAsync(string packageId)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            string cmd = string.Format(_upgradeCmd, packageId);

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(cmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Upgrading the package failed.", cmd, e);
            }
        }

        /// <summary>
        /// Asynchronously upgrades a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> that for the upgrade</param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the upgrade was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<bool> UpgradePackageAsync(WinGetPackage package)
        {
            ArgsHelper.ThrowIfObjectIsNull(package, "package");

            if (package.HasShortenedId)
            {
                return await UpgradePackageAsync(package.Name);
            }

            return await UpgradePackageAsync(package.Id);
        }

        /// <summary>
        /// Tries to upgrade all packages using winget.
        /// </summary>
        /// <remarks>
        /// The action might run succesfully without upgrading every or even any package.
        /// </remarks>
        /// <returns>
        /// <see langword="true"/> if the action run successfully or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public bool UpgradeAllPackages()
        {
            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(_upgradeAllCmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Upgrading all packages failed.", _upgradeAllCmd, e);
            }
        }

        /// <summary>
        /// Asynchronously tries to upgrade all packages using winget.
        /// </summary>
        /// <remarks>
        /// The action might run succesfully without upgrading every or even any package.
        /// </remarks>
        /// <returns>
        /// <see langword="true"/> if the action run successfully or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public async Task<bool> UpgradeAllPackagesAsync()
        {
            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(_upgradeAllCmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Upgrading all packages failed.", _upgradeAllCmd, e);
            }
        }

        private string AddArgumentByVersion(string argument)
        {
            // Checking version to determine if "--include-unknown" is necessary.
            if (CheckWinGetVersion(new Version(1, 4, 0)))
            {
                // Winget version supports new argument, add "--include-unknown" to arguments
                argument += $" {_includeUnknown}";
            }
            return argument;
        }
        //---------------------------------------------------------------------------------------------

        //---Export and Import-------------------------------------------------------------------------
        /// <summary>
        /// Exports a list of all installed winget packages as json to the given file.
        /// </summary>
        /// <param name="file">The file for the export.</param>
        /// <returns>
        /// <see langword="true"/> if the export was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public bool ExportPackagesToFile(string file)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(file, "file");

            string cmd = string.Format(_exportCmd, file);

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(cmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Exporting packages failed.", cmd, e);
            }
        }

        /// <summary>
        /// Asynchronously exports a list of all installed winget packages as json to the given file.
        /// </summary>
        /// <param name="file">The file for the export.</param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the export was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<bool> ExportPackagesToFileAsync(string file)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(file, "file");

            string cmd = string.Format(_exportCmd, file);

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(cmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Exporting packages failed.", cmd, e);
            }
        }

        /// <summary>
        /// Imports packages and trys to installes/upgrade all pakages in the list, if possible.
        /// </summary>
        /// <remarks>
        /// This may take some time and winget may not install/upgrade all packages.
        /// </remarks>
        /// <param name="file">The file with the package data for the import.</param>
        /// <returns>
        /// <see langword="true"/> if the import was compleatly successful or 
        /// <see langword="false"/> if some or all packages failed to install.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public bool ImportPackagesFromFile(string file)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(file, "file");

            string cmd = string.Format(_importCmd, file);

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(cmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Importing packages failed.", cmd, e);
            }
        }

        /// <summary>
        /// Asynchronously imports packages and trys to installes/upgrade all pakages in the list, if possible.
        /// </summary>
        /// <remarks>
        /// This may take some time and winget may not install/upgrade all packages.
        /// </remarks>
        /// <param name="file">The file with the package data for the import.</param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the import was compleatly successful or 
        /// <see langword="false"/> if some or all packages failed to install.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<bool> ImportPackagesFromFileAsync(string file)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(file, "file");

            string cmd = string.Format(_importCmd, file);

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(cmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Importing packages failed.", cmd, e);
            }
        }
        //---------------------------------------------------------------------------------------------

        //---Hash--------------------------------------------------------------------------------------
        /// <summary>
        /// Executes the WinGet hash function, to calculate the hash for the given file.
        /// </summary>
        /// <param name="file">
        /// A <see cref="System.String"/> containing the path to the file.
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/> containing the hash.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        /// <exception cref="System.IO.FileNotFoundException">
        /// Unable to find the specified file.
        /// </exception>
        public string Hash(string file)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(file, "file");

            if (!File.Exists(file))
            {
                throw new FileNotFoundException($"Unable to find the specified file. File:'{file}'");
            }

            string cmd = string.Format(_hashCmd, file);

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(cmd);

                if (!result.Success)
                {
                    return string.Empty;
                }

                return HashResultToHash(result);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Hashing failed.", cmd, e);
            }
        }

        /// <summary>
        /// Executes the WinGet hash function, to calculate the hash for the given file.
        /// </summary>
        /// <param name="file">
        /// A <see cref="System.IO.FileInfo"/> object, of the file the hash should be calculated for.
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/> containing the hash.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        /// <exception cref="System.IO.FileNotFoundException">
        /// Unable to find the specified file.
        /// </exception>
        public string Hash(FileInfo file)
        {
            ArgsHelper.ThrowIfObjectIsNull(file, "file");

            if (!file.Exists)
            {
                throw new FileNotFoundException($"Unable to find the specified file. File:'{file.FullName}'");
            }

            string cmd = string.Format(_hashCmd, file.FullName);

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(cmd);

                if (!result.Success)
                {
                    return string.Empty;
                }

                return HashResultToHash(result);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Hashing failed.", cmd, e);
            }
        }

        /// <summary>
        /// Asynchronously executes the WinGet hash function, to calculate the hash for the given file.
        /// </summary>
        /// <param name="file">
        /// A <see cref="System.String"/> containing the path to the file.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.String"/> containing the hash.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        /// <exception cref="System.IO.FileNotFoundException">
        /// Unable to find the specified file.
        /// </exception>
        public async Task<string> HashAsync(string file)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(file, "file");

            if (!File.Exists(file))
            {
                throw new FileNotFoundException($"Unable to find the specified file. File:'{file}'");
            }

            string cmd = string.Format(_hashCmd, file);

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(cmd);

                if (!result.Success)
                {
                    return string.Empty;
                }

                return HashResultToHash(result);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Hashing failed.", cmd, e);
            }
        }

        /// <summary>
        /// Asynchronously executes the WinGet hash function, to calculate the hash for the given file.
        /// </summary>
        /// <param name="file">
        /// A <see cref="System.IO.FileInfo"/> object, of the file the hash should be calculated for.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.String"/> containing the hash.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        /// <exception cref="System.IO.FileNotFoundException">
        /// Unable to find the specified file.
        /// </exception>
        public async Task<string> HashAsync(FileInfo file)
        {
            ArgsHelper.ThrowIfObjectIsNull(file, "file");

            if (!file.Exists)
            {
                throw new FileNotFoundException($"Unable to find the specified file. File:'{file.FullName}'");
            }

            string cmd = string.Format(_hashCmd, file.FullName);

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(cmd);

                if (!result.Success)
                {
                    return string.Empty;
                }

                return HashResultToHash(result);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Hashing failed.", cmd, e);
            }
        }

        /// <summary>
        /// Reads the hash from the WinGet hash action result.
        /// </summary>
        /// <param name="result">
        /// The <see cref="WGetNET.ProcessResult"/> object of the action.
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/> containing the hash value.
        /// </returns>
        private string HashResultToHash(ProcessResult result)
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
        //---------------------------------------------------------------------------------------------

        //---Download----------------------------------------------------------------------------------
        /// <summary>
        /// Downloads the installer of a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package to download.</param>
        /// <param name="directory">Directory path the files will be downloaded to. It will be created if it does not exist.</param>
        /// <returns>
        /// <see langword="true"/> if the download was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public bool Download(string packageId, string directory)
        {
            if (!CheckWinGetVersion(_downloadMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_downloadMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(directory, "directory");

            string cmd = string.Format(_downloadCmd, packageId, directory);

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(cmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Download failed.", cmd, e);
            }
        }

        /// <summary>
        /// Downloads the installer of a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package to download.</param>
        /// <param name="directory">
        /// A <see cref="System.IO.DirectoryInfo"/> object of the directory the files will be downloaded to. 
        /// It will be created if it does not exist.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the download was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public bool Download(string packageId, DirectoryInfo directory)
        {
            ArgsHelper.ThrowIfObjectIsNull(directory, "directory");

            return Download(packageId, directory.FullName);
        }

        /// <summary>
        /// Downloads the installer of a package using winget.
        /// </summary>
        /// <param name="package">The package to download.</param>
        /// <param name="directory">Directory path the files will be downloaded to. It will be created if it does not exist.</param>
        /// <returns>
        /// <see langword="true"/> if the download was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public bool Download(WinGetPackage package, string directory)
        {
            ArgsHelper.ThrowIfObjectIsNull(package, "package");

            if (package.HasShortenedId)
            {
                return Download(package.Name, directory);
            }

            return Download(package.Id, directory);
        }

        /// <summary>
        /// Downloads the installer of a package using winget.
        /// </summary>
        /// <param name="package">The package to download.</param>
        /// <param name="directory">
        /// A <see cref="System.IO.DirectoryInfo"/> object of the directory the files will be downloaded to. 
        /// It will be created if it does not exist.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the download was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public bool Download(WinGetPackage package, DirectoryInfo directory)
        {
            ArgsHelper.ThrowIfObjectIsNull(package, "package");
            ArgsHelper.ThrowIfObjectIsNull(directory, "directory");

            if (package.HasShortenedId)
            {
                return Download(package.Name, directory.FullName);
            }

            return Download(package.Id, directory.FullName);
        }

        /// <summary>
        /// Asynchronously downloads the installer of a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package to download.</param>
        /// <param name="directory">Directory path the files will be downloaded to. It will be created if it does not exist.</param>
        /// <returns>
        /// <see langword="true"/> if the download was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<bool> DownloadAsync(string packageId, string directory)
        {
            if (!CheckWinGetVersion(_downloadMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_downloadMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(directory, "directory");

            string cmd = string.Format(_downloadCmd, packageId, directory);

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(cmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Download failed.", cmd, e);
            }
        }

        /// <summary>
        /// Asynchronously downloads the installer of a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package to download.</param>
        /// <param name="directory">
        /// A <see cref="System.IO.DirectoryInfo"/> object of the directory the files will be downloaded to. 
        /// It will be created if it does not exist.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the download was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<bool> DownloadAsync(string packageId, DirectoryInfo directory)
        {
            ArgsHelper.ThrowIfObjectIsNull(directory, "directory");

            return await DownloadAsync(packageId, directory.FullName);
        }

        /// <summary>
        /// Asynchronously downloads the installer of a package using winget.
        /// </summary>
        /// <param name="package">The package to download.</param>
        /// <param name="directory">Directory path the files will be downloaded to. It will be created if it does not exist.</param>
        /// <returns>
        /// <see langword="true"/> if the download was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<bool> DownloadAsync(WinGetPackage package, string directory)
        {
            ArgsHelper.ThrowIfObjectIsNull(package, "package");

            if (package.HasShortenedId)
            {
                return await DownloadAsync(package.Name, directory);
            }

            return await DownloadAsync(package.Id, directory);
        }

        /// <summary>
        /// Asynchronously downloads the installer of a package using winget.
        /// </summary>
        /// <param name="package">The package to download.</param>
        /// <param name="directory">
        /// A <see cref="System.IO.DirectoryInfo"/> object of the directory the files will be downloaded to. 
        /// It will be created if it does not exist.</param>
        /// <returns>
        /// <see langword="true"/> if the download was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<bool> DownloadAsync(WinGetPackage package, DirectoryInfo directory)
        {
            ArgsHelper.ThrowIfObjectIsNull(package, "package");
            ArgsHelper.ThrowIfObjectIsNull(directory, "directory");

            if (package.HasShortenedId)
            {
                return await DownloadAsync(package.Name, directory.FullName);
            }

            return await DownloadAsync(package.Id, directory.FullName);
        }
        //---------------------------------------------------------------------------------------------

        //---Pin List----------------------------------------------------------------------------------
        /// <summary>
        /// Gets a list of all pinned packages.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        public List<WinGetPinnedPackage> GetPinnedPackages()
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(_pinListCmd);

                return ProcessOutputReader.ToPinnedPackageList(result.Output);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Listing all pinned packages failed.", _pinListCmd, e);
            }
        }

        /// <summary>
        /// Asynchronously gets a list of all pinned packages.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        public async Task<List<WinGetPinnedPackage>> GetPinnedPackagesAsync()
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(_pinListCmd);

                return ProcessOutputReader.ToPinnedPackageList(result.Output);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Listing all pinned packages failed.", _pinListCmd, e);
            }
        }
        //---------------------------------------------------------------------------------------------

        //---Pin Add-----------------------------------------------------------------------------------
        /// <summary>
        /// Adds a pinned package to winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package to pin.</param>
        /// <param name="blocking">Set to <see langword="true"/> if updating of pinned package should be fully blocked.</param>
        /// <returns>
        /// <see langword="true"/> if the pin was added successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public bool PinAdd(string packageId, bool blocking = false)
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            string cmd = string.Format(_pinAddCmd, packageId);

            if (blocking)
            {
                cmd += " --blocking";
            }

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(cmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Pinning the package failed.", cmd, e);
            }
        }

        /// <summary>
        /// Adds a pinned package to winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package to pin.</param>
        /// <param name="version">
        /// <see cref="System.String"/> representing the version to pin. 
        /// Please refer to the WinGet documentation for more info about version pinning.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the pin was added successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public bool PinAdd(string packageId, string version)
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(version, "version");

            string cmd = string.Format(_pinAddByVersionCmd, packageId, version);

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(cmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Pinning the package failed.", cmd, e);
            }
        }

        /// <summary>
        /// Adds a pinned package to winget.
        /// </summary>
        /// <param name="package">The package to pin.</param>
        /// <param name="blocking">Set to <see langword="true"/> if updating of pinned package should be fully blocked.</param>
        /// <returns>
        /// <see langword="true"/> if the pin was added successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public bool PinAdd(WinGetPackage package, bool blocking = false)
        {
            ArgsHelper.ThrowIfObjectIsNull(package, "package");

            if (package.HasShortenedId)
            {
                return PinAdd(package.Name, blocking);
            }

            return PinAdd(package.Id, blocking);
        }

        /// <summary>
        /// Adds a pinned package to winget.
        /// </summary>
        /// <param name="package">The package to pin.</param>
        /// <param name="version">
        /// <see cref="System.String"/> representing the version to pin. 
        /// Please refer to the WinGet documentation for more info about version pinning.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the pin was added successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public bool PinAdd(WinGetPackage package, string version)
        {
            ArgsHelper.ThrowIfObjectIsNull(package, "package");

            if (package.HasShortenedId)
            {
                return PinAdd(package.Name, version);
            }

            return PinAdd(package.Id, version);
        }

        /// <summary>
        /// Asynchronously adds a pinned package to winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package to pin.</param>
        /// <param name="blocking">Set to <see langword="true"/> if updating of pinned package should be fully blocked.</param>
        /// <returns>
        /// <see langword="true"/> if the pin was added successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<bool> PinAddAsync(string packageId, bool blocking = false)
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            string cmd = string.Format(_pinAddCmd, packageId);

            if (blocking)
            {
                cmd += " --blocking";
            }

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(cmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Pinning the package failed.", cmd, e);
            }
        }

        /// <summary>
        /// Asynchronously adds a pinned package to winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package to pin.</param>
        /// <param name="version">
        /// <see cref="System.String"/> representing the version to pin. 
        /// Please refer to the WinGet documentation for more info about version pinning.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the pin was added successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<bool> PinAddAsync(string packageId, string version)
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(version, "version");

            string cmd = string.Format(_pinAddByVersionCmd, packageId, version);

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(cmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Pinning the package failed.", cmd, e);
            }
        }

        /// <summary>
        /// Asynchronously adds a pinned package to winget.
        /// </summary>
        /// <param name="package">The package to pin.</param>
        /// <param name="blocking">Set to <see langword="true"/> if updating of pinned package should be fully blocked.</param>
        /// <returns>
        /// <see langword="true"/> if the pin was added successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<bool> PinAddAsync(WinGetPackage package, bool blocking = false)
        {
            ArgsHelper.ThrowIfObjectIsNull(package, "package");

            if (package.HasShortenedId)
            {
                return await PinAddAsync(package.Name, blocking);
            }

            return await PinAddAsync(package.Id, blocking);
        }

        /// <summary>
        /// Asynchronously adds a pinned package to winget.
        /// </summary>
        /// <param name="package">The package to pin.</param>
        /// <param name="version">
        /// <see cref="System.String"/> representing the version to pin. 
        /// Please refer to the WinGet documentation for more info about version pinning.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the pin was added successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<bool> PinAddAsync(WinGetPackage package, string version)
        {
            ArgsHelper.ThrowIfObjectIsNull(package, "package");

            if (package.HasShortenedId)
            {
                return await PinAddAsync(package.Name, version);
            }

            return await PinAddAsync(package.Id, version);
        }

        /// <summary>
        /// Adds a pinned installed package to winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package to pin.</param>
        /// <param name="blocking">Set to <see langword="true"/> if updating of pinned package should be fully blocked.</param>
        /// <returns>
        /// <see langword="true"/> if the pin was added successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public bool PinAddInstalled(string packageId, bool blocking = false)
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            string cmd = string.Format(_pinAddInstalledCmd, packageId);

            if (blocking)
            {
                cmd += " --blocking";
            }

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(cmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Pinning the package failed.", cmd, e);
            }
        }

        /// <summary>
        /// Adds a pinned installed package to winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package to pin.</param>
        /// <param name="version">
        /// <see cref="System.String"/> representing the version to pin. 
        /// Please refer to the WinGet documentation for more info about version pinning.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the pin was added successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public bool PinAddInstalled(string packageId, string version)
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(version, "version");

            string cmd = string.Format(_pinAddInstalledByVersionCmd, packageId, version);

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(cmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Pinning the package failed.", cmd, e);
            }
        }

        /// <summary>
        /// Adds a pinned installed package to winget.
        /// </summary>
        /// <param name="package">The package to pin.</param>
        /// <param name="blocking">Set to <see langword="true"/> if updating of pinned package should be fully blocked.</param>
        /// <returns>
        /// <see langword="true"/> if the pin was added successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public bool PinAddInstalled(WinGetPackage package, bool blocking = false)
        {
            ArgsHelper.ThrowIfObjectIsNull(package, "package");

            if (package.HasShortenedId)
            {
                return PinAddInstalled(package.Name, blocking);
            }

            return PinAddInstalled(package.Id, blocking);
        }

        /// <summary>
        /// Adds a pinned installed package to winget.
        /// </summary>
        /// <param name="package">The package to pin.</param>
        /// <param name="version">
        /// <see cref="System.String"/> representing the version to pin. 
        /// Please refer to the WinGet documentation for more info about version pinning.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the pin was added successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public bool PinAddInstalled(WinGetPackage package, string version)
        {
            ArgsHelper.ThrowIfObjectIsNull(package, "package");

            if (package.HasShortenedId)
            {
                return PinAddInstalled(package.Name, version);
            }

            return PinAddInstalled(package.Id, version);
        }

        /// <summary>
        /// Asynchronously adds a pinned installed package to winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package to pin.</param>
        /// <param name="blocking">Set to <see langword="true"/> if updating of pinned package should be fully blocked.</param>
        /// <returns>
        /// <see langword="true"/> if the pin was added successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<bool> PinAddInstalledAsync(string packageId, bool blocking = false)
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            string cmd = string.Format(_pinAddInstalledCmd, packageId);

            if (blocking)
            {
                cmd += " --blocking";
            }

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(cmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Pinning the package failed.", cmd, e);
            }
        }

        /// <summary>
        /// Asynchronously adds a pinned installed package to winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package to pin.</param>
        /// <param name="version">
        /// <see cref="System.String"/> representing the version to pin. 
        /// Please refer to the WinGet documentation for more info about version pinning.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the pin was added successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<bool> PinAddInstalledAsync(string packageId, string version)
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(version, "version");

            string cmd = string.Format(_pinAddInstalledByVersionCmd, packageId, version);

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(cmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Pinning the package failed.", cmd, e);
            }
        }

        /// <summary>
        /// Asynchronously adds a pinned installed package to winget.
        /// </summary>
        /// <param name="package">The package to pin.</param>
        /// <param name="blocking">Set to <see langword="true"/> if updating of pinned package should be fully blocked.</param>
        /// <returns>
        /// <see langword="true"/> if the pin was added successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<bool> PinAddInstalledAsync(WinGetPackage package, bool blocking = false)
        {
            ArgsHelper.ThrowIfObjectIsNull(package, "package");

            if (package.HasShortenedId)
            {
                return await PinAddInstalledAsync(package.Name, blocking);
            }

            return await PinAddInstalledAsync(package.Id, blocking);
        }

        /// <summary>
        /// Asynchronously adds a pinned installed package to winget.
        /// </summary>
        /// <param name="package">The package to pin.</param>
        /// <param name="version">
        /// <see cref="System.String"/> representing the version to pin. 
        /// Please refer to the WinGet documentation for more info about version pinning.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the pin was added successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<bool> PinAddInstalledAsync(WinGetPackage package, string version)
        {
            ArgsHelper.ThrowIfObjectIsNull(package, "package");

            if (package.HasShortenedId)
            {
                return await PinAddInstalledAsync(package.Name, version);
            }

            return await PinAddInstalledAsync(package.Id, version);
        }
        //---------------------------------------------------------------------------------------------

        //---Pin Remove--------------------------------------------------------------------------------
        /// <summary>
        /// Removes a pinned package from winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package to unpin.</param>
        /// <returns>
        /// <see langword="true"/> if the removal of the pin was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public bool PinRemove(string packageId)
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            string cmd = string.Format(_pinRemoveCmd, packageId);

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(cmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Unpinning the package failed.", cmd, e);
            }
        }

        /// <summary>
        /// Removes a pinned package from winget.
        /// </summary>
        /// <param name="package">The package to unpin.</param>
        /// <returns>
        /// <see langword="true"/> if the removal of the pin was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public bool PinRemove(WinGetPackage package)
        {
            ArgsHelper.ThrowIfObjectIsNull(package, "package");

            if (package.HasShortenedId)
            {
                return PinRemove(package.Name);
            }

            return PinRemove(package.Id);
        }

        /// <summary>
        /// Asynchronously removes a pinned package from winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package to unpin.</param>
        /// <returns>
        /// <see langword="true"/> if the removal of the pin was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<bool> PinRemoveAsync(string packageId)
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            string cmd = string.Format(_pinRemoveCmd, packageId);

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(cmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Unpinning the package failed.", cmd, e);
            }
        }

        /// <summary>
        /// Asynchronously removes a pinned package from winget.
        /// </summary>
        /// <param name="package">The package to unpin.</param>
        /// <returns>
        /// <see langword="true"/> if the removal of the pin was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<bool> PinRemoveAsync(WinGetPackage package)
        {
            ArgsHelper.ThrowIfObjectIsNull(package, "package");

            if (package.HasShortenedId)
            {
                return await PinRemoveAsync(package.Name);
            }

            return await PinRemoveAsync(package.Id);
        }

        /// <summary>
        /// Removes a pinned package from winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package to unpin.</param>
        /// <returns>
        /// <see langword="true"/> if the removal of the pin was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public bool PinRemoveInstalled(string packageId)
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            string cmd = string.Format(_pinRemoveInstalledCmd, packageId);

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(cmd);
                
                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Unpinning the package failed.", cmd, e);
            }
        }

        /// <summary>
        /// Removes a pinned package from winget.
        /// </summary>
        /// <param name="package">The package to unpin.</param>
        /// <returns>
        /// <see langword="true"/> if the removal of the pin was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public bool PinRemoveInstalled(WinGetPackage package)
        {
            ArgsHelper.ThrowIfObjectIsNull(package, "package");

            if (package.HasShortenedId)
            {
                return PinRemoveInstalled(package.Name);
            }

            return PinRemoveInstalled(package.Id);
        }

        /// <summary>
        /// Asynchronously removes a pinned package from winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package to unpin.</param>
        /// <returns>
        /// <see langword="true"/> if the removal of the pin was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<bool> PinRemoveInstalledAsync(string packageId)
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            string cmd = string.Format(_pinRemoveInstalledCmd, packageId);

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(cmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Unpinning the package failed.", cmd, e);
            }
        }

        /// <summary>
        /// Asynchronously removes a pinned package from winget.
        /// </summary>
        /// <param name="package">The package to unpin.</param>
        /// <returns>
        /// <see langword="true"/> if the removal of the pin was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<bool> PinRemoveInstalledAsync(WinGetPackage package)
        {
            ArgsHelper.ThrowIfObjectIsNull(package, "package");

            if (package.HasShortenedId)
            {
                return await PinRemoveInstalledAsync(package.Name);
            }

            return await PinRemoveInstalledAsync(package.Id);
        }
        //---------------------------------------------------------------------------------------------

        //---Pin Reset---------------------------------------------------------------------------------
        /// <summary>
        /// Resets all pinned packages.
        /// </summary>
        /// <remarks>
        /// This will remove all pins and it is not possible to restore them.
        /// </remarks>
        /// <returns>
        /// <see langword="true"/> if the reset was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        public bool ResetPins()
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(_pinResetCmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Reseting of pins failed.", _pinResetCmd, e);
            }
        }

        /// <summary>
        /// Asynchronously resets all pinned packages.
        /// </summary>
        /// <remarks>
        /// This will remove all pins and it is not possible to restore them.
        /// </remarks>
        /// <returns>
        /// <see langword="true"/> if the reset was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        public async Task<bool> ResetPinsAsync()
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(_pinResetCmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Reseting of pins failed.", _pinResetCmd, e);
            }
        }
        //---------------------------------------------------------------------------------------------
    }
}
