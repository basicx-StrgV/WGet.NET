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
        /// Gets the name of the package.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Gets the id of the package.
        /// </summary>
        public string Id
        {
            get
            {
                return _id;
            }
        }

        /// <summary>
        /// Gets the version of the package.
        /// </summary>
        public string VersionString
        {
            get
            {
                return _versionString;
            }
        }

        /// <summary>
        /// Gets the version of the package.
        /// </summary>
        public Version Version
        {
            get
            {
                return _version;
            }
        }

        /// <summary>
        /// Gets the newest available version of the package.
        /// </summary>
        public string AvailableVersionString
        {
            get
            {
                return _availableVersionString;
            }
        }

        /// <summary>
        /// Gets the newest available version of the package.
        /// </summary>
        public Version AvailableVersion
        {
            get
            {
                return _availableVersion;
            }
        }

        /// <summary>
        /// Gets the source name for the package.
        /// </summary>
        public string SourceName
        {
            get
            {
                return _sourceName;
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

        private readonly string _name;
        private readonly string _id;
        private readonly string _versionString;
        private readonly Version _version;
        private readonly string _availableVersionString;
        private readonly Version _availableVersion;
        private readonly string _sourceName;

        private readonly bool _hasShortenedId = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetPackage"/> class.
        /// </summary>
        /// <param name="name">The name of the package.</param>
        /// <param name="id">The id of the package.</param>
        /// <param name="version">The current version of the package.</param>
        /// <param name="availableVersion">Heighest available version of the package.</param>
        /// <param name="sourceName">Name of the source the package comes from.</param>
        /// <param name="hasShortenedId">Sets if the id is shortened or not.</param>
        internal WinGetPackage(string name, string id, string version, string availableVersion, string sourceName, bool hasShortenedId)
        {
            _name = name;
            _id = id;
            
            _versionString = version;
            _version = VersionParser.Parse(_versionString);
            
            _availableVersionString = availableVersion;
            _availableVersion = VersionParser.Parse(_availableVersionString);

            _sourceName = sourceName;

            _hasShortenedId = hasShortenedId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetPackage"/> class.
        /// </summary>
        /// <param name="name">The name of the package.</param>
        /// <param name="id">The id of the package.</param>
        /// <param name="version">The current version of the package.</param>
        /// <param name="availableVersion">Heighest available version of the package.</param>
        /// <param name="sourceName">Name of the source the package comes from.</param>
        /// <param name="hasShortenedId">Sets if the id is shortened or not.</param>
        internal WinGetPackage(string name, string id, Version version, Version availableVersion, string sourceName, bool hasShortenedId)
        {
            _name = name;
            _id = id;

            _version = version;
            _versionString = _version.ToString();

            _availableVersion = availableVersion;
            _availableVersionString = _availableVersion.ToString();

            _sourceName = sourceName;

            _hasShortenedId = hasShortenedId;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="WGetNET.WinGetPackage"/> class and returns it.
        /// </summary>
        /// <param name="name">The name of the package.</param>
        /// <param name="id">The id of the package.</param>
        /// <param name="version">The current version of the package as a <see cref="System.String"/>. It will also be used for the available version.</param>
        /// <param name="sourceName">The name of the source the package comes from.</param>
        /// <param name="hasShortenedId">Sets if the id of the package is shortend or not.</param>
        /// <returns>
        /// The created instance of the <see cref="WGetNET.WinGetPackage"/> class.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public static WinGetPackage Create(string name, string id, string version, string sourceName = "", bool hasShortenedId = false)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(id, "id");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(version, "version");
            ArgsHelper.ThrowIfObjectIsNull(sourceName, "sourceName");

            return new WinGetPackage(name, id, version, version, sourceName, hasShortenedId);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="WGetNET.WinGetPackage"/> class and returns it.
        /// </summary>
        /// <param name="name">The name of the package.</param>
        /// <param name="id">The id of the package.</param>
        /// <param name="version">The current version of the package as a <see cref="System.String"/>.</param>
        /// <param name="availableVersion">The highest available version of the package as a <see cref="System.String"/>.</param>
        /// <param name="sourceName">The name of the source the package comes from.</param>
        /// <param name="hasShortenedId">Sets if the id of the package is shortend or not.</param>
        /// <returns>
        /// The created instance of the <see cref="WGetNET.WinGetPackage"/> class.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public static WinGetPackage Create(string name, string id, string version, string availableVersion, string sourceName = "", bool hasShortenedId = false)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(id, "id");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(version, "version");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(availableVersion, "availableVersion");
            ArgsHelper.ThrowIfObjectIsNull(sourceName, "sourceName");

            return new WinGetPackage(name, id, version, availableVersion, sourceName, hasShortenedId);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="WGetNET.WinGetPackage"/> class and returns it.
        /// </summary>
        /// <param name="name">The name of the package.</param>
        /// <param name="id">The id of the package.</param>
        /// <param name="version">The current version of the package. It will also be used for the available version.</param>
        /// <param name="sourceName">The name of the source the package comes from.</param>
        /// <param name="hasShortenedId">Sets if the id of the package is shortend or not.</param>
        /// <returns>
        /// The created instance of the <see cref="WGetNET.WinGetPackage"/> class.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public static WinGetPackage Create(string name, string id, Version version, string sourceName = "", bool hasShortenedId = false)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(id, "id");
            ArgsHelper.ThrowIfObjectIsNull(version, "version");
            ArgsHelper.ThrowIfObjectIsNull(sourceName, "sourceName");

            return new WinGetPackage(name, id, version, version, sourceName, hasShortenedId);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="WGetNET.WinGetPackage"/> class and returns it.
        /// </summary>
        /// <param name="name">The name of the package.</param>
        /// <param name="id">The id of the package.</param>
        /// <param name="version">The current version of the package.</param>
        /// <param name="availableVersion">The highest available version of the package.</param>
        /// <param name="sourceName">The name of the source the package comes from.</param>
        /// <param name="hasShortenedId">Sets if the id of the package is shortend or not.</param>
        /// <returns>
        /// The created instance of the <see cref="WGetNET.WinGetPackage"/> class.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public static WinGetPackage Create(string name, string id, Version version, Version availableVersion, string sourceName = "", bool hasShortenedId = false)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(id, "id");
            ArgsHelper.ThrowIfObjectIsNull(version, "version");
            ArgsHelper.ThrowIfObjectIsNull(availableVersion, "availableVersion");
            ArgsHelper.ThrowIfObjectIsNull(sourceName, "sourceName");

            return new WinGetPackage(name, id, version, availableVersion, sourceName, hasShortenedId);
        }
    }
}
