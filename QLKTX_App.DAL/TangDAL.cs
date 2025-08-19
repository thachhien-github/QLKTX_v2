using System.Data;
using System.Data.SqlClient;

namespace QLKTX_App.DAL
{
    public class TangDAL
    {
        private readonly DBHelper db = new DBHelper();

        public DataTable GetAll()
        {
            return db.ExecuteQuery("SELECT * FROM Tang", false);
        }

        public int Insert(string maTang, string tenTang)
        {
            return db.ExecuteNonQuery("sp_Tang_Them", true,
                new SqlParameter("@MaTang", maTang),
                new SqlParameter("@TenTang", tenTang));
        }

        public int Delete(string maTang)
        {
            return db.ExecuteNonQuery("sp_Tang_Xoa", true,
                new SqlParameter("@MaTang", maTang));
        }
    }
}
