using QLKTX_App.DTO;
using System;
using System.Data;
using System.Data.SqlClient;

namespace QLKTX_App.DAL
{
    public class PhanBoDAL
    {
        private readonly DBConnect _db = new DBConnect();

        public DataTable GetBySinhVien(string mssv)
        {
            using (var conn = _db.GetConnection())
            using (var da = new SqlDataAdapter("SELECT * FROM PhanBo WHERE MSSV = @MSSV", conn))
            {
                da.SelectCommand.Parameters.AddWithValue("@MSSV", mssv);
                var tb = new DataTable();
                da.Fill(tb);
                return tb;
            }
        }

        public DataTable GetAllPhong()
        {
            using (var conn = _db.GetConnection())
            using (var da = new SqlDataAdapter("SELECT MaPhong FROM Phong ORDER BY MaPhong", conn))
            {
                var tb = new DataTable();
                da.Fill(tb);
                return tb;
            }
        }


        public int Insert(PhanBoModel m)
        {
            using (var conn = _db.GetConnection())
            using (var cmd = new SqlCommand("INSERT INTO PhanBo (MSSV, MaPhong, SoThang, NgayPhanBo, MienTienPhong, SoDotThu, GhiChu) " +
                                            "VALUES (@MSSV, @MaPhong, @SoThang, @NgayPhanBo, @MienTienPhong, @SoDotThu, @GhiChu)", conn))
            {
                cmd.Parameters.AddWithValue("@MSSV", m.MSSV);
                cmd.Parameters.AddWithValue("@MaPhong", m.MaPhong); // bắt buộc, không để null
                cmd.Parameters.AddWithValue("@SoThang", m.SoThang);
                cmd.Parameters.AddWithValue("@NgayPhanBo", m.NgayPhanBo);
                cmd.Parameters.AddWithValue("@MienTienPhong", m.MienTienPhong);
                cmd.Parameters.AddWithValue("@SoDotThu", m.SoDotThu);
                cmd.Parameters.AddWithValue("@GhiChu", m.GhiChu ?? (object)DBNull.Value);

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
