using System.Data;
using System.Data.SqlClient;
using QLKTX_App.DTO;

namespace QLKTX_App.DAL
{
    public class GiaDienNuocDAL
    {
        private readonly DBConnect _db = new DBConnect();

        // Lấy giá hiện tại
        public GiaDienNuocModel GetCurrent()
        {
            using (var conn = _db.GetConnection())
            using (var cmd = new SqlCommand("SELECT TOP 1 GiaDien, GiaNuoc FROM GiaDienNuoc", conn))
            {
                conn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        return new GiaDienNuocModel
                        {
                            GiaDien = rd.GetDecimal(0),
                            GiaNuoc = rd.GetDecimal(1),
                        };
                    }
                }
            }
            return new GiaDienNuocModel();
        }

        // Cập nhật giá (chỉ 1 dòng duy nhất trong bảng)
        public int Update(GiaDienNuocModel m)
        {
            using (var conn = _db.GetConnection())
            using (var cmd = new SqlCommand("UPDATE GiaDienNuoc SET GiaDien = @GiaDien, GiaNuoc = @GiaNuoc", conn))
            {
                cmd.Parameters.AddWithValue("@GiaDien", m.GiaDien);
                cmd.Parameters.AddWithValue("@GiaNuoc", m.GiaNuoc);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
