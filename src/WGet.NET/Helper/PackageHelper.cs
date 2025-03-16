//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System.Collections.Generic;

namespace WGetNET.Helper
{
    /// <summary>
    /// The <see langword="static"/> <see cref="WGetNET.Helper.PackageHelper"/> class provides methods for winget package processing.
    /// </summary>
    internal static class PackageHelper
    {
        /// <summary>
        /// Tries to match a <see cref="WGetNET.WinGetPackage"/> to the provided search criteria.
        /// </summary>
        /// <param name="matchList">
        /// The list to try and finde match in.
        /// </param>
        /// <param name="matchString">
        /// The search criteria, that will be tried to be matched againts the package id and/or name.
        /// </param>
        /// <returns>
        /// The <see cref="WGetNET.WinGetPackage"/> that matches the search criteria, or <see langword="null"/> if no package matches.
        /// </returns>
        public static WinGetPackage? MatchExact(List<WinGetPackage> matchList, string matchString)
        {
            if (matchList == null || matchList.Count <= 0 || string.IsNullOrWhiteSpace(matchString))
            {
                return null;
            }

            for (int i = 0; i < matchList.Count; i++)
            {
                if (string.Equals(matchList[i].Id, matchString) || string.Equals(matchList[i].Name, matchString))
                {
                    return matchList[i];
                }
            }

            return null;
        }
    }
}
