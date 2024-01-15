//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BuildTool
{
    internal static class ConfigHandler
    {
        public class BuildConfig
        {
            [JsonPropertyName("currentVersion")]
            public string? CurrentVersion { get; set; }
        }

        public static BuildConfig? LoadConfig(string configFile)
        {
            try
            {
                string configContent = File.ReadAllText(configFile);

                return JsonSerializer.Deserialize<BuildConfig>(configContent);
            }
            catch
            {
                return null;
            }
        }

        public static bool SaveConfig(string configFile, BuildConfig config)
        {
            try
            {
                string configJson = JsonSerializer.Serialize(config, new JsonSerializerOptions()
                {
                    WriteIndented = true
                });

                File.WriteAllText(configFile, configJson);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
