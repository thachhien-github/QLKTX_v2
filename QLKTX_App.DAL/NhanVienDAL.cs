using System;
using System.Data;
using System.Data.SqlClient;

namespace QLKTX_App.DAL
{
    public class NhanVienDAL
    {
        private readonly DBHelper db = new DBHelper();

        // Lấy danh sách nhân viên
        public DataTable GetAll()
        {
            string query = @"SELECT MaNV, HoTen, GioiTinh, NgaySinh, SDT, Email FROM NhanVien";
            return db.ExecuteQuery(query, false);
        }

        // Thêm nhân viên mới
        public int Insert(string maNV, string hoTen, string gioiTinh, DateTime ngaySinh, string sdt, string email)
        {
            return db.ExecuteNonQuery("sp_NhanVien_Them", true,
                new SqlParameter("@MaNV", maNV),
                new SqlParameter("@HoTen", hoTen),
                new SqlParameter("@GioiTinh", gioiTinh),
                new SqlParameter("@NgaySinh", ngaySinh),
                new SqlParameter("@SDT", sdt),
                new SqlParameter("@Email", email));
        }

        // Cập nhật nhân viên
        public int Update(string maNV, string hoTen, string gioiTinh, DateTime ngaySinh, string sdt, string email)
        {
            return db.ExecuteNonQuery("sp_NhanVien_Sua", true,
                new SqlParameter("@MaNV", maNV),
                new SqlParameter("@HoTen", hoTen),
                new SqlParameter("@GioiTinh", gioiTinh),
                new SqlParameter("@NgaySinh", ngaySinh),
                new SqlParameter("@SDT", sdt),
                new SqlParameter("@Email", email));
        }

        // Xóa nhân viên
        public int Delete(string maNV)
        {
            return db.ExecuteNonQuery("sp_NhanVien_Xoa", true,
                new SqlParameter("@MaNV", maNV));
        }
    }
}
