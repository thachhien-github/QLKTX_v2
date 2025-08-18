using System.Data;
using QLKTX_App.DAL;
using QLKTX_App.DTO;

namespace QLKTX_App.BLL
{
    public class LoaiXeBLL
    {
        private readonly LoaiXeDAL _dal = new LoaiXeDAL();
        public DataTable GetAll() => _dal.GetAll();
        public int Insert(LoaiXeModel m) => _dal.Insert(m);
        public int Update(LoaiXeModel m) => _dal.Update(m);
        public int Delete(string ma) => _dal.Delete(ma);
    }
}
