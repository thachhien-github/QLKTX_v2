using System.Data;
using QLKTX_App.DAL;

namespace QLKTX_App.BLL
{
    public class QLTaiKhoanBLL
    {
        private readonly QLTaiKhoanDAL dal = new QLTaiKhoanDAL();

        // Lấy danh sách
        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        // Thêm tài khoản mới
        public string Insert(string maNV, string tenDN, string matKhau, bool trangThai, string vaiTro)
        {
            if (string.IsNullOrWhiteSpace(maNV)) return "⚠️ Chưa chọn nhân viên!";
            if (string.IsNullOrWhiteSpace(tenDN)) return "⚠️ Tên đăng nhập không được để trống!";
            if (string.IsNullOrWhiteSpace(matKhau)) return "⚠️ Mật khẩu không được để trống!";
            if (string.IsNullOrWhiteSpace(vaiTro)) return "⚠️ Chưa chọn vai trò!";

            int rows = dal.Insert(maNV.Trim(), tenDN.Trim(), matKhau.Trim(), trangThai, vaiTro.Trim());
            return rows > 0 ? "✅ Thêm tài khoản thành công" : "❌ Thêm tài khoản thất bại";
        }

        // Cập nhật tài khoản (dựa vào MaNV)
        public string Update(string maNV, string tenDN, string matKhau, bool trangThai, string vaiTro)
        {
            if (string.IsNullOrWhiteSpace(maNV)) return "⚠️ Chưa chọn nhân viên!";
            if (string.IsNullOrWhiteSpace(tenDN)) return "⚠️ Tên đăng nhập không được để trống!";
            if (string.IsNullOrWhiteSpace(matKhau)) return "⚠️ Mật khẩu không được để trống!";
            if (string.IsNullOrWhiteSpace(vaiTro)) return "⚠️ Chưa chọn vai trò!";

            int rows = dal.Update(maNV.Trim(), tenDN.Trim(), matKhau.Trim(), trangThai, vaiTro.Trim());
            return rows > 0 ? "✅ Cập nhật tài khoản thành công" : "❌ Cập nhật thất bại";
        }

        // Xóa tài khoản (dựa vào MaNV)
        public string Delete(string maNV)
        {
            if (string.IsNullOrWhiteSpace(maNV)) return "⚠️ Chưa chọn nhân viên!";
            int rows = dal.Delete(maNV.Trim());
            return rows > 0 ? "✅ Xóa tài khoản thành công" : "❌ Xóa thất bại";
        }
    }
}
