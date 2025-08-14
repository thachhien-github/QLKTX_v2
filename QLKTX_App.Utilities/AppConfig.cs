using System;
using System.IO;
using Newtonsoft.Json;

namespace QLKTX_App.Utilities
{
    public static class AppConfig
    {
        private static readonly string ConfigFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        public static DbConfig CurrentConfig { get; private set; }

        public static bool LoadConfig()
        {
            if (!File.Exists(ConfigFile)) return false;
            try
            {
                var json = File.ReadAllText(ConfigFile);
                CurrentConfig = JsonConvert.DeserializeObject<DbConfig>(json);
                return true;
            }
            catch { return false; }
        }

        public static bool SaveConfig(DbConfig cfg)
        {
            try
            {
                var json = JsonConvert.SerializeObject(cfg, Formatting.Indented);
                File.WriteAllText(ConfigFile, json);
                CurrentConfig = cfg;
                return true;
            }
            catch { return false; }
        }
    }
}
