//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System.Collections.Generic;

namespace WGetNET.Models
{
    /// <summary>
    /// Represents a winget source for JSON parsing.
    /// </summary>
    internal class SourceModel
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
        /// Gets or sets the arg of the source.
        /// </summary>
        public string Arg
        {
            get
            {
                return _arg;
            }
            set
            {
                if (value != null)
                {
                    _arg = value;
                }
                else
                {
                    _arg = string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets or sets the type of the source.
        /// </summary>
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
        /// Gets or sets the data of the source.
        /// </summary>
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
        /// Gets or sets the identifier of the source.
        /// </summary>
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
        /// Gets or sets whether the source was explicitly added.
        /// </summary>
        public bool Explicit { get; set; }

        /// <summary>
        /// Gets or sets the trust level of the source.
        /// </summary>
        public List<string> TrustLevel
        {
            get
            {
                return _trustLevel;
            }
            set
            {
                if (value != null)
                {
                    _trustLevel = value;
                }
                else
                {
                    _trustLevel = new List<string>();
                }
            }
        }

        private string _name = string.Empty;
        private string _arg = string.Empty;
        private string _type = string.Empty;
        private string _data = string.Empty;
        private string _identifier = string.Empty;
        private List<string> _trustLevel = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.Models.SourceModel"/> class.
        /// </summary>
        internal SourceModel()
        {
            // Empty constructor for JSON parsing.
        }

        /// <summary>
        /// Creates a <see cref="WGetNET.Models.SourceModel"/> instance from a <see cref="WGetNET.WinGetSource"/> instance.
        /// </summary>
        /// <param name="source">The <see cref="WGetNET.WinGetSource"/> instance.</param>
        /// <returns>
        /// The created <see cref="WGetNET.Models.SourceModel"/> instance.
        /// </returns>
        public static SourceModel FromWinGetSource(WinGetSource source)
        {
            return new SourceModel()
            {
                Name = source.Name,
                Arg = source.Arg,
                Type = source.Type,
                Data = source.Data,
                Identifier = source.Identifier,
                Explicit = source.Explicit,
                TrustLevel = source.TrustLevel
            };
        }
    }
}
