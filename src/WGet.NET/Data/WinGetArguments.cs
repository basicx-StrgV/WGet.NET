//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
namespace WGetNET
{
    /// <summary>
    /// Represents a winget arguments string for different winget actions.
    /// </summary>
    internal class WinGetArguments : IWinGetObject
    {
        /// <summary>
        /// The <see cref="WGetNET.WinGetArguments.WinGetAction"/> <see langword="enum"/> can be used to specify the winget action, 
        /// to influence the arguments generation with some flags.
        /// </summary>
        internal enum WinGetAction
        {
            Unspecified,
            Download,
            Export,
            Import,
            Hash,
        }

        /// <summary>
        /// Gets the generated arguments.
        /// </summary>
        public string Arguments
        {
            get
            {
                return _arguments.Trim();
            }
        }

        /// <inheritdoc/>
        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrWhiteSpace(_arguments);
            }
        }

        private readonly WinGetAction _action;

        private string _arguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetArguments"/> class.
        /// </summary>
        /// <param name="baseCmd">
        /// The base cmd of the arguments.
        /// </param>
        /// <param name="action">
        /// Specifies the base cmd to change the behavior of some flags. 
        /// For most base cmd’s, no specification is needed. 
        /// Default Value = "Unspecified"
        /// </param>
        internal WinGetArguments(string baseCmd, WinGetAction action = WinGetAction.Unspecified)
        {
            _arguments = baseCmd;
            _action = action;
        }

        //---Base Cmd's--------------------------------------------------------------------------------
        /// <summary>
        /// Creates a new winget arguments object with no base cmd. 
        /// Used for direct callin of the winget cmd with flags.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public static WinGetArguments WinGet()
        {
            return new WinGetArguments("");
        }

        /// <summary>
        /// Creates a new winget arguments object with "settings" as the base cmd.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public static WinGetArguments Settings()
        {
            return new WinGetArguments("settings");
        }

        /// <summary>
        /// Creates a new winget arguments object with "settings export" as the base cmd.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public static WinGetArguments SettingsExport()
        {
            return new WinGetArguments("settings export");
        }

        /// <summary>
        /// Creates a new winget arguments object with "list" as the base cmd.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public static WinGetArguments List()
        {
            return new WinGetArguments("list");
        }

        /// <summary>
        /// Creates a new winget arguments object with "search" as the base cmd.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public static WinGetArguments Search()
        {
            return new WinGetArguments("search");
        }

        /// <summary>
        /// Creates a new winget arguments object with "install" as the base cmd.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public static WinGetArguments Install()
        {
            return new WinGetArguments("install");
        }

        /// <summary>
        /// Creates a new winget arguments object with "upgrade" as the base cmd.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public static WinGetArguments Upgrade()
        {
            return new WinGetArguments("upgrade");
        }

        /// <summary>
        /// Creates a new winget arguments object with "uninstall" as the base cmd.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public static WinGetArguments Uninstall()
        {
            return new WinGetArguments("uninstall");
        }

        /// <summary>
        /// Creates a new winget arguments object with "download" as the base cmd.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public static WinGetArguments Download()
        {
            return new WinGetArguments("download", WinGetAction.Download);
        }

        /// <summary>
        /// Creates a new winget arguments object with "repair" as the base cmd.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public static WinGetArguments Repair()
        {
            return new WinGetArguments("repair");
        }

        /// <summary>
        /// Creates a new winget arguments object with "export" as the base cmd.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public static WinGetArguments Export()
        {
            return new WinGetArguments("export", WinGetAction.Export);
        }

        /// <summary>
        /// Creates a new winget arguments object with "import" as the base cmd.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public static WinGetArguments Import()
        {
            return new WinGetArguments("import", WinGetAction.Import);
        }

        /// <summary>
        /// Creates a new winget arguments object with "hash" as the base cmd.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public static WinGetArguments Hash()
        {
            return new WinGetArguments("hash", WinGetAction.Hash);
        }

        /// <summary>
        /// Creates a new winget arguments object with "pin list" as the base cmd.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public static WinGetArguments PinList()
        {
            return new WinGetArguments("pin list");
        }

        /// <summary>
        /// Creates a new winget arguments object with "pin add" as the base cmd.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public static WinGetArguments PinAdd()
        {
            return new WinGetArguments("pin add");
        }

        /// <summary>
        /// Creates a new winget arguments object with "pin remove" as the base cmd.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public static WinGetArguments PinRemove()
        {
            return new WinGetArguments("pin remove");
        }

        /// <summary>
        /// Creates a new winget arguments object with "pin reset" as the base cmd.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public static WinGetArguments PinReset()
        {
            return new WinGetArguments("pin reset");
        }

        /// <summary>
        /// Creates a new winget arguments object with "source add" as the base cmd.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public static WinGetArguments SourceAdd()
        {
            return new WinGetArguments("source add");
        }

        /// <summary>
        /// Creates a new winget arguments object with "source remove" as the base cmd.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public static WinGetArguments SourceRemove()
        {
            return new WinGetArguments("source remove");
        }

        /// <summary>
        /// Creates a new winget arguments object with "source update" as the base cmd.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public static WinGetArguments SourceUpdate()
        {
            return new WinGetArguments("source update");
        }

        /// <summary>
        /// Creates a new winget arguments object with "source reset" as the base cmd.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public static WinGetArguments SourceReset()
        {
            return new WinGetArguments("source reset");
        }

        /// <summary>
        /// Creates a new winget arguments object with "source export" as the base cmd.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public static WinGetArguments SourceExport()
        {
            return new WinGetArguments("source export");
        }

        //---Flags-------------------------------------------------------------------------------------
        /// <summary>
        /// Adds a query to the arguments.
        /// </summary>
        /// <remarks>
        /// Mainly used for Package based actions.
        /// </remarks>
        /// <param name="query">
        /// The <see cref="System.String"/> that should be added as a query.
        /// </param>
        /// <returns>
        /// The updated <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public WinGetArguments Query(string query)
        {
            _arguments += $" --query \"{query}\"";
            return this;
        }

        /// <summary>
        /// Adds a source query to the arguments.
        /// </summary>
        /// <param name="source">
        /// The <see cref="System.String"/> that contains the source name.
        /// </param>
        /// <returns>
        /// The updated <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public WinGetArguments Source(string source)
        {
            _arguments += $" --source \"{source}\"";
            return this;
        }

        /// <summary>
        /// Adds a file path to the arguments.
        /// </summary>
        /// <remarks>
        /// Behavior depends on base cmd. If the base cmd does not read or write from or to a file, the function will do nothing.
        /// </remarks>
        /// <param name="file">
        /// The <see cref="System.String"/> that contains the file path.
        /// </param>
        /// <returns>
        /// The updated <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public WinGetArguments File(string file)
        {
            switch (_action)
            {
                case WinGetAction.Hash:
                    _arguments += $" --file \"{file}\"";
                    break;
                case WinGetAction.Import:
                    _arguments += $" --import-file \"{file}\"";
                    break;
                case WinGetAction.Export:
                    _arguments += $" --output \"{file}\"";
                    break;
            }

            return this;
        }

        /// <summary>
        /// Adds a directory path to the arguments.
        /// </summary>
        /// <remarks>
        /// Behavior depends on base cmd. If the base cmd does not need a directory, the function will do nothing.
        /// </remarks>
        /// <param name="directory">
        /// The <see cref="System.String"/> that contains the directory.
        /// </param>
        /// <returns>
        /// The updated <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public WinGetArguments Directory(string directory)
        {
            // Remove backslash chars at the end of the path,
            // because they are not needed and will interfere with winget argument string by negating the last quotation mark char,
            // used for encasing the directory path.
#if NETCOREAPP3_1_OR_GREATER
            while (directory.EndsWith('\\'))
            {
                directory = directory[..^1];
            }
#elif NETSTANDARD2_0
            while (directory.EndsWith("\\"))
            {
                directory = directory.Substring(0, directory.Length - 1);
            }
#endif

            switch (_action)
            {
                case WinGetAction.Download:
                    _arguments += $" --download-directory \"{directory}\"";
                    break;
            }

            return this;
        }

        /// <summary>
        /// Adds a enable action to the arguments.
        /// </summary>
        /// <param name="query">
        /// A <see cref="System.String"/> containing the option to enable.
        /// </param>
        /// <returns>
        /// The updated <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public WinGetArguments Enable(string query)
        {
            _arguments += $" --enable \"{query}\"";
            return this;
        }

        /// <summary>
        /// Adds a disable action to the arguments.
        /// </summary>
        /// <param name="query">
        /// A <see cref="System.String"/> containing the option to disable.
        /// </param>
        /// <returns>
        /// The updated <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public WinGetArguments Disable(string query)
        {
            _arguments += $" --disable \"{query}\"";
            return this;
        }

        /// <summary>
        /// Adds name data to the arguments.
        /// </summary>
        /// <remarks>
        /// Mainly used for Source based actions.
        /// </remarks>
        /// <param name="name">
        /// A <see cref="System.String"/> containing the name to add to the arguments.
        /// </param>
        /// <returns>
        /// The updated <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public WinGetArguments Name(string name)
        {
            _arguments += $" --name \"{name}\"";
            return this;
        }

        /// <summary>
        /// Adds arg (Source argument) data to the arguments.
        /// </summary>
        /// <remarks>
        /// Mainly used for Source based actions.
        /// </remarks>
        /// <param name="arg">
        /// A <see cref="System.String"/> containing the arg (Source argument) to add to the arguments.
        /// </param>
        /// <returns>
        /// The updated <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public WinGetArguments Arg(string arg)
        {
            _arguments += $" --arg \"{arg}\"";
            return this;
        }

        /// <summary>
        /// Adds type data to the arguments.
        /// </summary>
        /// <remarks>
        /// Mainly used for Source based actions.
        /// </remarks>
        /// <param name="type">
        /// A <see cref="System.String"/> containing the type data to add to the arguments.
        /// </param>
        /// <returns>
        /// The updated <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public WinGetArguments Type(string type)
        {
            _arguments += $" --type \"{type}\"";
            return this;
        }

        /// <summary>
        /// Adds a version query to the arguments.
        /// </summary>
        /// <param name="version">
        /// The <see cref="System.String"/> that contains the version.
        /// </param>
        /// <returns>
        /// The updated <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public WinGetArguments Version(string version)
        {
            _arguments += $" --version \"{version}\"";
            return this;
        }

        /// <summary>
        /// Adds a version query to the arguments.
        /// </summary>
        /// <returns>
        /// The updated <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public WinGetArguments Version()
        {
            _arguments += $" --version";
            return this;
        }

        /// <summary>
        /// Adds the '--exact' flag to the arguments.
        /// </summary>
        /// <returns>
        /// The updated <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public WinGetArguments Exact()
        {
            _arguments += " --exact";
            return this;
        }

        /// <summary>
        /// Adds the '--silent' flag to the arguments.
        /// </summary>
        /// <returns>
        /// The updated <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public WinGetArguments Silent()
        {
            _arguments += " --silent";
            return this;
        }

        /// <summary>
        /// Adds the '--all' flag to the arguments.
        /// </summary>
        /// <returns>
        /// The updated <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public WinGetArguments All()
        {
            _arguments += " --all";
            return this;
        }

        /// <summary>
        /// Adds the '--include-unknown' flag to the arguments.
        /// </summary>
        /// <returns>
        /// The updated <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public WinGetArguments IncludeUnknown()
        {
            _arguments += " --include-unknown";
            return this;
        }

        /// <summary>
        /// Adds the '--accept-source-agreements' flag to the arguments.
        /// </summary>
        /// <returns>
        /// The updated <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public WinGetArguments AcceptSourceAgreements()
        {
            _arguments += " --accept-source-agreements";
            return this;
        }

        /// <summary>
        /// Adds the '--accept-package-agreements' flag to the arguments.
        /// </summary>
        /// <returns>
        /// The updated <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public WinGetArguments AcceptPackageAgreements()
        {
            _arguments += " --accept-package-agreements";
            return this;
        }

        /// <summary>
        /// Adds the '--ignore-unavailable' flag to the arguments.
        /// </summary>
        /// <returns>
        /// The updated <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public WinGetArguments IgnoreUnavailable()
        {
            _arguments += " --ignore-unavailable";
            return this;
        }

        /// <summary>
        /// Adds the '--installed' flag to the arguments.
        /// </summary>
        /// <returns>
        /// The updated <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public WinGetArguments Installed()
        {
            _arguments += " --installed";
            return this;
        }

        /// <summary>
        /// Adds the '--blocking' flag to the arguments.
        /// </summary>
        /// <returns>
        /// The updated <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public WinGetArguments Blocking()
        {
            _arguments += " --blocking";
            return this;
        }

        /// <summary>
        /// Adds the '--force' flag to the arguments.
        /// </summary>
        /// <returns>
        /// The updated <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public WinGetArguments Force()
        {
            _arguments += " --force";
            return this;
        }

        /// <summary>
        /// Adds the '--info' flag to the arguments.
        /// </summary>
        /// <returns>
        /// The updated <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public WinGetArguments Info()
        {
            _arguments += " --info";
            return this;
        }

        //---Others------------------------------------------------------------------------------------
        /// <inheritdoc/>
        public override string ToString()
        {
            return _arguments.Trim();
        }
    }
}
