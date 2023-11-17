//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;

namespace WGetNET
{
    /// <summary>
    /// Represents a winget pinned package.
    /// </summary>
    public sealed class WinGetPinnedPackage : IWinGetPackage, IEquatable<WinGetPinnedPackage>, ICloneable
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
        /// <param name="pinTypeString">Name of the winget pin type for the package.</param>
        /// <param name="pinType">The <see cref="WGetNET.PinType"/> of the package.</param>
        /// <param name="pinnedVersion"><see cref="System.String"/> containing the pinned version for the package.</param>
        /// <param name="hasShortenedId">Sets if the id is shortened or not.</param>
        /// <param name="name">The name of the package.</param>
        /// <param name="id">The id of the package.</param>
        /// <param name="versionString">The current version of the package as a <see cref="System.String"/>.</param>
        /// <param name="version">The current version of the package.</param>
        /// <param name="availableVersionString">Heighest available version of the package as a <see cref="System.String"/>.</param>
        /// <param name="availableVersion">Heighest available version of the package.</param>
        /// <param name="sourceName">Name of the source the package comes from.</param>
        internal WinGetPinnedPackage(
            string pinTypeString,
            PinType pinType,
            string pinnedVersion,
            string name,
            string id,
            string versionString,
            Version version,
            string availableVersionString,
            Version availableVersion,
            string sourceName,
            bool hasShortenedId)
        {
            _pinTypeString = pinTypeString;
            _pinnedVersionString = pinnedVersion;

            _pinType = pinType;

            _name = name;
            _id = id;

            _versionString = versionString;
            _version = version;

            _availableVersionString = availableVersionString;
            _availableVersion = availableVersion;

            _sourceName = sourceName;

            _hasShortenedId = hasShortenedId;
        }

        /// <inheritdoc/>
        public bool Equals(WinGetPinnedPackage? other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (_pinTypeString.Equals(other.PinTypeString) && _pinType.Equals(other.PinType) && _pinnedVersionString.Equals(other.PinnedVersion) &&
                _name.Equals(other.Name) && _id.Equals(other.Id) &&
                _versionString.Equals(other.VersionString) && _version.Equals(other.Version) &&
                _availableVersionString.Equals(other.AvailableVersionString) && _availableVersion.Equals(other.AvailableVersion) &&
                _sourceName.Equals(other.SourceName) && _hasShortenedId.Equals(other.HasShortenedId))
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public object Clone()
        {
            return new WinGetPinnedPackage(
                    _pinTypeString,
                    _pinType,
                    _pinnedVersionString,
                    _name,
                    _id,
                    _versionString,
                    _version,
                    _availableVersionString,
                    _availableVersion,
                    _sourceName,
                    _hasShortenedId
                );
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{_pinTypeString} {_name} {_versionString}";
        }
    }
}
