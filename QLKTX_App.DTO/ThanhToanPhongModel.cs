using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKTX_App.DTO
{
    public class ThanhToanPhongModel
    {
        public int ID { get; set; }
        public string MSSV { get; set; }
        public string MaPhong { get; set; }
        public int SoThangThu { get; set; }
        public DateTime NgayThu { get; set; }
        public string NguoiThu { get; set; }
        public string GhiChu { get; set; }
        public decimal TienPhong { get; set; }
        public decimal TienTheChan { get; set; }
        public decimal TongTien { get; set; }
    }
}
