using System.Data;
using System.Data.SqlClient;
using QLKTX_App.Utilities;

namespace QLKTX_App.DAL
{
    public class TaiKhoanDAL
    {
        public DataRow DangNhap(string user, string pass)
        {
            using (var cn = new SqlConnection(AppConfig.ConnectionString))
            {
                cn.Open();
                string sql = @"
                    SELECT tk.TenDangNhap, tk.MatKhau, tk.VaiTro, tk.TrangThai, nv.HoTen
                    FROM TaiKhoan tk
                    LEFT JOIN NhanVien nv ON tk.TenDangNhap = nv.TenDangNhap
                    WHERE tk.TenDangNhap = @u AND tk.MatKhau = @p";

                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@u", user);
                    cmd.Parameters.AddWithValue("@p", pass);

                    DataTable dt = new DataTable();
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                    return dt.Rows.Count > 0 ? dt.Rows[0] : null;
                }
            }
        }
    }
}
