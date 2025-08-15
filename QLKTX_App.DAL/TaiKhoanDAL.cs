using System;
using System.Data.SqlClient;
using QLKTX_App.DTO;
using QLKTX_App.Utilities;

namespace QLKTX_App.DAL
{
    public class TaiKhoanDAL
    {
        private readonly DbConfig _dbConfig;

        public TaiKhoanDAL()
        {
            _dbConfig = DbConfig.LoadFromFile("config.json");
        }

        public TaiKhoanModel KiemTraDangNhap(string username, string password)
        {
            string query = @"
                SELECT TOP 1 tk.TenDangNhap, tk.MatKhau, tk.VaiTro, tk.TrangThai, nv.HoTen
                FROM TaiKhoan tk
                INNER JOIN NhanVien nv ON tk.TenDangNhap = nv.TenDangNhap
                WHERE tk.TenDangNhap = @user AND tk.MatKhau = @pass";

            using (SqlConnection conn = new SqlConnection(_dbConfig.GetConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pass", password);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new TaiKhoanModel
                            {
                                TenDangNhap = reader["TenDangNhap"].ToString(),
                                MatKhau = reader["MatKhau"].ToString(),
                                VaiTro = reader["VaiTro"].ToString(),
                                TrangThai = Convert.ToBoolean(reader["TrangThai"]),
                                HoTen = reader["HoTen"].ToString() // Lấy từ bảng NhanVien
                            };
                        }
                    }
                }
            }
            return null; // Không tìm thấy tài khoản
        }
    }
}
