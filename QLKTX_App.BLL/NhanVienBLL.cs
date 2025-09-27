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

        public string Insert(string maNV, string hoTen, string gioiTinh, DateTime ngaySinh, string sdt, string email)
        {
            if (string.IsNullOrWhiteSpace(maNV)) return "Mã NV không được để trống!";
            if (string.IsNullOrWhiteSpace(hoTen)) return "Họ tên không được để trống!";
            if (ngaySinh > DateTime.Now) return "Ngày sinh không hợp lệ!";

            int rows = dal.Insert(maNV.Trim(), hoTen.Trim(), gioiTinh, ngaySinh, sdt?.Trim(), email?.Trim());
            return rows > 0 ? "Thêm nhân viên thành công" : "Thêm nhân viên thất bại";
        }

        public string Update(string maNV, string hoTen, string gioiTinh, DateTime ngaySinh, string sdt, string email)
        {
            if (string.IsNullOrWhiteSpace(maNV)) return "Mã NV không được để trống!";
            if (string.IsNullOrWhiteSpace(hoTen)) return "Họ tên không được để trống!";
            if (ngaySinh > DateTime.Now) return "Ngày sinh không hợp lệ!";

            int rows = dal.Update(maNV.Trim(), hoTen.Trim(), gioiTinh, ngaySinh, sdt?.Trim(), email?.Trim());
            return rows > 0 ? "Cập nhật nhân viên thành công" : "Cập nhật thất bại";
        }

        public string Delete(string maNV)
        {
            if (string.IsNullOrWhiteSpace(maNV)) return "⚠️ Chưa chọn nhân viên!";
            int rows = dal.Delete(maNV.Trim());
            return rows > 0 ? "Xóa nhân viên thành công" : "Xóa thất bại";
        }
    }
}
