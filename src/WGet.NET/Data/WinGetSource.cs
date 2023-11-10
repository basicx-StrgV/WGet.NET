//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using WGetNET.Models;
using WGetNET.HelperClasses;

namespace WGetNET
{
    /// <summary>
    /// Represents a winget source.
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
        }

        /// <summary>
        /// Gets the url of the source.
        /// </summary>
        public string Url
        {
            get
            {
                return _url;
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
        }

        /// <summary>
        /// Gets if the object is empty.
        /// </summary>
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

        private readonly string _name;
        private readonly string _url;
        private readonly string _type;
        private readonly string _data;
        private readonly string _identifier;

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

        /// <summary>
        /// Creates a new instance of the <see cref="WGetNET.WinGetSource"/> class and returns it.
        /// </summary>
        /// <param name="name">The name of the source.</param>
        /// <param name="identifier">The identifier of the source.</param>
        /// <param name="url">The URL of the source.</param>
        /// <param name="type">The type identifier for the source.</param>
        /// <returns>
        /// The created instance of the <see cref="WGetNET.WinGetSource"/> class.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public WinGetSource Create(string name, string identifier, string url, string type)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(identifier, "identifier");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(url, "url");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(type, "type");

            return new WinGetSource(name, url, type, null, identifier);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="WGetNET.WinGetSource"/> class and returns it.
        /// </summary>
        /// <param name="name">The name of the source.</param>
        /// <param name="identifier">The identifier of the source.</param>
        /// <param name="url">The URL of the source.</param>
        /// <param name="type">The type identifier for the source.</param>
        /// <param name="data">The data field of the source.</param>
        /// <returns>
        /// The created instance of the <see cref="WGetNET.WinGetSource"/> class.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null or empty.
        /// </exception>
        public WinGetSource Create(string name, string identifier, string url, string type, string data)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(identifier, "identifier");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(url, "url");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(type, "type");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(data, "data");

            return new WinGetSource(name, url, type, data, identifier);
        }

        /// <summary>
        /// Creates a <see cref="WGetNET.WinGetSource"/> instance from a <see cref="WGetNET.Models.SourceModel"/> instance.
        /// </summary>
        /// <param name="model">The <see cref="WGetNET.Models.SourceModel"/> instance.</param>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetSource"/> instance.
        /// </returns>
        internal static WinGetSource FromSourceModel(SourceModel model)
        {
            return new WinGetSource(model.Name, model.Arg, model.Type, model.Data, model.Identifier);
        }
    }
}
