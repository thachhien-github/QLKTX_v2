using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKTX_App.DTO
{
    public class ChiSoModel
    {
        public string MaPhong { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }

        public int DienCu { get; set; }
        public int DienMoi { get; set; }
        public int DienTieuThu { get; set; }  // computed

        public int NuocCu { get; set; }
        public int NuocMoi { get; set; }
        public int NuocTieuThu { get; set; }  // computed
    }
}

