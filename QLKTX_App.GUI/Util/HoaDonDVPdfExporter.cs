using System;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace QLKTX_App.GUI.Util
{
    public static class HoaDonDVPdfExporter
    {
        public class ChiTietDichVu
        {
            public string MaPhong { get; set; }
            public string ThangNam { get; set; }
            public int ChiSoDien { get; set; }
            public int ChiSoNuoc { get; set; }
            public int TongXe { get; set; }
            public decimal GiaDien { get; set; }
            public decimal GiaNuoc { get; set; }
            public decimal GiaXe { get; set; }
            public decimal TongTien { get; set; }
            public DateTime NgayLap { get; set; }
        }

        // ====== HÀM LOAD FONT TỪ RESOURCE ======
        private static PdfFont LoadFont(byte[] fontData)
        {
            using (var ms = new MemoryStream(fontData))
            {
                // Fix: Use the correct overload of CreateFont that accepts a byte array and encoding
                return PdfFontFactory.CreateFont(ms.ToArray(), PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
            }
        }

        public static void XuatHoaDonDichVu(ChiTietDichVu ct, string filePath)
        {
            try
            {
                if (File.Exists(filePath)) File.Delete(filePath);

                // Lấy font từ Resource
                var font = LoadFont(Properties.Resources.arial);
                var fontBold = LoadFont(Properties.Resources.arialbd);
                var fontItalic = LoadFont(Properties.Resources.ariali);

                using (var writer = new PdfWriter(filePath))
                using (var pdf = new PdfDocument(writer))
                using (var doc = new Document(pdf))
                {
                    // ===== HEADER =====
                    doc.Add(new Paragraph("TRƯỜNG CAO ĐẲNG GIAO THÔNG VẬN TẢI")
                        .SetFont(fontBold).SetFontSize(14).SetTextAlignment(TextAlignment.CENTER));
                    doc.Add(new Paragraph("KÝ TÚC XÁ SINH VIÊN")
                        .SetFont(fontBold).SetFontSize(13).SetTextAlignment(TextAlignment.CENTER));
                    doc.Add(new Paragraph("----o0o----\n\n")
                        .SetFont(font).SetTextAlignment(TextAlignment.CENTER));

                    doc.Add(new Paragraph($"HÓA ĐƠN DỊCH VỤ KÝ TÚC XÁ - THÁNG {ct.ThangNam}")
                        .SetFont(fontBold).SetFontSize(16).SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginBottom(15));

                    // ===== THÔNG TIN CHUNG =====
                    doc.Add(new Paragraph($"Phòng: {ct.MaPhong}").SetFont(font));
                    doc.Add(new Paragraph($"Tháng: {ct.ThangNam}").SetFont(font));
                    doc.Add(new Paragraph($"Ngày lập: {ct.NgayLap:dd/MM/yyyy}").SetFont(font));
                    doc.Add(new Paragraph(" "));

                    // ===== BẢNG CHI TIẾT =====
                    Table table = new Table(4).UseAllAvailableWidth();
                    string[] headers = { "STT", "Nội dung", "Số lượng", "Thành tiền" };

                    foreach (var header in headers)
                        table.AddHeaderCell(new Cell()
                            .Add(new Paragraph(header).SetFont(fontBold)
                            .SetTextAlignment(TextAlignment.CENTER)));

                    // Điện
                    table.AddCell(new Paragraph("1").SetTextAlignment(TextAlignment.CENTER).SetFont(font));
                    table.AddCell(new Paragraph($"Điện ({ct.GiaDien:N0}đ/Kwh)").SetFont(font));
                    table.AddCell(new Paragraph(ct.ChiSoDien.ToString()).SetTextAlignment(TextAlignment.RIGHT).SetFont(font));
                    table.AddCell(new Paragraph($"{ct.ChiSoDien * ct.GiaDien:N0}").SetTextAlignment(TextAlignment.RIGHT).SetFont(font));

                    // Nước
                    table.AddCell(new Paragraph("2").SetTextAlignment(TextAlignment.CENTER).SetFont(font));
                    table.AddCell(new Paragraph($"Nước ({ct.GiaNuoc:N0}đ/m3)").SetFont(font));
                    table.AddCell(new Paragraph(ct.ChiSoNuoc.ToString()).SetTextAlignment(TextAlignment.RIGHT).SetFont(font));
                    table.AddCell(new Paragraph($"{ct.ChiSoNuoc * ct.GiaNuoc:N0}").SetTextAlignment(TextAlignment.RIGHT).SetFont(font));

                    // Xe
                    table.AddCell(new Paragraph("3").SetTextAlignment(TextAlignment.CENTER).SetFont(font));
                    table.AddCell(new Paragraph($"Xe ({ct.GiaXe:N0}đ/xe)").SetFont(font));
                    table.AddCell(new Paragraph(ct.TongXe.ToString()).SetTextAlignment(TextAlignment.RIGHT).SetFont(font));
                    table.AddCell(new Paragraph($"{ct.TongXe * ct.GiaXe:N0}").SetTextAlignment(TextAlignment.RIGHT).SetFont(font));

                    // Tổng cộng
                    table.AddCell("").SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    table.AddCell(new Paragraph("TỔNG CỘNG").SetFont(fontBold)).SetTextAlignment(TextAlignment.LEFT);
                    table.AddCell("").SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    table.AddCell(new Paragraph($"{ct.TongTien:N0}").SetFont(fontBold).SetTextAlignment(TextAlignment.RIGHT));

                    doc.Add(table);

                    // ===== TỔNG BẰNG CHỮ =====
                    doc.Add(new Paragraph("Tổng số tiền thanh toán: " + $"{ct.TongTien:N0} đ")
                        .SetFont(fontBold).SetTextAlignment(TextAlignment.RIGHT));
                    string tongTienChu = ChuyenSoThanhChu(ct.TongTien);
                    doc.Add(new Paragraph($"Tổng số tiền (bằng chữ): {tongTienChu}")
                        .SetFont(fontItalic).SetTextAlignment(TextAlignment.LEFT));

                    // ===== CHỮ KÝ =====
                    Table kyTable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1 }))
                        .UseAllAvailableWidth().SetMarginTop(30);
                    kyTable.AddCell(new Cell().Add(new Paragraph("Người nhận hóa đơn").SetFont(fontBold))
                        .SetTextAlignment(TextAlignment.CENTER).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                    kyTable.AddCell(new Cell().Add(new Paragraph("Người lập hóa đơn").SetFont(fontBold))
                        .SetTextAlignment(TextAlignment.CENTER).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                    kyTable.AddCell(new Cell().Add(new Paragraph("(Ký, ghi rõ họ tên)").SetFont(fontItalic))
                        .SetTextAlignment(TextAlignment.CENTER).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                    kyTable.AddCell(new Cell().Add(new Paragraph("(Ký, ghi rõ họ tên)").SetFont(fontItalic))
                        .SetTextAlignment(TextAlignment.CENTER).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                    doc.Add(kyTable);

                    // Footer
                    doc.Add(new Paragraph("\n\n\n\n\n"));
                    doc.Add(new Paragraph("Cảm ơn bạn đã thanh toán.")
                        .SetFont(fontItalic).SetTextAlignment(TextAlignment.CENTER));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi xuất PDF: {ex.Message}", ex);
            }
        }

        // ================= Hàm chuyển số thành chữ ===================
        public static string ChuyenSoThanhChu(decimal number)
        {
            string[] dv = { "", "nghìn", "triệu", "tỷ" };
            string[] cs = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string result = "";

            long so = (long)number;
            int i = 0;
            while (so > 0)
            {
                int n = (int)(so % 1000);
                if (n != 0)
                {
                    string sub = DocBaSo(n);
                    result = sub + " " + dv[i] + " " + result;
                }
                so /= 1000;
                i++;
            }

            result = result.Trim();
            if (result.Length > 0)
                result = char.ToUpper(result[0]) + result.Substring(1);
            return result + " đồng chẵn";
        }

        private static string DocBaSo(int n)
        {
            string[] cs = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            int tram = n / 100;
            int chuc = (n % 100) / 10;
            int donvi = n % 10;
            string result = "";

            if (tram != 0)
            {
                result += cs[tram] + " trăm";
                if (chuc == 0 && donvi != 0)
                    result += " lẻ";
            }

            if (chuc != 0)
            {
                if (chuc == 1)
                    result += " mười";
                else
                    result += " " + cs[chuc] + " mươi";
            }

            if (donvi != 0)
            {
                if (chuc != 0 && donvi == 1)
                    result += " mốt";
                else if (chuc != 0 && donvi == 5)
                    result += " lăm";
                else
                    result += " " + cs[donvi];
            }

            return result.Trim();
        }
    }
}
