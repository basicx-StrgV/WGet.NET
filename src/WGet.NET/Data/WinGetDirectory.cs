//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System.IO;
using WGetNET.Abstractions;

namespace WGetNET
{
    /// <summary>
    /// Represents a winget directory in the info set.
    /// </summary>
    public sealed class WinGetDirectory : WinGetInfoEntry<WinGetDirectory>
    {
        /// <summary>
        /// Gets a value indicating whether the directory exists.
        /// </summary>
        public bool Exists
        {
            get
            {
                return _directoryInfo.Exists;
            }
        }

        /// <summary>
        /// Gets the name of the directory.
        /// </summary>
        public string Name
        {
            get
            {
                return _directoryInfo.Name;
            }
        }

        /// <summary>
        /// Gets the full path of the directory.
        /// </summary>
        public string FullName
        {
            get
            {
                return _directoryInfo.FullName;
            }
        }

        /// <summary>
        /// Gets the direcory info instance.
        /// </summary>
        public DirectoryInfo Info
        {
            get
            {
                return _directoryInfo;
            }
        }

        private readonly DirectoryInfo _directoryInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetDirectory"/> class.
        /// </summary>
        /// <param name="entryName">The name of the settings entry.</param>
        /// <param name="rawContent">The content of the settings entry.</param>
        /// <param name="hasShortenedContent">Sets if the content is shortened or not.</param>
        /// <param name="directoryInfo"><see cref="System.IO.DirectoryInfo"/> instance that was created from the raw content.</param>
        internal WinGetDirectory(string entryName, string rawContent, bool hasShortenedContent, DirectoryInfo directoryInfo) : base(entryName, rawContent, hasShortenedContent)
        {
            _directoryInfo = directoryInfo;
        }

        /// <inheritdoc/>
        public override bool Equals(WinGetDirectory? other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (_entryName.Equals(other.EntryName) && _rawContent.Equals(other.RawContent) &&
                _hasShortenedContent.Equals(other.HasShortenedContent) &&
                _directoryInfo.Equals(other.Info))
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public override object Clone()
        {
            return new WinGetDirectory(
                    _entryName,
                    _rawContent,
                    _hasShortenedContent,
                    _directoryInfo
                );
        }
    }
}
