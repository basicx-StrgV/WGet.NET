//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
namespace WGetNET
{
    /// <summary>
    /// Defines identifiers for all version ranges that generate a different output when using "winget --info".
    /// </summary>
    internal enum InfoActionVersionId
    {
        /// <summary>
        /// First WinGet version to version 1.4.3132
        /// </summary>
        VersionRange1,
        /// <summary>
        /// Version 1.4.3531 to version 1.5.101
        /// </summary>
        VersionRange2,
        /// <summary>
        /// Version 1.5.441
        /// </summary>
        VersionRange3,
        /// <summary>
        /// Version 1.5.1081 to newest version
        /// </summary>
        VersionRange4
    }
}
