//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.Text;

namespace WGetNET.HelperClasses
{
    /// <summary>
    /// Handels the parsing of strings to a <see cref="System.Version"/> instance.
    /// </summary>
    internal static class VersionParser
    {
        /// <summary>
        /// Parses a <see cref="System.String"/> to a <see cref="System.Version"/> instance as best as possible.
        /// </summary>
        /// <param name="input">The <see cref="System.String"/> to parse.</param>
        /// <returns>
        /// The created <see cref="System.Version"/> instance.
        /// </returns>
        public static Version Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new Version(0, 0);
            }

            if (Version.TryParse(input, out Version? result))
            {
                return result;
            }

            return CleanParse(input);
        }

        /// <summary>
        /// Parses a <see cref="System.String"/> to a <see cref="System.Version"/> instance as best as possible.
        /// </summary>
        /// <remarks>
        /// This tries to resolve the string one by one to parse it.
        /// </remarks>
        /// <param name="input">The <see cref="System.String"/> to parse.</param>
        /// <returns>
        /// The created <see cref="System.Version"/> instance.
        /// </returns>
        private static Version CleanParse(string input)
        {
            string[] versionParts = input.Split('.');

            int major = 0;
            int minor = 0;
            int build = -1;
            int revision = -1;

            if (versionParts.Length >= 1)
            {
                major = ParseToInt(versionParts[0]);
            }

            if (versionParts.Length >= 2)
            {
                minor = ParseToInt(versionParts[1]);
            }

            if (versionParts.Length >= 3)
            {
                build = ParseToInt(versionParts[2], -1);
            }

            if (versionParts.Length >= 4)
            {
                revision = ParseToInt(versionParts[3], -1);
            }

            if (build < 0)
            {
                return new Version(major, minor);
            }
            else if (revision < 0)
            {
                return new Version(major, minor, build);
            }
            else
            {
                return new Version(major, minor, build, revision);
            }
        }

        /// <summary>
        /// Parses a <see cref="System.String"/> to a <see cref="System.Int32"/> as best as possible.
        /// </summary>
        /// <param name="input">The <see cref="System.String"/> to parse.</param>
        /// <param name="defaultValue">Default value that is used if the input can't be parsed.</param>
        /// <returns>
        /// The created <see cref="System.Int32"/>.
        /// </returns>
        private static int ParseToInt(string input, int defaultValue = 0)
        {
            if (int.TryParse(input, out int result))
            {
                return result;
            }

            return CleanParseToInt(input, defaultValue);
        }

        /// <summary>
        /// Parses a <see cref="System.String"/> to a <see cref="System.Int32"/> as best as possible.
        /// </summary>
        /// <remarks>
        /// This tries to clean the provided input <see cref="System.String"/>, 
        /// with minimal data loss, so it can be parsed to a <see cref="System.Int32"/>
        /// </remarks>
        /// <param name="input">The <see cref="System.String"/> to parse.</param>
        /// <param name="defaultValue">Default value that is used if the input can't be parsed.</param>
        /// <returns>
        /// The created <see cref="System.Int32"/>.
        /// </returns>
        private static int CleanParseToInt(string input, int defaultValue = 0)
        {
            string cleanInput = CleanupNumberString(input);

            if (int.TryParse(cleanInput, out int result))
            {
                return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// Cleans a <see cref="System.String"/>, with minimal version information loss, so it can be parsed to a <see cref="System.Int32"/>.
        /// </summary>
        /// <remarks>
        /// If the <see cref="System.String"/> looks lick this '123ABC456' it will not be cleaned, because to much information will be lost.
        /// </remarks>
        /// <param name="input">The <see cref="System.String"/> to clean up.</param>
        /// <returns>
        /// The processed <see cref="System.String"/>:
        /// </returns>
        private static string CleanupNumberString(string input)
        {
            // Remove appendix (e.g. 123456-preview1) becaue it could contain a mix of numbers and letters,
            // which is not considert by the next cleanup.
            input = RemoveAppendix(input);

            char[] parts = input.ToCharArray();

            StringBuilder cleanString = new();
            bool lastCharWasALetter = false;
            for (int i = 0; i < parts.Length; i++)
            {
                if (char.IsDigit(parts[i]) && !lastCharWasALetter)
                {
                    cleanString.Append(parts[i]);
                }
                else if (char.IsDigit(parts[i]) && lastCharWasALetter)
                {
                    // Dont clenup a string that is a mix of numbers and letters (e.g. 123ABC456),
                    // only strings that have letters at the end (e.g. 123456ABC).
                    return string.Empty;
                }
                else
                {
                    lastCharWasALetter = true;
                }
            }

            return cleanString.ToString();
        }

        /// <summary>
        /// Removes version appendixes from the string.
        /// </summary>
        /// <remarks>
        /// An version appendix could look like this: '-preview', '-beta2' or '-pre'.
        /// </remarks>
        /// <param name="input">The <see cref="System.String"/> to process.</param>
        /// <returns>
        /// The processed <see cref="System.String"/>.
        /// </returns>
        private static string RemoveAppendix(string input)
        {
            string[] parts = input.Split('-');

            if (parts.Length >= 1)
            {
                return parts[0];
            }

            return input;
        }
    }
}
