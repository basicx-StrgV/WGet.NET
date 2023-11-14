//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.IO;

namespace WGetNET.HelperClasses
{
    internal static class ArgsHelper
    {
        /// <summary>
        /// Throws a <see cref="ArgumentNullException"/> if the given string is <see langword="null"/> or empty/whitespace.
        /// </summary>
        /// <param name="arg">
        /// The argument to check.
        /// </param>
        /// <param name="name">
        /// The name of the arument.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The given string is null or empty/whitespace.
        /// </exception>
        public static void ThrowIfStringIsNullOrWhiteSpace(string arg, string name)
        {
            if (string.IsNullOrWhiteSpace(arg))
            {
                throw new ArgumentNullException(name, "Value cannot be null or empty.");
            }
        }

        /// <summary>
        /// Throws a <see cref="ArgumentNullException"/> if the given object is <see langword="null"/>.
        /// </summary>
        /// <param name="arg">
        /// The argument to check.
        /// </param>
        /// <param name="name">
        /// The name of the arument.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The given object is <see langword="null"/>
        /// </exception>
        public static void ThrowIfObjectIsNull(object arg, string name)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        /// <summary>
        /// Throws a <see cref="ArgumentNullException"/> if the given winget object is <see langword="null"/> or empty.
        /// </summary>
        /// <param name="arg">
        /// The argument to check.
        /// </param>
        /// <param name="name">
        /// The name of the arument.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The given winget object is null or empty.
        /// </exception>
        public static void ThrowIfWinGetObjectIsNullOrEmpty(IWinGetObject arg, string name)
        {
            if (arg == null || arg.IsEmpty)
            {
                throw new ArgumentNullException(name);
            }
        }

        /// <summary>
        /// Throws one of the listet exception if the path is invalid or cant be accessed.
        /// </summary>
        /// <param name="arg">
        /// The argument to check.
        /// </param>
        /// <exception cref="System.ArgumentException">
        /// The path contains one or more invalid characters as defined by <see cref="System.IO.Path.InvalidPathChars"/>.
        /// </exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">
        /// The directory root does not exist.
        /// </exception>
        /// <exception cref="System.IO.PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        public static void ThrowIfPathIsInvalid(string arg)
        {
            // This will indirectly throw other exeption.
            string root = Directory.GetDirectoryRoot(arg);

            // Check if the path exists.
            if (!Directory.Exists(root))
            {
                throw new DirectoryNotFoundException($"The path root '{root}' does not exist.");
            }
        }
    }
}
