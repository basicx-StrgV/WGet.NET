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
            T[] tempArray = CopyTo(inputArray);

            //Add a entry to the main array
            //and copy the temp array to the main array.
            inputArray = CopyTo(tempArray, 1);

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
            T[] tempArray = CopyToWithoutRange(inputArray, index, count);

            //Copy the temp array to the main array.
            inputArray = CopyTo(tempArray);

            return inputArray;
        }

        /// <summary>
        /// Searches for an entry in a <see langword="array"/> that contains the given string.
        /// </summary>
        /// <param name="inputArray">
        /// The input <see langword="array"/>.
        /// </param>
        /// <param name="value">
        /// A <see cref="System.String"/> representing the value to check for.
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/> representing the index of the entry (-1 if the entry is not found).
        /// </returns>
        public static int GetEntryContains(string[] inputArray, string value)
        {
            int index = -1;

            for (int i = 0; i < inputArray.Length; i++)
            {
                if (inputArray[i].Contains(value))
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        /// <summary>
        /// Copys a <see langword="array"/> to a new one.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the array.
        /// </typeparam>
        /// <param name="inputArray">
        /// The <see langword="array"/> that should be copyed.
        /// </param>
        /// <param name="addLengthOf">
        /// A <see cref="System.Int32"/> representing a extra length,
        /// that should be added to the end of the new array. (DEFAULT = 0)
        /// </param>
        /// <returns>
        /// The new <see langword="array"/>.
        /// </returns>
        private static T[] CopyTo<T>(T[] inputArray, int addLengthOf = 0)
        {
            //Copy the input array to the new array.
            T[] newArray = new T[inputArray.Length + addLengthOf];
            for (int i = 0; i < inputArray.Length; i++)
            {
                newArray[i] = inputArray[i];
            }

            return newArray;
        }

        /// <summary>
        /// Copys a <see langword="array"/> to a new one, but ignores the given range.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the array.
        /// </typeparam>
        /// <param name="inputArray">
        /// The <see langword="array"/> that should be copyed.
        /// </param>
        /// <param name="startIndex">
        /// A <see cref="System.Int32"/> representing a start index of the range that should be ignored.
        /// </param>
        /// <param name="count">
        /// A <see cref="System.Int32"/> representing the range that should be ignored.
        /// </param>
        /// <returns>
        /// The new <see langword="array"/>.
        /// </returns>
        private static T[] CopyToWithoutRange<T>(T[] inputArray, int startIndex, int count)
        {
            //Only copy the needed range of the input array to the new array.
            T[] newArray = new T[inputArray.Length - count];
            for (int i = 0; i < inputArray.Length; i++)
            {
                if (i < startIndex)
                {
                    newArray[i] = inputArray[i];
                }
                else if (i + count < inputArray.Length)
                {
                    newArray[i] = inputArray[i + count];
                }
            }

            return newArray;
        }
    }
}
