//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
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
        public string PackageName
        {
            get => _packageName;
            set => _packageName = string.IsNullOrWhiteSpace(value) ? string.Empty : value;
        }
        /// <summary>
        /// Gets or sets the id of the package.
        /// </summary>
        public string PackageId
        {
            get => _packageId;
            set => _packageId = string.IsNullOrWhiteSpace(value) ? string.Empty : value;
        }
        /// <summary>
        /// Gets or sets the version of the package.
        /// </summary>
        public string PackageVersion
        {
            get => _packageVersion;
            set => _packageVersion = string.IsNullOrWhiteSpace(value) ? string.Empty : value;
        }
        /// <summary>
        /// Gets or sets the updatable version of the package.
        /// </summary>
        public string Source
        {
            get => _source;
            internal set => _source = string.IsNullOrWhiteSpace(value) ? string.Empty : value;
        }

        /// <summary>
        /// Gets if the object is empty.
        /// </summary>
        public virtual bool IsEmpty
        {
            get => string.IsNullOrWhiteSpace(_packageName) || string.IsNullOrWhiteSpace(_packageId) || string.IsNullOrWhiteSpace(_packageVersion);
        }

        private string _packageName = string.Empty;
        private string _packageId = string.Empty;
        private string _packageVersion = string.Empty;
        private string _source = string.Empty;

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{_packageName} ({_packageVersion})";
        }
    }
}
