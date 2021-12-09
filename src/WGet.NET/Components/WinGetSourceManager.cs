//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.IO;
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
        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetSourceManager"/> class.
        /// </summary>
        public WinGetSourceManager()
        {
        }

        //---List--------------------------------------------------------------------------------------
        /// <summary>
        /// Gets a list of all installed sources
        /// </summary>
        /// <returns>
        /// A List of sources that are used by winget
        /// </returns>
        public List<WinGetSource> GetInstalledSources()
        {
            try
            {
                //Set Arguments
                ExecutionInfo.WinGetStartInfo.Arguments = ExecutionInfo.SourceListCmd;

                //Output List
                List<string> output = new List<string>();

                //Create and run process
                using (Process searchProc = new Process() { StartInfo = ExecutionInfo.WinGetStartInfo })
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
        /// Gets a list of all installed sources
        /// </summary>
        /// <returns>
        /// A List of sources that are used by winget
        /// </returns>
        public async Task<List<WinGetSource>> GetInstalledSourcesAsync()
        {
            Task<List<WinGetSource>> getInstalledSources = Task.Run(() => GetInstalledSources());
            await getInstalledSources;
            return getInstalledSources.Result;
        }
        //---------------------------------------------------------------------------------------------

        //---Update------------------------------------------------------------------------------------
        /// <summary>
        /// Updates all installed winget sources
        /// </summary>
        /// <returns>
        /// True if the action was successfull
        /// </returns>
        public bool UpdateSources()
        {
            try
            {
                //Set Arguments
                ExecutionInfo.WinGetStartInfo.Arguments = ExecutionInfo.SourceUpdateCmd;

                //Output List
                List<string> output = new List<string>();

                int exitCode = -1;

                //Create and run process
                using (Process updateProc = new Process { StartInfo = ExecutionInfo.WinGetStartInfo })
                {
                    updateProc.Start();

                    //Read output to list
                    using StreamReader procOutputStream = updateProc.StandardOutput;
                    while (!procOutputStream.EndOfStream)
                    {
                        output.Add(procOutputStream.ReadLine());
                    }

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
        /// Updates all installed winget sources
        /// </summary>
        /// <returns>
        /// True if the action was successfull
        /// </returns>
        public async Task<bool> UpdateSourcesAsync()
        {
            Task<bool> updateSources = Task.Run(() => UpdateSources());
            await updateSources;
            return updateSources.Result;
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
