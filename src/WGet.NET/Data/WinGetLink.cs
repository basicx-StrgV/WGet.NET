//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using WGetNET.Abstractions;
using WGetNET.HelperClasses;

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
        private WinGetLink(string entryName, string rawContent, bool hasShortenedContent, Uri url) : base(entryName, rawContent, hasShortenedContent)
        {
            _url = url;
        }

        /// <summary>
        /// Creates a new <see cref="WGetNET.WinGetLink"/> instance.
        /// </summary>
        /// <param name="entryName">The name of the settings entry.</param>
        /// <param name="rawContent">The content of the settings entry.</param>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetLink"/> instance or <see langword="null"/> if parsing the data failed.
        /// </returns>
        internal static WinGetLink? Create(string entryName, string rawContent)
        {
            bool hasShortenedContent = ProcessOutputReader.CheckShortenedValue(rawContent);

            if (hasShortenedContent)
            {
                // Remove the char at the end of the shortened content.
                rawContent = rawContent.Remove(rawContent.Length - 1);
            }

            Uri? url = CreateUri(rawContent, hasShortenedContent);

            if (url == null)
            {
                return null;
            }

            return new WinGetLink(entryName, rawContent, hasShortenedContent, url);
        }

        /// <summary>
        /// Creates and returns a <see cref="System.Uri"/> instance from the content of the class.
        /// </summary>
        /// <returns>
        /// The created <see cref="System.Uri"/> instance.
        /// </returns>
        private static Uri? CreateUri(string rawContent, bool hasShortenedContent)
        {
            if (!hasShortenedContent)
            {
                Uri.TryCreate(rawContent, UriKind.Absolute, out Uri? uri);
                return uri;
            }

            // Fallback for an incomplete uri.
            Uri.TryCreate(TrimLastUriPart(rawContent), UriKind.Absolute, out Uri? shortenedUri);
            return shortenedUri;
        }

        /// <summary>
        /// Removes the last part from the given URI.
        /// </summary>
        /// <param name="uri">
        /// <see cref="System.String"/> containing the URI that sould be trimed.
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/> containing the trimed URI.
        /// </returns>
        private static string TrimLastUriPart(string uri)
        {
            int lastSeparatorIndex = -1;
            for (int i = 0; i < uri.Length; i++)
            {
                if (uri[i].Equals('/'))
                {
                    lastSeparatorIndex = i;
                }
            }

            if (lastSeparatorIndex > -1)
            {
                return uri.Remove(lastSeparatorIndex);
            }

            return uri;
        }
    }
}
