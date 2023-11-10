//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.Security;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using WGetNET.Models;
using WGetNET.Exceptions;
using WGetNET.HelperClasses;

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
        public List<WinGetSource> GetInstalledSources()
        {
            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(_sourceExportCmd);

                return ProcessOutputReader.ToSourceList(result.Output);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Getting installed sources failed.", _sourceExportCmd, e);
            }
        }

        /// <summary>
        /// Gets a list of installed sources that matches the provided name.
        /// </summary>
        /// <param name="sourceName">Name of the sources to export.</param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/> instances.
        /// </returns>
        public List<WinGetSource> GetInstalledSources(string sourceName)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(sourceName, "sourceName");

            string cmd = $"{_sourceExportCmd} -n {sourceName}";

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(cmd);

                return ProcessOutputReader.ToSourceList(result.Output);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Getting installed sources failed.", cmd, e);
            }
        }

        /// <summary>
        /// Asynchronously gets a list of all sources that are installed in winget.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/> instances.
        /// </returns>
        public async Task<List<WinGetSource>> GetInstalledSourcesAsync()
        {
            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(_sourceExportCmd);

                return ProcessOutputReader.ToSourceList(result.Output);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Getting installed sources failed.", _sourceExportCmd, e);
            }
        }

        /// <summary>
        /// Asynchronously gets a list of installed sources that matches the provided name.
        /// </summary>
        /// <param name="sourceName">Name of the sources to export.</param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/> instances.
        /// </returns>
        public async Task<List<WinGetSource>> GetInstalledSourcesAsync(string sourceName)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(sourceName, "sourceName");

            string cmd = $"{_sourceExportCmd} -n {sourceName}";

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(cmd);

                return ProcessOutputReader.ToSourceList(result.Output);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Getting installed sources failed.", cmd, e);
            }
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
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public bool AddSource(string name, string arg)
        {
            if (!PrivilegeChecker.CheckAdministratorPrivileges())
            {
                throw new SecurityException("Administrator privileges are missing.");
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(arg, "arg");

            string cmd = string.Format(_sourceAddCmd, name, arg);

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
                throw new WinGetActionFailedException("Getting installed sources failed.", cmd, e);
            }
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
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public bool AddSource(string name, string arg, string type)
        {
            if (!PrivilegeChecker.CheckAdministratorPrivileges())
            {
                throw new SecurityException("Administrator privileges are missing.");
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(arg, "arg");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(type, "type");

            string cmd = string.Format(_sourceAddWithTypeCmd, name, arg, type);

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
                throw new WinGetActionFailedException("Getting installed sources failed.", cmd, e);
            }
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
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
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
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/> objects to add.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if adding all sources was succesfull and <see langword="false"/> if one or more failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public bool AddSource(List<WinGetSource> sources)
        {
            ArgsHelper.ThrowIfObjectIsNull(sources, "sources");

            bool succes = true;
            for (int i = 0; i < sources.Count; i++)
            {
                if (!AddSource(sources[i]))
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
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the action was succesfull and <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public async Task<bool> AddSourceAsync(string name, string arg)
        {
            if (!PrivilegeChecker.CheckAdministratorPrivileges())
            {
                throw new SecurityException("Administrator privileges are missing.");
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(arg, "arg");

            string cmd = string.Format(_sourceAddCmd, name, arg);

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
                throw new WinGetActionFailedException("Getting installed sources failed.", cmd, e);
            }
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
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the action was succesfull and <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public async Task<bool> AddSourceAsync(string name, string arg, string type)
        {
            if (!PrivilegeChecker.CheckAdministratorPrivileges())
            {
                throw new SecurityException("Administrator privileges are missing.");
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(arg, "arg");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(type, "type");

            string cmd = string.Format(_sourceAddWithTypeCmd, name, arg, type);

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
                throw new WinGetActionFailedException("Getting installed sources failed.", cmd, e);
            }
        }

        /// <summary>
        /// Asynchronously adds a new source to winget (Needs administrator rights).
        /// </summary>
        /// <param name="source">
        /// The <see cref="WGetNET.WinGetSource"/> to add.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the action was succesfull and <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public async Task<bool> AddSourceAsync(WinGetSource source)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(source, "source");

            if (string.IsNullOrWhiteSpace(source.Type))
            {
                return await AddSourceAsync(source.Name, source.Arg);
            }

            return await AddSourceAsync(source.Name, source.Arg, source.Type);
        }

        /// <summary>
        /// Asynchronously adds multiple new sources to winget (Needs administrator rights).
        /// </summary>
        /// <remarks>
        /// The source type is optional but some sources like the "msstore" need it or adding it wil throw an error.
        /// </remarks>
        /// <param name="sources">
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/> objects to add.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if adding all sources was succesfull and <see langword="false"/> if one or more failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public async Task<bool> AddSourceAsync(List<WinGetSource> sources)
        {
            ArgsHelper.ThrowIfObjectIsNull(sources, "sources");

            bool succes = true;
            for (int i = 0; i < sources.Count; i++)
            {
                if (!(await AddSourceAsync(sources[i])))
                {
                    succes = false;
                }
            }

            return succes;
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
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public bool UpdateSources()
        {
            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(_sourceUpdateCmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Updating sources failed.", _sourceUpdateCmd, e);
            }
        }

        /// <summary>
        /// Asynchronously updates all sources that are installed in winget.
        /// </summary>
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
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public async Task<bool> UpdateSourcesAsync()
        {
            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(_sourceUpdateCmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Updating sources failed.", _sourceUpdateCmd, e);
            }
        }
        //---------------------------------------------------------------------------------------------

        //---Export------------------------------------------------------------------------------------
        /// <summary>
        /// Exports the winget sources in json format to a file.
        /// </summary>
        /// <param name="file">The file for the export.</param>
        /// <returns>
        /// <see langword="true"/> if the export was successful or <see langword="false"/> if the it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public bool ExportSourcesToFile(string file)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(file, "file");

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(_sourceExportCmd);

                return FileHandler.ExportOutputToFile(result, file);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Exporting sources failed.", _sourceExportCmd, e);
            }
        }

        /// <summary>
        /// Exports the winget sources in json format to a file.
        /// </summary>
        /// <param name="file">The file for the export.</param>
        /// <param name="sourceName">The name of the source for the export.</param>
        /// <returns>
        /// <see langword="true"/> if the export was successful or <see langword="false"/> if the it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public bool ExportSourcesToFile(string file, string sourceName)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(file, "file");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(sourceName, "sourceName");

            string cmd = $"{_sourceExportCmd} -n {sourceName}";

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(cmd);

                return FileHandler.ExportOutputToFile(result, file);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Exporting sources failed.", cmd, e);
            }
        }

        /// <summary>
        /// Exports the winget sources in json format to a file.
        /// </summary>
        /// <param name="file">
        /// The file for the export.
        /// </param>
        /// <param name="source">
        /// The <see cref="WGetNET.WinGetSource"/> for the export.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the export was successful or <see langword="false"/> if the it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public bool ExportSourcesToFile(string file, WinGetSource source)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(source, "source");

            return ExportSourcesToFile(file, source.Name);
        }

        /// <summary>
        /// Asynchronously exports the winget sources in json format to a file.
        /// </summary>
        /// <param name="file">The file for the export.</param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the export was successful or <see langword="false"/> if the it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<bool> ExportSourcesToFileAsync(string file)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(file, "file");

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(_sourceExportCmd);

                return await FileHandler.ExportOutputToFileAsync(result, file);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Exporting sources failed.", _sourceExportCmd, e);
            }
        }

        /// <summary>
        /// Asynchronously exports the winget sources in json format to a file.
        /// </summary>
        /// <param name="file">The file for the export.</param>
        /// <param name="sourceName">The name of the source for the export.</param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the export was successful or <see langword="false"/> if the it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<bool> ExportSourcesToFileAsync(string file, string sourceName)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(file, "file");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(sourceName, "sourceName");

            string cmd = $"{_sourceExportCmd} -n {sourceName}";

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(cmd);

                return await FileHandler.ExportOutputToFileAsync(result, file);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Exporting sources failed.", cmd, e);
            }
        }

        /// <summary>
        /// Asynchronously exports the winget sources in json format to a file.
        /// </summary>
        /// <param name="file">
        /// The file for the export.
        /// </param>
        /// <param name="source">
        /// The <see cref="WGetNET.WinGetSource"/> for the export.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the export was successful or <see langword="false"/> if the it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public async Task<bool> ExportSourcesToFileAsync(string file, WinGetSource source)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(source, "source");

            return await ExportSourcesToFileAsync(file, source.Name);
        }
        //---------------------------------------------------------------------------------------------

        //---Import------------------------------------------------------------------------------------
        /// <summary>
        /// Imports a source into winget.
        /// </summary>
        /// <param name="jsonString">
        /// A <see cref="System.String"/> containing the json for ONE source.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the action was successful and <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.InvalidJsonException">
        /// The provided JSON could not be deserialized.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public bool ImportSource(string jsonString)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(jsonString, "jsonString");

            SourceModel source = JsonHandler.StringToObject<SourceModel>(jsonString);
            
            return AddSource(WinGetSource.FromSourceModel(source));
        }

        /// <summary>
        /// Asynchronously imports a source into winget.
        /// </summary>
        /// <param name="jsonString">
        /// A <see cref="System.String"/> containing the json for ONE source.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the action was successful and <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.InvalidJsonException">
        /// The provided JSON could not be deserialized.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public async Task<bool> ImportSourceAsync(string jsonString)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(jsonString, "jsonString");

#if NETCOREAPP3_1_OR_GREATER
            SourceModel source = 
                await JsonHandler.StringToObjectAsync<SourceModel>(jsonString);
#elif NETSTANDARD2_0
            SourceModel source = 
                JsonHandler.StringToObject<SourceModel>(jsonString);
#endif

            return await AddSourceAsync(WinGetSource.FromSourceModel(source));
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
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public bool ResetSources()
        {
            if (!PrivilegeChecker.CheckAdministratorPrivileges())
            {
                throw new SecurityException("Administrator privileges are missing.");
            }

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(_sourceResetCmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Reset sources failed.", _sourceResetCmd, e);
            }
        }

        /// <summary>
        /// Asynchronously resets all sources that are installed in winget (Needs administrator rights).
        /// </summary>
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
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public async Task<bool> ResetSourcesAsync()
        {
            if (!PrivilegeChecker.CheckAdministratorPrivileges())
            {
                throw new SecurityException("Administrator privileges are missing.");
            }

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(_sourceResetCmd);

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Reset sources failed.", _sourceResetCmd, e);
            }
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
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public bool RemoveSources(string name)
        {
            if (!PrivilegeChecker.CheckAdministratorPrivileges())
            {
                throw new SecurityException("Administrator privileges are missing.");
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");

            string cmd = string.Format(_sourceRemoveCmd, name);

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
                throw new WinGetActionFailedException("Removing source failed.", cmd, e);
            }
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
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
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
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the remove was successful or <see langword="false"/> if the it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public async Task<bool> RemoveSourcesAsync(string name)
        {
            if (!PrivilegeChecker.CheckAdministratorPrivileges())
            {
                throw new SecurityException("Administrator privileges are missing.");
            }

            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");

            string cmd = string.Format(_sourceRemoveCmd, name);

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
                throw new WinGetActionFailedException("Removing source failed.", cmd, e);
            }
        }

        /// <summary>
        /// Asynchronously removes a source from winget (Needs administrator rights).
        /// </summary>
        /// <param name="source">
        /// The <see cref="WGetNET.WinGetSource"/> to remove.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the remove was successful or <see langword="false"/> if the it failed.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.Exceptions.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public async Task<bool> RemoveSourcesAsync(WinGetSource source)
        {
            ArgsHelper.ThrowIfWinGetObjectIsNullOrEmpty(source, "source");

            return await RemoveSourcesAsync(source.Name);
        }
        //---------------------------------------------------------------------------------------------
    }
}
