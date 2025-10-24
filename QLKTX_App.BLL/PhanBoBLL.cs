using QLKTX_App.DAL;
using QLKTX_App.DTO;
using System.Data;

namespace QLKTX_App.BLL
{
    public class PhanBoBLL
    {
        private readonly PhanBoDAL _dal = new PhanBoDAL();
        public DataTable GetAllPhong() => _dal.GetAllPhong();
        public DataTable GetBySinhVien(string mssv) => _dal.GetBySinhVien(mssv);
        public int Insert(PhanBoModel pb) => _dal.Insert(pb); // return int (rows or sp result)
        public bool DeleteByMSSV(string mssv) => _dal.DeleteByMSSV(mssv);
        public bool Delete(string mssv, string maPhong) => _dal.Delete(mssv, maPhong);
        public bool Update(PhanBoModel pb) => _dal.Update(pb);
        public bool ChuyenPhong(PhanBoModel pb, string maPhongCu) => _dal.ChuyenPhong(pb, maPhongCu);
        public int CountActiveByRoom(string maPhong) => _dal.CountActiveByRoom(maPhong);
        public bool CheckDangO(string mssv) => _dal.CheckDangO(mssv);
        public DataTable GetChiTietPhanBo(string mssv, string maPhong) => _dal.GetChiTietPhanBo(mssv, maPhong);
    }
}