//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace WGetNET
{
    /// <summary>
    /// The <see cref="WGetNET.WinGetInfo"/> class offers informations about the installed winget version.
    /// </summary>
    public class WinGetInfo
    {
        /// <summary>
        /// Gets if winget is installed on the system
        /// </summary>
        /// <returns>
        /// true if winget is installed
        /// </returns>
        public bool WinGetInstalled { get { return _winGetInstalled; } }

        /// <summary>
        /// Gets the number of the installed winget version
        /// </summary>
        /// <returns>
        /// The number of the installed winget version or a placeholder string if winget is not installed
        /// </returns>
        public string WinGetVersion { get { return _winGetVersion; } }

        private readonly bool _winGetInstalled;
        private readonly string _winGetVersion;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetInfo"/> class.
        /// </summary>
        public WinGetInfo()
        {
            _winGetVersion = CheckWinGetVersion();
            if (!_winGetVersion.Equals("[MISSING INSTALLATION]"))
            {
                _winGetInstalled = true;
            }
            else
            {
                _winGetInstalled = false;
            }
        }

        private string CheckWinGetVersion()
        {
            try
            {
                //Set Arguments
                ExecutionInfo.WinGetStartInfo.Arguments = ExecutionInfo.VersionCmd;

                //Output List
                List<string> output = new List<string>();
                
                int exitCode = -1;

                //Create and run process
                using (Process getVersionProc = new Process { StartInfo = ExecutionInfo.WinGetStartInfo })
                {
                    getVersionProc.Start();

                    //Read output to list
                    using StreamReader procOutputStream = getVersionProc.StandardOutput;
                    while (!procOutputStream.EndOfStream)
                    {
                        output.Add(procOutputStream.ReadLine());
                    }

                    //Wait till end and get exit code
                    getVersionProc.WaitForExit();
                    exitCode = getVersionProc.ExitCode;
                }

                //Check if the process was succsessfull
                if (exitCode == 0)
                {
                    for (int i = 0; i < output.Count; i++)
                    {
                        if (output[i].StartsWith("v"))
                        {
                            return (output[i]);
                        }
                    }
                    return ("[MISSING INSTALLATION]");
                }
                else
                {
                    return ("[MISSING INSTALLATION]");
                }
            }
            catch (Exception)
            {
                return ("[MISSING INSTALLATION]");
            }
        }
    }
}