using System;
using System.Data;
using QLKTX_App.DAL;

namespace QLKTX_App.BLL
{
    public class NhanVienBLL
    {
        private readonly NhanVienDAL dal = new NhanVienDAL();

        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        public string InsertOrUpdate(string maNV, string hoTen, string gioiTinh, DateTime ngaySinh, string sdt, string email)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(maNV)) return "Mã NV không được để trống!";
            if (string.IsNullOrWhiteSpace(hoTen)) return "Họ tên không được để trống!";
            if (ngaySinh > DateTime.Now) return "Ngày sinh không hợp lệ!";
            if (string.IsNullOrWhiteSpace(sdt)) return "SĐT không được để trống!";

            int rows = dal.InsertOrUpdate(maNV, hoTen.Trim(), gioiTinh, ngaySinh, sdt.Trim(), email?.Trim());
            return rows > 0 ? "Thành công" : "Thất bại";
        }

        public string Delete(string maNV)
        {
            if (string.IsNullOrWhiteSpace(maNV)) return "Chưa chọn nhân viên!";
            int rows = dal.Delete(maNV);
            return rows > 0 ? "Thành công" : "Thất bại";
        }
    }
}
