using System;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace QLKTX_App.Utilities
{
    public static class HoaDonHDPdfExporter
    {
        public class ChiTietHopDong
        {
            public string MSSV { get; set; }
            public string HoTen { get; set; }
            public string MaPhong { get; set; }
            public decimal TienPhong { get; set; }
            public decimal TienTheChan { get; set; }
            public string GhiChu { get; set; }
            public string NguoiThu { get; set; }   // Lấy từ CurrentUser
            public decimal TongTien => TienPhong + TienTheChan;
        }

        public static void XuatHoaDonHopDong(string maHD, string thangNam, DateTime ngayLap, ChiTietHopDong ct, string baseFolder)
        {
            try
            {
                // Tạo thư mục chứa file
                string folder = Path.Combine(baseFolder, ct.MaPhong, thangNam);
                Directory.CreateDirectory(folder);
                string filePath = Path.Combine(folder, $"{maHD}_{ct.MSSV}.pdf");

                // Load font từ Resources
                string projectDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
                string fontsDir = Path.Combine(projectDir, "QLKTX_App.GUI", "Resources", "Fonts");

                string fontPath = Path.Combine(fontsDir, "tahoma.ttf");
                string fontBoldPath = Path.Combine(fontsDir, "tahomabd.ttf");
                string fontItalicPath = Path.Combine(fontsDir, "ariali.ttf");

                var font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);
                var fontBold = PdfFontFactory.CreateFont(fontBoldPath, PdfEncodings.IDENTITY_H);
                var fontItalic = PdfFontFactory.CreateFont(fontItalicPath, PdfEncodings.IDENTITY_H);

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

                    doc.Add(new Paragraph("HÓA ĐƠN HỢP ĐỒNG KÝ TÚC XÁ")
                        .SetFont(fontBold).SetFontSize(16).SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginBottom(15));

                    // ===== THÔNG TIN CHUNG =====
                    doc.Add(new Paragraph($"Mã hóa đơn: {maHD}").SetFont(font));
                    doc.Add(new Paragraph($"MSSV: {ct.MSSV}").SetFont(font));
                    doc.Add(new Paragraph($"Họ tên: {ct.HoTen}").SetFont(font));
                    doc.Add(new Paragraph($"Phòng: {ct.MaPhong}").SetFont(font));
                    doc.Add(new Paragraph($"Ngày lập: {ngayLap:dd/MM/yyyy}").SetFont(font));
                    doc.Add(new Paragraph(" "));

                    // ===== BẢNG CHI TIẾT =====
                    Table table = new Table(4).UseAllAvailableWidth();
                    string[] headers = { "STT", "Nội dung", "Số tiền", "Ghi chú" };

                    foreach (var header in headers)
                        table.AddHeaderCell(new Cell()
                            .Add(new Paragraph(header).SetFont(fontBold)
                            .SetTextAlignment(TextAlignment.CENTER)));

                    // Tiền phòng
                    table.AddCell("1").SetTextAlignment(TextAlignment.CENTER);
                    table.AddCell("Tien phong").SetTextAlignment(TextAlignment.LEFT);
                    table.AddCell($"{ct.TienPhong:N0}").SetTextAlignment(TextAlignment.RIGHT);
                    table.AddCell(ct.GhiChu ?? "");

                    // Tiền thế chân
                    table.AddCell("2").SetTextAlignment(TextAlignment.CENTER);
                    table.AddCell("Tien the chan").SetTextAlignment(TextAlignment.LEFT);
                    table.AddCell($"{ct.TienTheChan:N0}").SetTextAlignment(TextAlignment.RIGHT);
                    table.AddCell("");

                    // Tổng cộng
                    table.AddCell("").SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    table.AddCell(new Paragraph("Tổng cộng").SetFont(fontBold)).SetTextAlignment(TextAlignment.LEFT);
                    table.AddCell(new Paragraph($"{ct.TongTien:N0}").SetFont(fontBold)).SetTextAlignment(TextAlignment.RIGHT);
                    table.AddCell("").SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                    doc.Add(table);
                    doc.Add(new Paragraph(" "));

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
                    kyTable.AddCell(new Cell().Add(new Paragraph(ct.NguoiThu ?? "(Ký, ghi rõ họ tên)").SetFont(fontItalic))
                        .SetTextAlignment(TextAlignment.CENTER).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                    doc.Add(kyTable);

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
