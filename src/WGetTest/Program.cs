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
            WinGetConnector connector = new WinGetConnector();
            Console.WriteLine("Winget Installed: " + connector.WinGetInstalled + 
                                "\nWinget Version: " + connector.WinGetVersion + "\n");

            //---Tests-----------------------------------------------------------------------------
            List<WinGetPackage> test = connector.SearchPackage("Git");
            Console.WriteLine(test[12].PackageName);

            bool test2 = connector.ImportPackages("C:\\Users\\Bjarne\\Downloads\\Test.json");
            Console.WriteLine(test2);
        }
    }
}
