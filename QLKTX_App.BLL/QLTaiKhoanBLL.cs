using System.Data;
using QLKTX_App.DAL;

namespace QLKTX_App.BLL
{
    public class QLTaiKhoanBLL
    {
        private readonly QLTaiKhoanDAL dal = new QLTaiKhoanDAL();

        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        public string InsertOrUpdate(string tenDN, string matKhau, string trangThai, string maNV)
        {
            if (string.IsNullOrWhiteSpace(tenDN)) return "Tên đăng nhập không được để trống!";
            if (string.IsNullOrWhiteSpace(matKhau)) return "Mật khẩu không được để trống!";
            if (string.IsNullOrWhiteSpace(maNV)) return "Chưa chọn nhân viên!";

            int rows = dal.InsertOrUpdate(tenDN.Trim(), matKhau.Trim(), trangThai, maNV);
            return rows > 0 ? "Thành công" : "Thất bại";
        }

        public string Delete(string tenDN)
        {
            if (string.IsNullOrWhiteSpace(tenDN)) return "Chưa chọn tài khoản!";
            int rows = dal.Delete(tenDN);
            return rows > 0 ? "Thành công" : "Thất bại";
        }
    }
}
