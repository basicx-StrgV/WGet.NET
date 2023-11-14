//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
namespace WGetNET
{
    /// <summary>
    /// Represents a winget source.
    /// </summary>
    public interface IWinGetSource : IWinGetObject
    {
        /// <summary>
        /// Gets the name of the source.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the URL/UNC of the source.
        /// </summary>
        public string Arg { get; }

        /// <summary>
        /// Gets the type of the source.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Gets the data of the source.
        /// </summary>
        public string Data { get; }

        /// <summary>
        /// Gets the identifier of the source.
        /// </summary>
        public string Identifier { get; }
    }
}
