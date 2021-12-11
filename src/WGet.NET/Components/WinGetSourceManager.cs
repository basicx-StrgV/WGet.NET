//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WGetNET
{
    /// <summary>
    /// The <see cref="WGetNET.WinGetSourceManager"/> class offers methods to manage the sources used by winget.
    /// </summary>
    public class WinGetSourceManager
    {
        private const string _sourceListCmd = "source list";
        private const string _sourceUpdateCmd = "source update";
        private const string _sourceExportCmd = "source export";

        private readonly ProcessStartInfo _winGetStartInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetSourceManager"/> class.
        /// </summary>
        public WinGetSourceManager()
        {
            _winGetStartInfo = new ProcessStartInfo()
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "winget",
                RedirectStandardOutput = true
            };
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
                //Set Arguments
                _winGetStartInfo.Arguments = _sourceListCmd;

                //Output List
                List<string> output = new List<string>();

                //Create and run process
                using (Process searchProc = new Process() { StartInfo = _winGetStartInfo })
                {
                    searchProc.Start();

                    //Read output to list
                    using StreamReader procOutputStream = searchProc.StandardOutput;
                    while (!procOutputStream.EndOfStream)
                    {
                        output.Add(procOutputStream.ReadLine());
                    }

                    searchProc.WaitForExit();
                }

                return ToSourceList(output);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception)
            {
                return new List<WinGetSource>();
            }
        }

        /// <summary>
        /// Gets a list of all sources that are installed in winget.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetSource"/> instances.
        /// </returns>
        public async Task<List<WinGetSource>> GetInstalledSourcesAsync()
        {
            return await Task.Run(() => GetInstalledSources());
        }
        //---------------------------------------------------------------------------------------------

        //---Update------------------------------------------------------------------------------------
        /// <summary>
        /// Updates all sources that are installed in winget.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the update was successfull or <see langword="false"/> if the it failed.
        /// </returns>
        public bool UpdateSources()
        {
            try
            {
                //Set Arguments
                _winGetStartInfo.Arguments = _sourceUpdateCmd;

                int exitCode = -1;

                //Create and run process
                using (Process updateProc = new Process { StartInfo = _winGetStartInfo })
                {
                    updateProc.Start();

                    //Wait till end and get exit code
                    updateProc.WaitForExit();
                    exitCode = updateProc.ExitCode;
                }

                //Check if installation was succsessfull
                if (exitCode == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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

        /// <summary>
        /// Updates all sources that are installed in winget.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the update was successfull or <see langword="false"/> if the it failed.
        /// </returns>
        public async Task<bool> UpdateSourcesAsync()
        {
            return await Task.Run(() => UpdateSources());
        }
        //---------------------------------------------------------------------------------------------
        
        //---Export------------------------------------------------------------------------------------
        /// <summary>
        /// Exports the winget sources as a json string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that contains the winget sorces in json format.
        /// </returns>
        public string ExportSources()
        {
            try
            {
                //Set Arguments
                _winGetStartInfo.Arguments = _sourceExportCmd;

                StringBuilder outputBuilder = new StringBuilder();

                int exitCode = -1;

                //Create and run process
                using (Process updateProc = new Process { StartInfo = _winGetStartInfo })
                {
                    updateProc.Start();

                    //Read output to list
                    using StreamReader procOutputStream = updateProc.StandardOutput;
                    while (!procOutputStream.EndOfStream)
                    {
                        outputBuilder.Append(procOutputStream.ReadLine());
                    }

                    //Wait till end and get exit code
                    updateProc.WaitForExit();
                    exitCode = updateProc.ExitCode;
                }

                //Check if installation was succsessfull
                if (exitCode == 0)
                {
                    return outputBuilder.ToString().Trim();
                }
                else
                {
                    return string.Empty;
                }
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
        public string ExportSources(string sourceName)
        {
            try
            {
                //Set Arguments
                _winGetStartInfo.Arguments =
                    _sourceExportCmd +
                    " -n " +
                    sourceName;

                StringBuilder outputBuilder = new StringBuilder();

                int exitCode = -1;

                //Create and run process
                using (Process updateProc = new Process { StartInfo = _winGetStartInfo })
                {
                    updateProc.Start();

                    //Read output to list
                    using StreamReader procOutputStream = updateProc.StandardOutput;
                    while (!procOutputStream.EndOfStream)
                    {
                        outputBuilder.Append(procOutputStream.ReadLine());
                    }

                    //Wait till end and get exit code
                    updateProc.WaitForExit();
                    exitCode = updateProc.ExitCode;
                }

                //Check if installation was succsessfull
                if (exitCode == 0)
                {
                    return outputBuilder.ToString().Trim();
                }
                else
                {
                    return string.Empty;
                }
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
        /// <returns>
        /// A <see cref="System.String"/> that contains the winget sorces in json format.
        /// </returns>
        public async Task<string> ExportSourcesAsync()
        {
            return await Task.Run(() => ExportSources());
        }

        /// <summary>
        /// Exports the winget sources as a json string.
        /// </summary>
        /// <param name="sourceName">The name of the source for the export.</param>
        /// <returns>
        /// A <see cref="System.String"/> that contains the winget sorces in json format.
        /// </returns>
        public async Task<string> ExportSourcesAsync(string sourceName)
        {
            return await Task.Run(() => ExportSources(sourceName));
        }

        /// <summary>
        /// Exports the winget sources in json format to a file.
        /// </summary>
        /// <param name="file">The file for the export.</param>
        /// <returns>
        /// <see langword="true"/> if the export was successfull or <see langword="false"/> if the it failed.
        /// </returns>
        public bool ExportSourcesToFile(string file)
        {
            try
            {
                //Set Arguments
                _winGetStartInfo.Arguments = _sourceExportCmd;

                StringBuilder outputBuilder = new StringBuilder();

                int exitCode = -1;

                //Create and run process
                using (Process updateProc = new Process { StartInfo = _winGetStartInfo })
                {
                    updateProc.Start();

                    //Read output to list
                    using StreamReader procOutputStream = updateProc.StandardOutput;
                    while (!procOutputStream.EndOfStream)
                    {
                        outputBuilder.Append(procOutputStream.ReadLine());
                    }

                    //Wait till end and get exit code
                    updateProc.WaitForExit();
                    exitCode = updateProc.ExitCode;
                }

                //Check if installation was succsessfull
                if (exitCode == 0 && outputBuilder.ToString().Trim() != string.Empty)
                {
                    File.WriteAllText(
                        file, 
                        outputBuilder
                        .ToString()
                        .Trim());
                    return true;
                }
                else
                {
                    return false;
                }
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
        public bool ExportSourcesToFile(string file, string sourceName)
        {
            try
            {
                //Set Arguments
                _winGetStartInfo.Arguments =
                    _sourceExportCmd +
                    " -n " +
                    sourceName;

                StringBuilder outputBuilder = new StringBuilder();

                int exitCode = -1;

                //Create and run process
                using (Process updateProc = new Process { StartInfo = _winGetStartInfo })
                {
                    updateProc.Start();

                    //Read output to list
                    using StreamReader procOutputStream = updateProc.StandardOutput;
                    while (!procOutputStream.EndOfStream)
                    {
                        outputBuilder.Append(procOutputStream.ReadLine());
                    }

                    //Wait till end and get exit code
                    updateProc.WaitForExit();
                    exitCode = updateProc.ExitCode;
                }

                //Check if installation was succsessfull
                if (exitCode == 0 && outputBuilder.ToString().Trim() != string.Empty)
                {
                    File.WriteAllText(
                        file,
                        outputBuilder
                        .ToString()
                        .Trim());
                    return true;
                }
                else
                {
                    return false;
                }
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
        /// <returns>
        /// <see langword="true"/> if the export was successfull or <see langword="false"/> if the it failed.
        /// </returns>
        public async Task<bool> ExportSourcesToFileAsync(string file)
        {
            return await Task.Run(() => ExportSourcesToFile(file));
        }

        /// <summary>
        /// Exports the winget sources in json format to a file.
        /// </summary>
        /// <param name="file">The file for the export.</param>
        /// <param name="sourceName">The name of the source for the export.</param>
        /// <returns>
        /// <see langword="true"/> if the export was successfull or <see langword="false"/> if the it failed.
        /// </returns>
        public async Task<bool> ExportSourcesToFileAsync(string file, string sourceName)
        {
            return await Task.Run(() => ExportSourcesToFile(file, sourceName));
        }
        //---------------------------------------------------------------------------------------------

        private List<WinGetSource> ToSourceList(List<string> output)
        {
            //Get top line index
            int topLineIndex = 0;
            for (int i = 0; i < output.Count; i++)
            {
                if (output[i].Contains("------"))
                {
                    topLineIndex = i;
                    break;
                }
            }

            //Get start indexes of each tabel colum
            int nameStartIndex = 0;

            int urlStartIndex = 0;

            int labelLine = topLineIndex - 1;
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
            output.RemoveRange(0, topLineIndex + 1);

            List<WinGetSource> resultList = new List<WinGetSource>();

            foreach (string line in output)
            {
                string name = 
                    line[nameStartIndex..urlStartIndex]
                    .Trim();
                string winGetId = 
                    line[urlStartIndex..]
                    .Trim();

                resultList.Add(
                    new WinGetSource() 
                    { 
                        SourceName = name, 
                        SourceUrl = winGetId 
                    });
            }

            return resultList;
        }
    }
}
