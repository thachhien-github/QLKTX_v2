using System.Data;
using QLKTX_App.DAL;
using QLKTX_App.DTO;

namespace QLKTX_App.BLL
{
    public class SinhVienBLL
    {
        private readonly SinhVienDAL _dal = new SinhVienDAL();
        private readonly PhongBLL _phongBLL = new PhongBLL();
        private readonly PhanBoDAL _pbDAL = new PhanBoDAL();

        public DataTable GetAll() => _dal.GetAll();
        public bool Insert(SinhVienModel sv) => _dal.Insert(sv) > 0;
        public DataTable GetByPhong(string maPhong) => _dal.GetByPhong(maPhong);

        public bool Delete(string mssv)
        {
            bool ok = _dal.Delete(mssv) > 0;
            if (ok)
            {
                string maPhong = _pbDAL.GetPhongByMSSV(mssv);
                if (!string.IsNullOrEmpty(maPhong))
                    _phongBLL.CapNhatTrangThai(maPhong);
            }
            return ok;
        }
    }
}
