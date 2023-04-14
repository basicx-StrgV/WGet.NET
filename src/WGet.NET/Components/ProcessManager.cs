//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System.IO;
using System.Diagnostics;
using WGetNET.HelperClasses;
using System.Text;

namespace WGetNET
{
    /// <summary>
    /// The <see langword="internal"/> class <see cref="WGetNET.ProcessManager"/> 
    /// provides the winget process execution.
    /// </summary>
    internal class ProcessManager
    {
        private readonly ProcessStartInfo _winGetStartInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.ProcessManager"/> class.
        /// </summary>
        /// <param name="processName">
        /// The name of the process to execute.
        /// </param>
        public ProcessManager(string processName)
        {
            _winGetStartInfo = new ProcessStartInfo()
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = processName,
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.Default,
            };
        }
    
        /// <summary>
        /// Executes a winget process with the given command and returns the result.
        /// </summary>
        /// <param name="cmd">
        /// A <see cref="System.String"/> representing the command that winget should be executed with.
        /// </param>
        /// <returns>
        /// A <see cref="WGetNET.ProcessResult"/> object, 
        /// containing the output an exit id of the process.
        /// </returns>
        public ProcessResult ExecuteWingetProcess(string cmd)
        {
            //Set Arguments
            _winGetStartInfo.Arguments = cmd;

            //Output List
            var output = new string[0];
            var exitCode = -1;

            //Create and run process
            using (var proc = new Process { StartInfo = _winGetStartInfo })
            {
                _ = proc.Start();
                
                //Read output to list
                using var procOutputStream = proc.StandardOutput;
                while (!procOutputStream.EndOfStream)
                {
                    var outputLine = procOutputStream.ReadLine();
                    if (outputLine is null)
                    {
                        continue;
                    }

                    output = ArrayManager.Add(output, outputLine);
                }

                //Wait till end and get exit code
                proc.WaitForExit();
                exitCode = proc.ExitCode;
            }

            return new ProcessResult()
            {
                ExitCode = exitCode,
                Output = output
            };
        }
    }
}
