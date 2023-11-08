//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
#if NETCOREAPP3_1_OR_GREATER
using System.Text.Json.Serialization;
#elif NETSTANDARD2_0
using Newtonsoft.Json;
#endif

namespace WGetNET
{
    /// <summary>
    /// Represents a winget source
    /// </summary>
    public class WinGetSource: IWinGetObject
    {
        /// <summary>
        /// Gets the name of the source.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != null)
                {
                    _name = value;
                }
                else
                {
                    _name = string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets the url of the source.
        /// </summary>
#if NETCOREAPP3_1_OR_GREATER
        [JsonPropertyName("Arg")]
#elif NETSTANDARD2_0
        [JsonProperty("Arg")]
#endif
        public string Url
        {
            get
            {
                return _url;
            }
            set
            {
                if (value != null)
                {
                    _url = value;
                }
                else
                {
                    _url = string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets the type of the source.
        /// </summary>
        /// <remarks>
        /// Will only be set on source export.
        /// </remarks>
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (value != null)
                {
                    _type = value;
                }
                else
                {
                    _type = string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets the data of the source.
        /// </summary>
        /// <remarks>
        /// Will only be set on source export.
        /// </remarks>
        public string Data
        {
            get
            {
                return _data;
            }
            set
            {
                if (value != null)
                {
                    _data = value;
                }
                else
                {
                    _data = string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets the identifier of the source.
        /// </summary>
        /// <remarks>
        /// Will only be set on source export.
        /// </remarks>
        public string Identifier
        {
            get
            {
                return _identifier;
            }
            set
            {
                if (value != null)
                {
                    _identifier = value;
                }
                else
                {
                    _identifier = string.Empty;
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
                if (string.IsNullOrWhiteSpace(_name) && 
                    string.IsNullOrWhiteSpace(_url) && 
                    string.IsNullOrWhiteSpace(_type) && 
                    string.IsNullOrWhiteSpace(_data) && 
                    string.IsNullOrWhiteSpace(_identifier))
                {
                    return true;
                }
                return false;
            }
        }

        private string _name = string.Empty;
        private string _url = string.Empty;
        private string _type = string.Empty;
        private string _data = string.Empty;
        private string _identifier = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetSource"/> class.
        /// </summary>
        internal WinGetSource()
        {
            // Empty constructor for json parsing.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetSource"/> class.
        /// </summary>
        /// <param name="name">The name of the source.</param>
        /// <param name="url">The url to the source.</param>
        /// <param name="type">Type identifier of the source.</param>
        /// <param name="data">Data of the source source. This field is only used by some sources.</param>
        /// <param name="identifier">The identifier of the package</param>
        internal WinGetSource(string name, string url, string? type = null, string? data = null, string? identifier = null)
        {
            _name = name;
            _url = url;

            if (type != null)
            {
                _type = type;
            }
            else
            {
                _type = string.Empty;
            }

            if (data != null)
            {
                _data = data;
            }
            else
            {
                _data = string.Empty;
            }

            if (identifier != null)
            {
                _identifier = identifier;
            }
            else
            {
                _identifier = string.Empty;
            }
        }
    }
}
