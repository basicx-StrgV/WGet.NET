//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using WGetNET.HelperClasses;

namespace WGetNET
{
    /// <summary>
    /// Represents a winget pinned package.
    /// </summary>
    public sealed class WinGetPinnedPackage : IWinGetPackage
    {
        /// <summary>
        /// Gets the pin type of the package as a <see cref="System.String"/>.
        /// </summary>
        public string PinTypeString
        {
            get
            {
                return _pinTypeString;
            }
        }

        /// <summary>
        /// Gets the pinned version or version range.
        /// </summary>
        public string PinnedVersion
        {
            get
            {
                return _pinnedVersionString;
            }
        }

        /// <summary>
        /// Gets the pin type of the package.
        /// </summary>
        public PinType PinType
        {
            get
            {
                return _pinType;
            }
        }

        /// <inheritdoc/>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <inheritdoc/>
        public string Id
        {
            get
            {
                return _id;
            }
        }

        /// <inheritdoc/>
        public string VersionString
        {
            get
            {
                return _versionString;
            }
        }

        /// <inheritdoc/>
        public Version Version
        {
            get
            {
                return _version;
            }
        }

        /// <inheritdoc/>
        public string AvailableVersionString
        {
            get
            {
                return _availableVersionString;
            }
        }

        /// <inheritdoc/>
        public Version AvailableVersion
        {
            get
            {
                return _availableVersion;
            }
        }

        /// <inheritdoc/>
        public string SourceName
        {
            get
            {
                return _sourceName;
            }
        }

        /// <inheritdoc/>
        public bool HasShortenedId
        {
            get
            {
                return _hasShortenedId;
            }
        }

        /// <inheritdoc/>
        public bool HasNoId
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_id))
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets if the object is empty.
        /// </summary>
        /// <remarks>
        /// A package object counts as empty if it does not contain a id and name.
        /// Because the rest of the information is useless in this state.
        /// </remarks>
        public bool IsEmpty
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_id) && string.IsNullOrWhiteSpace(_name))
                {
                    return true;
                }
                return false;
            }
        }

        private readonly string _pinTypeString;
        private readonly string _pinnedVersionString;
        private readonly PinType _pinType;

        private readonly string _name;
        private readonly string _id;
        private readonly string _versionString;
        private readonly Version _version;
        private readonly string _availableVersionString;
        private readonly Version _availableVersion;
        private readonly string _sourceName;

        private readonly bool _hasShortenedId = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetPinnedPackage"/> class.
        /// </summary>
        /// <param name="pinType">Name of the winget pin type for the package.</param>
        /// <param name="pinnedVersion"><see cref="System.String"/> containing the pinned version for the package.</param>
        /// <param name="hasShortenedId">Sets if the id is shortened or not.</param>
        /// <param name="name">The name of the package.</param>
        /// <param name="id">The id of the package.</param>
        /// <param name="version">The current version of the package.</param>
        /// <param name="availableVersion">Heighest available version of the package.</param>
        /// <param name="sourceName">Name of the source the package comes from.</param>
        internal WinGetPinnedPackage(
            string pinType,
            string pinnedVersion,
            string name,
            string id,
            string version,
            string availableVersion,
            string sourceName,
            bool hasShortenedId)
        {
            _pinTypeString = pinType;
            _pinnedVersionString = pinnedVersion;

            _pinType = _pinTypeString.ToUpper() switch
            {
                "PINNING" => PinType.Pinning,
                "BLOCKING" => PinType.Blocking,
                "GATING" => PinType.Gating,
                _ => PinType.Pinning,
            };

            _name = name;
            _id = id;

            _versionString = version;
            _version = VersionParser.Parse(_versionString);

            _availableVersionString = availableVersion;
            _availableVersion = VersionParser.Parse(_availableVersionString);

            _sourceName = sourceName;

            _hasShortenedId = hasShortenedId;
        }
    }
}
