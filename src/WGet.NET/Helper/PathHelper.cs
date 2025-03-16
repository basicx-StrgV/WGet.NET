//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System.IO;

namespace WGetNET.Helper
{
    /// <summary>
    /// The <see langword="static"/> <see cref="WGetNET.Helper.PathHelper"/> class provides methods for working with different path types.
    /// </summary>
    internal static class PathHelper
    {
        /// <summary>
        /// Enum for the selecetion of defferent path types.
        /// </summary>
        public enum PathType
        {
            Generic,
            URI,
            Directory
        }

        /// <summary>
        /// Removes the last directory from the given path.
        /// </summary>
        /// <param name="path">
        /// <see cref="System.String"/> containing the path that sould be trimed.
        /// </param>
        /// <param name="type">
        /// Selection of the path type for correct separator allocation.
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/> containing the trimed path.
        /// </returns>
        public static string TrimLastPathPart(string path, PathType type)
        {
            char separatorChar = '/';
            switch (type)
            {
                case PathType.URI:
                    separatorChar = '/';
                    break;
                case PathType.Directory:
                    separatorChar = Path.DirectorySeparatorChar;
                    break;
            }

            int lastSeparatorIndex = -1;
            for (int i = 0; i < path.Length; i++)
            {
                if (path[i].Equals(separatorChar))
                {
                    lastSeparatorIndex = i;
                }
            }

            if (lastSeparatorIndex > -1)
            {
                return path.Remove(lastSeparatorIndex);
            }

            return path;
        }
    }
}
