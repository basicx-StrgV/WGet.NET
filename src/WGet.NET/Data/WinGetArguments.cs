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
        internal enum FileType
        {
            None,
            Normal,
            ImportFile,
            ExportFile
        }

        /// <summary>
        /// Gets the generated arguments.
        /// </summary>
        public string Arguments
        {
            get
            {
                return _arguments;
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

        private readonly FileType _fileType;

        private string _arguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetArguments"/> class.
        /// </summary>
        /// <param name="baseCmd">
        /// The base cmd of the arguments.
        /// </param>
        /// <param name="fileType">
        /// File type for file based cmd's. The given value will influence the output of the 'File()' method;
        /// </param>
        internal WinGetArguments(string baseCmd, FileType fileType = FileType.None)
        {
            _arguments = baseCmd;
            _fileType = fileType;
        }

        //---Base Cmd's--------------------------------------------------------------------------------
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
        /// Creates a new winget arguments object with "export" as the base cmd.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public static WinGetArguments Export()
        {
            return new WinGetArguments("export", FileType.ExportFile);
        }

        /// <summary>
        /// Creates a new winget arguments object with "import" as the base cmd.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public static WinGetArguments Import()
        {
            return new WinGetArguments("import", FileType.ImportFile);
        }

        /// <summary>
        /// Creates a new winget arguments object with "hash" as the base cmd.
        /// </summary>
        /// <returns>
        /// The created <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public static WinGetArguments Hash()
        {
            return new WinGetArguments("hash", FileType.Normal);
        }

        //---Flags-------------------------------------------------------------------------------------
        /// <summary>
        /// Adds a query to the arguments.
        /// </summary>
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
        /// <param name="file">
        /// The <see cref="System.String"/> that contains the source name.
        /// </param>
        /// <returns>
        /// The updated <see cref="WGetNET.WinGetArguments"/> object.
        /// </returns>
        public WinGetArguments File(string file)
        {
            switch (_fileType)
            {
                case FileType.Normal:
                    _arguments += $" --file \"{file}\"";
                    break;
                case FileType.ImportFile:
                    _arguments += $" --import-file \"{file}\"";
                    break;
                case FileType.ExportFile:
                    _arguments += $" --output \"{file}\"";
                    break;
            }

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


        //---Others------------------------------------------------------------------------------------
        /// <inheritdoc/>
        public override string ToString()
        {
            return _arguments;
        }
    }
}
