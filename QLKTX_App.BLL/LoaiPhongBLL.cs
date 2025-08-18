using System.Data;
using QLKTX_App.DAL;
using QLKTX_App.DTO;

namespace QLKTX_App.BLL
{
    public class LoaiPhongBLL
    {
        private readonly LoaiPhongDAL _dal = new LoaiPhongDAL();

        public DataTable GetAll() => _dal.GetAll();

        public bool Insert(LoaiPhongModel m) => _dal.Insert(m) > 0;

        public bool Update(LoaiPhongModel m) => _dal.Update(m) > 0;

        public bool Delete(string maLoai) => _dal.Delete(maLoai) > 0;
    }
}
