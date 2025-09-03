using System;
using System.Data;
using System.Data.SqlClient;
using QLKTX_App.DTO;
using QLKTX_App.Utilities;

namespace QLKTX_App.DAL
{
    public class TaiKhoanDAL
    {
        private readonly DBHelper db = new DBHelper();

        #region LOGIN
        public TaiKhoanModel KiemTraDangNhap(string username, string password)
        {
            string query = @"
                SELECT TOP 1 
                    tk.MaNV,
                    tk.TenDangNhap, 
                    tk.MatKhau, 
                    tk.VaiTro, 
                    tk.TrangThai, 
                    nv.HoTen
                FROM TaiKhoan tk
                INNER JOIN NhanVien nv ON tk.MaNV = nv.MaNV
                WHERE tk.TenDangNhap = @user AND tk.MatKhau = @pass";

            DataTable dt = db.ExecuteQuery(query, false,
                new SqlParameter("@user", username),
                new SqlParameter("@pass", password));

            if (dt.Rows.Count == 0) return null;

            DataRow r = dt.Rows[0];
            return new TaiKhoanModel
            {
                MaNV = r["MaNV"].ToString(),
                TenDangNhap = r["TenDangNhap"].ToString(),
                MatKhau = r["MatKhau"].ToString(),
                VaiTro = r["VaiTro"].ToString(),
                TrangThai = Convert.ToBoolean(r["TrangThai"]),
                HoTen = r["HoTen"].ToString()
            };
        }
        #endregion

        #region CRUD
        public DataTable GetAll()
        {
            string query = @"
                SELECT 
                    tk.MaNV,
                    nv.HoTen,
                    tk.TenDangNhap,
                    tk.MatKhau,
                    CASE WHEN tk.TrangThai = 1 
                         THEN N'Còn hoạt động' ELSE N'Bị khóa' END AS TrangThai,
                    tk.VaiTro
                FROM TaiKhoan tk
                INNER JOIN NhanVien nv ON tk.MaNV = nv.MaNV";
            return db.ExecuteQuery(query, false);
        }

        public int Insert(string maNV, string tenDN, string matKhau, bool trangThai, string vaiTro)
        {
            return db.ExecuteNonQuery("sp_TaiKhoan_Them", true,
                new SqlParameter("@MaNV", maNV),
                new SqlParameter("@TenDangNhap", tenDN),
                new SqlParameter("@MatKhau", matKhau),
                new SqlParameter("@TrangThai", trangThai),
                new SqlParameter("@VaiTro", vaiTro));
        }

        public int UpdateTrangThai(string maNV, bool trangThai)
        {
            string sql = "UPDATE TaiKhoan SET TrangThai = @TrangThai WHERE MaNV = @MaNV";
            return db.ExecuteNonQuery(sql, false,
                new SqlParameter("@TrangThai", trangThai),
                new SqlParameter("@MaNV", maNV));
        }


        public int Delete(string maNV)
        {
            return db.ExecuteNonQuery("sp_TaiKhoan_Xoa", true,
                new SqlParameter("@MaNV", maNV));
        }
        #endregion
    }
}
