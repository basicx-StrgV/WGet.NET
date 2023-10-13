//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.Text;
using System.Security;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using WGetNET.HelperClasses;

namespace WGetNET
{
    /// <summary>
    /// The <see cref="WGetNET.WinGetSourceManager"/> class offers methods to manage the sources used by winget.
    /// </summary>
    public class WinGetSourceManager : WinGetInfo
    {
        private const string _sourceListCmd = "source list";
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
            //Provide empty constructor
        }

        //---List--------------------------------------------------------------------------------------
        /// <summary>
        /// Gets a list of all sources that are installed in winget.
        /// </summary>
        /// <remarks>
        /// Because the list source output is limited it is recommanded to use 
        /// <see cref="WGetNET.WinGetSourceManager.ExportSourcesToObject()"/> instead.
        /// </remarks>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/> instances.
        /// </returns>
        public List<WinGetSource> GetInstalledSources()
        {
            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(_sourceListCmd);

                return ProcessOutputReader.ToSourceList(result.Output);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Getting installed sources failed.", e);
            }
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Asynchronously gets a list of all sources that are installed in winget.
        /// </summary>
        /// <remarks>
        /// Because the list source output is limited it is recommanded to use 
        /// <see cref="WGetNET.WinGetSourceManager.ExportSourcesToObject()"/> instead.
        /// </remarks>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/> instances.
        /// </returns>
        public async Task<List<WinGetSource>> GetInstalledSourcesAsync()
        {
            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(_sourceListCmd);

                return ProcessOutputReader.ToSourceList(result.Output);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Getting installed sources failed.", e);
            }
        }
#endif
        //---------------------------------------------------------------------------------------------

        //---Add---------------------------------------------------------------------------------------
        /// <summary>
        /// Adds a new source to winget (Needs administrator rights).
        /// </summary>
        /// <remarks>
        /// The source type is optional but some sources like the "msstore" need it or adding it wil throw an error.
        /// </remarks>
        /// <param name="name">
        /// A <see cref="System.String"/> representing the name of the source to add.
        /// </param>
        /// <param name="arg">
        /// A <see cref="System.String"/> representing the source (eg. URL).
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the action was succesfull and <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
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

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(arg))
            {
                return false;
            }

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(
                        string.Format(_sourceAddCmd, name, arg));

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Getting installed sources failed.", e);
            }
        }

        /// <summary>
        /// Adds a new source to winget (Needs administrator rights).
        /// </summary>
        /// <remarks>
        /// The source type is optional but some sources like the "msstore" need it or adding it wil throw an error.
        /// </remarks>
        /// <param name="name">
        /// A <see cref="System.String"/> representing the name of the source to add.
        /// </param>
        /// <param name="arg">
        /// A <see cref="System.String"/> representing the source (eg. URL).
        /// </param>
        /// <param name="type">
        /// A <see cref="System.String"/> representing the source type.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the action was succesfull and <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
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

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(arg) || string.IsNullOrWhiteSpace(type))
            {
                return false;
            }

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(
                        string.Format(_sourceAddWithTypeCmd, name, arg, type));

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Getting installed sources failed.", e);
            }
        }

        /// <summary>
        /// Adds a new source to winget (Needs administrator rights).
        /// </summary>
        /// <remarks>
        /// The source type is optional but some sources like the "msstore" need it or adding it wil throw an error.
        /// </remarks>
        /// <param name="source">
        /// The <see cref="WGetNET.WinGetSource"/> to add.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the action was succesfull and <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public bool AddSource(WinGetSource source)
        {
            if (source == null)
            {
                return false;
            }

            if (source.IsEmpty)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(source.SourceType))
            {
                return AddSource(source.SourceName, source.SourceUrl);
            }

            return AddSource(source.SourceName, source.SourceUrl, source.SourceType);
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Asynchronously adds a new source to winget (Needs administrator rights).
        /// </summary>
        /// <remarks>
        /// The source type is optional but some sources like the "msstore" need it or adding it wil throw an error.
        /// </remarks>
        /// <param name="name">
        /// A <see cref="System.String"/> representing the name of the source to add.
        /// </param>
        /// <param name="arg">
        /// A <see cref="System.String"/> representing the source (eg. URL).
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the action was succesfull and <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
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

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(arg))
            {
                return false;
            }

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(
                        string.Format(_sourceAddCmd, name, arg));

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Getting installed sources failed.", e);
            }
        }

        /// <summary>
        /// Asynchronously adds a new source to winget (Needs administrator rights).
        /// </summary>
        /// <remarks>
        /// The source type is optional but some sources like the "msstore" need it or adding it wil throw an error.
        /// </remarks>
        /// <param name="name">
        /// A <see cref="System.String"/> representing the name of the source to add.
        /// </param>
        /// <param name="arg">
        /// A <see cref="System.String"/> representing the source (eg. URL).
        /// </param>
        /// <param name="type">
        /// A <see cref="System.String"/> representing the source type.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the action was succesfull and <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
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

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(arg) || string.IsNullOrWhiteSpace(type))
            {
                return false;
            }

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(
                        string.Format(_sourceAddWithTypeCmd, name, arg, type));

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Getting installed sources failed.", e);
            }
        }

        /// <summary>
        /// Asynchronously adds a new source to winget (Needs administrator rights).
        /// </summary>
        /// <remarks>
        /// The source type is optional but some sources like the "msstore" need it or adding it wil throw an error.
        /// </remarks>
        /// <param name="source">
        /// The <see cref="WGetNET.WinGetSource"/> to add.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the action was succesfull and <see langword="false"/> if it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public async Task<bool> AddSourceAsync(WinGetSource source)
        {
            if (source == null)
            {
                return false;
            }

            if (source.IsEmpty)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(source.SourceType))
            {
                return await AddSourceAsync(source.SourceName, source.SourceUrl);
            }

            return await AddSourceAsync(source.SourceName, source.SourceUrl, source.SourceType);
        }
#endif
        //---------------------------------------------------------------------------------------------

        //---Update------------------------------------------------------------------------------------
        /// <summary>
        /// Updates all sources that are installed in winget.
        /// </summary>
        /// <remarks>
        /// This may take a while depending on the sources.
        /// </remarks>
        /// <returns>
        /// <see langword="true"/> if the update was successfull or <see langword="false"/> if the it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
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
                throw new WinGetActionFailedException("Updating sources failed.", e);
            }
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Asynchronously updates all sources that are installed in winget.
        /// </summary>
        /// <remarks>
        /// This may take a while depending on the sources.
        /// </remarks>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the update was successfull or <see langword="false"/> if the it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
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
                throw new WinGetActionFailedException("Updating sources failed.", e);
            }
        }
#endif
        //---------------------------------------------------------------------------------------------

        //---Export------------------------------------------------------------------------------------
        /// <summary>
        /// Exports the winget sources as a json string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that contains the winget sorces in json format.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public string ExportSources()
        {
            try
            {
                ProcessResult result = 
                    _processManager.ExecuteWingetProcess(_sourceExportCmd);

                return ProcessOutputReader.ExportOutputToString(result);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Exporting sources failed.", e);
            }
        }

        /// <summary>
        /// Exports the winget sources as a json string.
        /// </summary>
        /// <param name="sourceName">The name of the source for the export.</param>
        /// <returns>
        /// A <see cref="System.String"/> that contains the winget sorces in json format.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public string ExportSources(string sourceName)
        {
            if (string.IsNullOrWhiteSpace(sourceName))
            {
                return string.Empty;
            }

            try
            {
                //Set Arguments
                string cmd =
                    _sourceExportCmd +
                    " -n " +
                    sourceName;

                ProcessResult result =
                    _processManager.ExecuteWingetProcess(cmd);

                return ProcessOutputReader.ExportOutputToString(result);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Exporting sources failed.", e);
            }
        }

        /// <summary>
        /// Exports the winget sources as a json string.
        /// </summary>
        /// <param name="source">
        /// The <see cref="WGetNET.WinGetSource"/> for the export.
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/> that contains the winget sorces in json format.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public string ExportSources(WinGetSource source)
        {
            if (source == null)
            {
                return string.Empty;
            }

            if (source.IsEmpty)
            {
                return string.Empty;
            }

            return ExportSources(source.SourceName);
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Asynchronously exports the winget sources as a json string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.String"/> that contains the winget sorces in json format.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public async Task<string> ExportSourcesAsync()
        {
            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(_sourceExportCmd);

                return ProcessOutputReader.ExportOutputToString(result);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Exporting sources failed.", e);
            }
        }

        /// <summary>
        /// Asynchronously exports the winget sources as a json string.
        /// </summary>
        /// <param name="sourceName">The name of the source for the export.</param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.String"/> that contains the winget sorces in json format.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public async Task<string> ExportSourcesAsync(string sourceName)
        {
            if (string.IsNullOrWhiteSpace(sourceName))
            {
                return string.Empty;
            }

            try
            {
                //Set Arguments
                string cmd =
                    _sourceExportCmd +
                    " -n " +
                    sourceName;

                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(cmd);

                return ProcessOutputReader.ExportOutputToString(result);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Exporting sources failed.", e);
            }
        }

        /// <summary>
        /// Asynchronously exports the winget sources as a json string.
        /// </summary>
        /// <param name="source">
        /// The <see cref="WGetNET.WinGetSource"/> for the export.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.String"/> that contains the winget sorces in json format.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public async Task<string> ExportSourcesAsync(WinGetSource source)
        {
            if (source == null)
            {
                return string.Empty;
            }

            if (source.IsEmpty)
            {
                return string.Empty;
            }

            return await ExportSourcesAsync(source.SourceName);
        }
#endif

        /// <summary>
        /// Exports the winget sources to a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/> objects.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/> objects.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public List<WinGetSource> ExportSourcesToObject()
        {
            return ExportStringToSources(ExportSources());
        }

        /// <summary>
        /// Exports the winget sources to a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/> objects.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/> objects.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public List<WinGetSource> ExportSourcesToObject(string sourceName)
        {
            return ExportStringToSources(ExportSources(sourceName));
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Asynchronously exports the winget sources to a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/> objects.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/> objects.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public async Task<List<WinGetSource>> ExportSourcesToObjectAsync()
        {
            return await ExportStringToSourcesAsync(await ExportSourcesAsync());
        }

        /// <summary>
        /// Asynchronously exports the winget sources to a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/> objects.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/> objects.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public async Task<List<WinGetSource>> ExportSourcesToObjectAsync(string sourceName)
        {
            return await ExportStringToSourcesAsync(await ExportSourcesAsync(sourceName));
        }
#endif

        /// <summary>
        /// Exports the winget sources in json format to a file.
        /// </summary>
        /// <param name="file">The file for the export.</param>
        /// <returns>
        /// <see langword="true"/> if the export was successfull or <see langword="false"/> if the it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public bool ExportSourcesToFile(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                return false;
            }

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
                throw new WinGetActionFailedException("Exporting sources failed.", e);
            }
        }

        /// <summary>
        /// Exports the winget sources in json format to a file.
        /// </summary>
        /// <param name="file">The file for the export.</param>
        /// <param name="sourceName">The name of the source for the export.</param>
        /// <returns>
        /// <see langword="true"/> if the export was successfull or <see langword="false"/> if the it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public bool ExportSourcesToFile(string file, string sourceName)
        {
            if (string.IsNullOrWhiteSpace(file) || string.IsNullOrWhiteSpace(sourceName))
            {
                return false;
            }

            try
            {
                //Set Arguments
                string cmd =
                    _sourceExportCmd +
                    " -n " +
                    sourceName;

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
                throw new WinGetActionFailedException("Exporting sources failed.", e);
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
        /// <see langword="true"/> if the export was successfull or <see langword="false"/> if the it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public bool ExportSourcesToFile(string file, WinGetSource source)
        {
            if (source == null)
            {
                return false;
            }

            if (source.IsEmpty || string.IsNullOrWhiteSpace(file))
            {
                return false;
            }

            return ExportSourcesToFile(file, source.SourceName);
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Asynchronously exports the winget sources in json format to a file.
        /// </summary>
        /// <param name="file">The file for the export.</param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the export was successfull or <see langword="false"/> if the it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public async Task<bool> ExportSourcesToFileAsync(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                return false;
            }

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
                throw new WinGetActionFailedException("Exporting sources failed.", e);
            }
        }

        /// <summary>
        /// Asynchronously exports the winget sources in json format to a file.
        /// </summary>
        /// <param name="file">The file for the export.</param>
        /// <param name="sourceName">The name of the source for the export.</param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the export was successfull or <see langword="false"/> if the it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public async Task<bool> ExportSourcesToFileAsync(string file, string sourceName)
        {
            if (string.IsNullOrWhiteSpace(file) || string.IsNullOrWhiteSpace(sourceName))
            {
                return false;
            }

            try
            {
                //Set Arguments
                string cmd =
                    _sourceExportCmd +
                    " -n " +
                    sourceName;

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
                throw new WinGetActionFailedException("Exporting sources failed.", e);
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
        /// The result is <see langword="true"/> if the export was successfull or <see langword="false"/> if the it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        public async Task<bool> ExportSourcesToFileAsync(string file, WinGetSource source)
        {
            if (source == null)
            {
                return false;
            }

            if (source.IsEmpty || string.IsNullOrWhiteSpace(file))
            {
                return false;
            }

            return await ExportSourcesToFileAsync(file, source.SourceName);
        }
#endif

        /// <summary>
        /// Convert the string output from winget source export to a 
        /// <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/> objects.
        /// </summary>
        /// <param name="exportString">
        /// A <see cref="System.String"/> containing the winget source export content.
        /// </param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/> objects.
        /// </returns>
        private List<WinGetSource> ExportStringToSources(string exportString)
        {
            List<WinGetSource> sourceList = new List<WinGetSource>();

#if NET6_0_OR_GREATER
            string[] jsonStrings = exportString.Split("}{");
#elif NETSTANDARD2_0
            string[] jsonStrings = exportString.Split(new string[1]{ "}{" }, StringSplitOptions.None);
#endif

            StringBuilder jsonString = new StringBuilder();
            for (int i = 0; i < jsonStrings.Length; i++)
            {
                if (!jsonStrings[i].StartsWith("{"))
                {
                    jsonString.Append("{");
                }

                jsonString.Append(jsonStrings[i]);

                if (!jsonStrings[i].EndsWith("}"))
                {
                    jsonString.Append("}");
                }
                
                WinGetSource? source =
                    JsonHandler.StringToObject<WinGetSource>(jsonString.ToString());

                if (source == null)
                {
                    throw new WinGetActionFailedException("Exporting sources failed. Could not parse json.");
                }

                sourceList.Add(source);

                jsonString.Clear();
            }

            return sourceList;
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Asynchronously convert the string output from winget source export to a 
        /// <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/> objects.
        /// </summary>
        /// <param name="exportString">
        /// A <see cref="System.String"/> containing the winget source export content.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/> objects.
        /// </returns>
        private async Task<List<WinGetSource>> ExportStringToSourcesAsync(string exportString)
        {
            List<WinGetSource> sourceList = new List<WinGetSource>();

            string[] jsonStrings = exportString.Split("}{");

            StringBuilder jsonString = new StringBuilder();
            for (int i = 0; i < jsonStrings.Length; i++)
            {
                if (!jsonStrings[i].StartsWith("{"))
                {
                    jsonString.Append("{");
                }

                jsonString.Append(jsonStrings[i]);

                if (!jsonStrings[i].EndsWith("}"))
                {
                    jsonString.Append("}");
                }

                WinGetSource? source =
                    await JsonHandler.StringToObjectAsync<WinGetSource>(jsonString.ToString());

                if (source == null)
                {
                    throw new WinGetActionFailedException("Exporting sources failed. Could not parse json.");
                }

                sourceList.Add(source);

                jsonString.Clear();
            }

            return sourceList;
        }
#endif
        //---------------------------------------------------------------------------------------------

        //---Import------------------------------------------------------------------------------------
        /// <summary>
        /// Imports sources into winget.
        /// </summary>
        /// <param name="winGetSources">
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/> objects.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the action was successfull and <see langword="false"/> if on or more sorces failed.
        /// </returns>
        public bool ImportSource(List<WinGetSource> winGetSources)
        {
            if (winGetSources == null || winGetSources.Count <= 0)
            {
                return false;
            }

            bool status = true;
            for (int i = 0; i < winGetSources.Count; i++)
            {
                if (!AddSource(winGetSources[i]))
                {
                    status = false;
                }
            }

            return status;
        }

        /// <summary>
        /// Imports a source into winget.
        /// </summary>
        /// <param name="winGetSource">
        /// A <see cref="WGetNET.WinGetSource"/> objects.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the action was successfull and <see langword="false"/> if it failed.
        /// </returns>
        public bool ImportSource(WinGetSource winGetSource)
        {
            return AddSource(winGetSource);
        }

        /// <summary>
        /// Imports a source into winget.
        /// </summary>
        /// <param name="jsonString">
        /// A <see cref="System.String"/> containing the json for ONE source.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the action was successfull and <see langword="false"/> if it failed.
        /// </returns>
        public bool ImportSource(string jsonString)
        {
            WinGetSource? source = JsonHandler.StringToObject<WinGetSource>(jsonString);

            if (source == null)
            {
                throw new WinGetActionFailedException("Importing source failed. Could not parse json.");
            }

            return AddSource(source);
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Asynchronously imports sources into winget.
        /// </summary>
        /// <param name="winGetSources">
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/> objects.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the action was successfull and <see langword="false"/> if on or more sorces failed.
        /// </returns>
        public async Task<bool> ImportSourceAsync(List<WinGetSource> winGetSources)
        {
            if (winGetSources == null || winGetSources.Count <= 0)
            {
                return false;
            }

            bool status = true;
            for (int i = 0; i < winGetSources.Count; i++)
            {
                if (!await AddSourceAsync(winGetSources[i]))
                {
                    status = false;
                }
            }

            return status;
        }

        /// <summary>
        /// Asynchronously imports a source into winget.
        /// </summary>
        /// <param name="winGetSource">
        /// A <see cref="WGetNET.WinGetSource"/> objects.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the action was successfull and <see langword="false"/> if it failed.
        /// </returns>
        public async Task<bool> ImportSourceAsync(WinGetSource winGetSource)
        {
            return await AddSourceAsync(winGetSource);
        }

        /// <summary>
        /// Asynchronously imports a source into winget.
        /// </summary>
        /// <param name="jsonString">
        /// A <see cref="System.String"/> containing the json for ONE source.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the action was successfull and <see langword="false"/> if it failed.
        /// </returns>
        public async Task<bool> ImportSourceAsync(string jsonString)
        {
            WinGetSource? source = await JsonHandler.StringToObjectAsync<WinGetSource>(jsonString);

            if (source == null)
            {
                throw new WinGetActionFailedException("Importing source failed. Could not parse json.");
            }

            return await AddSourceAsync(source);
        }
#endif
        //---------------------------------------------------------------------------------------------

        //---Reset-------------------------------------------------------------------------------------
        /// <summary>
        /// Resets all sources that are installed in winget (Needs administrator rights).
        /// </summary>
        /// <remarks>
        /// This may take a while depending on the sources.
        /// </remarks>
        /// <returns>
        /// <see langword="true"/> if the reset was successfull or <see langword="false"/> if the it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
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
                throw new WinGetActionFailedException("Reset sources failed.", e);
            }
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Asynchronously resets all sources that are installed in winget (Needs administrator rights).
        /// </summary>
        /// <remarks>
        /// This may take a while depending on the sources.
        /// </remarks>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the reset was successfull or <see langword="false"/> if the it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
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
                throw new WinGetActionFailedException("Reset sources failed.", e);
            }
        }
#endif
        //---------------------------------------------------------------------------------------------

        //---Remove------------------------------------------------------------------------------------
        /// <summary>
        /// Removes a source from winget (Needs administrator rights).
        /// </summary>
        /// <param name="name">
        /// A <see cref="System.String"/> representing the name of the source.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the remove was successfull or <see langword="false"/> if the it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
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

            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(string.Format(_sourceRemoveCmd, name));

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Removing source failed.", e);
            }
        }

        /// <summary>
        /// Removes a source from winget (Needs administrator rights).
        /// </summary>
        /// <param name="source">
        /// The <see cref="WGetNET.WinGetSource"/> to remove.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the remove was successfull or <see langword="false"/> if the it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public bool RemoveSources(WinGetSource source)
        {
            if (source == null)
            {
                return false;
            }

            if (source.IsEmpty)
            {
                return false;
            }

            return RemoveSources(source.SourceName);
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Asynchronously removes a source from winget (Needs administrator rights).
        /// </summary>
        /// <param name="name">
        /// A <see cref="System.String"/> representing the name of the source.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// The result is <see langword="true"/> if the remove was successfull or <see langword="false"/> if the it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
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

            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            try
            {
                ProcessResult result =
                    await _processManager.ExecuteWingetProcessAsync(string.Format(_sourceRemoveCmd, name));

                return result.Success;
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Removing source failed.", e);
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
        /// The result is <see langword="true"/> if the remove was successfull or <see langword="false"/> if the it failed.
        /// </returns>
        /// <exception cref="WGetNET.WinGetNotInstalledException">
        /// WinGet is not installed or not found on the system.
        /// </exception>
        /// <exception cref="WGetNET.WinGetActionFailedException">
        /// The current action failed for an unexpected reason.
        /// Please see inner exception.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The current user is missing administrator privileges for this call.
        /// </exception>
        public async Task<bool> RemoveSourcesAsync(WinGetSource source)
        {
            if (source == null)
            {
                return false;
            }

            if (source.IsEmpty)
            {
                return false;
            }

            return await RemoveSourcesAsync(source.SourceName);
        }
#endif
        //---------------------------------------------------------------------------------------------
    }
}
