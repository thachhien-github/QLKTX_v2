using System.Data;
using System.Data.SqlClient;
using QLKTX_App.DTO;
using QLKTX_App.Utilities;

namespace QLKTX_App.DAL
{
    public class PhongDAL
    {
        private readonly DBHelper _db = new DBHelper();

        public DataTable GetAll()
        {
            return _db.ExecuteQuery("SELECT * FROM Phong", false);
        }

        public DataTable GetByTang(string maTang)
        {
            var prms = new[] { new SqlParameter("@MaTang", maTang) };
            return _db.ExecuteQuery("SELECT * FROM Phong WHERE MaTang=@MaTang", false, prms);
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
            var prms = new[]
            {
                new SqlParameter("@MaPhong", p.MaPhong),
                new SqlParameter("@MaTang", p.MaTang),
                new SqlParameter("@MaLoai", p.MaLoai),
                new SqlParameter("@SoLuongToiDa", p.SoLuongToiDa)
            };
            return _db.ExecuteNonQuery("sp_Phong_Sua", true, prms) > 0;
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

        public int GetSoLuongSinhVien(string maPhong)
        {
            var prms = new[] { new SqlParameter("@MaPhong", maPhong) };
            object result = _db.ExecuteScalar("SELECT COUNT(*) FROM PhanBo WHERE MaPhong=@MaPhong", false, prms);
            return result != null ? (int)result : 0;
        }

        public int GetSoLuongToiDa(string maPhong)
        {
            var prms = new[] { new SqlParameter("@MaPhong", maPhong) };
            object result = _db.ExecuteScalar("SELECT SoLuongToiDa FROM Phong WHERE MaPhong=@MaPhong", false, prms);
            return result != null ? (int)result : 0;
        }

        public void UpdateTrangThai(string maPhong, string trangThai)
        {
            var prms = new[]
            {
                new SqlParameter("@MaPhong", maPhong),
                new SqlParameter("@TrangThai", trangThai)
            };
            _db.ExecuteNonQuery("UPDATE Phong SET TrangThai=@TrangThai WHERE MaPhong=@MaPhong", false, prms);
        }
    }
}
