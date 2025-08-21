using System.Data;
using QLKTX_App.DAL;
using QLKTX_App.DTO;

namespace QLKTX_App.BLL
{
    public class TheXeBLL
    {
        private readonly TheXeDAL _dal = new TheXeDAL();

        public bool Them(TheXeModel xe) => _dal.Them(xe);
        public bool Sua(TheXeModel xe) => _dal.Sua(xe);
        public bool Xoa(string maThe) => _dal.Xoa(maThe);
        public DataTable GetAll() => _dal.GetAll();
        public DataTable Search(string keyword) => _dal.Search(keyword);
        public DataTable GetLoaiXe() => _dal.GetLoaiXe();
    }
}
