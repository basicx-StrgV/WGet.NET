//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WGetNET.Extensions;
using WGetNET.Models;

namespace WGetNET.Components.Internal
{
    /// <summary>
    /// The <see langword="internal"/> class <see cref="SshProcessManager"/> 
    /// provides the winget process execution.
    /// </summary>
    internal class SshProcessManager : IProcessManager
    {
        private readonly SshClient? _sshClient = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SshProcessManager"/> class.
        /// </summary>
        /// <param name="sshClient">
        /// The <see cref="Renci.SshNet.SshClient"/> object.
        /// </param>
        public SshProcessManager(SshClient sshClient)
        {
            _sshClient = sshClient;
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
            cmd = $"winget {cmd}";
            return RunProcess(cmd);
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
            cmd = $"winget {cmd}";
            return await RunProcessAsync(cmd, cancellationToken);
        }

        /// <summary>
        /// Runs a process with the current start informations.
        /// </summary>
        /// <param name="cmd">
        /// A <see cref="System.String"/> containing the cmd that should be executed over ssh.
        /// </param>
        /// <returns>
        /// A <see cref="WGetNET.Models.ProcessResult"/> object, 
        /// containing the output an exit id of the process.
        /// </returns>
        private ProcessResult RunProcess(string cmd)
        {
            ProcessResult result = new();

            if (_sshClient == null)
            {
                return result;
            }

            try
            {
                _sshClient.Connect();

                SshCommand command = _sshClient.CreateCommand(cmd);

                command.Execute();
                StreamReader outputStream = new StreamReader(command.OutputStream);

                if (command.ExitStatus.HasValue)
                {
                    result.ExitCode = command.ExitStatus.Value;
                }

                result.Output = outputStream.ReadSreamOutputByLine();

                command.Dispose();
            }
            catch (SshConnectionException)
            {
                throw;
            }
            catch (SshAuthenticationException)
            {
                throw;
            }
            catch (Exception)
            {
                return result;
            }
            finally
            {
                _sshClient.Disconnect();
            }

            return result;
        }

        /// <summary>
        /// Asynchronous runs a process with the current start informations.
        /// </summary>
        /// <param name="cmd">
        /// A <see cref="System.String"/> containing the cmd that should be executed over ssh.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="WGetNET.Models.ProcessResult"/> object, 
        /// containing the output an exit id of the process.
        /// </returns>
        private async Task<ProcessResult> RunProcessAsync(string cmd, CancellationToken cancellationToken = default)
        {
            ProcessResult result = new();

            if (_sshClient == null)
            {
                return result;
            }

            try
            {
                _sshClient.Connect();

                SshCommand command = _sshClient.CreateCommand(cmd);

                await command.ExecuteAsync(cancellationToken);
                StreamReader outputStream = new StreamReader(command.OutputStream);

                if (cancellationToken.IsCancellationRequested)
                {
                    command.Dispose();

                    result.ExitCode = -1;

                    return result;
                }

                if (command.ExitStatus.HasValue)
                {
                    result.ExitCode = command.ExitStatus.Value;
                }

                result.Output = await outputStream.ReadSreamOutputByLineAsync(cancellationToken);

                command.Dispose();
            }
            catch (SshConnectionException)
            {
                throw;
            }
            catch (SshAuthenticationException)
            {
                throw;
            }
            catch (Exception)
            {
                return result;
            }
            finally
            {
                _sshClient.Disconnect();
            }

            return result;
        }
    }
}
