//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using WGetNET.Builder;
using WGetNET.HelperClasses;

namespace WGetNET
{
    /// <summary>
    /// Represents a winget package.
    /// </summary>
    public sealed class WinGetPackage : IWinGetPackage, IEquatable<WinGetPackage>, ICloneable
    {
        /// <inheritdoc/>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <inheritdoc/>
        public string Id
        {
            get
            {
                return _id;
            }
        }

        /// <inheritdoc/>
        public string VersionString
        {
            get
            {
                return _versionString;
            }
        }

        /// <inheritdoc/>
        public Version Version
        {
            get
            {
                return _version;
            }
        }

        /// <inheritdoc/>
        public string AvailableVersionString
        {
            get
            {
                return _availableVersionString;
            }
        }

        /// <inheritdoc/>
        public Version AvailableVersion
        {
            get
            {
                return _availableVersion;
            }
        }

        /// <inheritdoc/>
        public string SourceName
        {
            get
            {
                return _sourceName;
            }
        }

        /// <inheritdoc/>
        public bool HasShortenedId
        {
            get
            {
                return _hasShortenedId;
            }
        }

        /// <inheritdoc/>
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
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public static WinGetPackage Create(string name, string id, string version, string sourceName = "")
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(id, "id");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(version, "version");
            ArgsHelper.ThrowIfObjectIsNull(sourceName, "sourceName");

            PackageBuilder builder = new PackageBuilder();

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
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public static WinGetPackage Create(string name, string id, string version, string availableVersion, string sourceName = "")
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(id, "id");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(version, "version");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(availableVersion, "availableVersion");
            ArgsHelper.ThrowIfObjectIsNull(sourceName, "sourceName");

            PackageBuilder builder = new PackageBuilder();

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
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public static WinGetPackage Create(string name, string id, Version version, string sourceName = "")
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(id, "id");
            ArgsHelper.ThrowIfObjectIsNull(version, "version");
            ArgsHelper.ThrowIfObjectIsNull(sourceName, "sourceName");

            PackageBuilder builder = new PackageBuilder();

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
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public static WinGetPackage Create(string name, string id, Version version, Version availableVersion, string sourceName = "")
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(id, "id");
            ArgsHelper.ThrowIfObjectIsNull(version, "version");
            ArgsHelper.ThrowIfObjectIsNull(availableVersion, "availableVersion");
            ArgsHelper.ThrowIfObjectIsNull(sourceName, "sourceName");

            PackageBuilder builder = new PackageBuilder();

            builder.AddName(name);
            builder.AddId(id);
            builder.AddVersion(version);
            builder.AddAvailableVersion(availableVersion);
            builder.AddSourceName(sourceName);

            return builder.GetInstance();
        }

        /// <inheritdoc/>
        public bool Equals(WinGetPackage? other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (_name.Equals(other.Name) && _id.Equals(other.Id) &&
                _versionString.Equals(other.VersionString) && _version.Equals(other.Version) &&
                _availableVersionString.Equals(other.AvailableVersionString) && _availableVersion.Equals(other.AvailableVersion) &&
                _sourceName.Equals(other.SourceName) && _hasShortenedId.Equals(other.HasShortenedId))
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public object Clone()
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
