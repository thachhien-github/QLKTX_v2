using QLKTX_App.DAL;
using QLKTX_App.DTO;

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
    }
}
