# WGet.NET

[![Version](https://img.shields.io/github/v/release/basicx-StrgV/WGet.NET?label=Version)](https://github.com/basicx-StrgV/WGet.NET/releases)
[![NuGet](https://img.shields.io/nuget/dt/WGet.NET?label=NuGet%20Downloads)](https://www.nuget.org/packages/WGet.NET/)
[![License](https://img.shields.io/github/license/basicx-strgv/WGet.NET)](https://github.com/basicx-StrgV/WGet.NET/blob/main/LICENSE)
[![Issues](https://img.shields.io/github/issues/basicx-StrgV/WGet.NET)](https://github.com/basicx-StrgV/WGet.NET/issues)

## ❓ What is WGet.NET

WGet.NET is a WinGet wrapper library for **.Net** that allows you to easily install, update, uninstall and more, with the help of **WinGet**, from your application. 

![WGet.NET Icon](https://raw.githubusercontent.com/basicx-StrgV/WGet.NET/main/repo_design/WGet.NET_Icon_Small.png) 

I created it for another project and then made a library out of it in the hope it might be useful to someone else too.  

**If you miss a feature or find a problem with the library, feel free to create an [issue](https://github.com/basicx-StrgV/WGet.NET/issues).**

## ✨ Version 4.0

Version 4.0 is intended as a “health” update for the library and therefore includes changes to the structure, classes and functions.  
This includes breaking changes to the usage of the library. 

***But fear not, no functionality is lost and all breaking changes are documented and provide information on how to change the implementation.***

You can find the full migration documentation [here](https://github.com/basicx-StrgV/WGet.NET/blob/develop/migration_info/V3-V4.md).

This was done to improve maintainability and the experience of using the library, by improving the data structure, removing redundant or useless functions and making the implementation more consistent.

***More information about version 4.0 can be found [here](https://github.com/basicx-StrgV/WGet.NET/releases/tag/4.0.0).***

## 📓 Documentation

https://basicx-strgv.github.io/WGet.NET/

## ⚡ Features

**[Supported Framework Versions](https://www.nuget.org/packages/WGet.NET/#supportedframeworks-body-tab)**

- WinGet
  - [x] Get a list of installed packages
  - [x] Search installed packages
  - [x] Search packages
  - [x] Install packages
  - [x] Uninstall packages
  - [x] Upgrade packages
  - [x] Get a list of upgradeable packages
  - [x] Export and Import packages
  - [x] Check if WinGet is installed
  - [x] Get the WinGet version number
  - [x] Get installed sources
  - [x] Add sources
  - [x] Remove sources
  - [x] Update sources
  - [x] Export and Import sources
  - [x] Reset sources
  - [x] Calculate file hash
  - [x] Export Settings
  - [x] Download package installer
  - [x] Manage pinned packages
  - [x] Access info of the WinGet installation
- Other
  - [x] Asynchronous Execution

## 📦 NuGet Package

You can get the NuGet package here: https://www.nuget.org/packages/WGet.NET/

## ❗ Requirements

WinGet needs to be installed on the system.

If WinGet is not installed on your system you can get it here: https://github.com/microsoft/winget-cli

## 💡 Getting started 

The needed namespace is **WGetNET** (`using WGetNET;`).  
This namespace contains the three main classes that are used to perform actions or get information with winget, plus additional classes that are needed.

Exceptions are located in the `WGetNET.Exceptions` namespace.

### WinGetInfo:  
The ***WinGet*** class can be used to get information about WinGet itself.  
This class is inherited by the ***WinGetPackageManager*** and ***WinGetSourceManager*** class.

Using this class to check if winget is installed could look like this:
```csharp
WinGet winget = new WinGet();
if (winget.IsInstalled)
{
     Console.WriteLine("WinGet is installed.");
}
else
{
     Console.WriteLine("WinGet is NOT installed.");
}
```

### WinGetPackageManager:  
The ***WinGetPackageManager*** class is used for everything that has to do with packages.  
It can install, remove, upgrade, search, list, export and import packages.

The code for installing a package could look like this:
```csharp
WinGetPackageManager packageManager = new WinGetPackageManager();
packageManager.InstallPackage("Git.Git");
```

### WinGetSourceManager:  
The ***WinGetSourceManager*** class is used for everything that has to do with sources.  
It can list, add, update, export, reset and remove sources.

***To use the add, remove and reset functions, the process needs to have administrator privileges.  
(WinGet can’t perform these actions without administrator privileges)***

The code for adding a source could look like this:
```csharp
WinGetSourceManager sourceManager = new WinGetSourceManager();
sourceManager.AddSource("msstore", "https://storeedgefd.dsx.mp.microsoft.com/v9.0", "Microsoft.Rest");
```

### Find Latest Versions of a Package:

Using the ***WinGetPackageManager*** class you can use the `GetInstalledPackages` capability to get the latest version of a package and then retrieve the version number from the `AvailableVersionObject` property.

You would then be able to compare this to the current version of the package (`VersionObject` property) and determine if you need to notify users of an available upgrade.

```csharp
WinGetPackageManager packageManager = new WinGetPackageManager();
string packageId = "nkdAgility.AzureDevOpsMigrationTools";
WinGetPackage package = packageManager.GetInstalledPackages(packageId, true).FirstOrDefault();

if (package.AvailableVersion > package.Version)
{
    Console.WriteLine("You are currently running version {currentVersion} and a newer version ({latestVersion}) is available. You should update now using Winget command 'winget {packageId}' from the Windows Terminal.", package.Version, package.AvailableVersion, packageId);
}
```
