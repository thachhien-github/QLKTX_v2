using QLKTX_App.DAL;
using QLKTX_App.DTO;

namespace QLKTX_App.BLL
{
    public class GiaDienNuocBLL
    {
        private readonly GiaDienNuocDAL _dal = new GiaDienNuocDAL();
        public GiaDienNuocModel GetCurrent() => _dal.GetCurrent();
        public int Update(GiaDienNuocModel m) => _dal.Update(m);
    }
}
