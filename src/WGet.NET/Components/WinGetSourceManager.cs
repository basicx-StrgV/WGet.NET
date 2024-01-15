//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using WGetNET.Models;
using WGetNET.Helper;
using WGetNET.Components.Internal;

namespace WGetNET
{
    /// <summary>
    /// The <see cref="WGetNET.WinGetSourceManager"/> class offers methods to manage the sources used by winget.
    /// </summary>
    public class WinGetSourceManager : WinGet
    {
        private const string _sourceAddCmd = "source add -n {0} -a {1} --accept-source-agreements";
        private const string _sourceAddWithTypeCmd = "source add -n {0} -a {1} -t {2} --accept-source-agreements";
        private const string _sourceUpdateCmd = "source update";
        private const string _sourceExportCmd = "source export";
        private const string _sourceResetCmd = "source reset --force";
        private const string _sourceRemoveCmd = "source remove -n {0}";

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetSourceManager"/> class.
        /// </summary>
        public WinGetSourceManager()
        {
            // Provide empty constructor for xlm docs
        }

        //---List--------------------------------------------------------------------------------------
        /// <summary>
        /// Gets a list of all sources that are installed in winget.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/> instances.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        public List<WinGetSource> GetInstalledSources()
        {
            ProcessResult result = Execute(_sourceExportCmd);

            return ProcessOutputReader.ToSourceList(result.Output);
        }

        /// <summary>
        /// Gets a list of installed sources that matches the provided name.
        /// </summary>
        /// <param name="sourceName">Name of the sources to export.</param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/> instances.
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
        public List<WinGetSource> GetInstalledSources(string sourceName)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(sourceName, "sourceName");

            string cmd = $"{_sourceExportCmd} -n {sourceName}";

            ProcessResult result = Execute(cmd);

            return ProcessOutputReader.ToSourceList(result.Output);
        }

        /// <summary>
        /// Asynchronously gets a list of all sources that are installed in winget.
        /// </summary>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/> instances.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        public async Task<List<WinGetSource>> GetInstalledSourcesAsync(CancellationToken cancellationToken = default)
        {
            ProcessResult result = await ExecuteAsync(_sourceExportCmd, false, cancellationToken);

            // Return empty list if the task was cancled
            if (cancellationToken.IsCancellationRequested)
            {
                return new List<WinGetSource>();
            }

            return ProcessOutputReader.ToSourceList(result.Output);
        }

        /// <summary>
        /// Asynchronously gets a list of installed sources that matches the provided name.
        /// </summary>
        /// <param name="sourceName">Name of the sources to export.</param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/> instances.
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
        public async Task<List<WinGetSource>> GetInstalledSourcesAsync(string sourceName, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(sourceName, "sourceName");

            string cmd = $"{_sourceExportCmd} -n {sourceName}";

            ProcessResult result = await ExecuteAsync(cmd, false, cancellationToken);

            // Return empty list if the task was cancled
            if (cancellationToken.IsCancellationRequested)
            {
                return new List<WinGetSource>();
            }

            return ProcessOutputReader.ToSourceList(result.Output);
        }
        //---------------------------------------------------------------------------------------------

        //---Add---------------------------------------------------------------------------------------
        /// <summary>
        /// Adds a new source to winget (Needs administrator rights).
        /// </summary>
        /// <param name="name">
        /// A <see cref="System.String"/> representing the name of the source to add.
        /// </param>
        /// <param name="arg">
        /// A <see cref="System.String"/> representing the sources URL or UNC.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the action was succesfull and <see langword="false"/> if it failed.
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
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public bool AddSource(string name, string arg)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(arg, "arg");

            string cmd = string.Format(_sourceAddCmd, name, arg);

            ProcessResult result = Execute(cmd, true);

            return result.Success;
        }

        /// <summary>
        /// Adds a new source to winget (Needs administrator rights).
        /// </summary>
        /// <param name="name">
        /// A <see cref="System.String"/> representing the name of the source to add.
        /// </param>
        /// <param name="arg">
        /// A <see cref="System.String"/> representing the sources URL or UNC.
        /// </param>
        /// <param name="type">
        /// A <see cref="System.String"/> representing the source type.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the action was succesfull and <see langword="false"/> if it failed.
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
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public bool AddSource(string name, string arg, string type)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(arg, "arg");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(type, "type");

            string cmd = string.Format(_sourceAddWithTypeCmd, name, arg, type);

            ProcessResult result = Execute(cmd, true);

            return result.Success;
        }

        /// <summary>
        /// Adds a new source to winget (Needs administrator rights).
        /// </summary>
        /// <param name="source">
        /// The <see cref="WGetNET.WinGetSource"/> to add.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the action was succesfull and <see langword="false"/> if it failed.
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
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public bool AddSource(WinGetSource source)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(source, "source");

            if (string.IsNullOrWhiteSpace(source.Type))
            {
                return AddSource(source.Name, source.Arg);
            }

            return AddSource(source.Name, source.Arg, source.Type);
        }

        /// <summary>
        /// Adds multiple new sources to winget (Needs administrator rights).
        /// </summary>
        /// <param name="sources">
        /// A <see cref="System.Collections.Generic.IEnumerable{T}"/> of <see cref="WGetNET.WinGetSource"/> objects to add.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if adding all sources was succesfull and <see langword="false"/> if one or more failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public bool AddSource(IEnumerable<WinGetSource> sources)
        {
            ArgsHelper.ThrowIfObjectIsNull(sources, "sources");

            bool succes = true;
            foreach (WinGetSource source in sources)
            {
                if (!AddSource(source))
                {
                    succes = false;
                }
            }

            return succes;
        }

        /// <summary>
        /// Asynchronously adds a new source to winget (Needs administrator rights).
        /// </summary>
        /// <param name="name">
        /// A <see cref="System.String"/> representing the name of the source to add.
        /// </param>
        /// <param name="arg">
        /// A <see cref="System.String"/> representing the sources URL or UNC.
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
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public async Task<bool> AddSourceAsync(string name, string arg, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(arg, "arg");

            string cmd = string.Format(_sourceAddCmd, name, arg);

            ProcessResult result = await ExecuteAsync(cmd, true, cancellationToken);

            return result.Success;
        }

        /// <summary>
        /// Asynchronously adds a new source to winget (Needs administrator rights).
        /// </summary>
        /// <param name="name">
        /// A <see cref="System.String"/> representing the name of the source to add.
        /// </param>
        /// <param name="arg">
        /// A <see cref="System.String"/> representing the sources URL or UNC.
        /// </param>
        /// <param name="type">
        /// A <see cref="System.String"/> representing the source type.
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
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public async Task<bool> AddSourceAsync(string name, string arg, string type, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(arg, "arg");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(type, "type");

            string cmd = string.Format(_sourceAddWithTypeCmd, name, arg, type);

            ProcessResult result = await ExecuteAsync(cmd, true, cancellationToken);

            return result.Success;
        }

        /// <summary>
        /// Asynchronously adds a new source to winget (Needs administrator rights).
        /// </summary>
        /// <param name="source">
        /// The <see cref="WGetNET.WinGetSource"/> to add.
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
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public async Task<bool> AddSourceAsync(WinGetSource source, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(source, "source");

            if (string.IsNullOrWhiteSpace(source.Type))
            {
                return await AddSourceAsync(source.Name, source.Arg, cancellationToken);
            }

            return await AddSourceAsync(source.Name, source.Arg, source.Type, cancellationToken);
        }

        /// <summary>
        /// Asynchronously adds multiple new sources to winget (Needs administrator rights).
        /// </summary>
        /// <remarks>
        /// The source type is optional but some sources like the "msstore" need it or adding it wil throw an error.
        /// </remarks>
        /// <param name="sources">
        /// A <see cref="System.Collections.Generic.IEnumerable{T}"/> of <see cref="WGetNET.WinGetSource"/> objects to add.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if adding all sources was succesfull and <see langword="false"/> if one or more failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public async Task<bool> AddSourceAsync(IEnumerable<WinGetSource> sources, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfObjectIsNull(sources, "sources");

            bool success = true;
            foreach (WinGetSource source in sources)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return false;
                }

                if (!(await AddSourceAsync(source, cancellationToken)))
                {
                    success = false;
                }
            }

            return success;
        }
        //---------------------------------------------------------------------------------------------

        //---Update------------------------------------------------------------------------------------
        /// <summary>
        /// Updates all sources that are installed in winget.
        /// </summary>
        /// <remarks>
        /// This may take a while depending on the sources.
        /// </remarks>
        /// <returns>
        /// <see langword="true"/> if the update was successful or <see langword="false"/> if the it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        public bool UpdateSources()
        {
            ProcessResult result = Execute(_sourceUpdateCmd);

            return result.Success;
        }

        /// <summary>
        /// Asynchronously updates all sources that are installed in winget.
        /// </summary>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <remarks>
        /// This may take a while depending on the sources.
        /// </remarks>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the update was successful or <see langword="false"/> if the it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        public async Task<bool> UpdateSourcesAsync(CancellationToken cancellationToken = default)
        {
            ProcessResult result = await ExecuteAsync(_sourceUpdateCmd, false, cancellationToken);

            return result.Success;
        }
        //---------------------------------------------------------------------------------------------

        //---Export------------------------------------------------------------------------------------
        /// <summary>
        /// Exports the winget sources in json format to a file.
        /// </summary>
        /// <remarks>
        /// If the provided file and/or path does not exist, they will be created.
        /// </remarks>
        /// <param name="file">The file for the export.</param>
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
        public void ExportSourcesToFile(string file)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(file, "file");
            ArgsHelper.ThrowIfPathIsInvalid(file);

            FileHelper.WriteTextToFile(
                file,
                SourcesToJson(
                    GetInstalledSources()));
        }

        /// <summary>
        /// Exports the winget sources in json format to a file.
        /// </summary>
        /// <remarks>
        /// If the provided file and/or path does not exist, they will be created.
        /// </remarks>
        /// <param name="file">The file for the export.</param>
        /// <param name="sourceName">The name of the source for the export.</param>
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
        public void ExportSourcesToFile(string file, string sourceName)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(file, "file");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(sourceName, "sourceName");
            ArgsHelper.ThrowIfPathIsInvalid(file);

            FileHelper.WriteTextToFile(
                file,
                SourcesToJson(
                    GetInstalledSources(sourceName)));
        }

        /// <summary>
        /// Exports the winget sources in json format to a file.
        /// </summary>
        /// <remarks>
        /// If the provided file and/or path does not exist, they will be created.
        /// </remarks>
        /// <param name="file">
        /// The file for the export.
        /// </param>
        /// <param name="source">
        /// The <see cref="WGetNET.WinGetSource"/> for the export.
        /// </param>
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
        public void ExportSourcesToFile(string file, WinGetSource source)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(source, "source");
            ArgsHelper.ThrowIfPathIsInvalid(file);

            ExportSourcesToFile(file, source.Name);
        }

        /// <summary>
        /// Asynchronously exports the winget sources in json format to a file.
        /// </summary>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <remarks>
        /// If the provided file and/or path does not exist, they will be created.
        /// </remarks>
        /// <returns>
        /// The <see cref="System.Threading.Tasks.Task"/> for the action.
        /// </returns>
        /// <param name="file">The file for the export.</param>
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
        public async Task ExportSourcesToFileAsync(string file, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(file, "file");
            ArgsHelper.ThrowIfPathIsInvalid(file);

            await FileHelper.WriteTextToFileAsync(
                file, SourcesToJson(
                    await GetInstalledSourcesAsync(cancellationToken)),
                cancellationToken);
        }

        /// <summary>
        /// Asynchronously exports the winget sources in json format to a file.
        /// </summary>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <remarks>
        /// If the provided file and/or path does not exist, they will be created.
        /// </remarks>
        /// <returns>
        /// The <see cref="System.Threading.Tasks.Task"/> for the action.
        /// </returns>
        /// <param name="file">The file for the export.</param>
        /// <param name="sourceName">The name of the source for the export.</param>
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
        public async Task ExportSourcesToFileAsync(string file, string sourceName, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(file, "file");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(sourceName, "sourceName");
            ArgsHelper.ThrowIfPathIsInvalid(file);

            await FileHelper.WriteTextToFileAsync(
                file,
                SourcesToJson(
                    await GetInstalledSourcesAsync(sourceName, cancellationToken)),
                cancellationToken);
        }

        /// <summary>
        /// Asynchronously exports the winget sources in json format to a file.
        /// </summary>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <remarks>
        /// If the provided file and/or path does not exist, they will be created.
        /// </remarks>
        /// <returns>
        /// The <see cref="System.Threading.Tasks.Task"/> for the action.
        /// </returns>
        /// <param name="file">
        /// The file for the export.
        /// </param>
        /// <param name="source">
        /// The <see cref="WGetNET.WinGetSource"/> for the export.
        /// </param>
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
        public async Task ExportSourcesToFileAsync(string file, WinGetSource source, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(source, "source");
            ArgsHelper.ThrowIfPathIsInvalid(file);

            await ExportSourcesToFileAsync(file, source.Name, cancellationToken);
        }
        //---------------------------------------------------------------------------------------------

        //---Import------------------------------------------------------------------------------------
        /// <summary>
        /// Imports sources into winget from a json string.
        /// </summary>
        /// <param name="jsonString">
        /// A <see cref="System.String"/> containing the json for multiple sources.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if importing all sources was successful and <see langword="false"/> if on or more failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.InvalidJsonException">
        /// The provided JSON could not be deserialized.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public bool ImportSourcesFromJson(string jsonString)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(jsonString, "jsonString");

            List<SourceModel> sources = JsonHelper.StringToObject<List<SourceModel>>(jsonString);

            bool success = true;
            for (int i = 0; i < sources.Count; i++)
            {
                if (!AddSource(WinGetSource.FromSourceModel(sources[i])))
                {
                    success = false;
                }
            }

            return success;
        }

        /// <summary>
        /// Asynchronously imports sources into winget from a json string.
        /// </summary>
        /// <param name="jsonString">
        /// A <see cref="System.String"/> containing the json for multiple sources.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if importing all sources was successful and <see langword="false"/> if on or more failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.InvalidJsonException">
        /// The provided JSON could not be deserialized.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public async Task<bool> ImportSourcesFromJsonAsync(string jsonString, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(jsonString, "jsonString");

#if NETCOREAPP3_1_OR_GREATER
            List<SourceModel> sources =
                await JsonHelper.StringToObjectAsync<List<SourceModel>>(jsonString, cancellationToken);
#elif NETSTANDARD2_0
            List<SourceModel> sources =
                JsonHelper.StringToObject<List<SourceModel>>(jsonString);
#endif

            bool success = true;
            for (int i = 0; i < sources.Count; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return false;
                }

                if (!(await AddSourceAsync(WinGetSource.FromSourceModel(sources[i]), cancellationToken)))
                {
                    success = false;
                }
            }

            return success;
        }
        //---------------------------------------------------------------------------------------------

        //---Reset-------------------------------------------------------------------------------------
        /// <summary>
        /// Resets all sources that are installed in winget (Needs administrator rights).
        /// </summary>
        /// <remarks>
        /// This may take a while depending on the sources.
        /// </remarks>
        /// <returns>
        /// <see langword="true"/> if the reset was successful or <see langword="false"/> if the it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public bool ResetSources()
        {
            ProcessResult result = Execute(_sourceResetCmd, true);

            return result.Success;
        }

        /// <summary>
        /// Asynchronously resets all sources that are installed in winget (Needs administrator rights).
        /// </summary>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <remarks>
        /// This may take a while depending on the sources.
        /// </remarks>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the reset was successful or <see langword="false"/> if the it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public async Task<bool> ResetSourcesAsync(CancellationToken cancellationToken = default)
        {
            ProcessResult result = await ExecuteAsync(_sourceResetCmd, true, cancellationToken);

            return result.Success;
        }
        //---------------------------------------------------------------------------------------------

        //---Remove------------------------------------------------------------------------------------
        /// <summary>
        /// Removes a source from winget (Needs administrator rights).
        /// </summary>
        /// <param name="name">
        /// A <see cref="System.String"/> representing the name of the source.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the remove was successful or <see langword="false"/> if the it failed.
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
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public bool RemoveSources(string name)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");

            string cmd = string.Format(_sourceRemoveCmd, name);

            ProcessResult result = Execute(cmd, true);

            return result.Success;
        }

        /// <summary>
        /// Removes a source from winget (Needs administrator rights).
        /// </summary>
        /// <param name="source">
        /// The <see cref="WGetNET.WinGetSource"/> to remove.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the remove was successful or <see langword="false"/> if the it failed.
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
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public bool RemoveSources(WinGetSource source)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(source, "source");

            return RemoveSources(source.Name);
        }

        /// <summary>
        /// Asynchronously removes a source from winget (Needs administrator rights).
        /// </summary>
        /// <param name="name">
        /// A <see cref="System.String"/> representing the name of the source.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the remove was successful or <see langword="false"/> if the it failed.
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
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public async Task<bool> RemoveSourcesAsync(string name, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");

            string cmd = string.Format(_sourceRemoveCmd, name);

            ProcessResult result = await ExecuteAsync(cmd, true, cancellationToken);

            return result.Success;
        }

        /// <summary>
        /// Asynchronously removes a source from winget (Needs administrator rights).
        /// </summary>
        /// <param name="source">
        /// The <see cref="WGetNET.WinGetSource"/> to remove.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the remove was successful or <see langword="false"/> if the it failed.
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
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public async Task<bool> RemoveSourcesAsync(WinGetSource source, CancellationToken cancellationToken = default)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(source, "source");

            return await RemoveSourcesAsync(source.Name, cancellationToken);
        }
        //---------------------------------------------------------------------------------------------

        //---Other-------------------------------------------------------------------------------------
        /// <summary>
        /// Generates a valid json string from the provided sources.
        /// </summary>
        /// <param name="sources">
        /// The <see cref="System.Collections.Generic.IEnumerable{T}"/> of <see cref="WinGetSource"/> objects.
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/> containing the generated json.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        public string SourcesToJson(IEnumerable<WinGetSource> sources)
        {
            ArgsHelper.ThrowIfObjectIsNull(sources, "sources");

            // Create source models for json parsing
            List<SourceModel> models = new();
            foreach (WinGetSource source in sources)
            {
                models.Add(SourceModel.FromWinGetSource(source));
            }

            return JsonHelper.GetJson(models);
        }
        //---------------------------------------------------------------------------------------------
    }
}
