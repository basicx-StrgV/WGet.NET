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
        public string PackageName { get; set; }
        /// <summary>
        /// Gets or sets the id of the package.
        /// </summary>
        public string PackageId { get; set; }
        /// <summary>
        /// Gets or sets the version of the package.
        /// </summary>
        public string PackageVersion { get; set; }
    }
}
