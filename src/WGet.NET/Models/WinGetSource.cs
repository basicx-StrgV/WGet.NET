//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
namespace WGetNET
{
    /// <summary>
    /// Represents a winget source
    /// </summary>
    public class WinGetSource
    {
        /// <summary>
        /// Gets or sets the name of the source.
        /// </summary>
        public string SourceName { get; set; }
        /// <summary>
        /// Gets or sets the url of the source.
        /// </summary>
        public string SourceUrl { get; set; }
        /// <summary>
        /// Gets or sets the type of the source.
        /// </summary>
        public string SourceType { get; set; }
    }
}
