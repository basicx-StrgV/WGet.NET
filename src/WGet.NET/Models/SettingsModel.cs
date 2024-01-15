//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System.Collections.Generic;
#if NETCOREAPP3_1_OR_GREATER
using System.Text.Json;
using System.Text.Json.Serialization;
#elif NETSTANDARD2_0
using Newtonsoft.Json;
#endif

namespace WGetNET.Models
{
    /// <summary>
    /// Represents the global winget settings for parsing.
    /// </summary>
    internal class SettingsModel
    {
        /// <summary>
        /// Gets or sets a <see cref="System.Collections.Generic.Dictionary{TKey, TValue}"/> of admin settings.
        /// </summary>
#if NETCOREAPP3_1_OR_GREATER
        [JsonPropertyName("adminSettings")]
#elif NETSTANDARD2_0
        [JsonProperty("adminSettings")]
#endif
        public Dictionary<string, bool> AdminSettings { get; set; } = new();

        /// <summary>
        /// Gets or sets the user settings file path.
        /// </summary>
#if NETCOREAPP3_1_OR_GREATER
        [JsonPropertyName("userSettingsFile")]
#elif NETSTANDARD2_0
        [JsonProperty("userSettingsFile")]
#endif
        public string UserSettingsFile { get; set; } = string.Empty;
    }
}
