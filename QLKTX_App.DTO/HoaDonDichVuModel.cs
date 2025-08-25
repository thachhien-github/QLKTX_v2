using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKTX_App.DTO
{
    public class HoaDonDichVuModel
    {
        public string MaHD { get; set; }
        public string MaPhong { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public DateTime NgayLap { get; set; }

        public int DienTieuThu { get; set; }
        public int NuocTieuThu { get; set; }
        public int SoLuongXe { get; set; }

        public decimal TienDien { get; set; }
        public decimal TienNuoc { get; set; }
        public decimal TienGuiXe { get; set; }

        public decimal TongTien { get; set; }
    }
}
