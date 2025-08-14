using System;
using System.IO;
using Newtonsoft.Json;

namespace QLKTX_App.Utilities
{
    public static class AppConfig
    {
        private static readonly string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

        public static DbConfig CurrentConfig { get; private set; }
        public static string ConnString { get; private set; }

        public static string ConnectionString
        {
            get
            {
                if (CurrentConfig == null) return null;
                return BuildConn(CurrentConfig);
            }
        }

        public static bool LoadConfig()
        {
            try
            {
                if (!File.Exists(path)) return false;

                string json = File.ReadAllText(path);
                CurrentConfig = JsonConvert.DeserializeObject<DbConfig>(json);
                if (CurrentConfig == null) return false;

                ConnString = BuildConn(CurrentConfig);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void SaveConfig(DbConfig cfg)
        {
            string json = JsonConvert.SerializeObject(cfg, Formatting.Indented);
            File.WriteAllText(path, json);

            CurrentConfig = cfg;
            ConnString = BuildConn(cfg);
        }

        public static string BuildConn(DbConfig cfg)
        {
            if (!string.IsNullOrWhiteSpace(cfg.User))
                return $"Data Source={cfg.Server};Initial Catalog={cfg.Database};User ID={cfg.User};Password={cfg.Password};TrustServerCertificate=True;MultipleActiveResultSets=True";
            else
                return $"Data Source={cfg.Server};Initial Catalog={cfg.Database};Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True";
        }

        public static string GetConnectionString()
        {
            if (CurrentConfig == null)
                throw new Exception("Chưa cấu hình kết nối.");

            return $"Data Source={CurrentConfig.Server};Initial Catalog={CurrentConfig.Database};User ID={CurrentConfig.User};Password={CurrentConfig.Password};TrustServerCertificate=True;MultipleActiveResultSets=True;";
        }
    }
}
