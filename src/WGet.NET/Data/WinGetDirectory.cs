//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System.IO;

namespace WGetNET
{
    /// <summary>
    /// Represents a winget directory in the info set.
    /// </summary>
    public sealed class WinGetDirectory : WinGetInfoEntry
    {
        /// <summary>
        /// Gets the direcory info instance.
        /// </summary>
        public DirectoryInfo? Info
        {
            get
            {
                return _directoryInfo;
            }
        }

        private readonly DirectoryInfo? _directoryInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetDirectory"/> class.
        /// </summary>
        /// <param name="name">The name of the settings entry.</param>
        /// <param name="content">The content of the settings entry.</param>
        internal WinGetDirectory(string name, string content) : base(name, content)
        {
            _directoryInfo = CreateDirectoryInfo();
        }

        /// <summary>
        /// Creates and returns a <see cref="System.IO.DirectoryInfo"/> instance from the content of the class.
        /// </summary>
        /// <returns>
        /// The created <see cref="System.IO.DirectoryInfo"/> instance.
        /// </returns>
        private DirectoryInfo? CreateDirectoryInfo()
        {
            try
            {
                if (!_hasShortenedContent)
                {
                    return new DirectoryInfo(_content);
                }

                // Fallback for an incomplete directory path.
                return new DirectoryInfo(TrimLastDirectory(_content));
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Removes the last directory from the given path.
        /// </summary>
        /// <param name="path">
        /// <see cref="System.String"/> containing the path that sould be trimed.
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/> containing the trimed path.
        /// </returns>
        private string TrimLastDirectory(string path)
        {
            int lastSeparatorIndex = -1;
            for (int i = 0; i < path.Length; i++)
            {
                if (path[i].Equals(Path.DirectorySeparatorChar))
                {
                    lastSeparatorIndex = i;
                }
            }

            if (lastSeparatorIndex > -1)
            {
                return path.Remove(lastSeparatorIndex);
            }

            return path;
        }
    }
}
