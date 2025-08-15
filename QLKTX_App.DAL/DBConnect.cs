using System.Data.SqlClient;
using QLKTX_App.Utilities;

namespace QLKTX_App.DAL
{
    public class DBConnect
    {
        private string connectionString;

        public DBConnect()
        {
            // Load cấu hình từ file JSON
            DbConfig config = DbConfig.LoadFromFile("config.json");
            connectionString = config.GetConnectionString();
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
