using QLKTX_App.DAL;
using QLKTX_App.DTO;

namespace QLKTX_App.BLL
{
    public class BLL_TaiKhoan
    {
        private DAL_TaiKhoan _dal;

        public BLL_TaiKhoan()
        {
            _dal = new DAL_TaiKhoan();
        }

        public TaiKhoanModel KiemTraDangNhap(string username, string password)
        {
            return _dal.KiemTraDangNhap(username, password);
        }
    }
}
