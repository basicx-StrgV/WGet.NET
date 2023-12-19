//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using WGetNET.Models;
using WGetNET.Extensions;

namespace WGetNET.Components.Internal
{
    /// <summary>
    /// The <see langword="internal"/> class <see cref="ProcessManager"/> 
    /// provides the winget process execution.
    /// </summary>
    internal class ProcessManager
    {
        private readonly ProcessStartInfo _winGetStartInfoTemplate;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessManager"/> class.
        /// </summary>
        /// <param name="processName">
        /// The name of the process to execute.
        /// </param>
        public ProcessManager(string processName)
        {
            _winGetStartInfoTemplate = new ProcessStartInfo()
            {
                CreateNoWindow = true,
                FileName = processName,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.UTF8
            };
        }

        /// <summary>
        /// Executes a winget process with the given command and returns the result.
        /// </summary>
        /// <param name="cmd">
        /// A <see cref="string"/> representing the command that winget should be executed with.
        /// </param>
        /// <returns>
        /// A <see cref="ProcessResult"/> object, 
        /// containing the output an exit id of the process.
        /// </returns>
        public ProcessResult ExecuteWingetProcess(string cmd)
        {
            return RunProcess(GetStartInfo(cmd));
        }

        /// <summary>
        /// Asynchronous executes a winget process with the given command and returns the result.
        /// </summary>
        /// <param name="cmd">
        /// A <see cref="string"/> representing the command that winget should be executed with.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="ProcessResult"/> object, 
        /// containing the output an exit id of the process.
        /// </returns>
        public async Task<ProcessResult> ExecuteWingetProcessAsync(string cmd, CancellationToken cancellationToken = default)
        {
            return await RunProcessAsync(GetStartInfo(cmd), cancellationToken);
        }

        /// <summary>
        /// Gets the start info for a process.
        /// </summary>
        /// <param name="cmd">
        /// String containig the arguments for the action.
        /// </param>
        /// <returns>
        /// A <see cref="ProcessStartInfo"/> object, for the process.
        /// </returns>
        private ProcessStartInfo GetStartInfo(string cmd)
        {
            return new ProcessStartInfo()
            {
                CreateNoWindow = _winGetStartInfoTemplate.CreateNoWindow,
                FileName = _winGetStartInfoTemplate.FileName,
                RedirectStandardOutput = _winGetStartInfoTemplate.RedirectStandardOutput,
                StandardOutputEncoding = _winGetStartInfoTemplate.StandardOutputEncoding,
                UseShellExecute = _winGetStartInfoTemplate.UseShellExecute,
                WindowStyle = _winGetStartInfoTemplate.WindowStyle,
                Arguments = cmd
            };
        }

        /// <summary>
        /// Runs a process with the current start informations.
        /// </summary>
        /// <param name="processStartInfo">
        /// The <see cref="System.Diagnostics.ProcessStartInfo"/> for process that should be executed.
        /// </param>
        /// <returns>
        /// A <see cref="WGetNET.Models.ProcessResult"/> object, 
        /// containing the output an exit id of the process.
        /// </returns>
        private ProcessResult RunProcess(ProcessStartInfo processStartInfo)
        {
            ProcessResult result = new();

            //Create and run process
            using (Process proc = new() { StartInfo = processStartInfo })
            {
                proc.Start();

                result.Output = ReadSreamOutput(proc.StandardOutput);

                //Wait till end and get exit code
                proc.WaitForExit();

                // Make sure the process has exited
                if (!proc.HasExited)
                {
                    proc.Kill();
                }

                result.ExitCode = proc.ExitCode;
            }

            return result;
        }

        /// <summary>
        /// Asynchronous runs a process with the current start informations.
        /// </summary>
        /// <param name="processStartInfo">
        /// The <see cref="System.Diagnostics.ProcessStartInfo"/> for process that should be executed.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="WGetNET.Models.ProcessResult"/> object, 
        /// containing the output an exit id of the process.
        /// </returns>
        private async Task<ProcessResult> RunProcessAsync(ProcessStartInfo processStartInfo, CancellationToken cancellationToken = default)
        {
            ProcessResult result = new();

            //Create and run process
            using (Process proc = new() { StartInfo = processStartInfo })
            {
                proc.Start();

                result.Output = await ReadSreamOutputAsync(proc.StandardOutput, cancellationToken);

                // Kill the process and return, if the task is canceled
                if (cancellationToken.IsCancellationRequested && !proc.HasExited)
                {
                    proc.Kill();

                    result.ExitCode = -1;

                    return result;
                }

                //Wait for the processs to exit
                proc.WaitForExit();

                // Make sure the process has exited
                if (!proc.HasExited)
                {
                    proc.Kill();
                }

                result.ExitCode = proc.ExitCode;
            }

            return result;
        }

        /// <summary>
        /// Reads the data from the process output to a string array.
        /// </summary>
        /// <param name="output">
        /// The <see cref="StreamReader"/> with the process output.
        /// </param>
        /// <returns>
        /// A <see cref="string"/> array 
        /// containing the process output stream content by lines.
        /// </returns>
        private string[] ReadSreamOutput(StreamReader output)
        {
            string[] outputArray = Array.Empty<string>();

            //Read output to list
            while (!output.EndOfStream)
            {
                string? outputLine = output.ReadLine();
                if (outputLine is null)
                {
                    continue;
                }

                outputArray = outputArray.Add(outputLine);
            }

            return outputArray;
        }

        /// <summary>
        /// Asynchronous reads the data from the process output to a string array.
        /// </summary>
        /// <param name="output">
        /// The <see cref="System.IO.StreamReader"/> with the process output.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="string"/> array 
        /// containing the process output stream content by lines.
        /// </returns>
        private async Task<string[]> ReadSreamOutputAsync(StreamReader output, CancellationToken cancellationToken = default)
        {
            string[] outputArray = Array.Empty<string>();

            //Read output to list
            while (!output.EndOfStream)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                string? outputLine = await output.ReadLineAsync();
                if (outputLine is null)
                {
                    continue;
                }

                outputArray = outputArray.Add(outputLine);
            }

            return outputArray;
        }
    }
}
