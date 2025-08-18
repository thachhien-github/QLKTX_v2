// DAL/HopDongDAL.cs
using System.Data;
using System.Data.SqlClient;
using QLKTX_App.Utilities;

namespace QLKTX_App.DAL
{
    public class HopDongDAL
    {
        private readonly DBConnect _db = new DBConnect();

        public DataTable Search(string mssv, string maPhong, string trangThai)
        {
            string sql = @"SELECT pb.MaPhanBo, sv.MSSV, sv.HoTen, pb.MaPhong, 
                                  pb.NgayPhanBo, pb.SoThang, pb.MienTienPhong, pb.SoDotThu, pb.GhiChu
                           FROM PhanBo pb 
                           JOIN SinhVien sv ON pb.MSSV = sv.MSSV
                           WHERE (sv.MSSV LIKE @mssv OR @mssv = '')
                             AND (pb.MaPhong = @maPhong OR @maPhong = '')
                             AND ((@trangThai = '') OR
                                  (@trangThai = N'Còn hạn' AND DATEADD(MONTH, pb.SoThang, pb.NgayPhanBo) >= GETDATE()) OR
                                  (@trangThai = N'Hết hạn' AND DATEADD(MONTH, pb.SoThang, pb.NgayPhanBo) < GETDATE()))";

            using (var conn = _db.GetConnection())
            using (var da = new SqlDataAdapter(sql, conn))
            {
                da.SelectCommand.Parameters.AddWithValue("@mssv", $"%{mssv}%");
                da.SelectCommand.Parameters.AddWithValue("@maPhong", maPhong ?? "");
                da.SelectCommand.Parameters.AddWithValue("@trangThai", trangThai ?? "");

                var tb = new DataTable();
                da.Fill(tb);
                return tb;
            }
        }

        public int GiaHan(int maPhanBo, int soThangThem)
        {
            string sql = "UPDATE PhanBo SET SoThang = SoThang + @soThang WHERE MaPhanBo=@id";
            using (var conn = _db.GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", maPhanBo);
                cmd.Parameters.AddWithValue("@soThang", soThangThem);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
