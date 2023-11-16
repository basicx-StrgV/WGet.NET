//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.IO;
using WGetNET.Abstractions;
using WGetNET.HelperClasses;

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
        private WinGetDirectory(string entryName, string rawContent, bool hasShortenedContent, DirectoryInfo directoryInfo) : base(entryName, rawContent, hasShortenedContent)
        {
            _directoryInfo = directoryInfo;
        }

        /// <summary>
        /// Creates a new <see cref="WGetNET.WinGetDirectory"/> instance.
        /// </summary>
        /// <param name="entryName">The name of the settings entry.</param>
        /// <param name="rawContent">The content of the settings entry.</param>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetDirectory"/> instance or <see langword="null"/> if parsing the data failed.
        /// </returns>
        internal static WinGetDirectory? Create(string entryName, string rawContent)
        {
            bool hasShortenedContent = ProcessOutputReader.CheckShortenedValue(rawContent);

            if (hasShortenedContent)
            {
                // Remove the char at the end of the shortened content.
                rawContent = rawContent.Remove(rawContent.Length - 1);
            }

            DirectoryInfo? directoryInfo = CreateDirectoryInfo(rawContent, hasShortenedContent);

            if (directoryInfo == null)
            {
                return null;
            }

            return new WinGetDirectory(entryName, rawContent, hasShortenedContent, directoryInfo);
        }

        /// <summary>
        /// Creates and returns a <see cref="System.IO.DirectoryInfo"/> instance from the content of the class.
        /// </summary>
        /// <returns>
        /// The created <see cref="System.IO.DirectoryInfo"/> instance.
        /// </returns>
        private static DirectoryInfo? CreateDirectoryInfo(string rawContent, bool hasShortenedContent)
        {
            try
            {
                string path = rawContent;

#if NETCOREAPP3_1_OR_GREATER
                if (path.StartsWith('%'))
                {
                    path = Environment.ExpandEnvironmentVariables(path);
                }
#elif NETSTANDARD2_0
                if (path.StartsWith("%"))
                {
                    path = Environment.ExpandEnvironmentVariables(path);
                }
#endif

                if (!hasShortenedContent)
                {
                    return new DirectoryInfo(path);
                }

                // Fallback for an incomplete directory path.
                return new DirectoryInfo(TrimLastDirectory(path));
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
        private static string TrimLastDirectory(string path)
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
