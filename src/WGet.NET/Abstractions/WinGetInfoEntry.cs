//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;

namespace WGetNET.Abstractions
{
    /// <summary>
    /// Represents a basic winget info entry.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the derived class.
    /// </typeparam>
    public abstract class WinGetInfoEntry<T> : IWinGetObject, IEquatable<T>, ICloneable where T : WinGetInfoEntry<T>
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

        // \cond PRIVATE
        private protected readonly string _entryName;
        private protected readonly string _rawContent;
        private protected readonly bool _hasShortenedContent;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.Abstractions.WinGetInfoEntry{T}"/> class.
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
        // \endcond

        /// <inheritdoc/>
        public override string ToString()
        {
            return _entryName;
        }

        /// <inheritdoc/>
        public abstract bool Equals(T? other);

        /// <inheritdoc/>
        public abstract object Clone();
    }
}
