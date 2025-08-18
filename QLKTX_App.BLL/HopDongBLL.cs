using System.Data;
using QLKTX_App.DAL;

namespace QLKTX_App.BLL
{
    public class HopDongBLL
    {
        private readonly HopDongDAL dal = new HopDongDAL();

        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        public DataTable Search(string mssv, string phong, string trangThai)
        {
            return dal.Search(mssv, phong, trangThai);
        }

        public bool GiaHan(string mssv, int soThang)
        {
            return dal.GiaHan(mssv, soThang);
        }

    }
}
