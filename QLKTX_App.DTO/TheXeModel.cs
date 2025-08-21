using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKTX_App.DTO
{
    public class TheXeModel
    {
        public string MaThe { get; set; }
        public string MSSV { get; set; }
        public string HoTen { get; set; } // để hiển thị
        public string LoaiXe { get; set; } // MaLoaiXe
        public string TenLoaiXe { get; set; } // để hiển thị
        public string BienSo { get; set; }
        public DateTime NgayDangKy { get; set; }
    }
}

