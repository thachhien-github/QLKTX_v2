using QLKTX_App.Utilities;
using System;
using System.Data;
using System.Data.SqlClient;

namespace QLKTX_App.DAL
{
    public class NhanVienDAL
    {
        public DataTable GetAll()
        {
            using (SqlConnection conn = new SqlConnection(AppConfig.ConnectionString))
            {
                string sql = @"SELECT MaNV, HoTen, GioiTinh, NgaySinh, SoDienThoai, Email, TenDangNhap FROM NhanVien";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable Search(string keyword)
        {
            using (SqlConnection conn = new SqlConnection(AppConfig.ConnectionString))
            {
                string sql = @"SELECT MaNV, HoTen, GioiTinh, NgaySinh, SoDienThoai, Email, TenDangNhap
                               FROM NhanVien
                               WHERE MaNV LIKE @kw OR HoTen LIKE @kw";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.SelectCommand.Parameters.AddWithValue("@kw", "%" + keyword + "%");
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public bool Exists(string maNV)
        {
            using (SqlConnection conn = new SqlConnection(AppConfig.ConnectionString))
            {
                string sql = "SELECT COUNT(*) FROM NhanVien WHERE MaNV=@MaNV";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        public void Insert(string maNV, string hoTen, string gioiTinh, DateTime ngaySinh, string sdt, string email, string tenDangNhap)
        {
            using (SqlConnection conn = new SqlConnection(AppConfig.ConnectionString))
            {
                string sql = @"INSERT INTO NhanVien(MaNV, HoTen, GioiTinh, NgaySinh, SoDienThoai, Email, TenDangNhap) 
                               VALUES(@MaNV, @HoTen, @GioiTinh, @NgaySinh, @SDT, @Email, @TenDangNhap)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.Parameters.AddWithValue("@HoTen", hoTen);
                cmd.Parameters.AddWithValue("@GioiTinh", gioiTinh);
                cmd.Parameters.AddWithValue("@NgaySinh", ngaySinh);
                cmd.Parameters.AddWithValue("@SDT", sdt);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@TenDangNhap", string.IsNullOrWhiteSpace(tenDangNhap) ? (object)DBNull.Value : tenDangNhap);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(string maNV, string hoTen, string gioiTinh, DateTime ngaySinh, string sdt, string email)
        {
            using (SqlConnection conn = new SqlConnection(AppConfig.ConnectionString))
            {
                string sql = @"UPDATE NhanVien SET HoTen=@HoTen, GioiTinh=@GioiTinh, NgaySinh=@NgaySinh, 
                               SoDienThoai=@SDT, Email=@Email WHERE MaNV=@MaNV";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.Parameters.AddWithValue("@HoTen", hoTen);
                cmd.Parameters.AddWithValue("@GioiTinh", gioiTinh);
                cmd.Parameters.AddWithValue("@NgaySinh", ngaySinh);
                cmd.Parameters.AddWithValue("@SDT", sdt);
                cmd.Parameters.AddWithValue("@Email", email);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool Delete(string maNV)
        {
            using (SqlConnection conn = new SqlConnection(AppConfig.ConnectionString))
            {
                string sql = "DELETE FROM NhanVien WHERE MaNV=@MaNV AND (TenDangNhap IS NULL OR TenDangNhap='')";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
