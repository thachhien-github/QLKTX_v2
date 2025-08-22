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

        public DataTable GetChiTietPhanBo(string mssv, string maPhong)
        {
            string sql = @"
        SELECT pb.SoThang, pb.GhiChu,
               sv.HoTen,
               p.MaPhong, lp.GiaPhong, lp.SucChua
        FROM PhanBo pb
        JOIN SinhVien sv ON pb.MSSV = sv.MSSV
        JOIN Phong p ON pb.MaPhong = p.MaPhong
        JOIN LoaiPhong lp ON p.MaLoai = lp.MaLoai
        WHERE pb.MSSV=@MSSV AND pb.MaPhong=@MaPhong";

            var prms = new[]
            {
                new SqlParameter("@MSSV", mssv),
                new SqlParameter("@MaPhong", maPhong)
            };

            return _db.ExecuteQuery(sql, false, prms);
        }


    }

}
