//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
namespace WGetNET.HelperClasses
{
    /// <summary>
    /// The <see langword="static"/> <see cref="WGetNET.HelperClasses.ArrayManager"/> class,
    /// provieds methods to manage <see langword="array"/>'s.
    /// </summary>
    internal static class ArrayManager
    {
        /// <summary>
        /// Adds a new entry to a <see langword="array"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the <see langword="array"/>.
        /// </typeparam>
        /// <param name="inputArray">
        /// The input <see langword="array"/>.
        /// </param>
        /// <param name="value">
        /// The value to add to the <see langword="array"/>.
        /// </param>
        /// <returns>
        /// The <see langword="array"/> with the added entry.
        /// </returns>
        public static T[] Add<T>(T[] inputArray, T value)
        {
            //Copy the main array to the temp array.
            T[] tempArray = new T[inputArray.Length];
            for (int i = 0; i < inputArray.Length; i++)
            {
                tempArray[i] = inputArray[i];
            }

            //Add a entry to the main array
            //and copy the temp array to the main array.
            inputArray = new T[tempArray.Length + 1];
            for (int i = 0; i < tempArray.Length; i++)
            {
                inputArray[i] = tempArray[i];
            }

            //Add the new line to the new enty in the main array.
            //[^1] : Selects the last entry from the array.
            //(eg. "^" selects the index from the end
            //and 1 is the first index starting from the end of the array)
            inputArray[^1] = value;

            return inputArray;
        }

        /// <summary>
        /// Removes a range of entrys from a <see langword="array"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the <see langword="array"/>.
        /// </typeparam>
        /// <param name="inputArray">
        /// The input <see langword="array"/>.
        /// </param>
        /// <param name="index">
        /// A <see cref="System.Int32"/> representing the zero based start index.
        /// </param>
        /// <param name="count">
        /// A <see cref="System.Int32"/> representing the number of elements to remove.
        /// </param>
        /// <returns>
        /// The <see langword="array"/> with the range of entrys removed.
        /// </returns>
        public static T[] RemoveRange<T>(T[] inputArray, int index, int count)
        {
            //Only copy the needed range of the main array to the temp array.
            T[] tempArray = new T[inputArray.Length - count];
            for (int i = 0; i < inputArray.Length; i++)
            {
                if (i < index)
                {
                    tempArray[i] = inputArray[i];
                }
                else if (i + count < inputArray.Length)
                {
                    tempArray[i] = inputArray[i + count];
                }
            }

            //Copy the temp array to the main array.
            inputArray = new T[tempArray.Length];
            for (int i = 0; i < tempArray.Length; i++)
            {
                inputArray[i] = tempArray[i];
            }

            return inputArray;
        }
    }
}
