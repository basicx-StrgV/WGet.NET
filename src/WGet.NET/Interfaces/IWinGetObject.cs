//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
namespace WGetNET
{
    /// <summary>
    /// Interface for all WinGet related objects.
    /// </summary>
    public interface IWinGetObject
    {
        /// <summary>
        /// Gets if the object is empty.
        /// </summary>
        public bool IsEmpty { get; }
    }
}
