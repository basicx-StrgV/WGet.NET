//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WGetNET.HelperClasses
{
    /// <summary>
    /// The <see langword="static"/> <see cref="WGetNET.HelperClasses.JsonHandler"/> class,
    /// provieds methods handle json actions.
    /// </summary>
    internal static class JsonHandler
    {
        /// <summary>
        /// Deserializes a given json <see cref="System.String"/> to a object of the given class.
        /// </summary>
        /// <typeparam name="T">
        /// Class type the json should be deserialized to.
        /// </typeparam>
        /// <param name="jsonString">
        /// A <see cref="System.String"/> containing the json to deserialize.
        /// </param>
        /// <returns>
        /// A nullable object of the given class type. It will be <see langword="null"/> if the action failed.
        /// </returns>
        public static T? StringToObject<T>(string jsonString) where T: class
        {
            try
            {
                return JsonSerializer.Deserialize<T>(jsonString);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Asynchronously deserializes a given json <see cref="System.String"/> to a object of the given class.
        /// </summary>
        /// <typeparam name="T">
        /// Class type the json should be deserialized to.
        /// </typeparam>
        /// <param name="jsonString">
        /// A <see cref="System.String"/> containing the json to deserialize.
        /// </param>
        /// <returns>
        /// A nullable object of the given class type. It will be <see langword="null"/> if the action failed.
        /// </returns>
        public static async Task<T?> StringToObjectAsync<T>(string jsonString) where T : class
        {
            try
            {
                using (MemoryStream dataStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
                {
                    return await JsonSerializer.DeserializeAsync<T>(dataStream);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
