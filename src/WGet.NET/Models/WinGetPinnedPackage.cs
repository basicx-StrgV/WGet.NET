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
        internal WinGetPinnedPackage(string pinType, string pinnedVersion, bool hasShortenedId): base(hasShortenedId)
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
