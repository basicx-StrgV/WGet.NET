//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
namespace WGetNET
{
    /// <summary>
    /// Represents a basic WinGet info entry
    /// </summary>
    public class WinGetInfoEntry
    {
        /// <summary>
        /// Gets the name of this info entry.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Gets the content of this info entry.
        /// </summary>
        public string Content
        {
            get
            {
                return _content;
            }
        }

        /// <summary>
        /// Gets if content of the package is shortened.
        /// </summary>
        public bool HasShortenedContent
        {
            get
            {
                return _hasShortenedContent;
            }
        }

        private protected readonly string _name;
        private protected readonly string _content;
        private protected readonly bool _hasShortenedContent;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetInfoEntry"/> class.
        /// </summary>
        /// <param name="name">The name of the info entry.</param>
        /// <param name="content">The content of the info entry.</param>
        internal WinGetInfoEntry(string name, string content)
        {
            _name = name;
            _content = content;
            _hasShortenedContent = CheckShortenedContent(content);
            if (_hasShortenedContent)
            {
                // Remove the char at the end of the shortened content.
                _content = _content.Remove(_content.Length - 1);
            }
        }

        /// <summary>
        /// Checks if the content is possibly shortened.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the content is shortened or <see langword="false"/> if not.
        /// </returns>
        private static bool CheckShortenedContent(string content)
        {
            // Char 8230 is at the end of the shortened content if UTF-8 encoding is used.
#if NETCOREAPP3_1_OR_GREATER
            if (content.EndsWith((char)8230))
            {
                return true;
            }
#elif NETSTANDARD2_0
            if (content.EndsWith(((char)8230).ToString()))
            {
                return true;
            }
#endif

            return false;
        }
    }
}
