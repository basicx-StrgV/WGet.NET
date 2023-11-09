//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.Runtime.Serialization;

namespace WGetNET.Exceptions
{
    /// <summary>
    /// Exception that gets thrown if a winget feature is not supportet in the installed winget version.
    /// </summary>
    [Serializable]
    public class WinGetFeatureNotSupportedException : Exception
    {
        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public override string Message 
        { 
            get 
            {
                if (_minVersion == null)
                {
                    return "This feature is not supported in the installed WinGet version.";
                }

                return $"This feature is not supported in the installed WinGet version. WinGet {_minVersion} or higher is needed to use this feature.";
            } 
        
        }

        private readonly Version? _minVersion;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException"/> class.
        /// </summary>
        /// <param name="minVersion">Min WinGet version needed for the feature</param>
        public WinGetFeatureNotSupportedException(Version minVersion)
        {
            _minVersion = minVersion;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException"/> class.
        /// </summary>
        /// <param name="minVersion">Min WinGet version needed for the feature</param>
        /// <param name="innerException">The inner exception</param>
        public WinGetFeatureNotSupportedException(Version minVersion, Exception innerException) : base(null, innerException)
        {
            _minVersion = minVersion;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.Exceptions.WinGetFeatureNotSupportedException"/> class with serialized data.
        /// </summary>
        /// <param name="info">
        /// The <see cref="System.Runtime.Serialization.SerializationInfo"/> 
        /// that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="System.Runtime.Serialization.StreamingContext"/> 
        /// that contains contextual information about the source or destination.
        /// </param>
        protected WinGetFeatureNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
