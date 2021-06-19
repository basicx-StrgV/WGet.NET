//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WGetNET
{
    /// <summary>
    /// The WinGetConnector class offers methods to use winget.
    /// </summary>
    public class WinGetConnector
    {
        private static ProcessStartInfo _procStartInfo = new ProcessStartInfo()
        {
            WindowStyle = ProcessWindowStyle.Hidden,
            FileName = "winget",
            RedirectStandardOutput = true
        };

        private const string _searchCmd = "search {0}";
        private const string _installCmd = "install {0}";
        private const string _uninstallCmd = "uninstall {0}";

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetConnector"/> class.
        /// </summary>
        public WinGetConnector()
        {

        }

        /// <summary>
        /// Uses the winget search function to search for a package that maches the given name
        /// </summary>
        /// <param name="packageName">The name of the package that you want to search</param>
        /// <returns>
        /// A List of packages that mached the search criteria
        /// </returns>
        public List<WinGetPackage> SearchPackage(string packageName)
        {
            try
            {
                //Set Arguments
                _procStartInfo.Arguments = String.Format(_searchCmd, packageName);

                //Output List
                List<string> output = new List<string>();

                //Create and run process
                Process searchProc= new Process();
                searchProc.StartInfo = _procStartInfo;
                searchProc.Start();

                //Read output to list
                StreamReader procOutputStream = searchProc.StandardOutput;
                while (!procOutputStream.EndOfStream)
                {
                    output.Add(procOutputStream.ReadLine());
                }

                //Wait till end and close process
                searchProc.WaitForExit();
                searchProc.Close();
                searchProc.Dispose();
                procOutputStream.Close();
                procOutputStream.Dispose();


                //Get top line index
                int topLineIndex = 0;
                for (int i = 0; i < output.Count; i++)
                {
                    if (output[i].Contains("------"))
                    {
                        topLineIndex = i;
                        break;
                    }
                }

                //Get start indexes of each tabel colum
                int nameStartIndex = 0;

                int idStartIndex = 0;
                bool idStartIndexSet = false;

                int versionStartIndex = 0;
                bool versionStartIndexSet = false;

                int extraInfoStartIndex = 0;
                bool extraInfoStartIndexSet = false;

                int labelLine = topLineIndex - 1;
                bool checkForChar = false;
                for (int i = 0; i < output[labelLine].Length; i++)
                {
                    if (output[labelLine][i] != ' ' && checkForChar)
                    {
                        if (!idStartIndexSet)
                        {
                            idStartIndex = i;
                            idStartIndexSet = true;
                            checkForChar = false;
                        }
                        else if (!versionStartIndexSet)
                        {
                            versionStartIndex = i;
                            versionStartIndexSet = true;
                            checkForChar = false;
                        }
                        else if (!extraInfoStartIndexSet)
                        {
                            extraInfoStartIndex = i;
                            extraInfoStartIndexSet = true;
                            checkForChar = false;
                        }
                    }
                    else if (output[labelLine][i] == ' ')
                    {
                        checkForChar = true;
                    }
                }

                //Remove unneeded output Lines
                output.RemoveRange(0, topLineIndex + 1);

                List<WinGetPackage> resultList = new List<WinGetPackage>();

                foreach (string line in output)
                {
                    string name = line.Substring(nameStartIndex, idStartIndex - 1).Trim();
                    string winGetId = line.Substring(idStartIndex, (versionStartIndex - idStartIndex) - 1).Trim();
                    string version = line.Substring(versionStartIndex, (extraInfoStartIndex - versionStartIndex) - 1).Trim();

                    resultList.Add(new WinGetPackage() { PackageName = name, PackageId = winGetId, PackageVersion = version });
                }

                return resultList;
            }
            catch (System.ComponentModel.Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception)
            {
                return new List<WinGetPackage>();
            }
        }

        /// <summary>
        /// Uses the winget search function to search for a package that maches the given name
        /// </summary>
        /// <param name="packageName">The name of the package that you want to search</param>
        /// <returns>
        /// A Task object of the search task
        /// </returns>
        public async Task<List<WinGetPackage>> SearchPackageAsync(string packageName)
        {
            Task<List<WinGetPackage>> search = Task.Run(() => SearchPackage(packageName));
            await search;
            return search.Result;
        }

        /// <summary>
        /// Insatll a package using winget
        /// </summary>
        /// <param name="package">The id or name of the package that should be installed</param>
        /// <returns>
        /// True if the installation was successfull
        /// </returns>
        public bool InstallPackage(string package)
        {
            try
            {
                //Set Arguments
                _procStartInfo.Arguments = String.Format(_installCmd, package);

                //Output List
                List<string> output = new List<string>();

                //Create and run process
                Process installProc = new Process();
                installProc.StartInfo = _procStartInfo;
                installProc.Start();

                //Read output to list
                StreamReader procOutputStream = installProc.StandardOutput;
                while (!procOutputStream.EndOfStream)
                {
                    output.Add(procOutputStream.ReadLine());
                }

                //Wait till end and get exit code
                installProc.WaitForExit();
                int exitCode = installProc.ExitCode;

                //Close and Dispose the process and output stream
                installProc.Close();
                installProc.Dispose();
                procOutputStream.Close();
                procOutputStream.Dispose();

                //Check if installation was succsessfull
                bool installSuccessfull = false;
                if(exitCode == 0)
                {
                    installSuccessfull = true;
                }
                
                return installSuccessfull;
            }
            catch (System.ComponentModel.Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Insatll a package using winget
        /// </summary>
        /// <param name="package">The package that should be installed</param>
        /// <returns>
        /// True if the installation was successfull
        /// </returns>
        public bool InstallPackage(WinGetPackage package)
        {
            try
            {
                //Set Arguments
                _procStartInfo.Arguments = String.Format(_installCmd, package.PackageId);

                //Output List
                List<string> output = new List<string>();

                //Create and run process
                Process installProc = new Process();
                installProc.StartInfo = _procStartInfo;
                installProc.Start();

                //Read output to list
                StreamReader procOutputStream = installProc.StandardOutput;
                while (!procOutputStream.EndOfStream)
                {
                    output.Add(procOutputStream.ReadLine());
                }

                //Wait till end and get exit code
                installProc.WaitForExit();
                int exitCode = installProc.ExitCode;

                //Close and Dispose the process and output stream
                installProc.Close();
                installProc.Dispose();
                procOutputStream.Close();
                procOutputStream.Dispose();

                //Check if installation was succsessfull
                bool installSuccessfull = false;
                if (exitCode == 0)
                {
                    installSuccessfull = true;
                }

                return installSuccessfull;
            }
            catch (System.ComponentModel.Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Insatll a package using winget
        /// </summary>
        /// <param name="package">The id or name of the package that should be installed</param>
        /// <returns>
        /// A Task object of the install task
        /// </returns>
        public async Task<bool> InstallPackageAsync(string package)
        {
            Task<bool> install = Task.Run(() => InstallPackage(package));
            await install;
            return install.Result;
        }

        /// <summary>
        /// Insatll a package using winget
        /// </summary>
        /// <param name="package">The package that should be installed</param>
        /// <returns>
        /// A Task object of the install task
        /// </returns>
        public async Task<bool> InstallPackageAsync(WinGetPackage package)
        {
            Task<bool> install = Task.Run(() => InstallPackage(package));
            await install;
            return install.Result;
        }

        /// <summary>
        /// Uninsatll a package using winget
        /// </summary>
        /// <param name="package">The id or name of the package that should be uninstalled</param>
        /// <returns>
        /// True if the uninstallation was successfull
        /// </returns>
        public bool UninstallPackage(string package)
        {
            try
            {
                //Set Arguments
                _procStartInfo.Arguments = String.Format(_uninstallCmd, package);

                //Output List
                List<string> output = new List<string>();

                //Create and run process
                Process uninstallProc = new Process();
                uninstallProc.StartInfo = _procStartInfo;
                uninstallProc.Start();

                //Read output to list
                StreamReader procOutputStream = uninstallProc.StandardOutput;
                while (!procOutputStream.EndOfStream)
                {
                    output.Add(procOutputStream.ReadLine());
                }

                //Wait till end and get exit code
                uninstallProc.WaitForExit();
                int exitCode = uninstallProc.ExitCode;

                //Close and Dispose the process and output stream
                uninstallProc.Close();
                uninstallProc.Dispose();
                procOutputStream.Close();
                procOutputStream.Dispose();

                //Check if installation was succsessfull
                bool uninstallSuccessfull = false;
                if (exitCode == 0)
                {
                    uninstallSuccessfull = true;
                }

                return uninstallSuccessfull;
            }
            catch (System.ComponentModel.Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Uninsatll a package using winget
        /// </summary>
        /// <param name="package">The package that should be uninstalled</param>
        /// <returns>
        /// True if the uninstallation was successfull
        /// </returns>
        public bool UninstallPackage(WinGetPackage package)
        {
            try
            {
                //Set Arguments
                _procStartInfo.Arguments = String.Format(_uninstallCmd, package.PackageId);

                //Output List
                List<string> output = new List<string>();

                //Create and run process
                Process uninstallProc = new Process();
                uninstallProc.StartInfo = _procStartInfo;
                uninstallProc.Start();

                //Read output to list
                StreamReader procOutputStream = uninstallProc.StandardOutput;
                while (!procOutputStream.EndOfStream)
                {
                    output.Add(procOutputStream.ReadLine());
                }

                //Wait till end and get exit code
                uninstallProc.WaitForExit();
                int exitCode = uninstallProc.ExitCode;

                //Close and Dispose the process and output stream
                uninstallProc.Close();
                uninstallProc.Dispose();
                procOutputStream.Close();
                procOutputStream.Dispose();

                //Check if installation was succsessfull
                bool uninstallSuccessfull = false;
                if (exitCode == 0)
                {
                    uninstallSuccessfull = true;
                }

                return uninstallSuccessfull;
            }
            catch (System.ComponentModel.Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Uninsatll a package using winget
        /// </summary>
        /// <param name="package">The id or name of the package that should be uninstalled</param>
        /// <returns>
        /// A Task object of the uninstall task
        /// </returns>
        public async Task<bool> UninstallPackageAsync(string package)
        {
            Task<bool> install = Task.Run(() => UninstallPackage(package));
            await install;
            return install.Result;
        }

        /// <summary>
        /// Uninsatll a package using winget
        /// </summary>
        /// <param name="package">The package that should be uninstalled</param>
        /// <returns>
        /// A Task object of the uninstall task
        /// </returns>
        public async Task<bool> UninstallPackageAsync(WinGetPackage package)
        {
            Task<bool> install = Task.Run(() => UninstallPackage(package));
            await install;
            return install.Result;
        }

    }
}
