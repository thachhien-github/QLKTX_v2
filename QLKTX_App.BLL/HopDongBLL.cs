// BLL/HopDongBLL.cs
using System.Data;
using QLKTX_App.DAL;

namespace QLKTX_App.BLL
{
    public class HopDongBLL
    {
        private readonly HopDongDAL _dal = new HopDongDAL();

        public DataTable Search(string mssv, string maPhong, string trangThai)
            => _dal.Search(mssv, maPhong, trangThai);

        public bool GiaHan(int maPhanBo, int soThangThem)
            => _dal.GiaHan(maPhanBo, soThangThem) > 0;
    }
}
