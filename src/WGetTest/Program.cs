using System;
using System.Collections.Generic;
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
                Console.WriteLine("Winget Installed: " + info.WinGetInstalled +
                                    "\nWinget Version: " + info.WinGetVersion + "\n");

                //---Tests-----------------------------------------------------------------------------
                List<WinGetPackage> test = connector.SearchPackage("Git");
                Console.WriteLine(test[0].PackageName);
                Console.WriteLine(test[0].PackageId);

                List<WinGetPackage> test2 = connector.GetUpgradeablePackages();
                Console.WriteLine(test2[0].PackageName);
                Console.WriteLine(test2[0].PackageId);

                List<WinGetSource> sourceList = sourceManager.GetInstalledSources();
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
                Console.WriteLine(e);
            }
        }
    }
}
