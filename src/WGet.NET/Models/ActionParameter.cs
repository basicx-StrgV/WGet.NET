namespace WGetNET
{
    /// <summary>
    /// The <see cref="WGetNET.ActionParameter"/> class is used for the configuration of a winget action.
    /// </summary>
    public class ActionParameter
    {
        /// <summary>
        /// Get or set if unavailable packages should be ignored.
        /// (Default = false)
        /// </summary>
        /// <remarks>
        /// Used by: "import"
        /// </remarks>
        [ActionUsed(MainAction.IMPORT)]
        public bool IgnoreUnavailable { get; set; } = false;
        /// <summary>
        /// Get or set if package versions should be ignored.
        /// (Default = false)
        /// </summary>
        /// <remarks>
        /// Used by: "import"
        /// </remarks>
        [ActionUsed(MainAction.IMPORT)]
        public bool IgnoreVersions { get; set; } = false;
        /// <summary>
        /// Get or set if the package agreements should be automaticly accepted.
        /// (Default = false)
        /// </summary>
        /// <remarks>
        /// Used by: "import", "install", "upgrade"
        /// </remarks>
        [ActionUsed(MainAction.IMPORT, MainAction.INSTALL, MainAction.UPGRADE)]
        public bool AcceptPackageAgreements { get; set; } = false;
        /// <summary>
        /// Get or set if the source agreements should be automaticly accepted.
        /// (Default = false)
        /// </summary>
        /// <remarks>
        /// Used by: "import", "export", "install", "search", "list", "upgrade", "uninstall", "source add"
        /// </remarks>
        [ActionUsed(MainAction.IMPORT, MainAction.EXPORT, MainAction.INSTALL, MainAction.SEARCH, MainAction.LIST, MainAction.UPGRADE, MainAction.UNINSTALL, MainAction.SOURCE_ADD)]
        public bool AcceptSourceAgreements { get; set; } = false;
        /// <summary>
        /// Get or set if the package version should be included.
        /// (Default = false)
        /// </summary>
        /// <remarks>
        /// Used by: "export"
        /// </remarks>
        [ActionUsed(MainAction.EXPORT)]
        public bool IncludeVersions { get; set; } = false;
        /// <summary>
        /// Get or set if the package id should used for the action.
        /// (Default = false)
        /// </summary>
        /// <remarks>
        /// Used by: "install", "search", "list", "upgrade", "uninstall"
        /// </remarks>
        [ActionUsed(MainAction.INSTALL, MainAction.SEARCH, MainAction.LIST, MainAction.UPGRADE, MainAction.UNINSTALL)]
        public bool UseId { get; set; } = false;
        /// <summary>
        /// Get or set if the package name should used for the action.
        /// (Default = false)
        /// This will be ignored if <see cref="WGetNET.ActionParameter.UseId"/> is set to <see langword="true"/>.
        /// </summary>
        /// <remarks>
        /// Used by: "install", "search", "list", "upgrade", "uninstall"
        /// </remarks>
        [ActionUsed(MainAction.INSTALL, MainAction.SEARCH, MainAction.LIST, MainAction.UPGRADE, MainAction.UNINSTALL)]
        public bool UseName { get; set; } = false;
        /// <summary>
        /// Get or set if the action should be silent.
        /// (Default = false)
        /// </summary>
        /// <remarks>
        /// Used by: "install", "upgrade", "uninstall"
        /// </remarks>
        [ActionUsed(MainAction.INSTALL, MainAction.UPGRADE, MainAction.UNINSTALL)]
        public bool RequestSilent { get; set; } = false;
        /// <summary>
        /// Get or set if the action should be interactive.
        /// (Default = false)
        /// </summary>
        /// <remarks>
        /// Used by: "install", "upgrade", "uninstall"
        /// </remarks>
        [ActionUsed(MainAction.INSTALL, MainAction.UPGRADE, MainAction.UNINSTALL)]
        public bool RequestInteractive { get; set; } = false;
        /// <summary>
        /// Get or set if the action should be forced.
        /// (Default = false)
        /// This will disable the hash check for some actions.
        /// </summary>
        /// <remarks>
        /// Used by: "install", "upgrade", "source reset"
        /// </remarks>
        [ActionUsed(MainAction.INSTALL, MainAction.UPGRADE, MainAction.SOURCE_RESET)]
        public bool Force { get; set; } = false;
        /// <summary>
        /// Get or set if the action value should be taken exactly.
        /// (Default = false)
        /// </summary>
        /// <remarks>
        /// Used by: "install", "search", "list", "upgrade", "uninstall"
        /// </remarks>
        [ActionUsed(MainAction.INSTALL, MainAction.SEARCH, MainAction.LIST, MainAction.UPGRADE, MainAction.UNINSTALL)]
        public bool UseExact { get; set; } = false;
        /// <summary>
        /// Get or set if the action should be done for all posible opjects.
        /// (Default = false)
        /// </summary>
        /// <remarks>
        /// Used by: "upgrade"
        /// </remarks>
        [ActionUsed(MainAction.UPGRADE)]
        public bool All { get; set; } = false;
    }
}
