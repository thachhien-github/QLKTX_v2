using System.Collections.Generic;
using QLKTX_App.DAL;
using QLKTX_App.DTO;

namespace QLKTX_App.BLL
{
    public class ChiSoBLL
    {
        private readonly ChiSoDAL _dal = new ChiSoDAL();

        public List<ChiSoModel> GetAll() => _dal.GetAll();
        public ChiSoModel GetChiSoGanNhat(string maPhong) => _dal.GetChiSoGanNhat(maPhong);

        public void Them(ChiSoModel cs) => _dal.Them(cs);

        public void Sua(ChiSoModel cs) => _dal.Sua(cs);

        public void Xoa(string maPhong, int thang, int nam) => _dal.Xoa(maPhong, thang, nam);
    }
}
