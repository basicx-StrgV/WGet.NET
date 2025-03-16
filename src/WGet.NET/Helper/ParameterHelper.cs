//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
namespace WGetNET.Helper
{
    /// <summary>
    /// The <see langword="static"/> <see cref="WGetNET.Helper.ParameterHelper"/> class provides methods for winget call parameter extensions.
    /// </summary>
    internal static class ParameterHelper
    {
        private const string _includeUnknown = "--include-unknown";
        private const string _acceptSourceAgreements = "--accept-source-agreements";
        private const string _silent = "--silent";

        /// <summary>
        /// Adds the '--include-unknown' argument to the given <see cref="System.String"/> of arguments.
        /// </summary>
        /// <param name="argument">
        /// <see cref="System.String"/> containing the arguments that should be extended.
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/> containing the new process arguments.
        /// </returns>
        public static string IncludeUnknown(string argument)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                return argument;
            }

            argument += $" {_includeUnknown}";

            return argument;
        }

        /// <summary>
        /// Adds the '--accept-source-agreements' argument to the given <see cref="System.String"/> of arguments.
        /// </summary>
        /// <param name="argument">
        /// <see cref="System.String"/> containing the arguments that should be extended.
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/> containing the new process arguments.
        /// </returns>
        public static string AcceptSourceAgreements(string argument)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                return argument;
            }

            argument += $" {_acceptSourceAgreements}";

            return argument;
        }

        /// <summary>
        /// Adds the '--silent' argument to the given <see cref="System.String"/> of arguments.
        /// </summary>
        /// <param name="argument">
        /// <see cref="System.String"/> containing the arguments that should be extended.
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/> containing the new process arguments.
        /// </returns>
        public static string Silent(string argument)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                return argument;
            }

            argument += $" {_silent}";

            return argument;
        }
    }
}
