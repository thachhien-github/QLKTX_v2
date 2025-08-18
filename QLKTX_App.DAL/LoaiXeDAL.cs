using System.Data;
using System.Data.SqlClient;
using QLKTX_App.DTO;
using QLKTX_App.Utilities;

namespace QLKTX_App.DAL
{
    public class LoaiXeDAL
    {
        private readonly DBConnect _db = new DBConnect();

        public DataTable GetAll()
        {
            using (var conn = _db.GetConnection())
            using (var da = new SqlDataAdapter(
                "SELECT MaLoaiXe, TenLoai, GiaGiuXe FROM LoaiXe ORDER BY MaLoaiXe", conn))
            {
                var tb = new DataTable();
                da.Fill(tb);
                return tb;
            }
        }

        public int Insert(LoaiXeModel m)
        {
            using (var conn = _db.GetConnection())
            using (var cmd = new SqlCommand(
                "INSERT INTO LoaiXe (MaLoaiXe, TenLoai, GiaGiuXe) VALUES (@MaLoaiXe, @TenLoai, @GiaGiuXe)", conn))
            {
                cmd.Parameters.AddWithValue("@MaLoaiXe", m.MaLoaiXe);
                cmd.Parameters.AddWithValue("@TenLoai", m.TenLoai);
                cmd.Parameters.AddWithValue("@GiaGiuXe", m.GiaGiuXe);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public int Update(LoaiXeModel m)
        {
            using (var conn = _db.GetConnection())
            using (var cmd = new SqlCommand(
                "UPDATE LoaiXe SET TenLoai = @TenLoai, GiaGiuXe = @GiaGiuXe WHERE MaLoaiXe = @MaLoaiXe", conn))
            {
                cmd.Parameters.AddWithValue("@MaLoaiXe", m.MaLoaiXe);
                cmd.Parameters.AddWithValue("@TenLoai", m.TenLoai);
                cmd.Parameters.AddWithValue("@GiaGiuXe", m.GiaGiuXe);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public int Delete(string maLoaiXe)
        {
            using (var conn = _db.GetConnection())
            using (var cmd = new SqlCommand(
                "DELETE FROM LoaiXe WHERE MaLoaiXe = @MaLoaiXe", conn))
            {
                cmd.Parameters.AddWithValue("@MaLoaiXe", maLoaiXe);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
