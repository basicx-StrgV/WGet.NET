using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Serilog;
//using System.Collections.Generic;

namespace BuildTool
{

    class Build : NukeBuild
    {
        public static int Main() => Execute<Build>(x => x.Pack);

        [Parameter("Version of the package")]
        public readonly string Version = "0.0.0";

        [Solution]
        public readonly Solution Solution;

        // Allways set config to release, because build is intended for creating ready to ship packages
        private readonly Configuration _configuration = Configuration.Release;

        private static AbsolutePath _packages => RootDirectory / "nuget" / "packages";
        private static AbsolutePath _sourceDirectory => RootDirectory / "src";

        Target Info => _ => _
            .Unlisted()
            //.Before(Clean)
            .Before(Restore)
            .Before(Compile)
            .Before(Pack)
            .Executes(() =>
            {
                Log.Information("Creating \"WGet.NET\" nuget package");
                Log.Information("Version: {version}", Version);
                Log.Information("Output directory: {dir}", _packages);
            });

        /*Target Clean => _ => _
            .Unlisted()
            .Before(Restore)
            .Executes(() =>
            {*/
        //IReadOnlyCollection<AbsolutePath> directories = _sourceDirectory.GlobDirectories("**/bin", "**/obj");

        /*foreach (AbsolutePath directory in directories)
        {
            Log.Information($"{directory}");
            directory.DeleteDirectory();
        }
    });*/

        Target Restore => _ => _
            .Unlisted()
            .Executes(() =>
            {
                DotNetTasks.DotNetRestore(s => s
                    .SetProjectFile(Solution.GetProject("WGet.NET")));
            });

        Target Compile => _ => _
            //.DependsOn(Clean)
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
            .Executes(() =>
            {
                DotNetTasks.DotNetPack(s => s
                    .SetProject(Solution.GetProject("WGet.NET"))
                    .SetOutputDirectory(_packages)
                    .SetVersion(Version)
                    .SetFileVersion(Version)
                    .SetAssemblyVersion(Version)
                    .SetProperty("PackageReleaseNotes", "https://github.com/basicx-StrgV/WGet.NET/releases/tag/" + Version));
            });
    }
}
