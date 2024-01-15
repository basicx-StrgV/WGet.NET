//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System.IO;
using Serilog;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;

namespace BuildTool
{
    internal class Build : NukeBuild
    {
        public static int Main() => Execute<Build>(x => x.Pack);

        [Parameter("Update the major version (Default: Minor version will be updated)")]
        public bool Major = false;

        [Parameter("Update the patch version (Default: Minor version will be updated)")]
        public bool Patch = false;

        [Parameter("Don't update the version (Default: Minor version will be updated)")]
        public bool KeepVersion = false;

        [Parameter("Provide a new version for the package")]
        public readonly string? Version = null;

        [Parameter("The path of the doxygen bin directory (Default: C:\\Program Files\\doxygen\\bin)")]
        public readonly string DoxygenBin = "C:\\Program Files\\doxygen\\bin";

        [Parameter("Sets that no docs should be created (Default: Automatically updating the version from the build config)")]
        public readonly bool NoDocs = false;

        [Solution]
        public readonly Solution? Solution;

        // Allways set config to release, because build is intended for creating ready to ship packages
        private readonly Configuration _configuration = Configuration.Release;

        private string? _workingVersion = null;

        private static AbsolutePath _packages => RootDirectory / "nuget" / "packages";
        private static AbsolutePath _doxygenWorkingDir => RootDirectory / "doxygen";
        private static AbsolutePath _doxyfile => _doxygenWorkingDir / "Doxyfile";
        private static AbsolutePath _srcDirectory => RootDirectory / "src";
        private static AbsolutePath _configFile => _srcDirectory / "build.config.json";

        Target LoadConfig => _ => _
            .Unlisted()
            .Before(Info, Restore, Compile, Pack)
            .Executes(() =>
            {
                Log.Information("Loading the build config");
                Log.Information("File: {configFile}", _configFile);

                // Check if config exist
                if (!File.Exists(_configFile))
                {
                    if (Version != null)
                    {
                        Log.Warning("Config file not found. The provided version ({version}) will still be used", Version);
                        _workingVersion = Version;
                    }
                    else
                    {
                        Log.Warning("Config file not found. The version \"1.0.0\" will be used as a fallback");
                        _workingVersion = "1.0.0";
                    }

                    // Proceed with next target
                    return;
                }

                // Try to load data from config
                ConfigHandler.BuildConfig? buildConfig = ConfigHandler.LoadConfig(_configFile);
                if (buildConfig == null)
                {
                    if (Version != null)
                    {
                        Log.Warning("Failed to load the config file. The provided version ({version}) will still be used", Version);
                        _workingVersion = Version;
                    }
                    else
                    {
                        Log.Warning("Failed to load the config file. The version \"1.0.0\" will be used as a fallback");
                        _workingVersion = "1.0.0";
                    }

                    // Proceed with next target
                    return;
                }

                // Try to parse the configured version
                bool parsingResult = System.Version.TryParse(buildConfig.CurrentVersion, out System.Version? version);
                if (version == null || !parsingResult)
                {
                    if (Version != null)
                    {
                        Log.Warning("The version provided by the config file ({configVersion}) is invalid. The provided version ({version}) will still be used", version, Version);
                        _workingVersion = Version;
                    }
                    else
                    {
                        Log.Warning("The version provided by the config file ({configVersion}) is invalid. The version \"1.0.0\" will be used as a fallback", version);
                        _workingVersion = "1.0.0";
                    }

                    // Proceed with next target
                    return;
                }

                if (KeepVersion)
                {
                    _workingVersion = version.ToString();
                    return;
                }

                if (Major && Patch)
                {
                    // Cancel the build process
                    Assert.Fail("Can't update major and patch at the same time");
                }

                if ((Major || Patch))
                {
                    if (Major)
                    {
                        _workingVersion =
                            new System.Version(version.Major + 1, 0, 0)
                            .ToString();
                    }
                    else
                    {
                        _workingVersion =
                            new System.Version(version.Major, version.Minor, version.Build + 1)
                            .ToString();
                    }

                    return;
                }

                _workingVersion =
                    new System.Version(version.Major, version.Minor + 1, version.Build)
                    .ToString();
            });

        Target Info => _ => _
            .Unlisted()
            .Before(Restore, Compile, Pack)
            .Executes(() =>
            {
                Log.Information("Creating \"WGet.NET\" nuget package");
                Log.Information("Version: {version}", _workingVersion);
                Log.Information("Output directory: {dir}", _packages);
            });

        Target Restore => _ => _
            .Unlisted()
            .Executes(() =>
            {
                if (Solution == null)
                {
                    Assert.Fail("The solution was not loaded correctly");
                    return; // Normaly not needed but the compiler does not know that.
                }

                DotNetTasks.DotNetRestore(s => s
                    .SetProjectFile(Solution.GetProject("WGet.NET")));
            });

        Target Compile => _ => _
            .DependsOn(LoadConfig, Restore)
            .Executes(() =>
            {
                if (Solution == null)
                {
                    Assert.Fail("The solution was not loaded correctly");
                    return; // Normaly not needed but the compiler does not know that.
                }

                DotNetTasks.DotNetBuild(s => s
                    .SetProjectFile(Solution.GetProject("WGet.NET"))
                    .SetConfiguration(_configuration)
                    .EnableNoRestore()
                    .SetVersion(_workingVersion)
                    .SetFileVersion(_workingVersion)
                    .SetAssemblyVersion(_workingVersion));
            });

        Target Pack => _ => _
            .DependsOn(Info, Compile)
            .Triggers(Docs, UpdateConfig)
            .Executes(() =>
            {
                if (Solution == null)
                {
                    Assert.Fail("The solution was not loaded correctly");
                    return; // Normaly not needed but the compiler does not know that.
                }

                DotNetTasks.DotNetPack(s => s
                    .SetProject(Solution.GetProject("WGet.NET"))
                    .SetConfiguration(_configuration)
                    .SetNoBuild(true)
                    .SetOutputDirectory(_packages)
                    .SetVersion(_workingVersion)
                    .SetFileVersion(_workingVersion)
                    .SetAssemblyVersion(_workingVersion)
                    .SetProperty("PackageReleaseNotes", $"https://github.com/basicx-StrgV/WGet.NET/releases/tag/{_workingVersion}"));
            });

        Target Docs => _ => _
        .Unlisted()
        .OnlyWhenDynamic(() => !NoDocs)
        .ProceedAfterFailure()
        .Executes(() =>
        {
            if (_workingVersion == null)
            {
                Assert.Fail($"No version for the package is defined");
                return; // Normaly not needed but the compiler does not know that.
            }

            if (!Directory.Exists(DoxygenBin))
            {
                Assert.Fail($"The doxygen bin directory does not exist ({DoxygenBin})");
            }

            if (!Directory.Exists(_doxygenWorkingDir))
            {
                Assert.Fail($"The doxygen process working directory does not exist ({_doxygenWorkingDir})");
            }

            string doxygenExe = DoxygenBin + Path.DirectorySeparatorChar + "doxygen.exe";

            if (!File.Exists(doxygenExe))
            {
                Assert.Fail($"The doxygen executable does not exist ({doxygenExe})");
            }

            if (!File.Exists(_doxyfile))
            {
                Assert.Fail($"The doxyfile does not exist ({_doxyfile})");
            }

            DoxygenHandler doxygen = new(_doxyfile, doxygenExe, _doxygenWorkingDir);

            bool result = doxygen.GenerateDocs(_workingVersion);

            if (!result)
            {
                Assert.Fail("Failed to generate docs");
            }
        });

        Target UpdateConfig => _ => _
            .Unlisted()
            .After(Pack, Docs)
            .TriggeredBy(Pack)
            .ProceedAfterFailure()
            .Executes(() =>
            {
                Log.Information("Updating the build config");
                Log.Information("File: {configFile}", _configFile);

                if (!File.Exists(_configFile))
                {
                    Log.Information("The config file does not exist, it will be created");
                }

                bool saveResult = ConfigHandler.SaveConfig(
                    _configFile,
                    new ConfigHandler.BuildConfig()
                    {
                        CurrentVersion = _workingVersion
                    });

                if (!saveResult)
                {
                    Assert.Fail("Failed to update the build config");
                }
            });
    }
}
