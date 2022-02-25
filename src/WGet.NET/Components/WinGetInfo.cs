//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;

namespace WGetNET
{
    /// <summary>
    /// The <see cref="WGetNET.WinGetInfo"/> class offers informations about the installed winget version.
    /// </summary>
    public class WinGetInfo
    {
        private const string _versionCmd = "--version";

        private readonly ProcessManager _processManager;

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
            _processManager = new ProcessManager();

            _winGetVersion = CheckWinGetVersion();

            if (string.IsNullOrWhiteSpace(_winGetVersion))
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
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(_versionCmd);

                //Check if the process was succsessfull
                if (result.ExitCode == 0)
                {
                    for (int i = 0; i < result.Output.Length; i++)
                    {
                        if (result.Output[i].StartsWith("v"))
                        {
                            return result.Output[i].Trim();
                        }
                    }
                }
                return (string.Empty);
            }
            catch (Exception)
            {
                return (string.Empty);
            }
        }
    }
}