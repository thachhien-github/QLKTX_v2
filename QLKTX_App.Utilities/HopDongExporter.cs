using System;
using System.Data;
using System.IO;
using System.Text;

namespace QLKTX_App.Utilities
{
    public static class HopDongExporter
    {
        /// <summary>
        /// Sinh file hợp đồng (TXT hoặc DOCX, tuỳ ý) từ DataRow
        /// </summary>
        public static void ExportHopDong(DataRow r, string filePath)
        {
            var sb = new StringBuilder();

            sb.AppendLine("=============== HỢP ĐỒNG KÝ TÚC XÁ ===============");
            sb.AppendLine($"MSSV: {r["MSSV"]} - Họ tên: {r["HoTen"]}");
            sb.AppendLine($"Phòng: {r["MaPhong"]}");
            sb.AppendLine($"Ngày phân bổ: {Convert.ToDateTime(r["NgayPhanBo"]).ToString("dd/MM/yyyy")}");
            sb.AppendLine($"Số tháng: {r["SoThang"]}");
            sb.AppendLine($"Miễn tiền phòng: {(Convert.ToBoolean(r["MienTienPhong"]) ? "Có" : "Không")}");
            sb.AppendLine($"Số đợt thu: {r["SoDotThu"]}");
            sb.AppendLine($"Ghi chú: {r["GhiChu"]}");
            sb.AppendLine("==================================================");
            sb.AppendLine("Xác nhận của Ban Quản Lý và Sinh viên.");

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }
    }
}
