using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using WGetNET;

namespace WGetTestLegacySupport
{
#pragma warning disable
    internal class Program
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
                WinGet winget = new WinGet();
                Console.WriteLine("Winget Installed: " + winget.IsInstalled +
                                    "\nWinget Version: " + winget.VersionString + "\n");

                Version winGetVersionObject = connector.Version;

                WinGetInfo info = winget.GetInfo();
                Console.WriteLine(info.Version);

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

                Task sorceJson3Task = sourceManager.ExportSourcesToFileAsync("C:\\Test\\AllSources.json");
                sorceJson3Task.Wait();
                sourceManager.ExportSourcesToFile("C:\\Test\\msstoreSources.json", "msstore");
                //bool addSuccess = sourceManager.AddSource("msstore", "https://storeedgefd.dsx.mp.microsoft.com/v9.0", "Microsoft.Rest");

                string hash = connector.Hash("C:\\Test\\HashTest.txt");
                Console.WriteLine(hash);

                string settings = connector.ExportSettings();
                Task<bool> settingExportStatusTask = connector.ExportSettingsToFileAsync("C:\\Test\\Settings.json");
                settingExportStatusTask.Wait();
                bool settingExportStatus = settingExportStatusTask.Result;

                //bool upAllresult = connector.UpgradeAllPackages();

                bool downloadResult = connector.Download("7zip.7zip", "C:\\Test");

                Console.WriteLine(connector.PinAdd("7zip.7zip", true));
                List<WinGetPinnedPackage> pinnedList1 = connector.GetPinnedPackages();
                Console.WriteLine(pinnedList1[0].Name + ": " + pinnedList1[0].PinTypeString);
                Console.WriteLine(connector.PinRemove("7zip.7zip"));
                Console.WriteLine(connector.PinAdd("7zip.7zip", "23.*"));
                List<WinGetPinnedPackage> pinnedList2 = connector.GetPinnedPackages();
                Console.WriteLine(pinnedList2[0].Name + ": " + pinnedList2[0].PinTypeString);
                Console.WriteLine(connector.PinRemove("7zip.7zip"));

                Console.WriteLine(connector.PinAddInstalled("7zip.7zip", true));
                Console.WriteLine(connector.PinRemoveInstalled("7zip.7zip"));
                Console.WriteLine(connector.PinAddInstalled("7zip.7zip", "23.*"));
                Console.WriteLine(connector.PinRemoveInstalled("7zip.7zip"));

                Console.WriteLine("\nEnd of Test! Press any button to exit.");
                Console.Read();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
#pragma warning restore
}
