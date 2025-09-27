using QLKTX_App.DTO;
using QLKTX_App.Utilities;
using System;
using System.Data;
using System.Data.SqlClient;

namespace QLKTX_App.DAL
{
    public class SinhVienDAL
    {
        private readonly DBConnect _db = new DBConnect();
        private readonly DBHelper _dbh = new DBHelper();

        public DataTable GetAll()
        {
            using (var conn = _db.GetConnection())
            using (var da = new SqlDataAdapter("SELECT * FROM SinhVien ORDER BY MSSV", conn))
            {
                var tb = new DataTable();
                da.Fill(tb);
                return tb;
            }
        }

        public int Insert(SinhVienModel sv)
        {
            var prms = new[]
            {
                new SqlParameter("@MSSV", sv.MSSV),
                new SqlParameter("@HoTen", sv.HoTen),
                new SqlParameter("@GioiTinh", (object)sv.GioiTinh ?? DBNull.Value),
                new SqlParameter("@NgaySinh", (object)sv.NgaySinh ?? DBNull.Value),
                new SqlParameter("@SDT", (object)sv.SDT ?? DBNull.Value),
                new SqlParameter("@DiaChi", (object)sv.DiaChi ?? DBNull.Value)
            };
            return _dbh.ExecuteNonQuery("sp_SinhVien_Them", true, prms);
        }

        public int Update(SinhVienModel sv)
        {
            var prms = new[]
            {
                new SqlParameter("@MSSV", sv.MSSV),
                new SqlParameter("@HoTen", sv.HoTen),
                new SqlParameter("@GioiTinh", (object)sv.GioiTinh ?? DBNull.Value),
                new SqlParameter("@NgaySinh", (object)sv.NgaySinh ?? DBNull.Value),
                new SqlParameter("@SDT", (object)sv.SDT ?? DBNull.Value),
                new SqlParameter("@DiaChi", (object)sv.DiaChi ?? DBNull.Value)
            };
            return _dbh.ExecuteNonQuery("sp_SinhVien_Sua", true, prms);
        }

        public DataTable GetByPhong(string maPhong)
        {
            var prms = new[] { new SqlParameter("@MaPhong", maPhong) };
            return _dbh.ExecuteQuery(
                @"SELECT sv.MSSV, sv.HoTen, sv.NgaySinh, sv.GioiTinh
                  FROM SinhVien sv
                  INNER JOIN PhanBo pb ON sv.MSSV = pb.MSSV
                  WHERE pb.MaPhong=@MaPhong", false, prms);
        }
    }
}
