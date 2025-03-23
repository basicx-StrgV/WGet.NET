//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.IO;
using WGetNET.Abstractions;
using WGetNET.Helper;

namespace WGetNET.Builder
{
    /// <summary>
    /// Builder to create a new <see cref="WGetNET.WinGetDirectory"/> instance.
    /// </summary>
    internal class WinGetDirectoryBuilder : WinGetObjectBuilder<WinGetDirectory?>
    {
        private string _entryName = string.Empty;
        private string _rawContent = string.Empty;
        private bool _hasShortenedContent = false;
        private DirectoryInfo? _directoryInfo = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.Builder.WinGetDirectoryBuilder"/> class.
        /// </summary>
        public WinGetDirectoryBuilder()
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
#if NETCOREAPP3_1_OR_GREATER
                _rawContent = rawContent[..^1];
#elif NETSTANDARD2_0
                _rawContent = rawContent.Remove(rawContent.Length - 1);
#endif
            }
            else
            {
                _rawContent = rawContent;
            }

            SetDirectoryInfo(_rawContent, _hasShortenedContent);
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
        /// Creates and sets the <see cref="System.IO.DirectoryInfo"/> instance from the raw content.
        /// </summary>
        /// <param name="rawContent"><see cref="System.String"/> containing the raw data that should get parsed.</param>
        /// <param name="hasShortenedContent">Indcates if the information in the raw content is shortened.</param>
        private void SetDirectoryInfo(string rawContent, bool hasShortenedContent)
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
                    _directoryInfo = new DirectoryInfo(path);
                }

                // Fallback for an incomplete directory path.
                _directoryInfo = new DirectoryInfo(PathHelper.TrimLastPathPart(path, PathHelper.PathType.Directory));
            }
            catch
            {
                _directoryInfo = null;
            }
        }
    }
}
