//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;

namespace WGetNET
{
    /// <summary>
    /// Represents a winget package.
    /// </summary>
    public interface IWinGetPackage : IWinGetObject
    {
        /// <summary>
        /// Gets the name of the package.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the id of the package.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the version of the package.
        /// </summary>
        public string VersionString { get; }

        /// <summary>
        /// Gets the version of the package.
        /// </summary>
        public Version Version { get; }

        /// <summary>
        /// Gets the newest available version of the package.
        /// </summary>
        public string AvailableVersionString { get; }

        /// <summary>
        /// Gets the newest available version of the package.
        /// </summary>
        public Version AvailableVersion { get; }

        /// <summary>
        /// Gets the source name for the package.
        /// </summary>
        public string SourceName { get; }

        /// <summary>
        /// Gets if id of the package is shortened.
        /// </summary>
        public bool HasShortenedId { get; }

        /// <summary>
        /// Gets if the package does not provide an id.
        /// </summary>
        /// <remarks>
        /// If this is true somthing whent wrong in the creation of the package.
        /// The name of the package will be used for all actions performd with this package.
        /// </remarks>
        public bool HasNoId { get; }
    }
}
