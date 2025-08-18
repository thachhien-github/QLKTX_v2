// BLL/SinhVienBLL.cs
using System.Data;
using QLKTX_App.DAL;
using QLKTX_App.DTO;

namespace QLKTX_App.BLL
{
    public class SinhVienBLL
    {
        private readonly SinhVienDAL _dal = new SinhVienDAL();

        public DataTable GetAll() => _dal.GetAll();
        public bool Insert(SinhVienModel sv) => _dal.Insert(sv) > 0;
        public bool Delete(string mssv) => _dal.Delete(mssv) > 0;
    }
}
