//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;

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
        public Uri? Url
        {
            get
            {
                return _url;
            }
        }

        private readonly Uri? _url;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetLink"/> class.
        /// </summary>
        /// <param name="name">The name of the settings entry.</param>
        /// <param name="content">The content of the settings entry.</param>
        internal WinGetLink(string name, string content) : base(name, content)
        {
            _url = CreateUri();
        }

        /// <summary>
        /// Creates and returns a <see cref="System.Uri"/> instance from the content of the class.
        /// </summary>
        /// <returns>
        /// The created <see cref="System.Uri"/> instance.
        /// </returns>
        private Uri? CreateUri()
        {
            if (!_hasShortenedContent)
            {
                Uri.TryCreate(_content, UriKind.Absolute, out Uri? uri);
                return uri;
            }

            // Fallback for an incomplete uri.
            Uri.TryCreate(TrimLastUriPart(_content), UriKind.Absolute, out Uri? shortenedUri);
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
        private string TrimLastUriPart(string uri)
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
