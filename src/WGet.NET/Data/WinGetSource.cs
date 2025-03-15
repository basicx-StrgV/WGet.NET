//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.Collections.Generic;
using WGetNET.Helper;
using WGetNET.Models;

namespace WGetNET
{
    /// <summary>
    /// Represents a winget source.
    /// </summary>
    public class WinGetSource : IWinGetObject, ICloneable
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
        /// Gets the URL/UNC of the source.
        /// </summary>
        public string Arg
        {
            get
            {
                return _arg;
            }
        }

        /// <summary>
        /// Gets the uri of the source.
        /// </summary>
        /// <remarks>
        /// <see langword="null"/> if <see cref="WGetNET.WinGetSource.Arg"/> can't be parsed to a <see cref="System.Uri"/> instance.
        /// </remarks>
        public Uri? Uri
        {
            get
            {
                return _uri;
            }
        }

        /// <summary>
        /// Gets the type of the source.
        /// </summary>
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
        public string Identifier
        {
            get
            {
                return _identifier;
            }
        }

        /// <summary>
        /// Gets whether the source was explicitly added.
        /// </summary>
        public bool Explicit
        {
            get
            {
                return _explicit;
            }
        }

        /// <summary>
        /// Gets the trust level of the source.
        /// </summary>
        public List<string> TrustLevel
        {
            get
            {
                return _trustLevel;
            }
        }


        /// <inheritdoc/>
        public bool IsEmpty
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_name) &&
                    string.IsNullOrWhiteSpace(_arg) &&
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
        private readonly string _arg;
        private readonly Uri? _uri;
        private readonly string _type;
        private readonly string _data;
        private readonly string _identifier;
        private readonly bool _explicit;
        private readonly List<string> _trustLevel;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetSource"/> class.
        /// </summary>
        /// <param name="name">The name of the source.</param>
        /// <param name="arg">The URL or UNC of the source.</param>
        /// <param name="uri">The URI of the source.</param>
        /// <param name="type">Type identifier of the source.</param>
        /// <param name="data">Data of the source source. This field is only used by some sources.</param>
        /// <param name="identifier">The identifier of the package</param>
        /// <param name="explicitSource">Indicates if the source was explicitly added.</param>
        /// <param name="trustLevel">Trust level of the source.</param>
        internal WinGetSource(string name, string arg, Uri? uri, string type, string identifier, bool explicitSource = false, List<string>? trustLevel = null, string? data = null)
        {
            _name = name;
            _arg = arg;
            _uri = uri;
            _type = type;
            _identifier = identifier;
            _explicit = explicitSource;

            if (trustLevel != null)
            {
                _trustLevel = trustLevel;
            }
            else
            {
                _trustLevel = new List<string>();
            }

            if (data != null)
            {
                _data = data;
            }
            else
            {
                _data = string.Empty;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="WGetNET.WinGetSource"/> class and returns it.
        /// </summary>
        /// <param name="name">The name of the source.</param>
        /// <param name="identifier">The identifier of the source.</param>
        /// <param name="arg">The URL or UNC of the source.</param>
        /// <param name="type">The type identifier for the source.</param>
        /// <returns>
        /// The created instance of the <see cref="WGetNET.WinGetSource"/> class.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        public static WinGetSource Create(string name, string identifier, string arg, string type)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(identifier, "identifier");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(arg, "arg");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(type, "type");

            Uri.TryCreate(arg, UriKind.Absolute, out Uri? uri);

            return new WinGetSource(name, arg, uri, type, identifier);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="WGetNET.WinGetSource"/> class and returns it.
        /// </summary>
        /// <param name="name">The name of the source.</param>
        /// <param name="identifier">The identifier of the source.</param>
        /// <param name="arg">The URL or UNC of the source.</param>
        /// <param name="type">The type identifier for the source.</param>
        /// <param name="data">The data field of the source.</param>
        /// <returns>
        /// The created instance of the <see cref="WGetNET.WinGetSource"/> class.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// A provided argument is empty.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// A provided argument is null.
        /// </exception>
        public static WinGetSource Create(string name, string identifier, string arg, string type, string data)
        {
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(name, "name");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(identifier, "identifier");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(arg, "url");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(type, "type");
            ArgsHelper.ThrowIfStringIsNullOrWhiteSpace(data, "data");

            Uri.TryCreate(arg, UriKind.Absolute, out Uri? uri);

            return new WinGetSource(name, arg, uri, type, identifier, data: data);
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
            Uri.TryCreate(model.Arg, UriKind.Absolute, out Uri? uri);

            return new WinGetSource(model.Name, model.Arg, uri, model.Type, model.Identifier, model.Explicit, model.TrustLevel, model.Data);
        }

        /// <inheritdoc/>
        public object Clone()
        {
            return new WinGetSource(
                    _name,
                    _arg,
                    _uri,
                    _type,
                    _identifier,
                    _explicit,
                    _trustLevel,
                    _data
                );
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return _name;
        }
    }
}
