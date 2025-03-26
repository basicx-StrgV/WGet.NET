//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WGetNET.Components.Internal;
using WGetNET.Exceptions;
using WGetNET.Helper;
using WGetNET.Models;

namespace WGetNET
{
    /// <summary>
    /// The <see cref="WGetNET.WinGetPackageManager"/> class offers methods to manage packages with winget.
    /// </summary>
    public class WinGetPackageManager : WinGet
    {
        private readonly Version _downloadMinVersion = new(1, 6, 0);
        private readonly Version _repairMinVersion = new(1, 7, 0);
        private readonly Version _pinMinVersion = new(1, 5, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetPackageManager"/> class.
        /// </summary>
        /// <remarks>
        /// Used to for local winget handling.
        /// </remarks>
        public WinGetPackageManager()
        {
            // Provide empty constructor for xlm docs
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetPackageManager"/> class.
        /// </summary>
        /// <remarks>
        /// Used to for winget handling over ssh.
        /// </remarks>
        /// <param name="host">
        /// A <see cref="System.String"/> containing the host information (Hostaname or IP).
        /// </param>
        /// <param name="username">
        /// A <see cref="System.String"/> containing the ssh username.
        /// </param>
        /// <param name="password">
        /// A <see cref="System.String"/> containing the ssh user password.
        /// </param>
        public WinGetPackageManager(string host, string username, string password) : base(host, username, password)
        {
            // Provide constructor for ssh client.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetPackageManager"/> class.
        /// </summary>
        /// <remarks>
        /// Used to for winget handling over ssh.
        /// </remarks>
        /// <param name="host">
        /// A <see cref="System.String"/> containing the host information (Hostaname or IP).
        /// </param>
        /// <param name="port">
        /// The port for the ssh connection.
        /// </param>
        /// <param name="username">
        /// A <see cref="System.String"/> containing the ssh username.
        /// </param>
        /// <param name="password">
        /// A <see cref="System.String"/> containing the ssh user password.
        /// </param>
        public WinGetPackageManager(string host, int port, string username, string password) : base(host, port, username, password)
        {
            // Provide constructor for ssh client.
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
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public List<WinGetPackage> SearchPackage(string packageId, bool exact = false)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            WinGetArguments cmd = WinGetArguments.Search().Query(packageId).AcceptSourceAgreements();

            if (exact)
            {
                cmd.Exact();
            }

            ProcessResult result = Execute(cmd);

            return ProcessOutputReader.ToPackageList(result.Output, PackageAction.Search);
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
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public List<WinGetPackage> SearchPackage(string packageId, string sourceName, bool exact = false)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(sourceName, "sourceName");

            WinGetArguments cmd = WinGetArguments.Search().Query(packageId).Source(sourceName).AcceptSourceAgreements();

            if (exact)
            {
                cmd.Exact();
            }

            ProcessResult result = Execute(cmd);

            return ProcessOutputReader.ToPackageList(result.Output, PackageAction.SearchBySource, sourceName);
        }

        /// <summary>
        /// Uses the winget search function to asynchronously search for a package that maches the given name.
        /// </summary>
        /// <param name="packageId">
        /// The id or name of the package for the search.
        /// </param>
        /// <param name="exact">Use exact match.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<List<WinGetPackage>> SearchPackageAsync(string packageId, bool exact = false, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            WinGetArguments cmd = WinGetArguments.Search().Query(packageId).AcceptSourceAgreements();

            if (exact)
            {
                cmd.Exact();
            }

            ProcessResult result = await ExecuteAsync(cmd, false, cancellationToken);

            // Return empty list if the task was cancled
            if (cancellationToken.IsCancellationRequested)
            {
                return new List<WinGetPackage>();
            }

            return ProcessOutputReader.ToPackageList(result.Output, PackageAction.Search);
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
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<List<WinGetPackage>> SearchPackageAsync(string packageId, string sourceName, bool exact = false, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(sourceName, "sourceName");

            WinGetArguments cmd = WinGetArguments.Search().Query(packageId).Source(sourceName).AcceptSourceAgreements();

            if (exact)
            {
                cmd.Exact();
            }

            ProcessResult result = await ExecuteAsync(cmd, false, cancellationToken);

            // Return empty list if the task was cancled
            if (cancellationToken.IsCancellationRequested)
            {
                return new List<WinGetPackage>();
            }

            return ProcessOutputReader.ToPackageList(result.Output, PackageAction.SearchBySource, sourceName);
        }
        //---------------------------------------------------------------------------------------------

        //---List--------------------------------------------------------------------------------------
        /// <summary>
        /// Gets a list of all installed packages.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public List<WinGetPackage> GetInstalledPackages()
        {
            ProcessResult result = Execute(WinGetArguments.List().AcceptSourceAgreements());

            return ProcessOutputReader.ToPackageList(result.Output, PackageAction.InstalledList);
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
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public List<WinGetPackage> GetInstalledPackages(string packageId, bool exact = false)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            WinGetArguments cmd = WinGetArguments.List().Query(packageId).AcceptSourceAgreements();

            if (exact)
            {
                cmd.Exact();
            }

            ProcessResult result = Execute(cmd);

            return ProcessOutputReader.ToPackageList(result.Output, PackageAction.InstalledList);
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
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public List<WinGetPackage> GetInstalledPackages(string packageId, string sourceName, bool exact = false)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(sourceName, "sourceName");

            WinGetArguments cmd = WinGetArguments.List().Query(packageId).Source(sourceName).AcceptSourceAgreements();

            if (exact)
            {
                cmd.Exact();
            }

            ProcessResult result = Execute(cmd);

            return ProcessOutputReader.ToPackageList(result.Output, PackageAction.InstalledListBySource, sourceName);
        }

        /// <summary>
        /// Asynchronously gets a list of all installed packages.
        /// </summary>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<List<WinGetPackage>> GetInstalledPackagesAsync(CancellationToken cancellationToken = default)
        {
            ProcessResult result = await ExecuteAsync(WinGetArguments.List().AcceptSourceAgreements(), false, cancellationToken);

            // Return empty list if the task was cancled
            if (cancellationToken.IsCancellationRequested)
            {
                return new List<WinGetPackage>();
            }

            return ProcessOutputReader.ToPackageList(result.Output, PackageAction.InstalledList);
        }

        /// <summary>
        /// Asynchronously gets a list of all installed packages. That match the provided name.
        /// </summary>
        /// <param name="packageId">
        /// The id or name of the package for the search.
        /// </param>
        /// <param name="exact">Use exact match.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<List<WinGetPackage>> GetInstalledPackagesAsync(string packageId, bool exact = false, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            WinGetArguments cmd = WinGetArguments.List().Query(packageId).AcceptSourceAgreements();

            if (exact)
            {
                cmd.Exact();
            }

            ProcessResult result = await ExecuteAsync(cmd, false, cancellationToken);

            // Return empty list if the task was cancled
            if (cancellationToken.IsCancellationRequested)
            {
                return new List<WinGetPackage>();
            }

            return ProcessOutputReader.ToPackageList(result.Output, PackageAction.InstalledList);
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
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<List<WinGetPackage>> GetInstalledPackagesAsync(string packageId, string sourceName, bool exact = false, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(sourceName, "sourceName");

            WinGetArguments cmd = WinGetArguments.List().Query(packageId).Source(sourceName).AcceptSourceAgreements();

            if (exact)
            {
                cmd.Exact();
            }

            ProcessResult result = await ExecuteAsync(cmd, false, cancellationToken);

            // Return empty list if the task was cancled
            if (cancellationToken.IsCancellationRequested)
            {
                return new List<WinGetPackage>();
            }

            return ProcessOutputReader.ToPackageList(result.Output, PackageAction.InstalledListBySource, sourceName);
        }

        /// <summary>
        /// Gets a installed package, that matchs the provided id/name. If there are multiple matches, the first match will be returned.
        /// </summary>
        /// <remarks>
        /// This method does an internal match and does not use the winget "exact" functionality.
        /// </remarks>
        /// <param name="packageId">
        /// The id or name of the package for the search.
        /// </param>
        /// <returns>
        /// A <see cref="WGetNET.WinGetPackage"/> instances or <see langword="null"/> if no match was found.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public WinGetPackage? GetExactInstalledPackage(string packageId)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            ProcessResult result = Execute(WinGetArguments.List().AcceptSourceAgreements());

            return PackageHelper.MatchExact(
                ProcessOutputReader.ToPackageList(result.Output, PackageAction.InstalledList),
                packageId.Trim()
            );
        }

        /// <summary>
        /// Gets a installed package, that matchs the provided id/name. If there are multiple matches, the first match will be returned.
        /// </summary>
        /// <remarks>
        /// This method does an internal match and does not use the winget "exact" functionality.
        /// </remarks>
        /// <param name="packageId">
        /// The id or name of the package for the search.
        /// </param>
        /// <param name="sourceName">
        /// The name of the source for the search.
        /// </param>
        /// <returns>
        /// A <see cref="WGetNET.WinGetPackage"/> instances or <see langword="null"/> if no match was found.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public WinGetPackage? GetExactInstalledPackage(string packageId, string sourceName)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(sourceName, "sourceName");

            ProcessResult result = Execute(WinGetArguments.List().Query(packageId).Source(sourceName).AcceptSourceAgreements());

            return PackageHelper.MatchExact(
                ProcessOutputReader.ToPackageList(result.Output, PackageAction.InstalledList),
                packageId.Trim()
            );
        }

        /// <summary>
        /// Asynchronously gets a installed package, that matchs the provided id/name. If there are multiple matches, the first match will be returned.
        /// </summary>
        /// <remarks>
        /// This method does an internal match and does not use the winget "exact" functionality.
        /// </remarks>
        /// <param name="packageId">
        /// The id or name of the package for the search.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="WGetNET.WinGetPackage"/> instances or <see langword="null"/> if no match was found.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<WinGetPackage?> GetExactInstalledPackageAsync(string packageId, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            ProcessResult result = await ExecuteAsync(WinGetArguments.List().AcceptSourceAgreements(), false, cancellationToken);

            // Return null if the task was cancled
            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            return PackageHelper.MatchExact(
                ProcessOutputReader.ToPackageList(result.Output, PackageAction.InstalledList),
                packageId.Trim()
            );
        }

        /// <summary>
        /// Asynchronously gets a installed package, that matchs the provided id/name. If there are multiple matches, the first match will be returned.
        /// </summary>
        /// <remarks>
        /// This method does an internal match and does not use the winget "exact" functionality.
        /// </remarks>
        /// <param name="packageId">
        /// The id or name of the package for the search.
        /// </param>
        /// <param name="sourceName">
        /// The name of the source for the search.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="WGetNET.WinGetPackage"/> instances or <see langword="null"/> if no match was found.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<WinGetPackage?> GetExactInstalledPackageAsync(string packageId, string sourceName, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(sourceName, "sourceName");

            ProcessResult result =
                await ExecuteAsync(
                    WinGetArguments.List().Query(packageId).Source(sourceName).AcceptSourceAgreements(),
                    false, cancellationToken);

            // Return null if the task was cancled
            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            return PackageHelper.MatchExact(
                ProcessOutputReader.ToPackageList(result.Output, PackageAction.InstalledList),
                packageId.Trim()
            );
        }
        //---------------------------------------------------------------------------------------------

        //---Install-----------------------------------------------------------------------------------
        /// <summary>
        /// Install a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package for the installation.</param>
        /// <returns>
        /// <see langword="true"/> if the installation was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool InstallPackage(string packageId)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            ProcessResult result = Execute(WinGetArguments.Install().Query(packageId).AcceptSourceAgreements().AcceptPackageAgreements());

            return result.Success;
        }

        /// <summary>
        /// Install a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package for the installation.</param>
        /// <param name="silent">Request silent installation of packages.</param>
        /// <returns>
        /// <see langword="true"/> if the installation was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool InstallPackage(string packageId, bool silent)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            WinGetArguments cmd = WinGetArguments.Install().Query(packageId).AcceptSourceAgreements().AcceptPackageAgreements();

            if (silent)
            {
                cmd.Silent();
            }

            ProcessResult result = Execute(cmd);

            return result.Success;
        }

        /// <summary>
        /// Install a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> for the installation.</param>
        /// <returns>
        /// <see langword="true"/> if the installation was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool InstallPackage(WinGetPackage package)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
            {
                return InstallPackage(package.Name);
            }

            return InstallPackage(package.Id);
        }

        /// <summary>
        /// Install a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> for the installation.</param>
        /// <param name="silent">Request silent installation of packages.</param>
        /// <returns>
        /// <see langword="true"/> if the installation was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool InstallPackage(WinGetPackage package, bool silent)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
            {
                return InstallPackage(package.Name, silent);
            }

            return InstallPackage(package.Id, silent);
        }

        /// <summary>
        /// Asynchronously install a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package for the installation.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the installation was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> InstallPackageAsync(string packageId, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            ProcessResult result =
                await ExecuteAsync(
                    WinGetArguments.Install().Query(packageId).AcceptSourceAgreements().AcceptPackageAgreements(),
                    false, cancellationToken);

            return result.Success;
        }

        /// <summary>
        /// Asynchronously install a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package for the installation.</param>
        /// <param name="silent">Request silent installation of packages.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the installation was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> InstallPackageAsync(string packageId, bool silent, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            WinGetArguments cmd = WinGetArguments.Install().Query(packageId).AcceptSourceAgreements().AcceptPackageAgreements();

            if (silent)
            {
                cmd.Silent();
            }

            ProcessResult result = await ExecuteAsync(cmd, false, cancellationToken);

            return result.Success;
        }

        /// <summary>
        /// Asynchronously install a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> for the installation.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the installation was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> InstallPackageAsync(WinGetPackage package, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
            {
                return await InstallPackageAsync(package.Name, cancellationToken);
            }

            return await InstallPackageAsync(package.Id, cancellationToken);
        }

        /// <summary>
        /// Asynchronously install a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> for the installation.</param>
        /// <param name="silent">Request silent installation of packages.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the installation was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> InstallPackageAsync(WinGetPackage package, bool silent, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
            {
                return await InstallPackageAsync(package.Name, silent, cancellationToken);
            }

            return await InstallPackageAsync(package.Id, silent, cancellationToken);
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
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool UninstallPackage(string packageId)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            ProcessResult result = Execute(WinGetArguments.Uninstall().Query(packageId));

            return result.Success;
        }

        /// <summary>
        /// Uninsatll a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package for uninstallation.</param>
        /// <param name="silent">Request silent package uninstall.</param>
        /// <returns>
        /// <see langword="true"/> if the uninstallation was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool UninstallPackage(string packageId, bool silent)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            WinGetArguments cmd = WinGetArguments.Uninstall().Query(packageId);

            if (silent)
            {
                cmd.Silent();
            }

            ProcessResult result = Execute(cmd);

            return result.Success;
        }

        /// <summary>
        /// Uninstall a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> for the uninstallation.</param>
        /// <returns>
        /// <see langword="true"/> if the uninstallation was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool UninstallPackage(WinGetPackage package)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
            {
                return UninstallPackage(package.Name);
            }

            return UninstallPackage(package.Id);
        }

        /// <summary>
        /// Uninstall a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> for the uninstallation.</param>
        /// <param name="silent" > Request silent package uninstall.</param>
        /// <returns>
        /// <see langword="true"/> if the uninstallation was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool UninstallPackage(WinGetPackage package, bool silent)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
            {
                return UninstallPackage(package.Name, silent);
            }

            return UninstallPackage(package.Id, silent);
        }

        /// <summary>
        /// Asynchronously uninsatll a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package for uninstallation.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the uninstallation was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> UninstallPackageAsync(string packageId, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            ProcessResult result = await ExecuteAsync(WinGetArguments.Uninstall().Query(packageId), false, cancellationToken);

            return result.Success;
        }

        /// <summary>
        /// Asynchronously uninsatll a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package for uninstallation.</param>
        /// <param name="silent" > Request silent package uninstall.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the uninstallation was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> UninstallPackageAsync(string packageId, bool silent, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            WinGetArguments cmd = WinGetArguments.Uninstall().Query(packageId);

            if (silent)
            {
                cmd.Silent();
            }

            ProcessResult result =
                await ExecuteAsync(cmd, false, cancellationToken);

            return result.Success;
        }

        /// <summary>
        /// Asynchronously uninstall a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> for the uninstallation.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the uninstallation was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> UninstallPackageAsync(WinGetPackage package, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
            {
                return await UninstallPackageAsync(package.Name, cancellationToken);
            }

            return await UninstallPackageAsync(package.Id, cancellationToken);
        }

        /// <summary>
        /// Asynchronously uninstall a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> for the uninstallation.</param>
        /// <param name="silent" > Request silent package uninstall.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the uninstallation was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> UninstallPackageAsync(WinGetPackage package, bool silent, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
            {
                return await UninstallPackageAsync(package.Name, silent, cancellationToken);
            }

            return await UninstallPackageAsync(package.Id, silent, cancellationToken);
        }
        //---------------------------------------------------------------------------------------------

        //---List Upgrades-----------------------------------------------------------------------------
        /// <summary>
        /// Get all upgradeable packages.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public List<WinGetPackage> GetUpgradeablePackages()
        {
            WinGetArguments cmd =
                IncludeUnknownbyVersion(
                    WinGetArguments.Upgrade().AcceptSourceAgreements());

            ProcessResult result = Execute(cmd);

            return ProcessOutputReader.ToPackageList(result.Output, PackageAction.UpgradeList);
        }

        /// <summary>
        /// Asynchronously get all upgradeable packages.
        /// </summary>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<List<WinGetPackage>> GetUpgradeablePackagesAsync(CancellationToken cancellationToken = default)
        {
            WinGetArguments cmd =
                IncludeUnknownbyVersion(
                    WinGetArguments.Upgrade().AcceptSourceAgreements());

            ProcessResult result = await ExecuteAsync(cmd, false, cancellationToken);

            // Return empty list if the task was cancled
            if (cancellationToken.IsCancellationRequested)
            {
                return new List<WinGetPackage>();
            }

            return ProcessOutputReader.ToPackageList(result.Output, PackageAction.UpgradeList);
        }
        //---------------------------------------------------------------------------------------------

        //---Upgrade-----------------------------------------------------------------------------------
        /// <summary>
        /// Upgrades a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package that should be upgraded.</param>
        /// <returns>
        /// <see langword="true"/> if the upgrade was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool UpgradePackage(string packageId)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            ProcessResult result = Execute(WinGetArguments.Upgrade().Query(packageId).AcceptSourceAgreements().AcceptPackageAgreements());

            return result.Success;
        }

        /// <summary>
        /// Upgrades a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package that should be upgraded.</param>
        /// <param name="silent">Request silent upgrade of packages.</param>
        /// <returns>
        /// <see langword="true"/> if the upgrade was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool UpgradePackage(string packageId, bool silent)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            WinGetArguments cmd = WinGetArguments.Upgrade().Query(packageId).AcceptSourceAgreements().AcceptPackageAgreements();

            if (silent)
            {
                cmd.Silent();
            }

            ProcessResult result = Execute(cmd);

            return result.Success;
        }

        /// <summary>
        /// Upgrades a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> that should be upgraded.</param>
        /// <returns>
        /// <see langword="true"/> if the upgrade was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool UpgradePackage(WinGetPackage package)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
            {
                return UpgradePackage(package.Name);
            }

            return UpgradePackage(package.Id);
        }

        /// <summary>
        /// Upgrades a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> that should be upgraded.</param>
        /// <param name="silent">Request silent upgrade of packages.</param>
        /// <returns>
        /// <see langword="true"/> if the upgrade was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool UpgradePackage(WinGetPackage package, bool silent)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
            {
                return UpgradePackage(package.Name, silent);
            }

            return UpgradePackage(package.Id, silent);
        }

        /// <summary>
        /// Asynchronously upgrades a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package that should be upgraded.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the upgrade was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> UpgradePackageAsync(string packageId, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            ProcessResult result =
                await ExecuteAsync(
                    WinGetArguments.Upgrade().Query(packageId).AcceptSourceAgreements().AcceptPackageAgreements(),
                    false, cancellationToken);

            return result.Success;
        }

        /// <summary>
        /// Asynchronously upgrades a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package that should be upgraded.</param>
        /// <param name="silent">Request silent upgrade of packages.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the upgrade was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> UpgradePackageAsync(string packageId, bool silent, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            WinGetArguments cmd = WinGetArguments.Upgrade().Query(packageId).AcceptSourceAgreements().AcceptPackageAgreements();

            if (silent)
            {
                cmd.Silent();
            }

            ProcessResult result = await ExecuteAsync(cmd, false, cancellationToken);

            return result.Success;
        }

        /// <summary>
        /// Asynchronously upgrades a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> that should be upgraded.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the upgrade was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> UpgradePackageAsync(WinGetPackage package, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
            {
                return await UpgradePackageAsync(package.Name, cancellationToken);
            }

            return await UpgradePackageAsync(package.Id, cancellationToken);
        }

        /// <summary>
        /// Asynchronously upgrades a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> that should be upgraded.</param>
        /// <param name="silent">Request silent upgrade of packages.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the upgrade was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> UpgradePackageAsync(WinGetPackage package, bool silent, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
            {
                return await UpgradePackageAsync(package.Name, silent, cancellationToken);
            }

            return await UpgradePackageAsync(package.Id, silent, cancellationToken);
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
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool UpgradeAllPackages()
        {
            ProcessResult result = Execute(WinGetArguments.Upgrade().All().AcceptSourceAgreements().AcceptPackageAgreements());

            return result.Success;
        }

        /// <summary>
        /// Tries to upgrade all packages using winget.
        /// </summary>
        /// <remarks>
        /// The action might run succesfully without upgrading every or even any package.
        /// </remarks>
        /// <param name="silent">Request silent upgrade of packages.</param>
        /// <returns>
        /// <see langword="true"/> if the action run successfully or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool UpgradeAllPackages(bool silent)
        {
            WinGetArguments cmd = WinGetArguments.Upgrade().All().AcceptSourceAgreements().AcceptPackageAgreements();

            if (silent)
            {
                cmd.Silent();
            }

            ProcessResult result = Execute(cmd);

            return result.Success;
        }

        /// <summary>
        /// Asynchronously tries to upgrade all packages using winget.
        /// </summary>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <remarks>
        /// The action might run succesfully without upgrading every or even any package.
        /// </remarks>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the action run successfully or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> UpgradeAllPackagesAsync(CancellationToken cancellationToken = default)
        {
            ProcessResult result =
                await ExecuteAsync(
                    WinGetArguments.Upgrade().All().AcceptSourceAgreements().AcceptPackageAgreements(),
                    false, cancellationToken);

            return result.Success;
        }

        /// <summary>
        /// Asynchronously tries to upgrade all packages using winget.
        /// </summary>
        /// <param name="silent">Request silent upgrade of packages.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <remarks>
        /// The action might run succesfully without upgrading every or even any package.
        /// </remarks>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the action run successfully or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> UpgradeAllPackagesAsync(bool silent, CancellationToken cancellationToken = default)
        {
            WinGetArguments cmd = WinGetArguments.Upgrade().All().AcceptSourceAgreements().AcceptPackageAgreements();

            if (silent)
            {
                cmd.Silent();
            }

            ProcessResult result = await ExecuteAsync(cmd, false, cancellationToken);

            return result.Success;
        }
        //---------------------------------------------------------------------------------------------

        //---Repair------------------------------------------------------------------------------------
        /// <summary>
        /// Repairs a package using winget.
        /// </summary>
        /// <remarks>Limited to packages with an installer that supports this function.</remarks>
        /// <param name="packageId">The id or name of the package for the repair action.</param>
        /// <returns>
        /// <see langword="true"/> if the repair action was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool RepairPackage(string packageId)
        {
            if (!CheckWinGetVersion(_repairMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_repairMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            ProcessResult result = Execute(WinGetArguments.Repair().Query(packageId).AcceptSourceAgreements().AcceptPackageAgreements());

            return result.Success;
        }

        /// <summary>
        /// Repairs a package using winget.
        /// </summary>
        /// <remarks>Limited to packages with an installer that supports this function.</remarks>
        /// <param name="packageId">The id or name of the package for the repair action.</param>
        /// <param name="silent">Request silent package repair.</param>
        /// <returns>
        /// <see langword="true"/> if the repair action was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool RepairPackage(string packageId, bool silent)
        {
            if (!CheckWinGetVersion(_repairMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_repairMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            WinGetArguments cmd = WinGetArguments.Repair().Query(packageId).AcceptSourceAgreements().AcceptPackageAgreements();

            if (silent)
            {
                cmd.Silent();
            }

            ProcessResult result = Execute(cmd);

            return result.Success;
        }

        /// <summary>
        /// Repairs a package using winget.
        /// </summary>
        /// <remarks>Limited to packages with an installer that supports this function.</remarks>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> for the repair action.</param>
        /// <returns>
        /// <see langword="true"/> if the repair action was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool RepairPackage(WinGetPackage package)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
            {
                return RepairPackage(package.Name);
            }

            return RepairPackage(package.Id);
        }

        /// <summary>
        /// Repairs a package using winget.
        /// </summary>
        /// <remarks>Limited to packages with an installer that supports this function.</remarks>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> for the repair action.</param>
        /// <param name="silent">Request silent package repair.</param>
        /// <returns>
        /// <see langword="true"/> if the repair action was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool RepairPackage(WinGetPackage package, bool silent)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
            {
                return RepairPackage(package.Name, silent);
            }

            return RepairPackage(package.Id, silent);
        }

        /// <summary>
        /// Asynchronously repairs a package using winget.
        /// </summary>
        /// <remarks>Limited to packages with an installer that supports this function.</remarks>
        /// <param name="packageId">The id or name of the package for the repair action.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the repair action was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> RepairPackageAsync(string packageId, CancellationToken cancellationToken = default)
        {
            if (!CheckWinGetVersion(_repairMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_repairMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            ProcessResult result =
                await ExecuteAsync(
                    WinGetArguments
                    .Repair()
                    .Query(packageId)
                    .AcceptSourceAgreements()
                    .AcceptPackageAgreements(),
                    false, cancellationToken);

            return result.Success;
        }

        /// <summary>
        /// Asynchronously repairs a package using winget.
        /// </summary>
        /// <remarks>Limited to packages with an installer that supports this function.</remarks>
        /// <param name="packageId">The id or name of the package for the repair action.</param>
        /// <param name="silent">Request silent package repair.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the repair action was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> RepairPackageAsync(string packageId, bool silent, CancellationToken cancellationToken = default)
        {
            if (!CheckWinGetVersion(_repairMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_repairMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            WinGetArguments cmd = WinGetArguments.Repair().Query(packageId).AcceptSourceAgreements().AcceptPackageAgreements();

            if (silent)
            {
                cmd.Silent();
            }

            ProcessResult result = await ExecuteAsync(cmd, false, cancellationToken);

            return result.Success;
        }

        /// <summary>
        /// Asynchronously repair a package using winget.
        /// </summary>
        /// <remarks>Limited to packages with an installer that supports this function.</remarks>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> for the repair action.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the repair action was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> RepairPackageAsync(WinGetPackage package, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
            {
                return await RepairPackageAsync(package.Name, cancellationToken);
            }

            return await RepairPackageAsync(package.Id, cancellationToken);
        }

        /// <summary>
        /// Asynchronously repair a package using winget.
        /// </summary>
        /// <remarks>Limited to packages with an installer that supports this function.</remarks>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> for the repair action.</param>
        /// <param name="silent">Request silent package repair.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the repair action was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> RepairPackageAsync(WinGetPackage package, bool silent, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
            {
                return await RepairPackageAsync(package.Name, silent, cancellationToken);
            }

            return await RepairPackageAsync(package.Id, silent, cancellationToken);
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
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool ExportPackagesToFile(string file)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(file, "file");

            ProcessResult result = Execute(WinGetArguments.Export().File(file).AcceptSourceAgreements());

            return result.Success;
        }

        /// <summary>
        /// Asynchronously exports a list of all installed winget packages as json to the given file.
        /// </summary>
        /// <param name="file">The file for the export.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the export was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> ExportPackagesToFileAsync(string file, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(file, "file");

            ProcessResult result =
                await ExecuteAsync(WinGetArguments.Export().File(file).AcceptSourceAgreements(), false, cancellationToken);

            return result.Success;
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
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool ImportPackagesFromFile(string file)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(file, "file");

            ProcessResult result =
                Execute(
                    WinGetArguments
                    .Import()
                    .File(file)
                    .IgnoreUnavailable()
                    .AcceptSourceAgreements()
                    .AcceptPackageAgreements());

            return result.Success;
        }

        /// <summary>
        /// Asynchronously imports packages and trys to installes/upgrade all pakages in the list, if possible.
        /// </summary>
        /// <remarks>
        /// This may take some time and winget may not install/upgrade all packages.
        /// </remarks>
        /// <param name="file">The file with the package data for the import.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the import was compleatly successful or 
        /// <see langword="false"/> if some or all packages failed to install.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> ImportPackagesFromFileAsync(string file, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(file, "file");

            ProcessResult result =
                await ExecuteAsync(
                    WinGetArguments
                    .Import()
                    .File(file)
                    .IgnoreUnavailable()
                    .AcceptSourceAgreements()
                    .AcceptPackageAgreements(),
                    false, cancellationToken);

            return result.Success;
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
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="System.IO.FileNotFoundException">
        /// Unable to find the specified file.
        /// (Not used for remote clients, only local execution)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public string Hash(string file)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(file, "file");

            if (!IsRemoteClient && !File.Exists(file))
            {
                throw new FileNotFoundException($"Unable to find the specified file. File:'{file}'");
            }

            ProcessResult result = Execute(WinGetArguments.Hash().File(file));

            if (!result.Success)
            {
                return string.Empty;
            }

            return ProcessOutputReader.ResultToHash(result);
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
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="System.IO.FileNotFoundException">
        /// Unable to find the specified file.
        /// (Not used for remote clients, only local execution)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public string Hash(FileInfo file)
        {
            ArgsHelper.ThrowIfObjectIsNull(file, "file");

            if (!IsRemoteClient && !file.Exists)
            {
                throw new FileNotFoundException($"Unable to find the specified file. File:'{file.FullName}'");
            }

            return Hash(file.FullName);
        }

        /// <summary>
        /// Asynchronously executes the WinGet hash function, to calculate the hash for the given file.
        /// </summary>
        /// <param name="file">
        /// A <see cref="System.String"/> containing the path to the file.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.String"/> containing the hash.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="System.IO.FileNotFoundException">
        /// Unable to find the specified file.
        /// (Not used for remote clients, only local execution)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<string> HashAsync(string file, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(file, "file");

            if (!IsRemoteClient && !File.Exists(file))
            {
                throw new FileNotFoundException($"Unable to find the specified file. File:'{file}'");
            }

            ProcessResult result = await ExecuteAsync(WinGetArguments.Hash().File(file), false, cancellationToken);

            if (!result.Success || cancellationToken.IsCancellationRequested)
            {
                return string.Empty;
            }

            return ProcessOutputReader.ResultToHash(result);
        }

        /// <summary>
        /// Asynchronously executes the WinGet hash function, to calculate the hash for the given file.
        /// </summary>
        /// <param name="file">
        /// A <see cref="System.IO.FileInfo"/> object, of the file the hash should be calculated for.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.String"/> containing the hash.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="System.IO.FileNotFoundException">
        /// Unable to find the specified file.
        /// (Not used for remote clients, only local execution)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<string> HashAsync(FileInfo file, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfObjectIsNull(file, "file");

            if (!IsRemoteClient && !file.Exists)
            {
                throw new FileNotFoundException($"Unable to find the specified file. File:'{file.FullName}'");
            }

            return await HashAsync(file.FullName, cancellationToken);
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
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool Download(string packageId, string directory)
        {
            if (!CheckWinGetVersion(_downloadMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_downloadMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(directory, "directory");

            ProcessResult result =
                Execute(
                    WinGetArguments
                    .Download()
                    .Query(packageId)
                    .Directory(directory)
                    .AcceptSourceAgreements()
                    .AcceptPackageAgreements());

            return result.Success;
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
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
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
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool Download(WinGetPackage package, string directory)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
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
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool Download(WinGetPackage package, DirectoryInfo directory)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");
            ArgsHelper.ThrowIfObjectIsNull(directory, "directory");

            if (package.HasShortenedId || package.HasNoId)
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
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the download was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> DownloadAsync(string packageId, string directory, CancellationToken cancellationToken = default)
        {
            if (!CheckWinGetVersion(_downloadMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_downloadMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(directory, "directory");

            ProcessResult result =
                await ExecuteAsync(
                    WinGetArguments
                    .Download()
                    .Query(packageId)
                    .Directory(directory)
                    .AcceptSourceAgreements()
                    .AcceptPackageAgreements(),
                    false, cancellationToken);

            return result.Success;
        }

        /// <summary>
        /// Asynchronously downloads the installer of a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package to download.</param>
        /// <param name="directory">
        /// A <see cref="System.IO.DirectoryInfo"/> object of the directory the files will be downloaded to. 
        /// It will be created if it does not exist.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the download was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> DownloadAsync(string packageId, DirectoryInfo directory, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfObjectIsNull(directory, "directory");

            return await DownloadAsync(packageId, directory.FullName, cancellationToken);
        }

        /// <summary>
        /// Asynchronously downloads the installer of a package using winget.
        /// </summary>
        /// <param name="package">The package to download.</param>
        /// <param name="directory">Directory path the files will be downloaded to. It will be created if it does not exist.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the download was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> DownloadAsync(WinGetPackage package, string directory, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
            {
                return await DownloadAsync(package.Name, directory, cancellationToken);
            }

            return await DownloadAsync(package.Id, directory, cancellationToken);
        }

        /// <summary>
        /// Asynchronously downloads the installer of a package using winget.
        /// </summary>
        /// <param name="package">The package to download.</param>
        /// <param name="directory">
        /// A <see cref="System.IO.DirectoryInfo"/> object of the directory the files will be downloaded to. 
        /// It will be created if it does not exist.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the download was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> DownloadAsync(WinGetPackage package, DirectoryInfo directory, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");
            ArgsHelper.ThrowIfObjectIsNull(directory, "directory");

            if (package.HasShortenedId || package.HasNoId)
            {
                return await DownloadAsync(package.Name, directory.FullName, cancellationToken);
            }

            return await DownloadAsync(package.Id, directory.FullName, cancellationToken);
        }
        //---------------------------------------------------------------------------------------------

        //---Pin List----------------------------------------------------------------------------------
        /// <summary>
        /// Gets a list of all pinned packages.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public List<WinGetPinnedPackage> GetPinnedPackages()
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ProcessResult result = Execute(WinGetArguments.PinList());

            return ProcessOutputReader.ToPinnedPackageList(result.Output);
        }

        /// <summary>
        /// Asynchronously gets a list of all pinned packages.
        /// </summary>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<List<WinGetPinnedPackage>> GetPinnedPackagesAsync(CancellationToken cancellationToken = default)
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ProcessResult result = await ExecuteAsync(WinGetArguments.PinList(), false, cancellationToken);

            // Return empty list if the task was cancled
            if (cancellationToken.IsCancellationRequested)
            {
                return new List<WinGetPinnedPackage>();
            }

            return ProcessOutputReader.ToPinnedPackageList(result.Output);
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
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool PinAdd(string packageId, bool blocking = false)
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            WinGetArguments cmd = WinGetArguments.PinAdd().Query(packageId);

            if (blocking)
            {
                cmd.Blocking();
            }

            ProcessResult result = Execute(cmd);

            return result.Success;
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
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool PinAdd(string packageId, string version)
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(version, "version");

            ProcessResult result = Execute(WinGetArguments.PinAdd().Query(packageId).Version(version));

            return result.Success;
        }

        /// <summary>
        /// Adds a pinned package to winget.
        /// </summary>
        /// <param name="package">The package to pin.</param>
        /// <param name="blocking">Set to <see langword="true"/> if updating of pinned package should be fully blocked.</param>
        /// <returns>
        /// <see langword="true"/> if the pin was added successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool PinAdd(WinGetPackage package, bool blocking = false)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
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
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool PinAdd(WinGetPackage package, string version)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
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
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the pin was added successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> PinAddAsync(string packageId, bool blocking = false, CancellationToken cancellationToken = default)
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            WinGetArguments cmd = WinGetArguments.PinAdd().Query(packageId);

            if (blocking)
            {
                cmd.Blocking();
            }

            ProcessResult result = await ExecuteAsync(cmd, false, cancellationToken);

            return result.Success;
        }

        /// <summary>
        /// Asynchronously adds a pinned package to winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package to pin.</param>
        /// <param name="version">
        /// <see cref="System.String"/> representing the version to pin. 
        /// Please refer to the WinGet documentation for more info about version pinning.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the pin was added successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> PinAddAsync(string packageId, string version, CancellationToken cancellationToken = default)
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(version, "version");

            ProcessResult result = await ExecuteAsync(WinGetArguments.PinAdd().Query(packageId).Version(version), false, cancellationToken);

            return result.Success;
        }

        /// <summary>
        /// Asynchronously adds a pinned package to winget.
        /// </summary>
        /// <param name="package">The package to pin.</param>
        /// <param name="blocking">Set to <see langword="true"/> if updating of pinned package should be fully blocked.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the pin was added successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> PinAddAsync(WinGetPackage package, bool blocking = false, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
            {
                return await PinAddAsync(package.Name, blocking, cancellationToken);
            }

            return await PinAddAsync(package.Id, blocking, cancellationToken);
        }

        /// <summary>
        /// Asynchronously adds a pinned package to winget.
        /// </summary>
        /// <param name="package">The package to pin.</param>
        /// <param name="version">
        /// <see cref="System.String"/> representing the version to pin. 
        /// Please refer to the WinGet documentation for more info about version pinning.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the pin was added successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> PinAddAsync(WinGetPackage package, string version, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
            {
                return await PinAddAsync(package.Name, version, cancellationToken);
            }

            return await PinAddAsync(package.Id, version, cancellationToken);
        }

        /// <summary>
        /// Adds a pinned installed package to winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package to pin.</param>
        /// <param name="blocking">Set to <see langword="true"/> if updating of pinned package should be fully blocked.</param>
        /// <returns>
        /// <see langword="true"/> if the pin was added successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool PinAddInstalled(string packageId, bool blocking = false)
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            WinGetArguments cmd = WinGetArguments.PinAdd().Query(packageId).Installed();

            if (blocking)
            {
                cmd.Blocking();
            }

            ProcessResult result = Execute(cmd);

            return result.Success;
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
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool PinAddInstalled(string packageId, string version)
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(version, "version");

            ProcessResult result = Execute(WinGetArguments.PinAdd().Query(packageId).Installed().Version(version));

            return result.Success;
        }

        /// <summary>
        /// Adds a pinned installed package to winget.
        /// </summary>
        /// <param name="package">The package to pin.</param>
        /// <param name="blocking">Set to <see langword="true"/> if updating of pinned package should be fully blocked.</param>
        /// <returns>
        /// <see langword="true"/> if the pin was added successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool PinAddInstalled(WinGetPackage package, bool blocking = false)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
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
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool PinAddInstalled(WinGetPackage package, string version)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
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
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the pin was added successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> PinAddInstalledAsync(string packageId, bool blocking = false, CancellationToken cancellationToken = default)
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            WinGetArguments cmd = WinGetArguments.PinAdd().Query(packageId).Installed();

            if (blocking)
            {
                cmd.Blocking();
            }

            ProcessResult result = await ExecuteAsync(cmd, false, cancellationToken);

            return result.Success;
        }

        /// <summary>
        /// Asynchronously adds a pinned installed package to winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package to pin.</param>
        /// <param name="version">
        /// <see cref="System.String"/> representing the version to pin. 
        /// Please refer to the WinGet documentation for more info about version pinning.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the pin was added successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> PinAddInstalledAsync(string packageId, string version, CancellationToken cancellationToken = default)
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(version, "version");

            ProcessResult result =
                await ExecuteAsync(
                    WinGetArguments.PinAdd().Query(packageId).Installed().Version(version),
                    false, cancellationToken);

            return result.Success;
        }

        /// <summary>
        /// Asynchronously adds a pinned installed package to winget.
        /// </summary>
        /// <param name="package">The package to pin.</param>
        /// <param name="blocking">Set to <see langword="true"/> if updating of pinned package should be fully blocked.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the pin was added successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> PinAddInstalledAsync(WinGetPackage package, bool blocking = false, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
            {
                return await PinAddInstalledAsync(package.Name, blocking, cancellationToken);
            }

            return await PinAddInstalledAsync(package.Id, blocking, cancellationToken);
        }

        /// <summary>
        /// Asynchronously adds a pinned installed package to winget.
        /// </summary>
        /// <param name="package">The package to pin.</param>
        /// <param name="version">
        /// <see cref="System.String"/> representing the version to pin. 
        /// Please refer to the WinGet documentation for more info about version pinning.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the pin was added successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> PinAddInstalledAsync(WinGetPackage package, string version, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
            {
                return await PinAddInstalledAsync(package.Name, version, cancellationToken);
            }

            return await PinAddInstalledAsync(package.Id, version, cancellationToken);
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
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool PinRemove(string packageId)
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            ProcessResult result = Execute(WinGetArguments.PinRemove().Query(packageId));

            return result.Success;
        }

        /// <summary>
        /// Removes a pinned package from winget.
        /// </summary>
        /// <param name="package">The package to unpin.</param>
        /// <returns>
        /// <see langword="true"/> if the removal of the pin was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool PinRemove(WinGetPackage package)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
            {
                return PinRemove(package.Name);
            }

            return PinRemove(package.Id);
        }

        /// <summary>
        /// Asynchronously removes a pinned package from winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package to unpin.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the removal of the pin was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> PinRemoveAsync(string packageId, CancellationToken cancellationToken = default)
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            ProcessResult result = await ExecuteAsync(WinGetArguments.PinRemove().Query(packageId), false, cancellationToken);

            return result.Success;
        }

        /// <summary>
        /// Asynchronously removes a pinned package from winget.
        /// </summary>
        /// <param name="package">The package to unpin.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the removal of the pin was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> PinRemoveAsync(WinGetPackage package, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
            {
                return await PinRemoveAsync(package.Name, cancellationToken);
            }

            return await PinRemoveAsync(package.Id, cancellationToken);
        }

        /// <summary>
        /// Removes a pinned package from winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package to unpin.</param>
        /// <returns>
        /// <see langword="true"/> if the removal of the pin was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool PinRemoveInstalled(string packageId)
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            ProcessResult result = Execute(WinGetArguments.PinRemove().Query(packageId).Installed());

            return result.Success;
        }

        /// <summary>
        /// Removes a pinned package from winget.
        /// </summary>
        /// <param name="package">The package to unpin.</param>
        /// <returns>
        /// <see langword="true"/> if the removal of the pin was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool PinRemoveInstalled(WinGetPackage package)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
            {
                return PinRemoveInstalled(package.Name);
            }

            return PinRemoveInstalled(package.Id);
        }

        /// <summary>
        /// Asynchronously removes a pinned package from winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package to unpin.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the removal of the pin was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> PinRemoveInstalledAsync(string packageId, CancellationToken cancellationToken = default)
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(packageId, "packageId");

            ProcessResult result = await ExecuteAsync(WinGetArguments.PinRemove().Query(packageId).Installed(), false, cancellationToken);

            return result.Success;
        }

        /// <summary>
        /// Asynchronously removes a pinned package from winget.
        /// </summary>
        /// <param name="package">The package to unpin.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the removal of the pin was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> PinRemoveInstalledAsync(WinGetPackage package, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(package, "package");

            if (package.HasShortenedId || package.HasNoId)
            {
                return await PinRemoveInstalledAsync(package.Name, cancellationToken);
            }

            return await PinRemoveInstalledAsync(package.Id, cancellationToken);
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
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public bool ResetPins()
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ProcessResult result = Execute(WinGetArguments.PinReset().Force());

            return result.Success;
        }

        /// <summary>
        /// Asynchronously resets all pinned packages.
        /// </summary>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <remarks>
        /// This will remove all pins and it is not possible to restore them.
        /// </remarks>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the reset was successful or <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException">
        /// This feature is not supported in the installed WinGet version.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated. 
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// (Not used for local execution, only for remote clients)
        /// </exception>
        public async Task<bool> ResetPinsAsync(CancellationToken cancellationToken = default)
        {
            if (!CheckWinGetVersion(_pinMinVersion))
            {
                throw new WinGetFeatureNotSupportedException(_pinMinVersion);
            }

            ProcessResult result = await ExecuteAsync(WinGetArguments.PinReset().Force(), false, cancellationToken);

            return result.Success;
        }
        //---------------------------------------------------------------------------------------------

        //---Helper Functions--------------------------------------------------------------------------
        /// <summary>
        /// Adds the '--include-unknown' flag to the given <see cref="WGetNET.WinGetArguments"/>
        /// when the winget version is higher then 1.4.0.
        /// </summary>
        /// <param name="arguments">
        /// <see cref="WGetNET.WinGetArguments"/> object to process.
        /// </param>
        /// <returns>
        /// The updated <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        private WinGetArguments IncludeUnknownbyVersion(WinGetArguments arguments)
        {
            // Checking version to determine if "--include-unknown" is necessary.
            if (CheckWinGetVersion(new Version(1, 4, 0)))
            {
                // Winget version supports new argument, add "--include-unknown" to arguments
                arguments.IncludeUnknown();
            }
            return arguments;
        }
        //---------------------------------------------------------------------------------------------
    }
}
