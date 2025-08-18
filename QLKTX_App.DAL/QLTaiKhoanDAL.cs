using System.Data;
using System.Data.SqlClient;

namespace QLKTX_App.DAL
{
    public class QLTaiKhoanDAL
    {
        private readonly DBHelper db = new DBHelper();

        // Lấy toàn bộ danh sách tài khoản kèm thông tin nhân viên
        public DataTable GetAll()
        {
            string query = @"
                SELECT 
                    TK.MaNV,
                    NV.HoTen, 
                    TK.TenDangNhap, 
                    TK.MatKhau, 
                    CASE WHEN TK.TrangThai = 1 THEN N'Còn hoạt động' ELSE N'Bị khóa' END AS TrangThai,
                    TK.VaiTro
                FROM TaiKhoan TK
                INNER JOIN NhanVien NV ON TK.MaNV = NV.MaNV";
            return db.ExecuteQuery(query, false);
        }

        // Thêm tài khoản mới
        public int Insert(string maNV, string tenDN, string matKhau, bool trangThai, string vaiTro)
        {
            return db.ExecuteNonQuery("sp_TaiKhoan_Them", true,
                new SqlParameter("@MaNV", maNV),
                new SqlParameter("@TenDangNhap", tenDN),
                new SqlParameter("@MatKhau", matKhau),
                new SqlParameter("@TrangThai", trangThai),
                new SqlParameter("@VaiTro", vaiTro));

        }

        // Cập nhật tài khoản theo MaNV
        public int Update(string maNV, string tenDN, string matKhau, bool trangThai, string vaiTro)
        {
            return db.ExecuteNonQuery("sp_TaiKhoan_Sua", true,
                new SqlParameter("@MaNV", maNV),
                new SqlParameter("@TenDangNhap", tenDN),
                new SqlParameter("@MatKhau", matKhau),
                new SqlParameter("@TrangThai", trangThai),
                new SqlParameter("@VaiTro", vaiTro));
        }

        // Xóa tài khoản theo MaNV
        public int Delete(string maNV)
        {
            return db.ExecuteNonQuery("sp_TaiKhoan_Xoa", true,
                new SqlParameter("@MaNV", maNV));
        }
    }
}
