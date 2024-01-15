//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.Runtime.Serialization;

namespace WGetNET.Exceptions
{
    /// <summary>
    /// Exception that gets thrown if the provided json string could not be deserialized.
    /// </summary>
    [Serializable]
    public class InvalidJsonException : Exception
    {
        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public override string Message { get; } = "The provided JSON could not be deserialized.";

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.Exceptions.InvalidJsonException"/> class.
        /// </summary>
        public InvalidJsonException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.Exceptions.InvalidJsonException"/> class.
        /// </summary>
        /// <param name="message">Message of the exception</param>
        public InvalidJsonException(string message) : base(message)
        {
            Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.Exceptions.InvalidJsonException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception</param>
        public InvalidJsonException(Exception innerException) : base(string.Empty, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.Exceptions.InvalidJsonException"/> class.
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="innerException">The inner exception</param>
        public InvalidJsonException(string message, Exception innerException) : base(message, innerException)
        {
            Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.Exceptions.InvalidJsonException"/> class with serialized data.
        /// </summary>
        /// <param name="info">
        /// The <see cref="System.Runtime.Serialization.SerializationInfo"/> 
        /// that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="System.Runtime.Serialization.StreamingContext"/> 
        /// that contains contextual information about the source or destination.
        /// </param>
        protected InvalidJsonException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
