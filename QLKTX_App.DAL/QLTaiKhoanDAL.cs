using QLKTX_App.Utilities;
using System;
using System.Data;
using System.Data.SqlClient;

namespace QLKTX_App.DAL
{
    public class QLTaiKhoanDAL
    {
        public DataTable GetAll()
        {
            string query = @"
                SELECT tk.TenDangNhap, tk.MatKhau, nv.MaNV, nv.HoTen,
                    CASE WHEN tk.TrangThai = 1 THEN N'Đang hoạt động' ELSE N'Bị khóa' END AS TrangThai
                FROM TaiKhoan tk
                LEFT JOIN NhanVien nv ON tk.TenDangNhap = nv.TenDangNhap";
            return DbHelper.ExecuteQuery(query);
        }

        public DataTable Search(string keyword)
        {
            string query = @"
                SELECT tk.TenDangNhap, tk.MatKhau, nv.MaNV, nv.HoTen,
                    CASE WHEN tk.TrangThai = 1 THEN N'Đang hoạt động' ELSE N'Bị khóa' END AS TrangThai
                FROM TaiKhoan tk
                LEFT JOIN NhanVien nv ON tk.TenDangNhap = nv.TenDangNhap
                WHERE tk.TenDangNhap LIKE @keyword";
            return DbHelper.ExecuteQuery(query, new SqlParameter("@keyword", "%" + keyword + "%"));
        }

        public bool InsertOrUpdate(string tenDangNhap, string matKhau, string maNV, int trangThai)
        {
            string query = "EXEC sp_Them_Sua_TaiKhoan @TenDangNhap, @MatKhau, @MaNV, @TrangThai";
            int rows = DbHelper.ExecuteNonQuery(query,
                new SqlParameter("@TenDangNhap", tenDangNhap ?? (object)DBNull.Value),
                new SqlParameter("@MatKhau", matKhau ?? (object)DBNull.Value),
                new SqlParameter("@MaNV", maNV ?? (object)DBNull.Value),
                new SqlParameter("@TrangThai", trangThai) // int => OK
            );
            return rows > 0;
        }


        public bool Delete(string tenDangNhap)
        {
            string query = "DELETE FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap";
            int rows = DbHelper.ExecuteNonQuery(query, new SqlParameter("@TenDangNhap", tenDangNhap));
            return rows > 0;
        }
    }
}
