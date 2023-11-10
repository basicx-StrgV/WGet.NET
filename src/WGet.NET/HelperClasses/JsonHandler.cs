//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
#if NETCOREAPP3_1_OR_GREATER
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
#elif NETSTANDARD2_0
using Newtonsoft.Json;
#endif
using WGetNET.Exceptions;

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
        /// Object of the given class type.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.InvalidJsonException">
        /// The provided JSON could not be deserialized.
        /// </exception>
        public static T StringToObject<T>(string jsonString) where T: class
        {
            T? instance = null;

            try
            {
#if NETCOREAPP3_1_OR_GREATER
                instance = JsonSerializer.Deserialize<T>(jsonString);
#elif NETSTANDARD2_0
                instance = JsonConvert.DeserializeObject<T>(jsonString);
#endif
            }
            catch (Exception e)
            {
                throw new InvalidJsonException(e);
            }

            if (instance == null)
            {
                throw new InvalidJsonException();
            }

            return instance;
        }

#if NETCOREAPP3_1_OR_GREATER
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
        /// A <see cref="System.Threading.Tasks.Task"/>, containing the result.
        /// Object of the given class type.
        /// </returns>
        /// <exception cref="WGetNET.Exceptions.InvalidJsonException">
        /// The provided JSON could not be deserialized.
        /// </exception>
        public static async Task<T> StringToObjectAsync<T>(string jsonString) where T : class
        {
            T? instance = null;

            try
            {
                using MemoryStream dataStream = new(Encoding.UTF8.GetBytes(jsonString));

                instance = await JsonSerializer.DeserializeAsync<T>(dataStream);
            }
            catch (Exception e)
            {
                throw new InvalidJsonException(e);
            }

            if (instance == null)
            {
                throw new InvalidJsonException();
            }

            return instance;
        }
#endif

        /// <summary>
        /// Serializes a json <see cref="System.String"/> from the provided object.
        /// </summary>
        /// <param name="input">The object to serialize.</param>
        /// <returns>
        /// A <see cref="System.String"/> containing the generated json.
        /// </returns>
        public static string GetJson(object input)
        {
            string json = string.Empty;

#if NETCOREAPP3_1_OR_GREATER
                json = JsonSerializer.Serialize(input);
#elif NETSTANDARD2_0
                json = JsonConvert.SerializeObject(input);
#endif

            return json;
        }
    }
}
