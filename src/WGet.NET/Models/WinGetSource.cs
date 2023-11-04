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
    public class WinGetSource
    {
        /// <summary>
        /// Gets or sets the name of the source.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value is null)
                {
                    _name = string.Empty;
                }
                else
                {
                    _name = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the url of the source.
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
                if (value is null)
                {
                    _url = string.Empty;
                }
                else
                {
                    _url = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the type of the source.
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
                if (value is null)
                {
                    _type = string.Empty;
                }
                else
                {
                    _type = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the data of the source.
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
                if (value is null)
                {
                    _data = string.Empty;
                }
                else
                {
                    _data = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the identifier of the source.
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
                if (value is null)
                {
                    _identifier = string.Empty;
                }
                else
                {
                    _identifier = value;
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
                if ((_name.Length + _url.Length + _type.Length + _data.Length + _identifier.Length) > 0)
                {
                    return false;
                }
                return true;
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
        public WinGetSource()
        {
            // Provide empty constructor for xlm docs
        }
    }
}
