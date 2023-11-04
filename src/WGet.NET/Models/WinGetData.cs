//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.Collections.Generic;

namespace WGetNET
{
    /// <summary>
    /// Represents WinGet related data
    /// </summary>
    public class WinGetData
    {
        /// <summary>
        /// Gets the version number of the winget installation as a <see cref="System.String"/>.
        /// </summary>
        public string WinGetVersionString
        {
            get
            {
                return _wingetVersionString;
            }
        }

        /// <summary>
        /// Gets the version number of the winget installation.
        /// </summary>
        public Version WinGetVersion
        {
            get
            {
                return _wingetVersion;
            }
        }

        /// <summary>
        /// Gets a list of the winget direcories.
        /// </summary>
        public List<WinGetInfoEntry> Directories
        {
            get
            {
                return _directories;
            }
        }

        /// <summary>
        /// Gets a list of the winget related links.
        /// </summary>
        public List<WinGetInfoEntry> Links
        {
            get
            {
                return _links;
            }
        }

        /// <summary>
        /// Gets a list of the winget admin setting states.
        /// </summary>
        public List<WinGetAdminOption> AdminSetting
        {
            get
            {
                return _adminSetting;
            }
        }

        /// <summary>
        /// Gets if the object is empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                if (string.IsNullOrWhiteSpace(WinGetVersionString) && Directories.Count <= 0 && Links.Count <= 0 && AdminSetting.Count <= 0)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets an empty instance of this object.
        /// </summary>
        internal static WinGetData Empty
        {
            get
            {
                return new WinGetData("", new List<WinGetInfoEntry>(), new List<WinGetInfoEntry>(), new List<WinGetAdminOption>());
            }
        }

        private readonly string _wingetVersionString;
        private readonly Version _wingetVersion;
        private readonly List<WinGetInfoEntry> _directories;
        private readonly List<WinGetInfoEntry> _links;
        private readonly List<WinGetAdminOption> _adminSetting;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetData"/> class.
        /// </summary>
        /// <param name="wingetVersion">The installed winget version.</param>
        /// <param name="directories">
        /// <see cref="System.Collections.Generic.List{T}"/> of info entries containing the WinGet directories.
        /// </param>
        /// <param name="links">
        /// <see cref="System.Collections.Generic.List{T}"/> of info entries containing WinGet related links.
        /// </param>
        /// <param name="adminSetting">
        /// <see cref="System.Collections.Generic.List{T}"/> of info entries containing the WinGet admin setting states.
        /// </param>
        internal WinGetData(string wingetVersion, List<WinGetInfoEntry> directories, List<WinGetInfoEntry> links, List<WinGetAdminOption> adminSetting)
        {
            _wingetVersionString = wingetVersion;
            _wingetVersion = CreateVersionObject(wingetVersion);
            _directories = directories;
            _links = links;
            _adminSetting = adminSetting;
        }

        private Version CreateVersionObject(string version)
        {
            if (!Version.TryParse(version, out Version? versionObject))
            {
                versionObject = Version.Parse("0.0.0");
            }

            return versionObject;
        }
    }
}
