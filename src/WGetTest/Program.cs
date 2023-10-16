using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using WGetNET;

namespace WGetTest
{
#pragma warning disable
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

                Version winGetVersionObject = connector.WinGetVersionObject;

                //---Tests-----------------------------------------------------------------------------
                List<WinGetPackage> test = connector.SearchPackage("git", "winget");
                Console.WriteLine(test[3].Name);
                Console.WriteLine(test[3].Id);

                List<WinGetPackage> test2 = connector.GetUpgradeablePackages();
                Console.WriteLine(test2[0].Name);
                Console.WriteLine(test2[0].Id);

                List<WinGetPackage> test3 = connector.GetInstalledPackages();
                Console.WriteLine(test3[0].Name);
                Console.WriteLine(test3[0].Id);

                List<WinGetSource> sourceList = sourceManager.GetInstalledSources();
                bool sourceUpdateStatus = sourceManager.UpdateSources();
                //bool sourceResetStatus = sourceManager.ResetSources();

                string sorceJson = sourceManager.ExportSources();
                string sorceJson2 = sourceManager.ExportSources("msstore");
                bool sorceJson3 = sourceManager.ExportSourcesToFile("C:\\Test\\AllSources.json");
                bool sorceJson4 = sourceManager.ExportSourcesToFile("C:\\Test\\msstoreSources.json", "msstore");
                //bool addSuccess = sourceManager.AddSource("msstore", "https://storeedgefd.dsx.mp.microsoft.com/v9.0", "Microsoft.Rest");

                List<WinGetSource> sorceJson5 = sourceManager.ExportSourcesToObject();

                string hash = connector.Hash("C:\\Test\\HashTest.txt");
                Console.WriteLine(hash);

                Task<string> settingsTask = connector.ExportSettingsAsync();
                settingsTask.Wait();
                string settings = settingsTask.Result;
                bool settingExportStatus = connector.ExportSettingsToFile("C:\\Test\\Settings.json");

                //bool upAllresult = connector.UpgradeAllPackages();

                bool downloadResult = connector.Download("7zip.7zip", "C:\\Test");

                Console.WriteLine(connector.PinAdd("7zip.7zip", true));
                List<WinGetPinnedPackage> pinnedList1 = connector.GetPinnedPackages();
                Console.WriteLine(connector.PinRemove("7zip.7zip"));
                Console.WriteLine(connector.PinAdd("7zip.7zip", "23.*"));
                List<WinGetPinnedPackage> pinnedList2 = connector.GetPinnedPackages();
                Console.WriteLine(connector.PinRemove("7zip.7zip"));

                Console.WriteLine(connector.PinAddInstalled("7zip.7zip", true));
                Console.WriteLine(connector.PinRemoveInstalled("7zip.7zip"));
                Console.WriteLine(connector.PinAddInstalled("7zip.7zip", "23.*"));
                Console.WriteLine(connector.PinRemoveInstalled("7zip.7zip"));

                Console.WriteLine(connector.ResetPins());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
#pragma warning restore
}