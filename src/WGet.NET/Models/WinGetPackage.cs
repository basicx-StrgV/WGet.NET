//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using WGetNET.HelperClasses;

namespace WGetNET
{
    /// <summary>
    /// Represents a winget package
    /// </summary>
    public class WinGetPackage
    {
        /// <summary>
        /// Gets or sets the name of the package.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value is null)
                {
                    _name = string.Empty;
                }
                else
                {
                    _name = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the id of the package.
        /// </summary>
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value is null)
                {
                    _id = string.Empty;
                }
                else
                {
                    _id = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the version of the package.
        /// </summary>
        public string Version
        {
            get
            {
                return _version;
            }
            set
            {
                if (value is null)
                {
                    _version = string.Empty;
                }
                else
                {
                    _version = value;
                    _versionObject = VersionParser.Parse(_version);
                }
            }
        }

        /// <summary>
        /// Gets the version of the package.
        /// </summary>
        public Version VersionObject
        {
            get
            {
                return _versionObject;
            }
        }

        /// <summary>
        /// Gets or sets the newest available version of the package.
        /// </summary>
        public string AvailableVersion
        {
            get
            {
                return _availableVersion;
            }
            set
            {
                if (value is null)
                {
                    _availableVersion = string.Empty;
                }
                else
                {
                    _availableVersion = value;
                    _availableVersionObject = VersionParser.Parse(_availableVersion);
                }
            }
        }

        /// <summary>
        /// Gets the newest available version of the package.
        /// </summary>
        public Version AvailableVersionObject
        {
            get
            {
                return _availableVersionObject;
            }
        }

        /// <summary>
        /// Gets or sets the source name for the package.
        /// </summary>
        public string SourceName
        {
            get
            {
                return _sourceName;
            }
            set
            {
                if (value is null)
                {
                    _sourceName = string.Empty;
                }
                else
                {
                    _sourceName = value;
                }
            }
        }

        /// <summary>
        /// Gets if the object is empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                if ((_name.Length + _id.Length + _version.Length + _availableVersion.Length + _sourceName.Length) > 0)
                {
                    return false;
                }
                return true;
            }
        }
        
        /// <summary>
        /// Gets if id of the package is shortened.
        /// </summary>
        public bool HasShortenedId
        {
            get
            {
                return _hasShortenedId;
            }
        }

        private string _name = string.Empty;
        private string _id = string.Empty;
        private string _version = string.Empty;
        private Version _versionObject = new(0, 0);
        private string _availableVersion = string.Empty;
        private Version _availableVersionObject = new(0, 0);
        private string _sourceName = string.Empty;

        private readonly bool _hasShortenedId = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetPackage"/> class.
        /// </summary>
        public WinGetPackage()
        {
            // Provide empty constructor
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetPackage"/> class.
        /// </summary>
        /// <param name="hasShortenedId">Sets if the id is shortened or not.</param>
        internal WinGetPackage(bool hasShortenedId)
        {
            _hasShortenedId = hasShortenedId;
        }
    }
}
