using System.Data;
using QLKTX_App.DTO;
using QLKTX_App.DAL;

namespace QLKTX_App.BLL
{
    public class ThanhToanPhongBLL
    {
        private ThanhToanPhongDAL dal = new ThanhToanPhongDAL();

        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        public int Insert(ThanhToanPhongModel model)
        {
            return dal.Insert(model);
        }

        public int Delete(int id)
        {
            return dal.Delete(id);
        }
    }
}
