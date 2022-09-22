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
        /// <param name="processName">
        /// The name of the process to execute.
        /// </param>
        public ProcessManager(string processName)
        {
            _winGetStartInfo = new ProcessStartInfo()
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = processName,
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

            return RunProcess();
        }

        /// <summary>
        /// Runs a process with the current start informations.
        /// </summary>
        /// <returns>
        /// A <see cref="WGetNET.ProcessResult"/> object, 
        /// containing the output an exit id of the process.
        /// </returns>
        private ProcessResult RunProcess()
        {
            ProcessResult result = new ProcessResult();

            //Create and run process
            using (Process proc = new Process { StartInfo = _winGetStartInfo })
            {
                proc.Start();

                result.Output = ReadSreamOutput(proc.StandardOutput);

                //Wait till end and get exit code
                proc.WaitForExit();
                result.ExitCode = proc.ExitCode;
            }

            return result;
        }

        /// <summary>
        /// Reads the data from the process output to a string array.
        /// </summary>
        /// <param name="output">
        /// The <see cref="System.IO.StreamReader"/> with the process output.
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/> array 
        /// containing the process output stream content by lines.
        /// </returns>
        private string[] ReadSreamOutput(StreamReader output)
        {
            string[] outputArray = new string[0];

            //Read output to list
            while (!output.EndOfStream)
            {
                string? outputLine = output.ReadLine();
                if (outputLine is null)
                {
                    continue;
                }

                outputArray = ArrayManager.Add(outputArray, outputLine);
            }

            return outputArray;
        }
    }
}
