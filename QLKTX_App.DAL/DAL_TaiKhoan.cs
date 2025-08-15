using System;
using System.Data;
using System.Data.SqlClient;
using QLKTX_App.DTO;
using QLKTX_App.Utilities;

namespace QLKTX_App.DAL
{
    public class DAL_TaiKhoan
    {
        private readonly DbConfig _dbConfig;

        public DAL_TaiKhoan()
        {
            _dbConfig = DbConfig.LoadFromFile("config.json");
        }

        public TaiKhoanModel KiemTraDangNhap(string username, string password)
        {
            TaiKhoanModel tk = null;

            using (SqlConnection conn = new SqlConnection(_dbConfig.GetConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT TenDangNhap, MatKhau, VaiTro, TrangThai " +
                    "FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap AND MatKhau = @MatKhau", conn))
                {
                    cmd.Parameters.AddWithValue("@TenDangNhap", username);
                    cmd.Parameters.AddWithValue("@MatKhau", password);

                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            tk = new TaiKhoanModel
                            {
                                TenDangNhap = rd["TenDangNhap"].ToString(),
                                MatKhau = rd["MatKhau"].ToString(),
                                VaiTro = rd["VaiTro"].ToString(),
                                TrangThai = Convert.ToBoolean(rd["TrangThai"])
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
