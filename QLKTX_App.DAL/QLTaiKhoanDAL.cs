using System.Data;
using System.Data.SqlClient;

namespace QLKTX_App.DAL
{
    public class QLTaiKhoanDAL
    {
        public DataTable GetAll()
        {
            string query = @"SELECT TK.TenDangNhap, NV.MaNV, NV.HoTen, TK.MatKhau, TK.TrangThai
                             FROM TaiKhoan TK 
                             INNER JOIN NhanVien NV ON TK.TenDangNhap = NV.TenDangNhap";
            return DBConnect.Instance.ExecuteQuery(query);
        }

        public int InsertOrUpdate(string tenDN, string matKhau, string trangThai, string maNV)
        {
            return DBConnect.Instance.ExecuteNonQuery("sp_Them_Sua_TaiKhoan",
                new SqlParameter("@TenDangNhap", tenDN),
                new SqlParameter("@MatKhau", matKhau),
                new SqlParameter("@TrangThai", trangThai),
                new SqlParameter("@MaNV", maNV));
        }

        public int Delete(string tenDN)
        {
            return DBConnect.Instance.ExecuteNonQuery("sp_Xoa_TaiKhoan",
                new SqlParameter("@TenDangNhap", tenDN));
        }
    }
}
