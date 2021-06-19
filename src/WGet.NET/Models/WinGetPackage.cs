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
        /// Name of the package
        /// </summary>
        public string PackageName { get; set; }
        /// <summary>
        /// Id of the package
        /// </summary>
        public string PackageId { get; set; }
        /// <summary>
        /// Version of the package
        /// </summary>
        public string PackageVersion { get; set; }
    }
}
