//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;

namespace WGetNET.HelperClasses
{
    internal static class ArgsHelper
    {
        public static void ThrowIfStringIsNullOrWhiteSpace(string arg, string name)
        {
            if (string.IsNullOrWhiteSpace(arg))
            {
                throw new ArgumentNullException(name, "Value cannot be null or empty.");
            }
        }

        public static void ThrowIfObjectIsNull(object arg, string name)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void ThrowIfWinGetObjectIsNullOrEmpty(IWinGetObject arg, string name)
        {
            if (arg == null || arg.IsEmpty)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}
