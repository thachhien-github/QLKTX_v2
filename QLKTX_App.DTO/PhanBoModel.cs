using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKTX_App.DTO
{
    public class PhanBoModel
    {
        public string MSSV { get; set; }
        public string MaPhong { get; set; }
        public int SoThang { get; set; }
        public DateTime NgayPhanBo { get; set; }
        public bool MienTienPhong { get; set; }
        public int SoDotThu { get; set; }
        public string GhiChu { get; set; }
    }
}
