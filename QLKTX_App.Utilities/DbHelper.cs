using System.Data;
using System.Data.SqlClient;

namespace QLKTX_App.Utilities
{
    public static class DbHelper
    {
        public static bool TestConnection(string connStr)
        {
            try
            {
                using (var cn = new SqlConnection(connStr))
                {
                    cn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        // Hàm thực thi câu lệnh SELECT trả về DataTable
        public static DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(AppConfig.ConnString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null && parameters.Length > 0)
                        cmd.Parameters.AddRange(parameters);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        // Hàm thực thi câu lệnh INSERT, UPDATE, DELETE (không trả kết quả)
        public static int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(AppConfig.ConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null && parameters.Length > 0)
                        cmd.Parameters.AddRange(parameters);

                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
