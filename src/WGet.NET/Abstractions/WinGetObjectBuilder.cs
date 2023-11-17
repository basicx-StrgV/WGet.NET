namespace WGetNET.Abstractions
{
    /// <summary>
    /// Base class for internal builders.
    /// </summary>
    /// <typeparam name="T">
    /// Type of the class the builder creates an instance of.
    /// The class needs to inherit <see cref="WGetNET.IWinGetObject"/>.
    /// </typeparam>
    internal abstract class WinGetObjectBuilder<T> where T : IWinGetObject
    {
        /// <summary>
        /// Returns an instance that is created from the data provided to the builder.
        /// </summary>
        /// <returns></returns>
        public abstract T GetInstance();

        /// <summary>
        /// Cleares all added data from the builder.
        /// </summary>
        public abstract void Clear();
    }
}
