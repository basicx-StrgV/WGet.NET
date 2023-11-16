//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
namespace WGetNET.Abstractions
{
    /// <summary>
    /// Represents a basic winget info entry.
    /// </summary>
    public abstract class WinGetInfoEntry : IWinGetObject
    {
        /// <summary>
        /// Gets the name of the info entry.
        /// </summary>
        public string EntryName
        {
            get
            {
                return _entryName;
            }
        }

        /// <summary>
        /// Gets the raw content of the info entry.
        /// </summary>
        public string RawContent
        {
            get
            {
                return _rawContent;
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

        /// <summary>
        /// Gets if the object is empty.
        /// </summary>
        public virtual bool IsEmpty
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_entryName) && string.IsNullOrWhiteSpace(_rawContent))
                {
                    return true;
                }
                return false;
            }
        }

        private protected readonly string _entryName;
        private protected readonly string _rawContent;
        private protected readonly bool _hasShortenedContent;

        /// <summary>
        /// Initializes a new instance of the <see cref="WinGetInfoEntry"/> class.
        /// </summary>
        /// <param name="entryName">The name of the info entry.</param>
        /// <param name="rawContent">The content of the info entry.</param>
        /// <param name="hasShortenedContent">Sets if the content is shortened or not.</param>
        private protected WinGetInfoEntry(string entryName, string rawContent, bool hasShortenedContent)
        {
            _entryName = entryName;
            _rawContent = rawContent;
            _hasShortenedContent = hasShortenedContent;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return _entryName;
        }
    }
}
