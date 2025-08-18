using QLKTX_App.DAL;
using QLKTX_App.DTO;
using System.Data;

namespace QLKTX_App.BLL
{
    public class PhanBoBLL
    {
        private readonly PhanBoDAL _dal = new PhanBoDAL();

        public DataTable GetBySinhVien(string mssv) => _dal.GetBySinhVien(mssv);

        public DataTable GetAllPhong()
        {
            return _dal.GetAllPhong();
        }

        public bool Insert(PhanBoModel m)
        {
            if (string.IsNullOrWhiteSpace(m.MSSV) || string.IsNullOrWhiteSpace(m.MaPhong))
                return false;

            return _dal.Insert(m) > 0;
        }
    }
}
