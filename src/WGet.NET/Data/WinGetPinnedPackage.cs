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
    public class WinGetPinnedPackage : WinGetPackage
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

        private readonly string _pinTypeString;
        private readonly string _pinnedVersionString;
        private readonly PinType _pinType;

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
            bool hasShortenedId) : base(name, id, versionString, version, availableVersionString, availableVersion, sourceName, hasShortenedId)
        {
            _pinTypeString = pinTypeString;
            _pinnedVersionString = pinnedVersion;

            _pinType = pinType;
        }

        /// <inheritdoc/>
        public override object Clone()
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
    }
}
