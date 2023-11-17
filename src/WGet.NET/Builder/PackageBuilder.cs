//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using WGetNET.Abstractions;
using WGetNET.HelperClasses;

namespace WGetNET.Builder
{
    /// <summary>
    /// Builder to create a new <see cref="WGetNET.WinGetPackage"/> instance.
    /// </summary>
    internal class PackageBuilder : WinGetObjectBuilder<WinGetPackage>
    {
        private string _name = string.Empty;
        private string _id = string.Empty;
        private string _versionString = string.Empty;
        private Version? _version = null;
        private string _availableVersionString = string.Empty;
        private Version? _availableVersion = null;
        private string _sourceName = string.Empty;
        private bool _hasShortenedId = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.Builder.PackageBuilder"/> class.
        /// </summary>
        public PackageBuilder()
        {
            // Provide empty constructor
        }

        /// <summary>
        /// Addss the name of the package.
        /// </summary>
        /// <param name="name">
        /// The name of the package.
        /// </param>
        public void AddName(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Addss the id of the package.
        /// </summary>
        /// <remarks>
        /// The id will be parsed accordingly if a shortened id is detected.
        /// </remarks>
        /// <param name="id">
        /// The id of the package.
        /// </param>
        public void AddId(string id)
        {
            _hasShortenedId = ProcessOutputReader.CheckShortenedValue(id);
            if (_hasShortenedId)
            {
                // Remove the char at the end of the shortened id.
                _id = id.Remove(id.Length - 1);
            }
            else
            {
                _id = id;
            }
        }

        /// <summary>
        /// Adds the current version of the package.
        /// </summary>
        /// <param name="version">
        /// The current version of the package contained in a <see cref="System.String"/>.
        /// </param>
        public void AddVersion(string version)
        {
            _versionString = version;
            _version = VersionParser.Parse(version);

            if (string.IsNullOrWhiteSpace(_availableVersionString) || _availableVersion == null)
            {
                // Set the available version the th current version as a default,
                // if it is not set already.
                AddAvailableVersion(version);
            }
        }

        /// <summary>
        /// Adds the current version of the package.
        /// </summary>
        /// <param name="version">
        /// The current version of the package.
        /// </param>
        public void AddVersion(Version version)
        {
            _versionString = version.ToString();
            _version = version;

            if (string.IsNullOrWhiteSpace(_availableVersionString) || _availableVersion == null)
            {
                // Set the available version the th current version as a default,
                // if it is not set already.
                AddAvailableVersion(version);
            }
        }

        /// <summary>
        /// Adds the heigest available version of the package.
        /// </summary>
        /// <param name="availableVersion">
        /// The heigest available version of the package contained in a <see cref="System.String"/>.
        /// </param>
        public void AddAvailableVersion(string availableVersion)
        {
            _availableVersionString = availableVersion;
            _availableVersion = VersionParser.Parse(availableVersion);
        }

        /// <summary>
        /// Adds the heigest available version of the package.
        /// </summary>
        /// <param name="availableVersion">
        /// The heigest available version of the package.
        /// </param>
        public void AddAvailableVersion(Version availableVersion)
        {
            _availableVersionString = availableVersion.ToString();
            _availableVersion = availableVersion;
        }

        /// <summary>
        /// Adds the source name of the package.
        /// </summary>
        /// <param name="sourceName">
        /// The name of the source for the package.
        /// </param>
        public void AddSourceName(string sourceName)
        {
            _sourceName = sourceName;
        }

        /// <summary>
        /// Returns a <see cref="WGetNET.WinGetPackage"/> instance from data provided to the builder.
        /// </summary>
        /// <returns></returns>
        public override WinGetPackage GetInstance()
        {
            if (_version == null)
            {
                _version = VersionParser.Parse(_versionString);
            }

            if (_availableVersion == null)
            {
                _availableVersion = VersionParser.Parse(_availableVersionString);
            }

            return new WinGetPackage(_name, _id, _versionString, _version, _availableVersionString, _availableVersion, _sourceName, _hasShortenedId);
        }

        /// <summary>
        /// Cleares all added data from the builder.
        /// </summary>
        public override void Clear()
        {
            _name = string.Empty;
            _id = string.Empty;
            _versionString = string.Empty;
            _version = null;
            _availableVersionString = string.Empty;
            _availableVersion = null;
            _sourceName = string.Empty;
            _hasShortenedId = false;
        }
    }
}
