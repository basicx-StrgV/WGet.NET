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
            get => _sourceName;
            set => _sourceName = string.IsNullOrWhiteSpace(value) ? string.Empty : value;
        }
        /// <summary>
        /// Gets or sets the url of the source.
        /// </summary>
        public string SourceUrl
        {
            get => _sourceUrl;
            set => _sourceUrl = string.IsNullOrWhiteSpace(value) ? string.Empty : value;
        }
        /// <summary>
        /// Gets or sets the type of the source.
        /// </summary>
        public string SourceType
        {
            get => _sourceType; 
            set => _sourceType = string.IsNullOrWhiteSpace(value) ? string.Empty : value;
        }
        /// <summary>
        /// Gets if the object is empty.
        /// </summary>
        public bool IsEmpty
        {
            get => string.IsNullOrWhiteSpace(_sourceName) || string.IsNullOrWhiteSpace(_sourceUrl);
        }

        private string _sourceName = string.Empty;
        private string _sourceUrl = string.Empty;
        private string _sourceType = string.Empty;
    }
}
