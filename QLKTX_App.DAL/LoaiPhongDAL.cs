using System.Data;
using System.Data.SqlClient;
using QLKTX_App.DTO;

namespace QLKTX_App.DAL
{
    public class LoaiPhongDAL
    {
        private readonly DBConnect _db = new DBConnect();

        public DataTable GetAll()
        {
            using (var conn = _db.GetConnection())
            using (var da = new SqlDataAdapter(
                "SELECT MaLoai, TenLoai, GiaPhong, SucChua FROM LoaiPhong ORDER BY MaLoai", conn))
            {
                var tb = new DataTable();
                da.Fill(tb);
                return tb;
            }
        }

        public int Insert(LoaiPhongModel m)
        {
            using (var conn = _db.GetConnection())
            using (var cmd = new SqlCommand(
                "INSERT INTO LoaiPhong(MaLoai, TenLoai, GiaPhong, SucChua) VALUES(@MaLoai,@TenLoai,@GiaPhong,@SucChua)", conn))
            {
                cmd.Parameters.AddWithValue("@MaLoai", m.MaLoai);
                cmd.Parameters.AddWithValue("@TenLoai", m.TenLoai);
                cmd.Parameters.AddWithValue("@GiaPhong", m.GiaPhong);
                cmd.Parameters.AddWithValue("@SucChua", m.SucChua);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public int Update(LoaiPhongModel m)
        {
            using (var conn = _db.GetConnection())
            using (var cmd = new SqlCommand(
                "UPDATE LoaiPhong SET TenLoai=@TenLoai, GiaPhong=@GiaPhong, SucChua=@SucChua WHERE MaLoai=@MaLoai", conn))
            {
                cmd.Parameters.AddWithValue("@MaLoai", m.MaLoai);
                cmd.Parameters.AddWithValue("@TenLoai", m.TenLoai);
                cmd.Parameters.AddWithValue("@GiaPhong", m.GiaPhong);
                cmd.Parameters.AddWithValue("@SucChua", m.SucChua);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public int Delete(string maLoai)
        {
            using (var conn = _db.GetConnection())
            using (var cmd = new SqlCommand("DELETE FROM LoaiPhong WHERE MaLoai=@MaLoai", conn))
            {
                cmd.Parameters.AddWithValue("@MaLoai", maLoai);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
