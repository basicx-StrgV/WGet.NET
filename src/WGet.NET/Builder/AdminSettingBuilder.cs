﻿//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using WGetNET.Abstractions;

namespace WGetNET.Builder
{
    /// <summary>
    /// Builder to create a new <see cref="WGetNET.WinGetAdminSetting"/> instance.
    /// </summary>
    internal class AdminSettingBuilder : WinGetObjectBuilder<WinGetAdminSetting?>
    {
        private string _entryName = string.Empty;
        private string _rawContent = string.Empty;
        private bool _hasShortenedContent = false;
        private bool? _isEnabled = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.Builder.AdminSettingBuilder"/> class.
        /// </summary>
        public AdminSettingBuilder()
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

            _isEnabled = ParseToBool(_rawContent, _hasShortenedContent);
        }

        /// <summary>
        /// Adds the status of admin settings.
        /// </summary>
        /// <remarks>
        /// There is no need to add raw content after using this method.
        /// Using <see cref="AdminSettingBuilder.AddRawContent(string)"/> will override this value again.
        /// </remarks>
        /// <param name="status">
        /// The status of the admin setting.
        /// </param>
        public void AddStatus(bool status)
        {
            _isEnabled = status;

            // Set the raw content to a value that could be parsed.
            if (_isEnabled.Value)
            {
                _rawContent = "Enabled";
            }
            else
            {
                _rawContent = "Disabled";
            }
        }

        /// <summary>
        /// Returns a <see cref="WGetNET.WinGetAdminSetting"/> instance from data provided to the builder.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetAdminSetting"/> instance, or <see langword="null"/> if the provided data failed to be parsed.
        /// </returns>
        public override WinGetAdminSetting? GetInstance()
        {
            if (!_isEnabled.HasValue)
            {
                return null;
            }

            return new WinGetAdminSetting(_entryName, _rawContent, _hasShortenedContent, _isEnabled.Value);
        }

        /// <inheritdoc/>
        public override void Clear()
        {
            _entryName = string.Empty;
            _rawContent = string.Empty;
            _hasShortenedContent = false;
            _isEnabled = false;
        }

        /// <summary>
        /// Parses the raw content to a <see cref="System.Boolean"/> value.
        /// </summary>
        /// <param name="rawContent">
        /// A <see cref="System.String"/> containing the raw content.
        /// </param>
        /// <param name="hasShortenedContent">
        /// Indicates if the content is shortened or not.
        /// </param>
        /// <returns>
        /// The paresed <see cref="System.Boolean"/> value or <see langword="null"/> if parsing failed.
        /// </returns>
        private bool? ParseToBool(string rawContent, bool hasShortenedContent)
        {
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
                // But the value is short enough, so it should not come to this.
#if NETCOREAPP3_1_OR_GREATER
                if (rawContent.ToUpper().StartsWith('E'))
                {
                    isEnabled = true;
                    parsed = true;
                }
                else if (rawContent.ToUpper().StartsWith('D'))
                {
                    isEnabled = false;
                    parsed = true;
                }
#elif NETSTANDARD2_0
                if (rawContent.ToUpper().StartsWith("E"))
                {
                    isEnabled = true;
                    parsed = true;
                }
                else if (rawContent.ToUpper().StartsWith("D"))
                {
                    isEnabled = false;
                    parsed = true;
                }
#endif
            }

            if (!parsed)
            {
                return null;
            }

            return isEnabled;
        }
    }
}
