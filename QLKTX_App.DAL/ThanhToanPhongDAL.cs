using System;
using System.Data;
using System.Data.SqlClient;
using QLKTX_App.DTO;
using QLKTX_App.Utilities;

namespace QLKTX_App.DAL
{
    public class ThanhToanPhongDAL
    {
        private DBHelper db = new DBHelper();

        public DataTable GetAll()
        {
            string sql = "SELECT * FROM ThanhToanPhong";
            return db.ExecuteQuery(sql, false);
        }

        public int Insert(ThanhToanPhongModel model)
        {
            return db.ExecuteNonQuery("sp_ThanhToanPhong_Them", true,
                new SqlParameter("@MSSV", model.MSSV),
                new SqlParameter("@MaPhong", model.MaPhong),
                new SqlParameter("@SoThangThu", model.SoThangThu),
                new SqlParameter("@NgayThu", model.NgayThu),
                new SqlParameter("@NguoiThu", model.NguoiThu ?? (object)DBNull.Value),
                new SqlParameter("@GhiChu", model.GhiChu ?? (object)DBNull.Value)
            );
        }

        public int Delete(int id)
        {
            return db.ExecuteNonQuery("sp_ThanhToanPhong_Xoa", true,
                new SqlParameter("@ID", id));
        }
    }
}
