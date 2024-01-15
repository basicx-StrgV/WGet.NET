//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;

namespace WGetNET.Extensions
{
    /// <summary>
    /// The <see langword="static"/> <see cref="ArrayExtensions"/> class,
    /// provieds extension methods for generic and type specific arrays.
    /// </summary>
    internal static class ArrayExtensions
    {
        /// <summary>
        /// Adds a new entry to a <see langword="array"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the <see langword="array"/>.
        /// </typeparam>
        /// <param name="array">
        /// The input <see langword="array"/> for the action.
        /// </param>
        /// <param name="value">
        /// The value to add to the <see langword="array"/>.
        /// </param>
        /// <returns>
        /// The <see langword="array"/> with the added entry.
        /// </returns>
        public static T[] Add<T>(this T[] array, T value)
        {
            if (value is null)
            {
                return array;
            }

            Array.Resize(ref array, array.Length + 1);

            //Add the new line to the new enty in the main array.
#if NETCOREAPP3_1_OR_GREATER
            array[^1] = value;
#elif NETSTANDARD2_0
            array[array.Length - 1] = value;
#endif

            return array;
        }

        /// <summary>
        /// Removes a range of entrys from a <see langword="array"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the <see langword="array"/>.
        /// </typeparam>
        /// <param name="array">
        /// The input <see langword="array"/> for the action.
        /// </param>
        /// <param name="index">
        /// A <see cref="int"/> representing the zero based start index.
        /// </param>
        /// <param name="count">
        /// A <see cref="int"/> representing the number of elements to remove.
        /// </param>
        /// <returns>
        /// The <see langword="array"/> with the range of entrys removed.
        /// </returns>
        public static T[] RemoveRange<T>(this T[] array, int index, int count)
        {
            if (array.Length - 1 < index)
            {
                return array;
            }

            //Only copy the needed range of the input array to the new array.
            T[] newArray = new T[array.Length - count];
            for (int i = 0; i < array.Length; i++)
            {
                if (i < index)
                {
                    newArray[i] = array[i];
                }
                else if (i + count < array.Length)
                {
                    newArray[i] = array[i + count];
                }
            }

            return newArray;
        }

        /// <summary>
        /// Searches for an entry in a <see langword="array"/> that contains the given string.
        /// </summary>
        /// <param name="array">
        /// The input <see langword="array"/> for the action.
        /// </param>
        /// <param name="value">
        /// A <see cref="string"/> representing the value to check for.
        /// </param>
        /// <returns>
        /// A <see cref="int"/> representing the index of the entry (-1 if the entry is not found).
        /// </returns>
        public static int GetEntryContains(this string[] array, string value)
        {
            int index = -1;

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Contains(value))
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        /// <summary>
        /// Removes empty entries from a <see langword="array"/> of <see cref="string"/>'s.
        /// </summary>
        /// <param name="array">
        /// The input <see langword="array"/> for the action.
        /// </param>
        /// <returns>
        /// The <see langword="array"/> with empty entries removed.
        /// </returns>
        public static string[] RemoveEmptyEntries(this string[] array)
        {
            string[] newArray = Array.Empty<string>();

            for (int i = 0; i < array.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(array[i]))
                {
                    newArray = newArray.Add(array[i]);
                }
            }

            return newArray;
        }
    }
}
