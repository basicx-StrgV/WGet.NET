//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using WGetNET.HelperClasses;

namespace WGetNET
{
    /// <summary>
    /// Represents a winget pinned package
    /// </summary>
    public class WinGetPinnedPackage: WinGetPackage
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
        /// Gets the pinned version as a <see cref="System.String"/>.
        /// </summary>
        public string PinnedVersion
        {
            get
            {
                return _pinnedVersion;
            }
        }

        /// <summary>
        /// Gets the pinned version.
        /// </summary>
        public Version PinnedVersionObject
        {
            get
            {
                return _pinnedVersionObject;
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
        private readonly string _pinnedVersion;
        private readonly Version _pinnedVersionObject;
        private readonly PinType _pinType;

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
            bool hasShortenedId) : base(name, id, version, availableVersion, sourceName, hasShortenedId)
        {
            _pinTypeString = pinType;
            _pinnedVersion = pinnedVersion;

            _pinnedVersionObject = VersionParser.Parse(_pinnedVersion);

            _pinType = _pinTypeString.ToUpper() switch
            {
                "PINNING" => PinType.Pinning,
                "BLOCKING" => PinType.Blocking,
                "GATING" => PinType.Gating,
                _ => PinType.Pinning,
            };
        }
    }
}
