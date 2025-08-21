using System;
using System.Data;
using System.Data.SqlClient;

namespace QLKTX_App.DAL
{
    public class ThongKeDAL
    {
        private readonly DBHelper _db = new DBHelper();

        // Lấy chi tiết thống kê
        public DataTable GetThongKeChiTiet(DateTime tuNgay, DateTime denNgay)
        {
            var prms = new[]
            {
        new SqlParameter("@TuNgay", tuNgay),
        new SqlParameter("@DenNgay", denNgay)
    };

            string sql = @"
        SELECT 
            hd.MaHD,
            sv.MSSV, sv.HoTen, 
            hd.MaPhong,
            ct.TienPhong,
            ct.TienDien,
            ct.TienNuoc,
            ct.TienGuiXe,
            (ct.TongTien) AS TongTien
        FROM ChiTietHoaDon ct
        JOIN HoaDon hd ON ct.MaHD = hd.MaHD
        JOIN SinhVien sv ON ct.MSSV = sv.MSSV
        WHERE hd.NgayLap BETWEEN @TuNgay AND @DenNgay
        ORDER BY hd.MaPhong, sv.MSSV";

            return _db.ExecuteQuery(sql, false, prms);
        }


        // Lấy tổng hợp doanh thu theo loại
        public DataTable GetTongHopDoanhThu(DateTime tuNgay, DateTime denNgay)
        {
            var prms = new[]
            {
                new SqlParameter("@TuNgay", tuNgay),
                new SqlParameter("@DenNgay", denNgay)
            };

            string sql = @"
                SELECT 
                    SUM(CASE WHEN ct.LoaiDichVu = 'Phong' THEN (ct.TienPhong * ISNULL(ct.SoThang,1)) ELSE 0 END) AS DoanhThuPhong,
                    SUM(CASE WHEN ct.LoaiDichVu = 'Dien'  THEN ct.ThanhTien ELSE 0 END) AS DoanhThuDien,
                    SUM(CASE WHEN ct.LoaiDichVu = 'Nuoc'  THEN ct.ThanhTien ELSE 0 END) AS DoanhThuNuoc,
                    SUM(CASE WHEN ct.LoaiDichVu = 'GiuXe' THEN ct.ThanhTien ELSE 0 END) AS DoanhThuGiuXe
                FROM ChiTietHoaDon ct
                JOIN HoaDon hd ON ct.MaHD = hd.MaHD
                WHERE hd.NgayLap BETWEEN @TuNgay AND @DenNgay";

            return _db.ExecuteQuery(sql, false, prms);
        }
    }
}
