//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using WGetNET.Helper;
using WGetNET.Builder;

namespace WGetNET
{
    /// <summary>
    /// Represents a winget package.
    /// </summary>
    public class WinGetPackage : IWinGetObject, ICloneable
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
        /// <remarks>
        /// The name of the package will be used for all actions performd with this package.
        /// </remarks>
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
        /// Gets if the package can be upgraded.
        /// </summary>
        public bool HasUpgrade
        {
            get
            {
                if (_availableVersion > _version)
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

        // \cond PRIVATE
        private protected readonly string _name;
        private protected readonly string _id;
        private protected readonly string _versionString;
        private protected readonly Version _version;
        private protected readonly string _availableVersionString;
        private protected readonly Version _availableVersion;
        private protected readonly string _sourceName;

        private protected readonly bool _hasShortenedId = false;
        // \endcond

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetPackage"/> class.
        /// </summary>
        /// <param name="name">The name of the package.</param>
        /// <param name="id">The id of the package.</param>
        /// <param name="versionString">The current version of the package as a <see cref="System.String"/>.</param>
        /// <param name="version">The current version of the package.</param>
        /// <param name="availableVersion">Heighest available version of the package.</param>
        /// <param name="availableVersionString">Heighest available version of the package as a <see cref="System.String"/>.</param>
        /// <param name="sourceName">Name of the source the package comes from.</param>
        /// <param name="hasShortenedId">Sets if the id is shortened or not.</param>
        internal WinGetPackage(
            string name,
            string id,
            string versionString,
            Version version,
            string availableVersionString,
            Version availableVersion,
            string sourceName,
            bool hasShortenedId)
        {
            _name = name;
            _id = id;

            _versionString = versionString;
            _version = version;

            _availableVersionString = availableVersionString;
            _availableVersion = availableVersion;

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
        /// <returns>
        /// The created instance of the <see cref="WGetNET.WinGetPackage"/> class.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        public static WinGetPackage Create(string name, string id, string version, string sourceName = "")
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(id, "id");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(version, "version");
            ArgsHelper.ThrowIfObjectIsNull(sourceName, "sourceName");

            PackageBuilder builder = new();

            builder.AddName(name);
            builder.AddId(id);
            builder.AddVersion(version);
            builder.AddSourceName(sourceName);

            return builder.GetInstance();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="WGetNET.WinGetPackage"/> class and returns it.
        /// </summary>
        /// <param name="name">The name of the package.</param>
        /// <param name="id">The id of the package.</param>
        /// <param name="version">The current version of the package as a <see cref="System.String"/>.</param>
        /// <param name="availableVersion">The highest available version of the package as a <see cref="System.String"/>.</param>
        /// <param name="sourceName">The name of the source the package comes from.</param>
        /// <returns>
        /// The created instance of the <see cref="WGetNET.WinGetPackage"/> class.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        public static WinGetPackage Create(string name, string id, string version, string availableVersion, string sourceName = "")
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(id, "id");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(version, "version");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(availableVersion, "availableVersion");
            ArgsHelper.ThrowIfObjectIsNull(sourceName, "sourceName");

            PackageBuilder builder = new();

            builder.AddName(name);
            builder.AddId(id);
            builder.AddVersion(version);
            builder.AddAvailableVersion(availableVersion);
            builder.AddSourceName(sourceName);

            return builder.GetInstance();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="WGetNET.WinGetPackage"/> class and returns it.
        /// </summary>
        /// <param name="name">The name of the package.</param>
        /// <param name="id">The id of the package.</param>
        /// <param name="version">The current version of the package. It will also be used for the available version.</param>
        /// <param name="sourceName">The name of the source the package comes from.</param>
        /// <returns>
        /// The created instance of the <see cref="WGetNET.WinGetPackage"/> class.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        public static WinGetPackage Create(string name, string id, Version version, string sourceName = "")
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(id, "id");
            ArgsHelper.ThrowIfObjectIsNull(version, "version");
            ArgsHelper.ThrowIfObjectIsNull(sourceName, "sourceName");

            PackageBuilder builder = new();

            builder.AddName(name);
            builder.AddId(id);
            builder.AddVersion(version);
            builder.AddSourceName(sourceName);

            return builder.GetInstance();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="WGetNET.WinGetPackage"/> class and returns it.
        /// </summary>
        /// <param name="name">The name of the package.</param>
        /// <param name="id">The id of the package.</param>
        /// <param name="version">The current version of the package.</param>
        /// <param name="availableVersion">The highest available version of the package.</param>
        /// <param name="sourceName">The name of the source the package comes from.</param>
        /// <returns>
        /// The created instance of the <see cref="WGetNET.WinGetPackage"/> class.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        public static WinGetPackage Create(string name, string id, Version version, Version availableVersion, string sourceName = "")
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(id, "id");
            ArgsHelper.ThrowIfObjectIsNull(version, "version");
            ArgsHelper.ThrowIfObjectIsNull(availableVersion, "availableVersion");
            ArgsHelper.ThrowIfObjectIsNull(sourceName, "sourceName");

            PackageBuilder builder = new();

            builder.AddName(name);
            builder.AddId(id);
            builder.AddVersion(version);
            builder.AddAvailableVersion(availableVersion);
            builder.AddSourceName(sourceName);

            return builder.GetInstance();
        }

        /// <summary>
        /// Checks if two packages are the same and optionally if they also have the same version. 
        /// </summary>
        /// <param name="other">
        /// The <see cref="WGetNET.WinGetPackage"/> to compare with.
        /// </param>
        /// <param name="sameVersion">
        /// Set to <see langword="true"/> to also check for the same version.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if both packages are the same and <see langword="false"/> if not.
        /// </returns>
        public bool SamePackage(WinGetPackage other, bool sameVersion = false)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (_id.Equals(other.Id) && _sourceName.Equals(other.SourceName) &&
                (!sameVersion || (_versionString.Equals(other.VersionString) && _version.Equals(other.Version))))
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public virtual object Clone()
        {
            return new WinGetPackage(
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
            return $"{_name} {_versionString}";
        }
    }
}
