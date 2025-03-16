﻿//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using WGetNET.Abstractions;
using WGetNET.Helper;

namespace WGetNET.Builder
{
    /// <summary>
    /// Builder to create a new <see cref="WGetNET.WinGetLink"/> instance.
    /// </summary>
    internal class WinGetLinkBuilder : WinGetObjectBuilder<WinGetLink?>
    {
        private string _entryName = string.Empty;
        private string _rawContent = string.Empty;
        private bool _hasShortenedContent = false;
        private Uri? _url = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.Builder.WinGetLinkBuilder"/> class.
        /// </summary>
        public WinGetLinkBuilder()
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

            SetUri(_rawContent, _hasShortenedContent);
        }

        /// <summary>
        /// Returns a <see cref="WGetNET.WinGetLink"/> instance from data provided to the builder.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetLink"/> instance, or <see langword="null"/> if the provided data failed to be parsed.
        /// </returns>
        public override WinGetLink? GetInstance()
        {
            if (_url == null)
            {
                return null;
            }

            return new WinGetLink(_entryName, _rawContent, _hasShortenedContent, _url);
        }

        /// <inheritdoc/>
        public override void Clear()
        {
            _entryName = string.Empty;
            _rawContent = string.Empty;
            _hasShortenedContent = false;
            _url = null;
        }

        /// <summary>
        /// Creates and sets the <see cref="System.Uri"/> instance from the raw content.
        /// </summary>
        /// <param name="rawContent"><see cref="System.String"/> containing the raw data that should get parsed.</param>
        /// <param name="hasShortenedContent">Indcates if the information in the raw content is shortened.</param>
        private void SetUri(string rawContent, bool hasShortenedContent)
        {
            if (!hasShortenedContent)
            {
                Uri.TryCreate(rawContent, UriKind.Absolute, out Uri? uri);
                _url = uri;
            }

            // Fallback for an incomplete uri.
            Uri.TryCreate(PathHelper.TrimLastPathPart(rawContent, PathHelper.PathType.URI), UriKind.Absolute, out Uri? shortenedUri);
            _url = shortenedUri;
        }
    }
}
