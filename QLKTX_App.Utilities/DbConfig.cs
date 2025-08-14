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
            var dbPart = includeDatabase ? $"Initial Catalog={Database};" : "";
            return $"Data Source={Server};{dbPart}User ID={User};Password={Password};MultipleActiveResultSets=true;TrustServerCertificate=True";
        }
    }
}
