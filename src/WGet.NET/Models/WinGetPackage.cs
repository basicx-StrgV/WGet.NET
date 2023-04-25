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
            get
            {
                return _packageName;
            }
            set
            {
                if (value is null)
                {
                    _packageName = string.Empty;
                }
                else
                {
                    _packageName = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the id of the package.
        /// </summary>
        public string PackageId 
        {
            get
            {
                return _packageId;
            }
            set
            {
                if (value is null)
                {
                    _packageId = string.Empty;
                }
                else
                {
                    _packageId = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the version of the package.
        /// </summary>
        public string PackageVersion 
        {
            get
            {
                return _packageVersion;
            }
            set
            {
                if (value is null)
                {
                    _packageVersion = string.Empty;
                }
                else
                {
                    _packageVersion = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the newest available version of the package.
        /// </summary>
        public string PackageAvailableVersion
        {
            get
            {
                return _packageAvailableVersion;
            }
            set
            {
                if (value is null)
                {
                    _packageAvailableVersion = string.Empty;
                }
                else
                {
                    _packageAvailableVersion = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the source name for the package.
        /// </summary>
        public string PackageSourceName
        {
            get
            {
                return _packageSourceName;
            }
            set
            {
                if (value is null)
                {
                    _packageSourceName = string.Empty;
                }
                else
                {
                    _packageSourceName = value;
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
                if ((_packageName.Length + _packageId.Length + _packageVersion.Length + _packageAvailableVersion.Length + _packageSourceName.Length) > 0)
                {
                    return false;
                }
                return true;
            }
        }

        private string _packageName = string.Empty;
        private string _packageId = string.Empty;
        private string _packageVersion = string.Empty;
        private string _packageAvailableVersion = string.Empty;
        private string _packageSourceName = string.Empty;
    }
}
