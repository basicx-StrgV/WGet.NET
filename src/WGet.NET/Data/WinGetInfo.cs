//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WGetNET
{
    /// <summary>
    /// Represents winget related information.
    /// </summary>
    public sealed class WinGetInfo : IWinGetObject
    {
        /// <summary>
        /// Gets the version number of the winget installation as a <see cref="System.String"/>.
        /// </summary>
        public string VersionString
        {
            get
            {
                return _versionString;
            }
        }

        /// <summary>
        /// Gets the version number of the winget installation.
        /// </summary>
        public Version Version
        {
            get
            {
                return _version;
            }
        }

        /// <summary>
        /// Gets a collection of the winget direcories.
        /// </summary>
        public ReadOnlyCollection<WinGetDirectory> Directories
        {
            get
            {
                return _directories;
            }
        }

        /// <summary>
        /// Gets a collection of the winget related links.
        /// </summary>
        public ReadOnlyCollection<WinGetLink> Links
        {
            get
            {
                return _links;
            }
        }

        /// <summary>
        /// Gets a collection of the winget admin settings.
        /// </summary>
        public ReadOnlyCollection<WinGetAdminSetting> AdminSettings
        {
            get
            {
                return _adminSettings;
            }
        }

        /// <summary>
        /// Gets if the object is empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_versionString) &&
                    (_directories == null || _directories.Count <= 0) &&
                    (_links == null || _links.Count <= 0) &&
                    (_adminSettings == null || _adminSettings.Count <= 0))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets an empty instance of this object.
        /// </summary>
        internal static WinGetInfo Empty
        {
            get
            {
                return new WinGetInfo("", new Version(0, 0), new List<WinGetDirectory>(), new List<WinGetLink>(), new List<WinGetAdminSetting>());
            }
        }

        private readonly string _versionString;
        private readonly Version _version;
        private readonly ReadOnlyCollection<WinGetDirectory> _directories;
        private readonly ReadOnlyCollection<WinGetLink> _links;
        private readonly ReadOnlyCollection<WinGetAdminSetting> _adminSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetInfo"/> class.
        /// </summary>
        /// <param name="versionString">The installed winget version as a <see cref="System.String"/>.</param>
        /// <param name="version">The installed winget version.</param>
        /// <param name="directories">
        /// <see cref="System.Collections.Generic.List{T}"/> of info entries containing the WinGet directories.
        /// </param>
        /// <param name="links">
        /// <see cref="System.Collections.Generic.List{T}"/> of info entries containing WinGet related links.
        /// </param>
        /// <param name="adminSettings">
        /// <see cref="System.Collections.Generic.List{T}"/> of info entries containing the WinGet admin setting states.
        /// </param>
        internal WinGetInfo(string versionString, Version version, List<WinGetDirectory> directories, List<WinGetLink> links, List<WinGetAdminSetting> adminSettings)
        {
            _versionString = versionString;
            _version = version;
            _directories = new ReadOnlyCollection<WinGetDirectory>(directories);
            _links = new ReadOnlyCollection<WinGetLink>(links);
            _adminSettings = new ReadOnlyCollection<WinGetAdminSetting>(adminSettings);
        }
    }
}
