//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.Security;
using System.Security.Principal;

namespace WGetNET.HelperClasses
{
    /// <summary>
    /// The <see langword="static"/> <see cref="WGetNET.HelperClasses.PrivilegeChecker"/> class,
    /// provides methods for checking user privileges.
    /// </summary>
    internal static class PrivilegeChecker
    {
        /// <summary>
        /// Check if the current user has administrator privileges.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the current user has administrator privileges and
        /// <see langword="false"/> if not.
        /// </returns>
        public static bool CheckAdministratorPrivileges()
        {
            try
            {
                return CurrentUserIsAdmin();
            }
            catch (SecurityException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the current user is an administrator.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the user is a admin and <see langword="false"/> if not.
        /// </returns>
        private static bool CurrentUserIsAdmin()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                return new WindowsPrincipal(identity)
                    .IsInRole(WindowsBuiltInRole.Administrator);
            }
        }
    }
}
