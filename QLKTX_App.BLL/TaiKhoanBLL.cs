using QLKTX_App.DAL;
using QLKTX_App.DTO;
using System;

namespace QLKTX_App.BLL
{
    public class TaiKhoanBLL
    {
        private TaiKhoanDAL _dal = new TaiKhoanDAL();

        public TaiKhoanModel KiemTraDangNhap(string tenDangNhap, string matKhau)
        {
            // Gọi DAL để lấy tài khoản
            return _dal.KiemTraDangNhap(tenDangNhap, matKhau);
        }

        public string Update(string maNV, bool trangThai)
        {
            try
            {
                int rows = _dal.UpdateTrangThai(maNV, trangThai);
                return rows > 0 ? "Cập nhật trạng thái thành công!" : "Không tìm thấy tài khoản cần cập nhật!";
            }
            catch (Exception ex)
            {
                return "Lỗi khi cập nhật: " + ex.Message;
            }
        }

    }
}
