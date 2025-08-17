using System;
using System.Data;
using System.Data.SqlClient;

namespace QLKTX_App.DAL
{
    public class NhanVienDAL
    {
        public DataTable GetAll()
        {
            string query = "SELECT MaNV, HoTen, GioiTinh, NgaySinh, SDT, Email, TenDangNhap FROM NhanVien";
            return DBConnect.Instance.ExecuteQuery(query);
        }

        public int InsertOrUpdate(string maNV, string hoTen, string gioiTinh, DateTime ngaySinh, string sdt, string email)
        {
            return DBConnect.Instance.ExecuteNonQuery("sp_Them_Sua_NhanVien",
                new SqlParameter("@MaNV", maNV),
                new SqlParameter("@HoTen", hoTen),
                new SqlParameter("@GioiTinh", gioiTinh),
                new SqlParameter("@NgaySinh", ngaySinh),
                new SqlParameter("@SDT", sdt),
                new SqlParameter("@Email", email));
        }

        public int Delete(string maNV)
        {
            return DBConnect.Instance.ExecuteNonQuery("sp_Xoa_NhanVien",
                new SqlParameter("@MaNV", maNV));
        }
    }
}
