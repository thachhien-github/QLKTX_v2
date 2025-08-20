using System.Data;
using System.Data.SqlClient;
using QLKTX_App.DTO;
using QLKTX_App.Utilities;

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
            string sql = @"INSERT INTO SinhVien(MSSV, HoTen, NgaySinh, GioiTinh, SDT, DiaChi)
                           VALUES(@MSSV, @HoTen, @NgaySinh, @GioiTinh, @SDT, @DiaChi)";
            using (var conn = _db.GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MSSV", sv.MSSV);
                cmd.Parameters.AddWithValue("@HoTen", sv.HoTen);
                cmd.Parameters.AddWithValue("@NgaySinh", sv.NgaySinh);
                cmd.Parameters.AddWithValue("@GioiTinh", sv.GioiTinh);
                cmd.Parameters.AddWithValue("@SDT", sv.SDT);
                cmd.Parameters.AddWithValue("@DiaChi", sv.DiaChi);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public int Delete(string mssv)
        {
            using (var conn = _db.GetConnection())
            using (var cmd = new SqlCommand("DELETE FROM SinhVien WHERE MSSV=@mssv", conn))
            {
                cmd.Parameters.AddWithValue("@mssv", mssv);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
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
