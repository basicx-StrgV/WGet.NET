using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
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
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                Console.WriteLine("=== Winget Information ===");
                WinGetPackageManager connector = new WinGetPackageManager();
                WinGetSourceManager sourceManager = new WinGetSourceManager();
                WinGet winget = new WinGet();

                Console.WriteLine("Winget Installed: " + winget.IsInstalled);
                Console.WriteLine("Winget Version: " + winget.VersionString);
                Console.WriteLine("Is Preview: " + winget.IsPreview + "\n");

                Version winGetVersionObject = connector.Version;
                WinGetInfo info = winget.GetInfo();
                Console.WriteLine($"Winget Info Version: {info.Version}");

                //---Tests-----------------------------------------------------------------------------
                // Admin Settings Test
                Console.WriteLine("\n=== Admin Settings Test ===");
                List<WinGetAdminSetting> adminSettings = connector.GetAdminSettings();
                Console.WriteLine($"Total Admin Settings: {adminSettings.Count}");
                if (adminSettings.Count > 0)
                    Console.WriteLine($"Sample Admin Setting: {adminSettings[0].EntryName}");

                // Package Search Test
                Console.WriteLine("\n=== Package Search Test ===");
                List<WinGetPackage> test = connector.SearchPackage("git", "winget");
                Console.WriteLine($"Total Packages Found for 'git': {test.Count}");
                if (test.Count > 3)
                {
                    Console.WriteLine($"Sample Package Name: {test[3].Name}");
                    Console.WriteLine($"Sample Package ID: {test[3].Id}");
                }

                // Upgradeable Packages Test
                Console.WriteLine("\n=== Upgradeable Packages Test ===");
                List<WinGetPackage> test2 = connector.GetUpgradeablePackages();
                Console.WriteLine($"Total Upgradeable Packages: {test2.Count}");
                if (test2.Count > 0)
                {
                    Console.WriteLine($"Sample Upgradeable Package Name: {test2[0].Name}");
                    Console.WriteLine($"Sample Upgradeable Package ID: {test2[0].Id}");
                }

                // Installed Packages Test
                Console.WriteLine("\n=== Installed Packages Test ===");
                List<WinGetPackage> test3 = connector.GetInstalledPackages();
                Console.WriteLine($"Total Installed Packages: {test3.Count}");
                if (test3.Count > 0)
                {
                    Console.WriteLine($"Sample Installed Package Name: {test3[0].Name}");
                    Console.WriteLine($"Sample Installed Package ID: {test3[0].Id}");
                }

                // Exact Installed Package Test
                Console.WriteLine("\n=== Exact Installed Package Test ===");
                WinGetPackage? test4 = connector.GetExactInstalledPackage("Microsoft Edge");
                if (test4 != null)
                    Console.WriteLine($"Microsoft Edge Package Found: {test4.Name}");

                // Package Download Test
                Console.WriteLine("\n=== Package Download Test ===");
                List<WinGetPackage> packageList = connector.SearchPackage("7zip.7zip", "winget");
                Console.WriteLine($"Total Packages Found for '7zip.7zip': {packageList.Count}");
                if (packageList.Count > 0)
                {
                    bool downloadResult = connector.Download(packageList[0], @"Tests\");
                    Console.WriteLine($"Download Result for {packageList[0].Name}: {downloadResult}");
                }



                // Pinning Tests
                Console.WriteLine("\n=== Pinning Tests ===");
                Console.WriteLine($"Adding 7Zip as Pinned: {connector.PinAdd("7zip.7zip", true)}");
                List<WinGetPinnedPackage> pinnedList1 = connector.GetPinnedPackages();
                Console.WriteLine($"Total Pinned Packages: {pinnedList1.Count}");
                if (pinnedList1.Count > 0)
                    Console.WriteLine($"{pinnedList1[0].Name}: {pinnedList1[0].PinTypeString}");
                Console.WriteLine($"Removing 7Zip as Pinned: {connector.PinRemove("7zip.7zip")}");
                Console.WriteLine($"Adding 7Zip as Pinned: {connector.PinAdd("7zip.7zip", "23.*")}");
                List<WinGetPinnedPackage> pinnedList2 = connector.GetPinnedPackages();
                Console.WriteLine($"Total Pinned Packages after version-specific pin: {pinnedList2.Count}");
                if (pinnedList2.Count > 0)
                    Console.WriteLine($"{pinnedList2[0].Name}: {pinnedList2[0].PinTypeString}");
                Console.WriteLine($"Removing 7Zip as Pinned: {connector.PinRemove("7zip.7zip")}");

                Console.WriteLine($"Adding 7Zip as Pinned for Install: {connector.PinAddInstalled("7zip.7zip", true)}");
                Console.WriteLine($"Removing 7Zip as Pinned: {connector.PinRemoveInstalled("7zip.7zip")}");
                Console.WriteLine($"Adding 7Zip as Pinned Installed Version: {connector.PinAddInstalled("7zip.7zip", "23.*")}");
                Console.WriteLine($"Removing 7Zip as Pinned Installed Version: {connector.PinRemoveInstalled("7zip.7zip")}");

                Console.WriteLine($"Resetting all Pins: {connector.ResetPins()}");

                // Source Tests
                Console.WriteLine("\n=== Source Tests ===");
                List<WinGetSource> sourceList = sourceManager.GetInstalledSources();
                Console.WriteLine($"Total Sources Installed: {sourceList.Count}");
                foreach (var source in sourceList)
                {
                    Console.WriteLine($"Source: {source.Name} ({source.Identifier})");
                }

                bool sourceUpdateStatus = sourceManager.UpdateSources();
                Console.WriteLine($"Source Update Status: {sourceUpdateStatus}");

                string json = sourceManager.SourcesToJson(sourceList);
                Console.WriteLine($"Sources as JSON: {json}");

                sourceManager.ExportSourcesToFile(@"Tests\AllSources.json");
                sourceManager.ExportSourcesToFile(@"Tests\msstoreSources.json", "msstore");

                // Hash Test
                Console.WriteLine("\n=== Hash Test ===");
                string hash = connector.Hash(@"Tests\AllSources.json");
                Console.WriteLine($"Hash of 'AllSources.json': {hash}");


                // Package comparison examples with more information
                Console.WriteLine("\n=== Package Comparison Tests ===");
                WinGetPackage p1 = WinGetPackage.Create("SampleP1", "SampleId1", "1.0.0", "SampleSource");
                WinGetPackage p2 = WinGetPackage.Create("SampleP2", "SampleId2", "2.0.0", "3.0.0", "");
                WinGetPackage p3 = WinGetPackage.Create("SampleP1", "SampleId1", "2.0.0", "SampleSource");
                bool PackageTests = true;
                PackageTests = PackageTests && p1.Equals(p1);
                PackageTests = PackageTests && !p1.Equals(p2);
                PackageTests = PackageTests && !p1.Equals(p3);
                PackageTests = PackageTests && p2.SamePackage(p2);
                PackageTests = PackageTests && !p2.Equals(p1);
                PackageTests = PackageTests && !p2.Equals(p3);
                PackageTests = PackageTests && !p1.SamePackage(p3, true);
                Console.WriteLine($"Package Tests: {PackageTests}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                sw.Stop();
                Console.WriteLine("\n\nExecution Time: " + sw.Elapsed);
                Console.WriteLine("End of Test! Press any button to exit.");
                Console.Read();
            }
        }
    }
#pragma warning restore
}