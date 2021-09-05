//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;

namespace WGetNET
{
    /// <summary>
    /// Exception that gets thrown if winget is not installed.
    /// </summary>
    public class WinGetNotInstalledException : Exception
    {
        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public override string Message { get; } = "WinGet is not installed on this system or could not be found.";

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetNotInstalledException"/> class.
        /// </summary>
        public WinGetNotInstalledException()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetNotInstalledException"/> class.
        /// </summary>
        /// <param name="message">Message of the exception</param>
        public WinGetNotInstalledException(string message) : base(message)
        {
            Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetNotInstalledException"/> class.
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="innerException">The inner exception</param>
        public WinGetNotInstalledException(string message, Exception innerException) : base(message, innerException)
        {
            Message = message;
        }
    }
}
