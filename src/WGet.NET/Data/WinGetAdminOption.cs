//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
namespace WGetNET
{
    /// <summary>
    /// Represents a winget admin settings entry.
    /// </summary>
    public sealed class WinGetAdminOption : WinGetInfoEntry
    {
        /// <summary>
        /// Gets if the admin setting is enabled.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return _content.ToUpper() switch
                {
                    "ENABLED" => true,
                    "DISABLED" => false,
                    _ => false,
                };
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetAdminOption"/> class.
        /// </summary>
        /// <param name="name">The name of the settings entry.</param>
        /// <param name="content">The content of the settings entry.</param>
        internal WinGetAdminOption(string name, string content) : base(name, content)
        {
            // Handled by base
        }
    }
}
