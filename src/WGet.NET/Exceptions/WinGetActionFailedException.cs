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
        public override string Message {
            get
            {
                if (string.IsNullOrWhiteSpace(_action))
                {
                    return $"{_message} {_appendix}";
                }

                return $"{_message} (Action: winget {_action}) {_appendix}";
            }
        }

        private const string _appendix = "This might be due to an internal bug in ‘WGet.NET’. Please feel free to open a issue at ‘https://github.com/basicx-StrgV/WGet.NET’.";

        private readonly string _message = "The WinGet action failed.";
        private readonly string _action = string.Empty;

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
            _message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetActionFailedException"/> class.
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="action">The winget action that was executed</param>
        public WinGetActionFailedException(string message, string action) : base(message)
        {
            _message = message;
            _action = action;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetActionFailedException"/> class.
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="innerException">The inner exception</param>
        public WinGetActionFailedException(string message, Exception innerException) : base(message, innerException)
        {
            _message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetActionFailedException"/> class.
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="action">The winget action that was executed</param>
        /// <param name="innerException">The inner exception</param>
        public WinGetActionFailedException(string message, string action, Exception innerException) : base(message, innerException)
        {
            _message = message;
            _action = action;
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
