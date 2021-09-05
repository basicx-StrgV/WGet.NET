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
            Console.WriteLine(connector.WinGetInstalled + " " + connector.WinGetVersion);

            List<WinGetPackage> test = connector.SearchPackage("Git");
            Console.WriteLine(test[12].PackageName);
        }
    }
}
