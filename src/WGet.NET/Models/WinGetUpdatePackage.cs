using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGetNET
{
    /// <summary>
    /// Represents an updatable winget package
    /// </summary>
    public class WinGetUpdatePackage : WinGetPackage
    {
        /// <inheritdoc/>
        public override bool IsEmpty => base.IsEmpty || string.IsNullOrWhiteSpace(_updateVersion);

        /// <summary>
        /// Gets or sets the updatable version of the package.
        /// </summary>
        public string UpdateVersion
        {
            get => _updateVersion;
            internal set => _updateVersion = string.IsNullOrWhiteSpace(value) ? string.Empty : value;
        }

        
        private string _updateVersion = string.Empty;
        

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{PackageName} ({PackageVersion}) -> ({_updateVersion})";
        }
    }
}
