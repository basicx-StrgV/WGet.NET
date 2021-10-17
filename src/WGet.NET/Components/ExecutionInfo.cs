//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System.Diagnostics;

namespace WGetNET
{
    internal class ExecutionInfo
    {
        //Package Management
        public const string ListCmd = "list";
        public const string SearchCmd = "search {0}";
        public const string InstallCmd = "install {0}";
        public const string UpgradeCmd = "upgrade {0}";
        public const string GetUpgradeableCmd = "upgrade";
        public const string UninstallCmd = "uninstall {0}";
        public const string ExportCmd = "export -o {0}";
        public const string ImportCmd = "import -i {0} --ignore-unavailable";
        public const string VersionCmd = "--version";
        //Source Management
        public const string SourceListCmd = "source list";
        public const string SourceUpdateCmd = "source update";
        //Process start info
        public static readonly ProcessStartInfo WinGetStartInfo = new ProcessStartInfo()
        {
            WindowStyle = ProcessWindowStyle.Hidden,
            FileName = "winget",
            RedirectStandardOutput = true
        };
    }
}
