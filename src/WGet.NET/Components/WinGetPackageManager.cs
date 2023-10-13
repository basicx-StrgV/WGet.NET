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
        private const string _searchInstalledCmd = "list {0}";
        private const string _searchInstalledBySourceCmd = "list {0} --source {1}";
        private const string _searchCmd = "search {0} --accept-source-agreements";
        private const string _searchBySourceCmd = "search {0} --source {1} --accept-source-agreements";
        private const string _installCmd = "install {0}";
        private const string _upgradeCmd = "upgrade {0}";
        private const string _upgradeAllCmd = "upgrade --all";
        private const string _getUpgradeableCmd = "upgrade";
        private const string _includeUnknown = "--include-unknown";
        private const string _uninstallCmd = "uninstall {0}";
        private const string _exportCmd = "export -o {0}";
        private const string _importCmd = "import -i {0} --ignore-unavailable";
        private const string _hashCmd = "hash {0}";

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetPackageManager"/> class.
        /// </summary>
        public WinGetPackageManager()
        {
           //Provide empty constructor
        }

        //---Search------------------------------------------------------------------------------------
        /// <summary>
        /// Uses the winget search function to search for a package that maches the given name.
        /// </summary>
        /// <param name="packageName">
        /// The name of the package for the search.
        /// </param>
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
        public List<WinGetPackage> SearchPackage(string packageName)
        {
            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(
                        string.Format(_searchCmd, packageName));

                return ProcessOutputReader.ToPackageList(result.Output, PackageAction.Search);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The package search failed.", e);
            }
        }

        /// <summary>
        /// Uses the winget search function to search for a package that maches the given name.
        /// </summary>
        /// <param name="packageName">
        /// The name of the package for the search.
        /// </param>
        /// <param name="sourceName">
        /// The name of the source for the search.
        /// </param>
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
        public List<WinGetPackage> SearchPackage(string packageName, string sourceName)
        {
            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(
                        string.Format(_searchBySourceCmd, packageName, sourceName));

                return ProcessOutputReader.ToPackageList(result.Output, PackageAction.SearchBySource, sourceName);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The package search failed.", e);
            }
        }

        /// <summary>
        /// Uses the winget search function to asynchronously search for a package that maches the given name.
        /// </summary>
        /// <param name="packageName">
        /// The name of the package for the search.
        /// </param>
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
        public async Task<List<WinGetPackage>> SearchPackageAsync(string packageName)
        {
            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(
                        string.Format(_searchCmd, packageName));

                return ProcessOutputReader.ToPackageList(result.Output, PackageAction.Search);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The package search failed.", e);
            }
        }

        /// <summary>
        /// Uses the winget search function to asynchronously search for a package that maches the given name.
        /// </summary>
        /// <param name="packageName">
        /// The name of the package for the search.
        /// </param>
        /// <param name="sourceName">
        /// The name of the source for the search.
        /// </param>
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
        public async Task<List<WinGetPackage>> SearchPackageAsync(string packageName, string sourceName)
        {
            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(
                        string.Format(_searchBySourceCmd, packageName, sourceName));

                return ProcessOutputReader.ToPackageList(result.Output, PackageAction.SearchBySource, sourceName);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The package search failed.", e);
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
                throw new WinGetActionFailedException("The search of installed packages failed.", e);
            }
        }

        /// <summary>
        /// Gets a list of all installed packages. That match the provided name.
        /// </summary>
        /// <param name="packageName">
        /// The name of the package for the search.
        /// </param>
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
        public List<WinGetPackage> GetInstalledPackages(string packageName)
        {
            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(string.Format(_searchInstalledCmd, packageName));

                return ProcessOutputReader.ToPackageList(result.Output, PackageAction.InstalledList);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The search of installed packages failed.", e);
            }
        }

        /// <summary>
        /// Gets a list of all installed packages. That match the provided name.
        /// </summary>
        /// <param name="packageName">
        /// The name of the package for the search.
        /// </param>
        /// <param name="sourceName">
        /// The name of the source for the search.
        /// </param>
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
        public List<WinGetPackage> GetInstalledPackages(string packageName, string sourceName)
        {
            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(string.Format(_searchInstalledBySourceCmd, packageName, sourceName));

                return ProcessOutputReader.ToPackageList(result.Output, PackageAction.InstalledListBySource, sourceName);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The search of installed packages failed.", e);
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
                throw new WinGetActionFailedException("The search of installed packages failed.", e);
            }
        }

        /// <summary>
        /// Asynchronously gets a list of all installed packages. That match the provided name.
        /// </summary>
        /// <param name="packageName">
        /// The name of the package for the search.
        /// </param>
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
        public async Task<List<WinGetPackage>> GetInstalledPackagesAsync(string packageName)
        {
            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(string.Format(_searchInstalledCmd, packageName));

                return ProcessOutputReader.ToPackageList(result.Output, PackageAction.InstalledList);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The search of installed packages failed.", e);
            }
        }

        /// <summary>
        /// Asynchronously gets a list of all installed packages. That match the provided name.
        /// </summary>
        /// <param name="packageName">
        /// The name of the package for the search.
        /// </param>
        /// <param name="sourceName">
        /// The name of the source for the search.
        /// </param>
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
        public async Task<List<WinGetPackage>> GetInstalledPackagesAsync(string packageName, string sourceName)
        {
            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(string.Format(_searchInstalledBySourceCmd, packageName, sourceName));

                return ProcessOutputReader.ToPackageList(result.Output, PackageAction.InstalledListBySource, sourceName);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The search of installed packages failed.", e);
            }
        }

        /// <summary>
        /// Install a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package for the installation.</param>
        /// <returns>
        /// <see langword="true"/> if the installation was successfull or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public bool InstallPackage(string packageId)
        {
            if (string.IsNullOrWhiteSpace(packageId))
            {
                return false;
            }

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(
                        string.Format(_installCmd, packageId));

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The package installtion failed.", e);
            }
        }

        /// <summary>
        /// Install a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> for the installation.</param>
        /// <returns>
        /// <see langword="true"/> if the installation was successfull or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public bool InstallPackage(WinGetPackage package)
        {
            if (package == null)
            {
                return false;
            }

            if (package.IsEmpty)
            {
                return false;
            }

            return InstallPackage(package.PackageId);
        }

        /// <summary>
        /// Asynchronously install a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package for the installation.</param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the installation was successfull or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public async Task<bool> InstallPackageAsync(string packageId)
        {
            if (string.IsNullOrWhiteSpace(packageId))
            {
                return false;
            }

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(
                        string.Format(_installCmd, packageId));

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The package installtion failed.", e);
            }
        }

        /// <summary>
        /// Asynchronously install a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> for the installation.</param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the installation was successfull or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public async Task<bool> InstallPackageAsync(WinGetPackage package)
        {
            if (package == null)
            {
                return false;
            }

            if (package.IsEmpty)
            {
                return false;
            }

            return await InstallPackageAsync(package.PackageId);
        }
        //---------------------------------------------------------------------------------------------

        //---Uninstall---------------------------------------------------------------------------------
        /// <summary>
        /// Uninsatll a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package for uninstallation.</param>
        /// <returns>
        /// <see langword="true"/> if the uninstallation was successfull or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public bool UninstallPackage(string packageId)
        {
            if (string.IsNullOrWhiteSpace(packageId))
            {
                return false;
            }

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(
                        string.Format(_uninstallCmd, packageId));

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The package uninstalltion failed.", e);
            }
        }

        /// <summary>
        /// Uninstall a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> for the uninstallation.</param>
        /// <returns>
        /// <see langword="true"/> if the uninstallation was successfull or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public bool UninstallPackage(WinGetPackage package)
        {
            if (package == null)
            {
                return false;
            }

            if (package.IsEmpty)
            {
                return false;
            }

            return UninstallPackage(package.PackageId);
        }

        /// <summary>
        /// Asynchronously uninsatll a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package for uninstallation.</param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the uninstallation was successfull or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public async Task<bool> UninstallPackageAsync(string packageId)
        {
            if (string.IsNullOrWhiteSpace(packageId))
            {
                return false;
            }

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(
                        string.Format(_uninstallCmd, packageId));

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The package uninstalltion failed.", e);
            }
        }

        /// <summary>
        /// Asynchronously uninstall a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> for the uninstallation.</param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the uninstallation was successfull or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public async Task<bool> UninstallPackageAsync(WinGetPackage package)
        {
            if (package == null)
            {
                return false;
            }

            if (package.IsEmpty)
            {
                return false;
            }

            return await UninstallPackageAsync(package.PackageId);
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
            try
            {
                string argument = AddArgumentByVersion(_getUpgradeableCmd);

                ProcessResult result =
                    _processManager.ExecuteWingetProcess(argument);

                return ProcessOutputReader.ToPackageList(result.Output, PackageAction.UpgradeList);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Getting updateable packages failed.", e);
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
            try
            {
                string argument = AddArgumentByVersion(_getUpgradeableCmd);

                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(argument);

                return ProcessOutputReader.ToPackageList(result.Output, PackageAction.UpgradeList);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Getting updateable packages failed.", e);
            }
        }

        /// <summary>
        /// Upgrades a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package for upgrade.</param>
        /// <returns>
        /// <see langword="true"/> if the upgrade was successfull or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public bool UpgradePackage(string packageId)
        {
            if (string.IsNullOrWhiteSpace(packageId))
            {
                return false;
            }

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(
                        string.Format(_upgradeCmd, packageId));

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Upgrading the package failed.", e);
            }
        }

        /// <summary>
        /// Upgrades a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> that for the upgrade</param>
        /// <returns>
        /// <see langword="true"/> if the upgrade was successfull or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public bool UpgradePackage(WinGetPackage package)
        {
            if (package == null)
            {
                return false;
            }
            
            if (package.IsEmpty)
            {
                return false;
            }

            return UpgradePackage(package.PackageId);
        }

        /// <summary>
        /// Asynchronously upgrades a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package for upgrade.</param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the upgrade was successfull or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public async Task<bool> UpgradePackageAsync(string packageId)
        {
            if (string.IsNullOrWhiteSpace(packageId))
            {
                return false;
            }

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(
                        string.Format(_upgradeCmd, packageId));

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Upgrading the package failed.", e);
            }
        }

        /// <summary>
        /// Asynchronously upgrades a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> that for the upgrade</param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the upgrade was successfull or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public async Task<bool> UpgradePackageAsync(WinGetPackage package)
        {
            if (package == null)
            {
                return false;
            }

            if (package.IsEmpty)
            {
                return false;
            }

            return await UpgradePackageAsync(package.PackageId);
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
                throw new WinGetActionFailedException("Upgrading all packages failed.", e);
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
                throw new WinGetActionFailedException("Upgrading all packages failed.", e);
            }
        }

        private string AddArgumentByVersion(string argument)
        {
            // Checking version to determine if "--include-unknown" is necessary.
            Version winGetVersion = WinGetVersionObject;
            if (winGetVersion.Major >= 1 && winGetVersion.Minor >= 4)
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
        /// <see langword="true"/> if the export was successfull or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public bool ExportPackagesToFile(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                return false;
            }

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(
                        string.Format(_exportCmd, file));

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Exporting packages failed.", e);
            }
        }

        /// <summary>
        /// Asynchronously exports a list of all installed winget packages as json to the given file.
        /// </summary>
        /// <param name="file">The file for the export.</param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the export was successfull or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public async Task<bool> ExportPackagesToFileAsync(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                return false;
            }

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(
                        string.Format(_exportCmd, file));

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Exporting packages failed.", e);
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
        /// <see langword="true"/> if the import was compleatly successfull or 
        /// <see langword="false"/> if some or all packages failed to install.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public bool ImportPackagesFromFile(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                return false;
            }

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(
                        string.Format(_importCmd, file));

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Importing packages failed.", e);
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
        /// The result is <see langword="true"/> if the import was compleatly successfull or 
        /// <see langword="false"/> if some or all packages failed to install.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public async Task<bool> ImportPackagesFromFileAsync(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                return false;
            }

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(
                        string.Format(_importCmd, file));

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Importing packages failed.", e);
            }
        }
        //---------------------------------------------------------------------------------------------

        //---Other-------------------------------------------------------------------------------------
        /// <summary>
        /// Executes the WinGet hash function, to calculate the hash for the given file.
        /// </summary>
        /// <param name="file">
        /// A <see cref="System.String"/> containing the path to the file.
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/> containing the hash.
        /// </returns>
        public string Hash(string file)
        {
            if (!File.Exists(file))
            {
                throw new WinGetActionFailedException("File does not exist.");
            }

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(
                        string.Format(_hashCmd, file));

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
                throw new WinGetActionFailedException("Hashing failed.", e);
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
        public string Hash(FileInfo file)
        {
            if (!file.Exists)
            {
                throw new WinGetActionFailedException("File does not exist.");
            }

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(
                        string.Format(_hashCmd, file.FullName));

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
                throw new WinGetActionFailedException("Hashing failed.", e);
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
        public async Task<string> HashAsync(string file)
        {
            if (!File.Exists(file))
            {
                throw new WinGetActionFailedException("File does not exist.");
            }

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(
                        string.Format(_hashCmd, file));

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
                throw new WinGetActionFailedException("Hashing failed.", e);
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
        public async Task<string> HashAsync(FileInfo file)
        {
            if (!file.Exists)
            {
                throw new WinGetActionFailedException("File does not exist.");
            }

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(
                        string.Format(_hashCmd, file.FullName));

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
                throw new WinGetActionFailedException("Hashing failed.", e);
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
    }
}
