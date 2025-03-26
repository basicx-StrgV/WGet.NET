//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.IO;
using System.Net.Sockets;
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
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// </exception>
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
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// </exception>
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
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// </exception>
        private ProcessResult RunProcess(string cmd)
        {
            ProcessResult result = new();

            if (_sshClient == null)
            {
                return result;
            }

            try
            {
                // Connect client and create the command
                _sshClient.Connect();

                SshCommand command = _sshClient.CreateCommand(cmd);

                // Create a stream reader with the command stream,
                // start command execution and start the reading of the stream.
                StreamReader outputStream = new StreamReader(command.OutputStream);

                IAsyncResult cmdResult = command.BeginExecute();

                result.Output = outputStream.ReadSreamOutputByLine();

                // If the command execution is not completed, wait for complition
                if (!cmdResult.IsCompleted)
                {
                    cmdResult.AsyncWaitHandle.WaitOne();
                    cmdResult.AsyncWaitHandle.Dispose();
                }

                // Get the exite code of the executed command
                if (command.ExitStatus.HasValue)
                {
                    result.ExitCode = command.ExitStatus.Value;
                }

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
            catch (SocketException)
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
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">
        /// The SSH connection was terminated.
        /// </exception>
        /// <exception cref="Renci.SshNet.Common.SshAuthenticationException">
        /// SSH authentication failed.
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// Failed to connect to SSH server.
        /// </exception>
        private async Task<ProcessResult> RunProcessAsync(string cmd, CancellationToken cancellationToken = default)
        {
            ProcessResult result = new();

            if (_sshClient == null)
            {
                return result;
            }

            try
            {
                // Connect client and create the command
                await _sshClient.ConnectAsync(cancellationToken);

                SshCommand command = _sshClient.CreateCommand(cmd);

                // Create a stream reader with the command stream,
                // start command execution and start the reading of the stream.
                StreamReader outputStream = new StreamReader(command.OutputStream);

                Task cmdTask = command.ExecuteAsync(cancellationToken);

                result.Output = await outputStream.ReadSreamOutputByLineAsync(cancellationToken);

                await cmdTask;

                // Stop if the action was canceled
                if (cancellationToken.IsCancellationRequested)
                {
                    command.Dispose();
                    _sshClient.Disconnect();

                    result.ExitCode = -1;

                    return result;
                }

                // Get the exite code of the executed command
                if (command.ExitStatus.HasValue)
                {
                    result.ExitCode = command.ExitStatus.Value;
                }

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
            catch (SocketException)
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
