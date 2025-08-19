using System.Data;
using QLKTX_App.DAL;

namespace QLKTX_App.BLL
{
    public class TangBLL
    {
        private readonly TangDAL dal = new TangDAL();

        public DataTable GetAll() => dal.GetAll();

        public bool Insert(string maTang, string tenTang)
        {
            return dal.Insert(maTang, tenTang) > 0;
        }

        public bool Delete(string maTang)
        {
            return dal.Delete(maTang) > 0;
        }
    }
}
