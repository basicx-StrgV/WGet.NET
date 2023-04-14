//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
namespace WGetNET
{
    /// <summary>
    /// The <see cref="WGetNET.WinGetInfo"/> class offers informations about the installed winget version.
    /// </summary>
    public class WinGetInfo
    {
        private const string _versionCmd = "--version";

        internal readonly ProcessManager _processManager;

        /// <summary>
        /// Gets if winget is installed on the system.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if winget is installed or <see langword="false"/> if not.
        /// </returns>
        public bool WinGetInstalled
        {
            get => CheckWinGetVersion() != string.Empty;
        }

        /// <summary>
        /// Gets the version number of the winget installation.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> with the version number.
        /// </returns>
        public string WinGetVersion
        {
            get => CheckWinGetVersion();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetInfo"/> class.
        /// </summary>
        public WinGetInfo()
        {
            _processManager = new ProcessManager("winget");
        }

        private string CheckWinGetVersion()
        {
            try
            {
                var result = _processManager.ExecuteWingetProcess(_versionCmd);

                for (var line = 0; line < result.Output.Length; line++)
                {
                    if (result.Output[line].StartsWith("v"))
                    {
                        return result.Output[line].Trim();
                    }
                }
            }
            catch
            {
                //No handling needed, winget can not be accessed
            }

            return string.Empty;
        }
    }
}