using System;
using System.Diagnostics;
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
            Stopwatch sw = Stopwatch.StartNew();
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
                List<WinGetAdminSetting> adminSettings = connector.GetAdminSettings();

                //bool enableResult = winget.EnableAdminSetting("LocalManifestFiles");
                //bool disableResult = winget.DisableAdminSetting("LocalManifestFiles");

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

                string json = sourceManager.SourcesToJson(sourceList);
                Console.WriteLine(json);

                sourceManager.ExportSourcesToFile("C:\\Test\\AllSources.json");
                sourceManager.ExportSourcesToFile("C:\\Test\\msstoreSources.json", "msstore");
                //bool addSuccess = sourceManager.AddSource("msstore", "https://storeedgefd.dsx.mp.microsoft.com/v9.0", "Microsoft.Rest");

                string hash = connector.Hash("C:\\Test\\HashTest.txt");
                Console.WriteLine(hash);

                Task<string> settingsTask = connector.ExportSettingsAsync();
                settingsTask.Wait();
                string settings = settingsTask.Result;
                connector.ExportSettingsToFile("C:\\Test\\Settings.json");

                //bool upAllresult = connector.UpgradeAllPackages();

                List<WinGetPackage> packageList = connector.SearchPackage("7zip.7zip", "winget");
                if (packageList.Count > 0)
                {
                    bool downloadResult = connector.Download(packageList[0], "C:\\Test");
                }

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

                Console.WriteLine(connector.ResetPins());

                Console.WriteLine("Same Package:");

                WinGetPackage p1 = WinGetPackage.Create("SampleP1", "SampleId1", "1.0.0", "SampleSource");
                WinGetPackage p2 = WinGetPackage.Create("SampleP2", "SampleId2", "2.0.0", "3.0.0", "");
                WinGetPackage p3 = WinGetPackage.Create("SampleP1", "SampleId1", "2.0.0", "SampleSource");

                Console.WriteLine(p1.SamePackage(p1)); // true
                Console.WriteLine(p1.SamePackage(p2)); // false
                Console.WriteLine(p1.SamePackage(p3)); // true
                Console.WriteLine(p1.SamePackage(p3, true)); // false
                Console.WriteLine(p2.SamePackage(p2)); // true
                Console.WriteLine(p2.SamePackage(p1)); // false
                Console.WriteLine(p2.SamePackage(p3)); // false
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                sw.Stop();
                Console.WriteLine("Execution Time: " + sw.Elapsed);

                Console.WriteLine("\nEnd of Test! Press any button to exit.");
                Console.Read();
            }
        }
    }
#pragma warning restore
}
