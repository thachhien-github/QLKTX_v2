using QLKTX_App.DTO;
using QLKTX_App.Utilities;
using System;
using System.Data;
using System.Data.SqlClient;

namespace QLKTX_App.DAL
{
    public class PhanBoDAL
    {
        private readonly DBHelper _db = new DBHelper();

        public DataTable GetAll()
        {
            return _db.ExecuteQuery("SELECT * FROM PhanBo", false);
        }

        public DataTable GetBySinhVien(string mssv)
        {
            var prms = new[] { new SqlParameter("@MSSV", mssv) };
            return _db.ExecuteQuery("SELECT * FROM PhanBo WHERE MSSV=@MSSV", false, prms);
        }

        public int Insert(PhanBoModel pb)
        {
            var prms = new[]
            {
                new SqlParameter("@MSSV", pb.MSSV),
                new SqlParameter("@MaPhong", pb.MaPhong),
                new SqlParameter("@SoThang", pb.SoThang),
                new SqlParameter("@NgayPhanBo", pb.NgayPhanBo),
                new SqlParameter("@MienTienPhong", pb.MienTienPhong),
                new SqlParameter("@SoDotThu", pb.SoDotThu),
                new SqlParameter("@GhiChu", pb.GhiChu ?? (object)DBNull.Value)
            };
            return _db.ExecuteNonQuery("sp_PhanBo_Them", true, prms);
        }

        public int Delete(int id)
        {
            var prms = new[] { new SqlParameter("@ID", id) };
            return _db.ExecuteNonQuery("sp_PhanBo_Xoa", true, prms);
        }

        public DataTable GetAllPhong()
        {
            return _db.ExecuteQuery("SELECT MaPhong FROM Phong", false);
        }

        public string GetPhongByMSSV(string mssv)
        {
            var prms = new[] { new SqlParameter("@MSSV", mssv) };
            object result = _db.ExecuteScalar("SELECT TOP 1 MaPhong FROM PhanBo WHERE MSSV=@MSSV", false, prms);
            return result?.ToString();
        }

        public bool CheckDangO(string mssv)
        {
            string sql = @"
        SELECT COUNT(*) 
        FROM PhanBo 
        WHERE MSSV=@MSSV
          AND DATEADD(MONTH, SoThang, NgayPhanBo) > GETDATE()";

            var prms = new[] { new SqlParameter("@MSSV", mssv) };
            object result = _db.ExecuteScalar(sql, false, prms);
            return Convert.ToInt32(result) > 0;
        }

    }

}
