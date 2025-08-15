using System;
using System.IO;
using Newtonsoft.Json;

namespace QLKTX_App.Utilities
{
    public class DbConfig
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public string GetConnectionString(bool includeDatabase = true)
        {
            string dbPart = includeDatabase ? $"Initial Catalog={Database};" : "";
            return $"Data Source={Server};{dbPart}User ID={User};Password={Password};TrustServerCertificate=True;MultipleActiveResultSets=true";
        }

        public static DbConfig LoadFromFile(string path = "config.json")
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Không tìm thấy file cấu hình: {path}");

            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<DbConfig>(json);
        }
    }
}
