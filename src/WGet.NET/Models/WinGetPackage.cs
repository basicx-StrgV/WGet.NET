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
    public class WinGetPackage: IWinGetObject
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
        /// Gets if id of the package is shortened.
        /// </summary>
        public bool HasShortenedId
        {
            get
            {
                return _hasShortenedId;
            }
        }

        /// <summary>
        /// Gets if the package does not provide an id.
        /// </summary>
        /// <remarks>
        /// If this is true somthing whent wrong in the creation of the package.
        /// The name of the package will be used for all actions performd with this package.
        /// </remarks>
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
        public virtual bool IsEmpty
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

        private protected string _name = string.Empty;
        private protected string _id = string.Empty;
        private protected string _version = string.Empty;
        private protected Version _versionObject = new(0, 0);
        private protected string _availableVersion = string.Empty;
        private protected Version _availableVersionObject = new(0, 0);
        private protected string _sourceName = string.Empty;

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
