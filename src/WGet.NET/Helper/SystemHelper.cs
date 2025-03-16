//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.IO;
using System.Security.Principal;

namespace WGetNET.Helper
{
    /// <summary>
    /// The <see langword="static"/> <see cref="WGetNET.Helper.SystemHelper"/> class provides methods for system related tasks.
    /// </summary>
    internal static class SystemHelper
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
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                // Making sure windows related functions dont get called on none windows systems.
                return false;
            }

            using WindowsIdentity? identity = WindowsIdentity.GetCurrent(false);

            if (identity != null)
            {
                return new WindowsPrincipal(identity)
                    .IsInRole(WindowsBuiltInRole.Administrator);
            }

            return false;
        }

        /// <summary>
        /// Checks if winget is installed on the system and returns the path to the executable.
        /// </summary>
        /// <returns>
        /// <see cref="System.String"/> containing the executable path if it was found or <see cref="System.String.Empty"/> if not. 
        /// </returns>
        public static string CheckWingetInstallation()
        {
            string? pathEnvVar = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User);
            if (string.IsNullOrWhiteSpace(pathEnvVar))
            {
                return string.Empty;
            }

            string[] paths = pathEnvVar.Split(';');

            string exePath;
            for (int i = 0; i < paths.Length; i++)
            {
                exePath = Path.Combine(paths[i], "winget.exe");
                if (File.Exists(exePath))
                {
                    return exePath;
                }
            }

            return string.Empty;
        }
    }
}
