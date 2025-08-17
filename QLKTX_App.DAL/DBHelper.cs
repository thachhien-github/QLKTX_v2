using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKTX_App.DAL
{
    public class DBHelper
    {
        private DBConnect dbConnect = new DBConnect();

        // 1. Truy vấn trả về DataTable (select từ bảng/view hoặc SP)
        public DataTable ExecuteQuery(string sqlOrSp, bool isStoredProcedure = true, params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = dbConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sqlOrSp, conn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                adapter.Fill(dt);
            }
            return dt;
        }

        // 2. Thực thi câu lệnh không trả về dữ liệu (INSERT, UPDATE, DELETE)
        public int ExecuteNonQuery(string sqlOrSp, bool isStoredProcedure = true, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = dbConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sqlOrSp, conn))
            {
                conn.Open();
                cmd.CommandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                return cmd.ExecuteNonQuery();
            }
        }

        // 3. Lấy giá trị scalar
        public object ExecuteScalar(string sqlOrSp, bool isStoredProcedure = true, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = dbConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sqlOrSp, conn))
            {
                conn.Open();
                cmd.CommandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                return cmd.ExecuteScalar();
            }
        }
    }
}
