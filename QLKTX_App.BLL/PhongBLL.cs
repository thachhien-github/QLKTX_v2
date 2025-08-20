using System.Data;
using QLKTX_App.DAL;
using QLKTX_App.DTO;

namespace QLKTX_App.BLL
{
    public class PhongBLL
    {
        private readonly PhongDAL _dal = new PhongDAL();

        public DataTable GetAll() => _dal.GetAll();
        public DataTable GetByTang(string maTang) => _dal.GetByTang(maTang);
        public bool Insert(PhongModel p) => !_dal.CheckExists(p.MaPhong) && _dal.Insert(p);
        public bool Update(PhongModel p) => _dal.CheckExists(p.MaPhong) && _dal.Update(p);
        public bool Delete(string maPhong) => _dal.Delete(maPhong);

        public void CapNhatTrangThai(string maPhong)
        {
            int soLuongToiDa = _dal.GetSoLuongToiDa(maPhong);
            int soLuongHienTai = _dal.GetSoLuongSinhVien(maPhong);

            string trangThai = "Trống";
            if (soLuongHienTai >= soLuongToiDa) trangThai = "Đầy";
            else if (soLuongHienTai > 0) trangThai = "Còn chỗ";

            _dal.UpdateTrangThai(maPhong, trangThai);
        }
    }
}
