﻿using System;
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
            WinGetPackageManager connector = new WinGetPackageManager();
            WinGetSourceManager sourceManager = new WinGetSourceManager();
            WinGetInfo info = new WinGetInfo();
            Console.WriteLine("Winget Installed: " + info.WinGetInstalled + 
                                "\nWinget Version: " + info.WinGetVersion + "\n");
            
            //---Tests-----------------------------------------------------------------------------
            List<WinGetPackage> test = connector.SearchPackage("Git");
            Console.WriteLine(test[49].PackageName);
            Console.WriteLine(test[49].PackageId);

            List<WinGetSource> sourceList = sourceManager.GetInstalledSources();
            bool sourceUpdateStatus = sourceManager.UpdateSources();
        }
    }
}
