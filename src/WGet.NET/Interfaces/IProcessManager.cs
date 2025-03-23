//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System.Threading;
using System.Threading.Tasks;
using WGetNET.Models;

namespace WGetNET
{
    /// <summary>
    /// Interface for winget process managers.
    /// </summary>
    internal interface IProcessManager
    {
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
        public ProcessResult ExecuteWingetProcess(string cmd);

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
        public Task<ProcessResult> ExecuteWingetProcessAsync(string cmd, CancellationToken cancellationToken = default);
    }
}
