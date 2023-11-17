//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using WGetNET.Abstractions;

namespace WGetNET
{
    /// <summary>
    /// Represents a winget admin settings entry.
    /// </summary>
    public sealed class WinGetAdminOption : WinGetInfoEntry
    {
        /// <summary>
        /// Gets if the admin option is enabled.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
        }

        private readonly bool _isEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetAdminOption"/> class.
        /// </summary>
        /// <param name="entryName">The name of the settings entry.</param>
        /// <param name="rawContent">The content of the settings entry.</param>
        /// <param name="hasShortenedContent">Sets if the content is shortened or not.</param>
        /// <param name="isEnabled">Idicator for the setting state.</param>
        internal WinGetAdminOption(string entryName, string rawContent, bool hasShortenedContent, bool isEnabled) : base(entryName, rawContent, hasShortenedContent)
        {
            _isEnabled = isEnabled;
        }
    }
}
