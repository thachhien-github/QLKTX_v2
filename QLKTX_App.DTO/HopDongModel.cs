using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKTX_App.DTO
{
    public class HopDongModel
    {
        public int MaPhanBo { get; set; }   // Khóa chính của bảng PhanBo
        public string MSSV { get; set; }
        public string HoTen { get; set; }
        public string MaPhong { get; set; }
        public string TenPhong { get; set; }   // nếu cần join thêm
        public DateTime NgayPhanBo { get; set; }
        public int SoThang { get; set; }
        public bool MienTienPhong { get; set; }
        public int SoDotThu { get; set; }
        public string GhiChu { get; set; }

        // thông tin trạng thái
        public DateTime NgayHetHan => NgayPhanBo.AddMonths(SoThang);
        public string TrangThai => (NgayHetHan < DateTime.Now) ? "Hết hạn" : "Còn hạn";
    }
}
