//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.IO;
using WGetNET.Abstractions;

namespace WGetNET.Builder
{
    /// <summary>
    /// Builder to create a new <see cref="WGetNET.WinGetDirectory"/> instance.
    /// </summary>
    internal class DirectoryBuilder : WinGetObjectBuilder<WinGetDirectory?>
    {
        private string _entryName = string.Empty;
        private string _rawContent = string.Empty;
        private bool _hasShortenedContent = false;
        private DirectoryInfo? _directoryInfo = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.Builder.DirectoryBuilder"/> class.
        /// </summary>
        public DirectoryBuilder()
        {
            // Provide empty constructor
        }

        /// <summary>
        /// Addes the entry name for the winget info entry.
        /// </summary>
        /// <param name="entryName">The name of the info entry.</param>
        public void AddEntryName(string entryName)
        {
            _entryName = entryName;
        }

        /// <summary>
        /// Adds the raw content of the winget info entry.
        /// </summary>
        /// <remarks>
        /// The data will be parsed to the content of the direcory entry.
        /// </remarks>
        /// <param name="rawContent">The raw data of the info entry as a <see cref="System.String"/></param>
        public void AddRawContent(string rawContent)
        {
            _hasShortenedContent = CheckShortenedValue(rawContent);

            if (_hasShortenedContent)
            {
                // Remove the char at the end of the shortened content.
                _rawContent = rawContent.Remove(rawContent.Length - 1);
            }
            else
            {
                _rawContent = rawContent;
            }

            _directoryInfo = CreateDirectoryInfo(_rawContent, _hasShortenedContent);
        }

        /// <summary>
        /// Returns a <see cref="WGetNET.WinGetDirectory"/> instance from data provided to the builder.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetDirectory"/> instance, or <see langword="null"/> if the provided data failed to be parsed.
        /// </returns>
        public override WinGetDirectory? GetInstance()
        {
            if (_directoryInfo == null)
            {
                return null;
            }

            return new WinGetDirectory(_entryName, _rawContent, _hasShortenedContent, _directoryInfo);
        }

        /// <inheritdoc/>
        public override void Clear()
        {
            _entryName = string.Empty;
            _rawContent = string.Empty;
            _hasShortenedContent = false;
            _directoryInfo = null;
        }

        /// <summary>
        /// Creates and returns a <see cref="System.IO.DirectoryInfo"/> instance from the raw content.
        /// </summary>
        /// <param name="rawContent"><see cref="System.String"/> containing the raw data that should get parsed.</param>
        /// <param name="hasShortenedContent">Indcates if the information in the raw content is shortened.</param>
        /// <returns>
        /// The created <see cref="System.IO.DirectoryInfo"/> instance, or <see langword="null"/> if parsing the raw content failed.
        /// </returns>
        private DirectoryInfo? CreateDirectoryInfo(string rawContent, bool hasShortenedContent)
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
