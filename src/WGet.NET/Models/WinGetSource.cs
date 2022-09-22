//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
namespace WGetNET
{
    /// <summary>
    /// Represents a winget source
    /// </summary>
    public class WinGetSource
    {
        /// <summary>
        /// Gets or sets the name of the source.
        /// </summary>
        public string SourceName 
        {
            get
            {
                return _sourceName;
            }
            set
            {
                if (value is null)
                {
                    _sourceName = string.Empty;
                }
                else
                {
                    _sourceName = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the url of the source.
        /// </summary>
        public string SourceUrl 
        {
            get
            {
                return _sourceUrl;
            }
            set
            {
                if (value is null)
                {
                    _sourceUrl = string.Empty;
                }
                else
                {
                    _sourceUrl = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the type of the source.
        /// </summary>
        public string SourceType 
        {
            get
            {
                return _sourceType;
            }
            set
            {
                if (value is null)
                {
                    _sourceType = string.Empty;
                }
                else
                {
                    _sourceType = value;
                }
            }
        }
        /// <summary>
        /// Gets if the object is empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                if ((_sourceName.Length + _sourceUrl.Length + _sourceType.Length) > 0)
                {
                    return false;
                }
                return true;
            }
        }

        private string _sourceName = string.Empty;
        private string _sourceUrl = string.Empty;
        private string _sourceType = string.Empty;
    }
}
