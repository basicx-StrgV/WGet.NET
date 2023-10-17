//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
namespace WGetNET
{
    /// <summary>
    /// Enum of winget pin types.
    /// </summary>
    public enum PinType
    {
        /// <summary>
        /// The package is pinned.
        /// </summary>
        /// <remarks>
        /// Package can't be updatet automatically.
        /// </remarks>
        Pinning,
        /// <summary>
        /// The package is blocked.
        /// </summary>
        /// <remarks>
        /// Package can't be updatet automatically and manually.
        /// </remarks>
        Blocking,
        /// <summary>
        /// The package is gated.
        /// </summary>
        /// <remarks>
        /// Package can't be updated to versions, that are not contained in the provided pinned version.
        /// </remarks>
        Gating
    }
}
