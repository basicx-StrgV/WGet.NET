//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using Serilog;
using System;
using System.IO;
using System.Diagnostics;

namespace BuildTool
{
    internal class DoxygenHandler
    {
        private readonly string _doxyfile;
        private readonly string _doxygen;
        private readonly string _workingDir;

        public DoxygenHandler(string doxyfile, string doxygen, string workingDir)
        {
            _doxyfile = doxyfile;
            _doxygen = doxygen;
            _workingDir = workingDir;
        }

        public bool GenerateDocs(string projectVersion)
        {
            try
            {
                Log.Information("Updating doxyfile");

                if (!UpdateDoxyfile(projectVersion))
                {
                    Log.Error("Failed to update the doxyfile");
                    return false;
                }

                Log.Information("Executing doxygen");

                int exitCode = ExecuteDoxygen();

                if (exitCode != 0)
                {
                    Log.Error("The doxygen process failed with the exit code \"{exitCode}\"", exitCode);
                    return false;
                }
            }
            catch (Exception e)
            {
                Log.Error("Exception thrown on docs generation: {ex}", e);
                return false;
            }

            return true;
        }

        private bool UpdateDoxyfile(string projectVersion)
        {
            string[] doxyfileContent = File.ReadAllLines(_doxyfile);

            for (int i = 0; i < doxyfileContent.Length; i++)
            {
                if (doxyfileContent[i].Trim().StartsWith("PROJECT_NUMBER"))
                {
                    doxyfileContent[i] = $"PROJECT_NUMBER = {projectVersion}";

                    File.WriteAllLines(_doxyfile, doxyfileContent);

                    return true;
                }
            }

            return false;
        }

        private int ExecuteDoxygen()
        {
            ProcessStartInfo startInfo = new()
            {
                FileName = _doxygen,
                Arguments = $"\"{_doxyfile}\"",
                WorkingDirectory = _workingDir,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardOutput = true,
            };

            Process doxygenProc = new()
            {
                StartInfo = startInfo
            };

            doxygenProc.Start();

            using (StreamReader output = doxygenProc.StandardOutput)
            {
                // Log the doxygen output
                while (!output.EndOfStream)
                {
                    string? line = output.ReadLine();

                    if (line == null)
                    {
                        continue;
                    }

                    Log.Information(line);
                }
            }

            // Wait for process exit with a timeout of 10 min
            doxygenProc.WaitForExit(600000);

            if (!doxygenProc.HasExited)
            {
                // Kill the process if it has not exited at this point
                doxygenProc.Kill();
            }

            return doxygenProc.ExitCode;
        }
    }
}
