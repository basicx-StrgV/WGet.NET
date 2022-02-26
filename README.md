# WGet.NET

[![Version](https://img.shields.io/github/v/release/basicx-StrgV/WGet.NET?label=Version)](https://github.com/basicx-StrgV/WGet.NET/releases)
[![NuGet](https://img.shields.io/nuget/dt/WGet.NET?label=NuGet%20Downloads)](https://www.nuget.org/packages/WGet.NET/)

| Main    | Develop |
| ------- | ------- |
[![CodeQL_main](https://github.com/basicx-StrgV/WGet.NET/actions/workflows/codeql-analysis.yml/badge.svg?branch=main)](https://github.com/basicx-StrgV/WGet.NET/actions/workflows/codeql-analysis.yml) [![BCH compliance](https://bettercodehub.com/edge/badge/basicx-StrgV/WGet.NET?branch=main)](https://bettercodehub.com/) |  [![CodeQL_develop](https://github.com/basicx-StrgV/WGet.NET/actions/workflows/codeql-analysis.yml/badge.svg?branch=develop)](https://github.com/basicx-StrgV/WGet.NET/actions/workflows/codeql-analysis.yml) |

## ❓ What is WGet.NET

WGet.NET is a WinGet wrapper library for .Net.

I created it for another project and then made a library out of it in hope it might be usefull to someone else too.<br>
Therefore the functions are limited at the moment.<br>
**If you tried this library, please give me some feedback. And if you miss a feature, you can create an issue to let me know.**

## 🎈 Version 2.0

Version 2.0 is out now, with big changes, improvements and new features.  
But before updating to the new version, read the changelog first, because version 2.0 is not compatible with older versions of this library.

You can finde the changelog here: https://github.com/basicx-StrgV/WGet.NET/releases/tag/2.0.0

## ⚡ Features

- Get a list of installed packages
- Search packages
- Install packages
- Uninstall packages
- Upgrade packages
- Get a list of upgradeable packages
- Export and Import packages
- Check if WinGet is installed
- Get the WinGet version number
- Get installed sources
- Add sources
- Remove sources
- Update sources
- Export sources
- Reset sources

## 📦 NuGet Package

You can get the NuGet package here: https://www.nuget.org/packages/WGet.NET/

## ❗ Requirements

WinGet needs to be installed on the system.

If WinGet is not installed on your system you can get it here: https://github.com/microsoft/winget-cli

## 📍 Informations

### Namespaces

- WGetNET

### Classes

- WinGetInfo
- WinGetPackageManager
- WinGetPackage
- WinGetSourceManager
- WinGetSource
- WinGetNotInstalledException
- WinGetActionFailedException
