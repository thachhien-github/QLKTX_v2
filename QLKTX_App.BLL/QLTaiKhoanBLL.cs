using System.Data;
using QLKTX_App.DAL;

namespace QLKTX_App.BLL
{
    public class QLTaiKhoanBLL
    {
        private QLTaiKhoanDAL dal = new QLTaiKhoanDAL();

        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        public DataTable Search(string keyword)
        {
            return dal.Search(keyword);
        }

        public bool Save(string tenDangNhap, string matKhau, string maNV, int trangThai)
        {
            return dal.InsertOrUpdate(tenDangNhap, matKhau, maNV, trangThai);
        }

        public bool Delete(string tenDangNhap)
        {
            return dal.Delete(tenDangNhap);
        }
    }
}
