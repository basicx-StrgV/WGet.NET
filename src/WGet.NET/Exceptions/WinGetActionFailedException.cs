//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;

namespace WGetNET
{
    /// <summary>
    /// Exception that gets thrown if a winget action failed.
    /// </summary>
    public class WinGetActionFailedException : Exception
    {
        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public override string Message { get; } = "The WinGet action failed.";

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetActionFailedException"/> class.
        /// </summary>
        public WinGetActionFailedException()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetActionFailedException"/> class.
        /// </summary>
        /// <param name="message">Message of the exception</param>
        public WinGetActionFailedException(string message) : base(message)
        {
            Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetActionFailedException"/> class.
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="innerException">The inner exception</param>
        public WinGetActionFailedException(string message, Exception innerException) : base(message, innerException)
        {
            Message = message;
        }
    }
}
