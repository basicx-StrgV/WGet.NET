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
        private const string _versionCmd = "--version";

        private readonly ProcessStartInfo _winGetStartInfo;

        /// <summary>
        /// Gets if winget is installed on the system.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if winget is installed or <see langword="false"/> if not.
        /// </returns>
        public bool WinGetInstalled { get { return _winGetInstalled; } }

        /// <summary>
        /// Gets the version number of the winget installation.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> with the version number.
        /// </returns>
        public string WinGetVersion { get { return _winGetVersion; } }

        private readonly bool _winGetInstalled;
        private readonly string _winGetVersion;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetInfo"/> class.
        /// </summary>
        public WinGetInfo()
        {
            _winGetStartInfo = new ProcessStartInfo()
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "winget",
                RedirectStandardOutput = true
            };

            _winGetVersion = CheckWinGetVersion();

            if (string.IsNullOrEmpty(_winGetVersion))
            {
                _winGetInstalled = false;
            }
            else
            {
                _winGetInstalled = true;
            }
        }

        private string CheckWinGetVersion()
        {
            try
            {
                //Set Arguments
                _winGetStartInfo.Arguments = _versionCmd;

                //Output List
                List<string> output = new List<string>();
                
                int exitCode = -1;

                //Create and run process
                using (Process getVersionProc = new Process { StartInfo = _winGetStartInfo })
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
                    return (string.Empty);
                }
                else
                {
                    return (string.Empty);
                }
            }
            catch (Exception)
            {
                return (string.Empty);
            }
        }
    }
}