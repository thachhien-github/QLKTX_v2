using System.Data;
using QLKTX_App.DAL;
using QLKTX_App.DTO;

namespace QLKTX_App.BLL
{
    public class PhanBoBLL
    {
        private readonly PhanBoDAL _dal = new PhanBoDAL();

        public DataTable GetAll() => _dal.GetAll();
        public DataTable GetBySinhVien(string mssv) => _dal.GetBySinhVien(mssv);
        public bool Insert(PhanBoModel pb) => _dal.Insert(pb) > 0;
        public bool DeleteByMSSV(string mssv)
        {
            return _dal.DeleteByMSSV(mssv);
        }

        public DataTable GetAllPhong() => _dal.GetAllPhong();
        public bool CheckDangO(string mssv) => _dal.CheckDangO(mssv);
        public DataTable GetChiTietPhanBo(string mssv, string maPhong) => _dal.GetChiTietPhanBo(mssv, maPhong);
        public bool Update(PhanBoModel pb)
        {
            return _dal.Update(pb);
        }

        public bool Delete(string mssv, string maPhong)
        {
            return _dal.Delete(mssv, maPhong);
        }


        public bool ChuyenPhong(PhanBoModel pb, string maPhongCu)
        {
            return _dal.ChuyenPhong(pb, maPhongCu);
        }
    }
}
