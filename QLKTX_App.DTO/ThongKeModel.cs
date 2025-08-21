using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKTX_App.DTO
{
    public class ThongKeModel
    {
        public int STT { get; set; }
        public string MaPhong { get; set; }
        public string MSSV { get; set; }
        public string HoTen { get; set; }
        public string LoaiPhong { get; set; }
        public decimal SoTienThang { get; set; }
        public int SoThang { get; set; }
        public decimal ThanhTien { get; set; }
        public string GhiChu { get; set; }
    }
}
