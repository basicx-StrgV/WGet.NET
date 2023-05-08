//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System.Text.Json.Serialization;

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
        [JsonPropertyName("Name")]
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
        [JsonPropertyName("Arg")]
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
        /// <remarks>
        /// Will only be set on source export.
        /// </remarks>
        [JsonPropertyName("Type")]
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
        /// Gets or sets the data of the source.
        /// </summary>
        /// <remarks>
        /// Will only be set on source export.
        /// </remarks>
        [JsonPropertyName("Data")]
        public string SourceData
        {
            get
            {
                return _sourceData;
            }
            set
            {
                if (value is null)
                {
                    _sourceData = string.Empty;
                }
                else
                {
                    _sourceData = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the identifier of the source.
        /// </summary>
        /// <remarks>
        /// Will only be set on source export.
        /// </remarks>
        [JsonPropertyName("Identifier")]
        public string SourceIdentifier
        {
            get
            {
                return _sourceIdentifier;
            }
            set
            {
                if (value is null)
                {
                    _sourceIdentifier = string.Empty;
                }
                else
                {
                    _sourceIdentifier = value;
                }
            }
        }

        /// <summary>
        /// Gets if the object is empty.
        /// </summary>
        [JsonIgnore]
        public bool IsEmpty
        {
            get
            {
                if ((_sourceName.Length + _sourceUrl.Length + _sourceType.Length + _sourceData.Length + _sourceIdentifier.Length) > 0)
                {
                    return false;
                }
                return true;
            }
        }

        private string _sourceName = string.Empty;
        private string _sourceUrl = string.Empty;
        private string _sourceType = string.Empty;
        private string _sourceData = string.Empty;
        private string _sourceIdentifier = string.Empty;
    }
}
