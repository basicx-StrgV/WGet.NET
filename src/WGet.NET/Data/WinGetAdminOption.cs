//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using WGetNET.Abstractions;
using WGetNET.HelperClasses;

namespace WGetNET
{
    /// <summary>
    /// Represents a winget admin settings entry.
    /// </summary>
    public sealed class WinGetAdminOption : WinGetInfoEntry
    {
        /// <summary>
        /// Gets if the admin setting is enabled.
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
        private WinGetAdminOption(string entryName, string rawContent, bool hasShortenedContent, bool isEnabled) : base(entryName, rawContent, hasShortenedContent)
        {
            _isEnabled = isEnabled;
        }

        /// <summary>
        /// Creates a new <see cref="WGetNET.WinGetAdminOption"/> instance.
        /// </summary>
        /// <param name="entryName">The name of the settings entry.</param>
        /// <param name="rawContent">The content of the settings entry.</param>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetAdminOption"/> instance or <see langword="null"/> if parsing the data failed.
        /// </returns>
        internal static WinGetAdminOption? Create(string entryName, string rawContent)
        {
            bool hasShortenedContent = ProcessOutputReader.CheckShortenedValue(rawContent);

            if (hasShortenedContent)
            {
                // Remove the char at the end of the shortened content.
                rawContent = rawContent.Remove(rawContent.Length - 1);
            }

            bool isEnabled = false;
            bool parsed = false;

            if (!hasShortenedContent)
            {
                switch (rawContent.ToUpper())
                {
                    case "ENABLED":
                        isEnabled = true;
                        parsed = true;
                        break;
                    case "DISABLED":
                        isEnabled = false;
                        parsed = true;
                        break;
                }
            }
            else
            {
                // Try to parse data with smalles amount of information possible.
                if (rawContent.ToUpper().StartsWith("ENA"))
                {
                    isEnabled = true;
                    parsed = true;
                }
                else if (rawContent.ToUpper().StartsWith("DIS"))
                {
                    isEnabled = false;
                    parsed = true;
                }
            }

            if (!parsed)
            {
                return null;
            }

            return new WinGetAdminOption(entryName, rawContent, hasShortenedContent, isEnabled);
        }
    }
}
