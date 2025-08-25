using QLKTX_App.DAL;
using QLKTX_App.DTO;
using System;
using System.Data;

namespace QLKTX_App.BLL
{
    public class HoaDonDichVuBLL
    {
        private HoaDonDichVuDAL dal = new HoaDonDichVuDAL();

        // Tạo hóa đơn (lấy giá từ DB, không fix cứng)
        public HoaDonDichVuModel TaoHoaDon(string maPhong, int thang, int nam, DateTime ngayLap)
        {
            // lấy chỉ số điện nước
            DataTable cs = dal.GetChiSoDienNuoc(maPhong, thang, nam);
            int dien = cs.Rows.Count > 0 ? Convert.ToInt32(cs.Rows[0]["DienTieuThu"]) : 0;
            int nuoc = cs.Rows.Count > 0 ? Convert.ToInt32(cs.Rows[0]["NuocTieuThu"]) : 0;

            // lấy số xe
            int soXe = dal.GetSoLuongXe(maPhong);

            // lấy giá dịch vụ từ DB
            var gia = GetGiaDichVu();

            decimal tienDien = dien * gia.GiaDien;
            decimal tienNuoc = nuoc * gia.GiaNuoc;
            decimal tienXe = soXe * gia.GiaXe;

            HoaDonDichVuModel hd = new HoaDonDichVuModel
            {
                MaHD = $"HD_{maPhong}_{thang:D2}{nam}",
                MaPhong = maPhong,
                Thang = thang,
                Nam = nam,
                NgayLap = ngayLap,
                DienTieuThu = dien,
                NuocTieuThu = nuoc,
                SoLuongXe = soXe,
                TienDien = tienDien,
                TienNuoc = tienNuoc,
                TienGuiXe = tienXe
            };

            dal.ThemHoaDon(hd);
            return hd;
        }

        // Xóa hóa đơn
        public void XoaHoaDon(string maHD)
        {
            dal.XoaHoaDon(maHD);
        }

        // Lấy danh sách hóa đơn (toàn bộ hoặc theo tháng/năm)
        public DataTable LayDanhSachHoaDon(int? thang = null, int? nam = null)
        {
            return dal.GetAllHoaDon(thang, nam);
        }

        // Lấy giá điện/nước/xe từ DB
        public (decimal GiaDien, decimal GiaNuoc, decimal GiaXe) GetGiaDichVu()
        {
            DataTable dtGiaDN = new DBHelper().ExecuteQuery("SELECT GiaDien, GiaNuoc FROM GiaDienNuoc", false);
            DataTable dtGiaXe = new DBHelper().ExecuteQuery("SELECT TOP 1 GiaGiuXe FROM LoaiXe", false);

            decimal giaDien = dtGiaDN.Rows.Count > 0 ? Convert.ToDecimal(dtGiaDN.Rows[0]["GiaDien"]) : 0;
            decimal giaNuoc = dtGiaDN.Rows.Count > 0 ? Convert.ToDecimal(dtGiaDN.Rows[0]["GiaNuoc"]) : 0;
            decimal giaXe = dtGiaXe.Rows.Count > 0 ? Convert.ToDecimal(dtGiaXe.Rows[0]["GiaGiuXe"]) : 0;

            return (giaDien, giaNuoc, giaXe);
        }

        public decimal LayTongTienDien(DateTime tuNgay, DateTime denNgay)
            => dal.GetTongTienDien(tuNgay, denNgay);

        public decimal LayTongTienNuoc(DateTime tuNgay, DateTime denNgay)
            => dal.GetTongTienNuoc(tuNgay, denNgay);

        public decimal LayTongTienXe(DateTime tuNgay, DateTime denNgay)
            => dal.GetTongTienXe(tuNgay, denNgay);
    }
}
