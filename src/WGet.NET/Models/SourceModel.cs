//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
namespace WGetNET
{
    /// <summary>
    /// Represents a winget source for json parsing.
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
        /// Gets sets the type of the source.
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

        private string _name = string.Empty;
        private string _arg = string.Empty;
        private string _type = string.Empty;
        private string _data = string.Empty;
        private string _identifier = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.SourceModel"/> class.
        /// </summary>
        internal SourceModel()
        {
            // Empty constructor for json parsing.
        }
    }
}
