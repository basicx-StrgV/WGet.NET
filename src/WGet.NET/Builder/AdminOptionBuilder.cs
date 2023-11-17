//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using WGetNET.Abstractions;
using WGetNET.HelperClasses;

namespace WGetNET.Builder
{
    /// <summary>
    /// Builder to create a new <see cref="WGetNET.WinGetAdminOption"/> instance.
    /// </summary>
    internal class AdminOptionBuilder : WinGetObjectBuilder<WinGetAdminOption?>
    {
        private string _entryName = string.Empty;
        private string _rawContent = string.Empty;
        private bool _hasShortenedContent = false;
        private bool _isEnabled = false;

        private bool _parsed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.Builder.AdminOptionBuilder"/> class.
        /// </summary>
        public AdminOptionBuilder()
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
            _hasShortenedContent = ProcessOutputReader.CheckShortenedValue(rawContent);

            if (_hasShortenedContent)
            {
                // Remove the char at the end of the shortened content.
                _rawContent = rawContent.Remove(rawContent.Length - 1);
            }
            else
            {
                _rawContent = rawContent;
            }

            bool? isEnabled = ParseToBool(_rawContent, _hasShortenedContent);
            if (isEnabled != null)
            {
                _isEnabled = isEnabled.Value;
                _parsed = true;
            }
        }

        /// <summary>
        /// Returns a <see cref="WGetNET.WinGetAdminOption"/> instance from data provided to the builder.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetAdminOption"/> instance, or <see langword="null"/> if the provided data failed to be parsed.
        /// </returns>
        public override WinGetAdminOption? GetInstance()
        {
            if (!_parsed)
            {
                return null;
            }

            return new WinGetAdminOption(_entryName, _rawContent, _hasShortenedContent, _isEnabled);
        }

        /// <inheritdoc/>
        public override void Clear()
        {
            _entryName = string.Empty;
            _rawContent = string.Empty;
            _hasShortenedContent = false;
            _isEnabled = false;
        }

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
