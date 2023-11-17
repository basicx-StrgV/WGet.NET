//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
namespace WGetNET.Abstractions
{
    /// <summary>
    /// Base class for internal builders.
    /// </summary>
    /// <typeparam name="T">
    /// Type of the class the builder creates an instance of.
    /// The class needs to inherit <see cref="WGetNET.IWinGetObject"/>.
    /// </typeparam>
    internal abstract class WinGetObjectBuilder<T> where T : IWinGetObject?
    {
        /// <summary>
        /// Returns an instance that is created from the data provided to the builder.
        /// </summary>
        /// <returns>
        /// The created instance.
        /// </returns>
        public abstract T GetInstance();

        /// <summary>
        /// Cleares all added data from the builder.
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Checks if the given value is possibly shortened.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the value is shortened or <see langword="false"/> if not.
        /// </returns>
        public static bool CheckShortenedValue(string value)
        {
            // Char 8230 is at the end of the shortened id if UTF-8 encoding is used.
#if NETCOREAPP3_1_OR_GREATER
            if (value.EndsWith((char)8230))
            {
                return true;
            }
#elif NETSTANDARD2_0
            if (value.EndsWith(((char)8230).ToString()))
            {
                return true;
            }
#endif

            return false;
        }
    }
}
