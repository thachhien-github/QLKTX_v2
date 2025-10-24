using QLKTX_App.DAL;

namespace QLKTX_App.BLL
{
    public class PhongBLL
    {
        private readonly PhongDAL _dal = new PhongDAL();

        public System.Data.DataTable GetAll() => _dal.GetAll();
        public System.Data.DataTable GetByTang(string maTang) => _dal.GetByTang(maTang);
        public bool Insert(QLKTX_App.DTO.PhongModel p) => _dal.Insert(p);
        public bool Update(QLKTX_App.DTO.PhongModel p) => _dal.Update(p);
        public bool Delete(string maPhong) => _dal.Delete(maPhong);
        public bool CheckExists(string maPhong) => _dal.CheckExists(maPhong);
        public int GetSoLuongSinhVien(string maPhong) => _dal.GetSoLuongSinhVien(maPhong);
        public int GetSoLuongToiDa(string maPhong) => _dal.GetSoLuongToiDa(maPhong);
        public void UpdateTrangThai(string maPhong, string trangThai) => _dal.UpdateTrangThai(maPhong, trangThai);
        public void RefreshTrangThai(string maPhong) => _dal.RefreshTrangThai(maPhong);
    }
}