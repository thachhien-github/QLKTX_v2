using System.Data;
using System.Data.SqlClient;
using QLKTX_App.DTO;

namespace QLKTX_App.DAL
{
    public class TheXeDAL
    {
        private readonly DBHelper _db = new DBHelper();

        public bool Them(TheXeModel xe)
        {
            var prms = new[]
            {
                new SqlParameter("@MaThe", xe.MaThe),
                new SqlParameter("@MSSV", xe.MSSV),
                new SqlParameter("@MaLoaiXe", xe.LoaiXe),
                new SqlParameter("@BienSo", xe.BienSo),
                new SqlParameter("@NgayDangKy", xe.NgayDangKy)
            };
            return _db.ExecuteNonQuery("sp_TheXe_Them", true, prms) > 0;
        }

        public bool Sua(TheXeModel xe)
        {
            var prms = new[]
            {
                new SqlParameter("@MaThe", xe.MaThe),
                new SqlParameter("@MaLoaiXe", xe.LoaiXe),
                new SqlParameter("@BienSo", xe.BienSo),
                new SqlParameter("@NgayDangKy", xe.NgayDangKy)
            };
            return _db.ExecuteNonQuery("sp_TheXe_Sua", true, prms) > 0;
        }

        public bool Xoa(string maThe)
        {
            var prms = new[]
            {
                new SqlParameter("@MaThe", maThe)
            };
            return _db.ExecuteNonQuery("sp_TheXe_Xoa", true, prms) > 0;
        }

        public DataTable GetAll()
        {
            string sql = @"SELECT TX.MaThe, TX.MSSV, SV.HoTen, LX.MaLoaiXe, LX.TenLoai, 
                                  TX.BienSo, TX.NgayDangKy
                           FROM TheXe TX
                           JOIN SinhVien SV ON TX.MSSV = SV.MSSV
                           JOIN LoaiXe LX ON TX.MaLoaiXe = LX.MaLoaiXe";
            return _db.ExecuteQuery(sql, false);
        }

        public DataTable Search(string keyword)
        {
            string sql = @"SELECT TX.MaThe, TX.MSSV, SV.HoTen, LX.MaLoaiXe, LX.TenLoai, 
                                  TX.BienSo, TX.NgayDangKy
                           FROM TheXe TX
                           JOIN SinhVien SV ON TX.MSSV = SV.MSSV
                           JOIN LoaiXe LX ON TX.MaLoaiXe = LX.MaLoaiXe
                           WHERE TX.MaThe LIKE @kw OR TX.MSSV LIKE @kw 
                                 OR SV.HoTen LIKE @kw OR TX.BienSo LIKE @kw";
            return _db.ExecuteQuery(sql, false, new SqlParameter("@kw", "%" + keyword + "%"));
        }

        public DataTable GetLoaiXe()
        {
            return _db.ExecuteQuery("SELECT MaLoaiXe, TenLoai FROM LoaiXe", false);
        }
    }
}
