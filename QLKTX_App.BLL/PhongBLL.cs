using System.Data;
using QLKTX_App.DAL;
using QLKTX_App.DTO;

namespace QLKTX_App.BLL
{
    public class PhongBLL
    {
        private readonly PhongDAL _dal = new PhongDAL();

        public DataTable GetAll()
        {
            return _dal.GetAll();
        }

        public bool Insert(PhongModel p)
        {
            if (_dal.CheckExists(p.MaPhong)) return false;
            return _dal.Insert(p);
        }

        public bool Update(PhongModel p)
        {
            if (!_dal.CheckExists(p.MaPhong)) return false; // kiểm tra tồn tại trước
            return _dal.Update(p);
        }

        public bool Delete(string maPhong)
        {
            return _dal.Delete(maPhong);
        }

        public bool CheckExists(string maPhong)
        {
            return _dal.CheckExists(maPhong);
        }

        public DataTable GetByTang(string maTang)
        {
            return _dal.GetByTang(maTang);
        }

    }
}
