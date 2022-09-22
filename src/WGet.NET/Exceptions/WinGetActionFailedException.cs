//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.Runtime.Serialization;

namespace WGetNET
{
    /// <summary>
    /// Exception that gets thrown if a winget action failed.
    /// </summary>
    [Serializable]
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

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetActionFailedException"/> class with serialized data.
        /// </summary>
        /// <param name="info">
        /// The <see cref="System.Runtime.Serialization.SerializationInfo"/> 
        /// that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="System.Runtime.Serialization.StreamingContext"/> 
        /// that contains contextual information about the source or destination.
        /// </param>
        protected WinGetActionFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
