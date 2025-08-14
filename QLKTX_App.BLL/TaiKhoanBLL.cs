using QLKTX_App.DAL;
using System.Data;

namespace QLKTX_App.BLL
{
    public class TaiKhoanBLL
    {
        private TaiKhoanDAL dal = new TaiKhoanDAL();

        public DataRow DangNhap(string user, string pass)
        {
            return dal.DangNhap(user, pass);
        }
    }
}
