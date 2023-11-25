//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.Collections.Generic;
using WGetNET.Parser;
using WGetNET.Abstractions;

namespace WGetNET.Builder
{
    /// <summary>
    /// Builder to create a new <see cref="WGetNET.WinGetInfo"/> instance.
    /// </summary>
    internal class InfoSetBuilder : WinGetObjectBuilder<WinGetInfo>
    {
        private string _versionString = string.Empty;
        private Version? _version = null;
        private List<WinGetDirectory> _directories = new();
        private List<WinGetLink> _links = new();
        private List<WinGetAdminSetting> _adminSettings = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.Builder.InfoSetBuilder"/> class.
        /// </summary>
        public InfoSetBuilder()
        {
            // Provide empty constructor
        }

        /// <summary>
        /// Adds the current winget version.
        /// </summary>
        /// <param name="version">
        /// The current winget version contained in a <see cref="System.String"/>.
        /// </param>
        public void AddVersion(string version)
        {
            _versionString = version;
            _version = VersionParser.Parse(version);
        }

        /// <summary>
        /// Adds the current winget version.
        /// </summary>
        /// <param name="version">
        /// The current winget version.
        /// </param>
        public void AddVersion(Version version)
        {
            _version = version;
            _versionString = version.ToString();
        }

        /// <summary>
        /// Adds a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WinGetDirectory"/> objects.
        /// </summary>
        /// <param name="directories">
        /// The <see cref="System.Collections.Generic.List{T}"/> of <see cref="WinGetDirectory"/> objects.
        /// </param>
        public void AddDirectories(List<WinGetDirectory> directories)
        {
            _directories.AddRange(directories);
        }

        /// <summary>
        /// Adds a <see cref="WinGetDirectory"/> object.
        /// </summary>
        /// <param name="directory">
        /// The <see cref="WinGetDirectory"/> object.
        /// </param>
        public void AddDirectory(WinGetDirectory? directory)
        {
            if (directory != null)
            {
                _directories.Add(directory);
            }
        }

        /// <summary>
        /// Adds a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WinGetLink"/> objects.
        /// </summary>
        /// <param name="links">
        /// The <see cref="System.Collections.Generic.List{T}"/> of <see cref="WinGetLink"/> objects.
        /// </param>
        public void AddLinks(List<WinGetLink> links)
        {
            _links.AddRange(links);
        }

        /// <summary>
        /// Adds a <see cref="WinGetLink"/> object.
        /// </summary>
        /// <param name="link">
        /// The <see cref="WinGetLink"/> object.
        /// </param>
        public void AddLink(WinGetLink? link)
        {
            if (link != null)
            {
                _links.Add(link);
            }
        }

        /// <summary>
        /// Adds a <see cref="System.Collections.Generic.List{T}"/> of <see cref="WinGetAdminSetting"/> objects.
        /// </summary>
        /// <param name="adminOptions">
        /// The <see cref="System.Collections.Generic.List{T}"/> of <see cref="WinGetAdminSetting"/> objects.
        /// </param>
        public void AddAdminOptions(List<WinGetAdminSetting> adminOptions)
        {
            _adminSettings.AddRange(adminOptions);
        }

        /// <summary>
        /// Adds a <see cref="WinGetAdminSetting"/> object.
        /// </summary>
        /// <param name="adminOption">
        /// The <see cref="WinGetAdminSetting"/> object.
        /// </param>
        public void AddAdminOption(WinGetAdminSetting? adminOption)
        {
            if (adminOption != null)
            {
                _adminSettings.Add(adminOption);
            }
        }

        /// <summary>
        /// Returns a <see cref="WGetNET.WinGetInfo"/> instance from data provided to the builder.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetInfo"/> instance.
        /// </returns>
        public override WinGetInfo GetInstance()
        {
            if (_version == null)
            {
                _version = VersionParser.Parse(_versionString);
            }

            return new WinGetInfo(_versionString, _version, _directories, _links, _adminSettings);
        }

        /// <inheritdoc/>
        public override void Clear()
        {
            _versionString = string.Empty;
            _version = null;
            _directories = new List<WinGetDirectory>();
            _links = new List<WinGetLink>();
            _adminSettings = new List<WinGetAdminSetting>();
        }
    }
}
