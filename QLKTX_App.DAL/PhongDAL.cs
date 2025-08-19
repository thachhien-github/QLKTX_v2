using System.Data;
using System.Data.SqlClient;
using QLKTX_App.DTO;

namespace QLKTX_App.DAL
{
    public class PhongDAL
    {
        private readonly DBHelper _db = new DBHelper();

        public DataTable GetAll()
        {
            return _db.ExecuteQuery("SELECT * FROM Phong", false);
        }

        public bool Insert(PhongModel p)
        {
            var prms = new[]
            {
                new SqlParameter("@MaPhong", p.MaPhong),
                new SqlParameter("@MaTang", p.MaTang),
                new SqlParameter("@MaLoai", p.MaLoai),
                new SqlParameter("@SoLuongToiDa", p.SoLuongToiDa)
            };
            return _db.ExecuteNonQuery("sp_Phong_Them", true, prms) > 0;
        }

        public bool Update(PhongModel p)
        {
            // 1. Gọi sp_Phong_Sua
            var prms = new[]
            {
                new SqlParameter("@MaPhong", p.MaPhong),
                new SqlParameter("@MaTang", p.MaTang),
                new SqlParameter("@MaLoai", p.MaLoai),
                new SqlParameter("@SoLuongToiDa", p.SoLuongToiDa)
            };
            bool ok = _db.ExecuteNonQuery("sp_Phong_Sua", true, prms) > 0;

            // 2. Nếu thành công thì gọi thêm sp_Phong_CapNhatTrangThai
            if (ok)
            {
                var prmsTrangThai = new[]
                {
                    new SqlParameter("@MaPhong", p.MaPhong),
                    new SqlParameter("@TrangThai", p.TrangThai)
                };
                _db.ExecuteNonQuery("sp_Phong_CapNhatTrangThai", true, prmsTrangThai);
            }

            return ok;
        }

        public bool Delete(string maPhong)
        {
            var prms = new[] { new SqlParameter("@MaPhong", maPhong) };
            return _db.ExecuteNonQuery("sp_Phong_Xoa", true, prms) > 0;
        }

        public bool CheckExists(string maPhong)
        {
            var prms = new[] { new SqlParameter("@MaPhong", maPhong) };
            object result = _db.ExecuteScalar("SELECT COUNT(*) FROM Phong WHERE MaPhong=@MaPhong", false, prms);
            return (int)result > 0;
        }

        public DataTable GetByTang(string maTang)
        {
            var prms = new[] { new SqlParameter("@MaTang", maTang) };
            return _db.ExecuteQuery("SELECT * FROM Phong WHERE MaTang=@MaTang", false, prms);
        }

    }
}
