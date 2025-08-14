using QLKTX_App.DAL;
using System;
using System.Data;

namespace QLKTX_App.BLL
{
    public class NhanVienBLL
    {
        private readonly NhanVienDAL dal = new NhanVienDAL();

        public DataTable GetAll() => dal.GetAll();
        public DataTable Search(string keyword) => dal.Search(keyword);

        public bool Save(string maNV, string hoTen, string gioiTinh, DateTime ngaySinh, string sdt, string email, string tenDangNhap)
        {
            if (dal.Exists(maNV))
            {
                dal.Update(maNV, hoTen, gioiTinh, ngaySinh, sdt, email);
                return false; // false = cập nhật
            }
            else
            {
                dal.Insert(maNV, hoTen, gioiTinh, ngaySinh, sdt, email, tenDangNhap);
                return true; // true = thêm mới
            }
        }

        public bool Delete(string maNV) => dal.Delete(maNV);
    }
}
