//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System.IO;
using System.Diagnostics;
using WGetNET.HelperClasses;

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
        public ProcessManager()
        {
            _winGetStartInfo = new ProcessStartInfo()
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "winget",
                RedirectStandardOutput = true
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
            string[] output = new string[0];

            int exitCode = -1;

            //Create and run process
            using (Process proc = new Process { StartInfo = _winGetStartInfo })
            {
                proc.Start();
                
                //Read output to list
                using StreamReader procOutputStream = proc.StandardOutput;
                while (!procOutputStream.EndOfStream)
                {
                    output = ArrayManager.Add(output, procOutputStream.ReadLine());
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
