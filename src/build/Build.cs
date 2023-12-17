using System.IO;
using Serilog;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;

namespace BuildTool
{
    class Build : NukeBuild
    {
        public static int Main() => Execute<Build>(x => x.Pack);

        [Parameter("Version of the package")]
        public readonly string Version = "0.0.0";

        [Parameter("The path of the doxygen bin directory (Default: C:\\Program Files\\doxygen\\bin)")]
        public readonly string DoxygenBin = "C:\\Program Files\\doxygen\\bin";

        [Parameter("Sets that no docs should be created")]
        public readonly bool NoDocs = false;

        [Solution]
        public readonly Solution Solution;

        // Allways set config to release, because build is intended for creating ready to ship packages
        private readonly Configuration _configuration = Configuration.Release;

        private static AbsolutePath _packages => RootDirectory / "nuget" / "packages";
        private static AbsolutePath _doxygenWorkingDir => RootDirectory / "doxygen";
        private static AbsolutePath _doxyfile => _doxygenWorkingDir / "Doxyfile";

        Target Info => _ => _
            .Unlisted()
            .Before(Restore)
            .Before(Compile)
            .Before(Pack)
            .Executes(() =>
            {
                Log.Information("Creating \"WGet.NET\" nuget package");
                Log.Information("Version: {version}", Version);
                Log.Information("Output directory: {dir}", _packages);
            });

        Target Restore => _ => _
            .Unlisted()
            .Executes(() =>
            {
                DotNetTasks.DotNetRestore(s => s
                    .SetProjectFile(Solution.GetProject("WGet.NET")));
            });

        Target Compile => _ => _
            .DependsOn(Restore)
            .Executes(() =>
            {
                DotNetTasks.DotNetBuild(s => s
                    .SetProjectFile(Solution.GetProject("WGet.NET"))
                    .SetConfiguration(_configuration)
                    .EnableNoRestore());
            });

        Target Pack => _ => _
            .DependsOn(Info)
            .DependsOn(Compile)
            .Triggers(Docs)
            .Executes(() =>
            {
                DotNetTasks.DotNetPack(s => s
                    .SetProject(Solution.GetProject("WGet.NET"))
                    .SetConfiguration(_configuration)
                    .SetNoBuild(true)
                    .SetOutputDirectory(_packages)
                    .SetVersion(Version)
                    .SetFileVersion(Version)
                    .SetAssemblyVersion(Version)
                    .SetProperty("PackageReleaseNotes", $"https://github.com/basicx-StrgV/WGet.NET/releases/tag/{Version}"));
            });

        Target Docs => _ => _
        .OnlyWhenDynamic(() => !NoDocs)
        .Executes(() =>
        {
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

            bool result = doxygen.GenerateDocs(Version);

            if (!result)
            {
                Assert.Fail("Failed to generate docs");
            }
        });
    }
}
