//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using WGetNET.Abstractions;

namespace WGetNET
{
    /// <summary>
    /// Represents a winget link in the info set.
    /// </summary>
    public sealed class WinGetLink : WinGetInfoEntry
    {
        /// <summary>
        /// Gets the url.
        /// </summary>
        public Uri Url
        {
            get
            {
                return _url;
            }
        }

        private readonly Uri _url;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetLink"/> class.
        /// </summary>
        /// <param name="entryName">The name of the settings entry.</param>
        /// <param name="rawContent">The content of the settings entry.</param>
        /// <param name="hasShortenedContent">Sets if the content is shortened or not.</param>
        /// <param name="url"><see cref="System.Uri"/> instance containing the url.</param>
        internal WinGetLink(string entryName, string rawContent, bool hasShortenedContent, Uri url) : base(entryName, rawContent, hasShortenedContent)
        {
            _url = url;
        }
    }
}
