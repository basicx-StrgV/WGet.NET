using System;
using System.Collections.Generic;
using System.Diagnostics;
using WGetNET;

namespace WGetTest
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program();
        }
        Program()
        {
            Run();
        }
    
        private void Run()
        {
            try
            {
                WinGetPackageManager connector = new WinGetPackageManager();
                WinGetSourceManager sourceManager = new WinGetSourceManager();
                WinGetInfo info = new WinGetInfo();
                Output("Winget Installed: " + info.WinGetInstalled +
                                    "\nWinget Version: " + info.WinGetVersion + "\n");

                //---Tests-----------------------------------------------------------------------------
                var test = connector.SearchPackage("Git");
                Output(test[0].PackageName);
                Output(test[0].PackageId);

                var test2 = connector.GetUpgradeablePackages();
                Output(test2[0].PackageName);
                Output(test2[0].PackageId);

                var sourceList = sourceManager.GetInstalledSources();
                bool sourceUpdateStatus = sourceManager.UpdateSources();
                //bool sourceResetStatus = sourceManager.ResetSources();

                string sorceJson = sourceManager.ExportSources();
                string sorceJson2 = sourceManager.ExportSources("msstore");
                bool sorceJson3 = sourceManager.ExportSourcesToFile("C:\\Test\\AllSources.json");
                bool sorceJson4 = sourceManager.ExportSourcesToFile("C:\\Test\\msstoreSources.json", "msstore");
                //bool addSuccess = sourceManager.AddSource("msstore", "https://storeedgefd.dsx.mp.microsoft.com/v9.0", "Microsoft.Rest");
            }
            catch (Exception e)
            {
                Output(e);
            }
        }

        private void Output(object message)
        {
            Console.WriteLine(message);
            Debug.WriteLine(message);
        }
    }
}
