//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.Text;

namespace WGetNET.HelperClasses
{
    internal static class VersionParser
    {
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

        private static int ParseToInt(string input, int defaultValue = 0)
        {
            if (int.TryParse(input, out int result))
            {
                return result;
            }

            return CleanParseToInt(input, defaultValue);
        }

        private static int CleanParseToInt(string input, int defaultValue = 0)
        {
            string cleanInput = CleanupNumberString(input);

            if (int.TryParse(cleanInput, out int result))
            {
                return result;
            }

            return defaultValue;
        }

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
