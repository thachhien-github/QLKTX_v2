using QLKTX_App.DTO;
using System;
using System.Data;
using System.Data.SqlClient;

namespace QLKTX_App.DAL
{
    public class HoaDonDichVuDAL
    {
        private DBHelper dbHelper = new DBHelper();

        // ====== Lấy chỉ số điện nước ======
        public DataTable GetChiSoDienNuoc(string maPhong, int thang, int nam)
        {
            string sql = @"SELECT DienTieuThu, NuocTieuThu 
                           FROM ChiSo 
                           WHERE MaPhong=@MaPhong AND Thang=@Thang AND Nam=@Nam";
            return dbHelper.ExecuteQuery(sql, false,
                new SqlParameter("@MaPhong", maPhong),
                new SqlParameter("@Thang", thang),
                new SqlParameter("@Nam", nam));
        }

        // ====== Lấy số lượng xe của phòng ======
        public int GetSoLuongXe(string maPhong)
        {
            string sql = @"SELECT COUNT(*) 
                           FROM TheXe tx
                           JOIN PhanBo pb ON tx.MSSV = pb.MSSV
                           WHERE pb.MaPhong = @MaPhong";
            object result = dbHelper.ExecuteScalar(sql, false,
                new SqlParameter("@MaPhong", maPhong));
            return Convert.ToInt32(result);
        }

        // ====== Lấy giá điện, nước (1 dòng duy nhất) ======
        public (decimal GiaDien, decimal GiaNuoc) GetGiaDienNuoc()
        {
            string sql = "SELECT GiaDien, GiaNuoc FROM GiaDienNuoc WHERE ID='1'";
            DataTable dt = dbHelper.ExecuteQuery(sql);
            if (dt.Rows.Count > 0)
            {
                return (
                    Convert.ToDecimal(dt.Rows[0]["GiaDien"]),
                    Convert.ToDecimal(dt.Rows[0]["GiaNuoc"])
                );
            }
            return (0, 0);
        }

        // ====== Lấy giá gửi xe (chọn loại mặc định LX01) ======
        public decimal GetGiaGiuXe(string maLoaiXe)
        {
            string sql = "SELECT GiaGiuXe FROM LoaiXe WHERE MaLoaiXe=@MaLoaiXe";
            object result = dbHelper.ExecuteScalar(sql, false,
                new SqlParameter("@MaLoaiXe", maLoaiXe));
            return result != null ? Convert.ToDecimal(result) : 0;
        }

        // ====== Thêm hóa đơn dịch vụ ======
        public void ThemHoaDon(HoaDonDichVuModel hd)
        {
            string sql = "sp_HoaDon_Them";
            dbHelper.ExecuteNonQuery(sql, true,   // true = StoredProcedure
                new SqlParameter("@MaHD", hd.MaHD),
                new SqlParameter("@MaPhong", hd.MaPhong),
                new SqlParameter("@Thang", hd.Thang),
                new SqlParameter("@Nam", hd.Nam),
                new SqlParameter("@NgayLap", hd.NgayLap),
                new SqlParameter("@DienTieuThu", hd.DienTieuThu),
                new SqlParameter("@NuocTieuThu", hd.NuocTieuThu),
                new SqlParameter("@SoLuongXe", hd.SoLuongXe),
                new SqlParameter("@TienDien", hd.TienDien),
                new SqlParameter("@TienNuoc", hd.TienNuoc),
                new SqlParameter("@TienGuiXe", hd.TienGuiXe));
        }

        // ====== Lấy danh sách hóa đơn (lọc theo tháng/năm nếu có) ======
        public DataTable GetAllHoaDon(int? thang = null, int? nam = null)
        {
            string sql = @"SELECT MaHD, MaPhong, Thang, Nam, NgayLap,
                                  DienTieuThu, NuocTieuThu, SoLuongXe,
                                  TienDien, TienNuoc, TienGuiXe,
                                  (TienDien + TienNuoc + TienGuiXe) AS TongTien
                           FROM HoaDon";

            if (thang != null && nam != null)
            {
                sql += " WHERE Thang=@Thang AND Nam=@Nam";
                return dbHelper.ExecuteQuery(sql, false,
                    new SqlParameter("@Thang", thang),
                    new SqlParameter("@Nam", nam));
            }

            return dbHelper.ExecuteQuery(sql, false);
        }

        // ====== Xóa hóa đơn ======
        public void XoaHoaDon(string maHD)
        {
            string sql = "sp_HoaDon_Xoa";
            dbHelper.ExecuteNonQuery(sql, true, new SqlParameter("@MaHD", maHD));
        }

        public decimal GetTongTienDien(DateTime tuNgay, DateTime denNgay)
        {
            string sql = @"SELECT ISNULL(SUM(TienDien),0) 
                           FROM HoaDon 
                           WHERE NgayLap BETWEEN @TuNgay AND @DenNgay";
            object result = dbHelper.ExecuteScalar(sql, false,
                new SqlParameter("@TuNgay", tuNgay),
                new SqlParameter("@DenNgay", denNgay));
            return Convert.ToDecimal(result);
        }

        public decimal GetTongTienNuoc(DateTime tuNgay, DateTime denNgay)
        {
            string sql = @"SELECT ISNULL(SUM(TienNuoc),0) 
                           FROM HoaDon 
                           WHERE NgayLap BETWEEN @TuNgay AND @DenNgay";
            object result = dbHelper.ExecuteScalar(sql, false,
                new SqlParameter("@TuNgay", tuNgay),
                new SqlParameter("@DenNgay", denNgay));
            return Convert.ToDecimal(result);
        }

        public decimal GetTongTienXe(DateTime tuNgay, DateTime denNgay)
        {
            string sql = @"SELECT ISNULL(SUM(TienGuiXe),0) 
                           FROM HoaDon 
                           WHERE NgayLap BETWEEN @TuNgay AND @DenNgay";
            object result = dbHelper.ExecuteScalar(sql, false,
                new SqlParameter("@TuNgay", tuNgay),
                new SqlParameter("@DenNgay", denNgay));
            return Convert.ToDecimal(result);
        }
    }
}
