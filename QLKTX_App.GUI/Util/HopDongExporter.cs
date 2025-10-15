using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Data;
using System.IO;

namespace QLKTX_App.GUI
{
    public static class HopDongExporter
    {
        // ======== Model chứa thông tin cần thiết ========
        public class ChiTietHopDong
        {
            public string MSSV { get; set; }
            public string HoTen { get; set; }
            public string MaPhong { get; set; }
            public decimal TienPhong { get; set; }
            public decimal TienTheChan { get; set; }
            public string GhiChu { get; set; }
            public string NguoiLap { get; set; }
            public DateTime? NgayPhanBo { get; set; }
            public DateTime? NgayHetHan { get; set; }

            public decimal TongTien => TienPhong + TienTheChan;
        }

        // ======== Load font từ resource (TTF nhúng trong Properties.Resources) ========
        private static PdfFont LoadFont(byte[] fontData)
        {
            // fontData như Properties.Resources.arial
            using (var ms = new MemoryStream(fontData))
            {
                return PdfFontFactory.CreateFont(ms.ToArray(), PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
            }
        }

        // ================= Public: xuất trực tiếp từ DataRow =================
        // Dùng khi bạn có DataTable / DataRow từ BLL. FilePath là đường dẫn đích (ví dụ từ SaveFileDialog).
        public static void ExportFromDataRow(DataRow row, string filePath)
        {
            // chuyển DataRow -> ChiTietHopDong (an toàn)
            var ct = new ChiTietHopDong();
            if (row == null) throw new ArgumentNullException(nameof(row));

            ct.MSSV = row.Table.Columns.Contains("MSSV") ? row["MSSV"]?.ToString() ?? "" : "";
            ct.HoTen = row.Table.Columns.Contains("HoTen") ? row["HoTen"]?.ToString() ?? "" : "";
            ct.MaPhong = row.Table.Columns.Contains("MaPhong") ? row["MaPhong"]?.ToString() ?? "" : "";
            ct.GhiChu = row.Table.Columns.Contains("GhiChu") ? row["GhiChu"]?.ToString() ?? "" : "";
            ct.NguoiLap = Environment.UserName;

            if (row.Table.Columns.Contains("GiaPhong") && row["GiaPhong"] != DBNull.Value)
                decimal.TryParse(row["GiaPhong"].ToString(), out decimal gp);
            decimal gia = 0;
            if (row.Table.Columns.Contains("GiaPhong") && row["GiaPhong"] != DBNull.Value)
                decimal.TryParse(row["GiaPhong"].ToString(), out gia);
            ct.TienPhong = gia;

            // Nếu có cột tiền thế chân
            if (row.Table.Columns.Contains("TienTheChan") && row["TienTheChan"] != DBNull.Value)
                decimal.TryParse(row["TienTheChan"].ToString(), out decimal tt);
            // else mặc định 0

            if (row.Table.Columns.Contains("NgayPhanBo") && row["NgayPhanBo"] != DBNull.Value)
                if (DateTime.TryParse(row["NgayPhanBo"].ToString(), out DateTime npb)) ct.NgayPhanBo = npb;
            if (row.Table.Columns.Contains("NgayHetHan") && row["NgayHetHan"] != DBNull.Value)
                if (DateTime.TryParse(row["NgayHetHan"].ToString(), out DateTime nhh)) ct.NgayHetHan = nhh;

            // Tạo số hợp đồng đơn giản; bạn có thể sửa format theo yêu cầu
            string soHopDong = $"HD-{ct.MSSV}-{ct.MaPhong}-{DateTime.Now:yyyyMMddHHmmss}";
            DateTime ngayLap = DateTime.Now;

            ExportHopDongPdf(soHopDong, ngayLap, ct, filePath);
        }

        // ================= Public: xuất hợp đồng - hàm chính =================
        public static void ExportHopDongPdf(string soHopDong, DateTime ngayLap, ChiTietHopDong ct, string filePath)
        {
            if (ct == null) throw new ArgumentNullException(nameof(ct));
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));

            try
            {
                if (File.Exists(filePath)) File.Delete(filePath);

                // Lấy font từ Resources trong project của bạn.
                // Bạn cần đảm bảo đã thêm các file ttf vào Resources với tên arial, arialbd, ariali (hoặc đổi tên tương ứng)
                var font = LoadFont(Properties.Resources.arial);
                var fontBold = LoadFont(Properties.Resources.arialbd);
                var fontItalic = LoadFont(Properties.Resources.ariali);

                using (var writer = new PdfWriter(filePath))
                using (var pdf = new PdfDocument(writer))
                using (var doc = new Document(pdf, iText.Kernel.Geom.PageSize.A4))
                {
                    doc.SetMargins(36, 36, 36, 36);

                    // ===== Header =====
                    doc.Add(new Paragraph("trường cao đẳng giao thông vận tải")
                        .SetFont(fontBold).SetFontSize(14).SetTextAlignment(TextAlignment.CENTER));
                    doc.Add(new Paragraph("KÝ TÚC XÁ SINH VIÊN")
                        .SetFont(fontBold).SetFontSize(12).SetTextAlignment(TextAlignment.CENTER));
                    doc.Add(new Paragraph($"Số: {soHopDong}").SetFont(font).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginBottom(10));

                    doc.Add(new Paragraph("HỢP ĐỒNG THUÊ PHÒNG KÝ TÚC XÁ")
                        .SetFont(fontBold).SetFontSize(16).SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginBottom(12));

                    // Thông tin 2 bên
                    doc.Add(new Paragraph($"Căn cứ theo quy định quản lý ký túc xá, hôm nay, ngày {ngayLap:dd/MM/yyyy}, chúng tôi gồm:")
                        .SetFont(font).SetFontSize(11).SetTextAlignment(TextAlignment.LEFT)
                        .SetMarginBottom(8));

                    Table parties = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1 })).UseAllAvailableWidth();
                    parties.SetBorder(Border.NO_BORDER);

                    // ====== BÊN A ======
                    doc.Add(new Paragraph("BÊN A (Bên cho thuê)")
                        .SetFont(fontBold)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetMarginTop(15)
                        .SetMarginBottom(5));

                    doc.Add(new Paragraph("Tên: Trường Cao đẳng Giao thông Vận tải")
                        .SetFont(font)
                        .SetMarginBottom(3));
                    doc.Add(new Paragraph("Đại diện: Ban quản lý Ký túc xá")
                        .SetFont(font)
                        .SetMarginBottom(3));
                    doc.Add(new Paragraph("Địa chỉ: Số 08, Nguyễn Ảnh Thủ, Phường Trung Mỹ Tây, TP. Hồ Chí Minh")
                        .SetFont(font)
                        .SetMarginBottom(3));
                    doc.Add(new Paragraph("Điện thoại: (028) 37.189.290")
                        .SetFont(font)
                        .SetMarginBottom(10));

                    // ====== BÊN B ======
                    doc.Add(new Paragraph("BÊN B (Bên thuê)")
                        .SetFont(fontBold)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetMarginTop(10)
                        .SetMarginBottom(5));

                    doc.Add(new Paragraph($"Họ và tên: {ct.HoTen}")
                        .SetFont(font)
                        .SetMarginBottom(3));
                    doc.Add(new Paragraph($"MSSV: {ct.MSSV}")
                        .SetFont(font)
                        .SetMarginBottom(3));
                    doc.Add(new Paragraph($"Phòng: {ct.MaPhong}")
                        .SetFont(font)
                        .SetMarginBottom(3));
                    doc.Add(new Paragraph($"Ngày phân bổ: {(ct.NgayPhanBo.HasValue ? ct.NgayPhanBo.Value.ToString("dd/MM/yyyy") : "...")}")
                        .SetFont(font)
                        .SetMarginBottom(3));
                    doc.Add(new Paragraph($"Ngày hết hạn: {(ct.NgayHetHan.HasValue ? ct.NgayHetHan.Value.ToString("dd/MM/yyyy") : "...")}")
                        .SetFont(font)
                        .SetMarginBottom(10));

                    // ===== Các điều khoản (ngắn gọn, trang trọng) =====
                    void AddClause(string title, string body)
                    {
                        doc.Add(new Paragraph(title).SetFont(fontBold).SetFontSize(12).SetMarginTop(8));
                        doc.Add(new Paragraph(body).SetFont(font).SetFontSize(11).SetTextAlignment(TextAlignment.JUSTIFIED));
                    }

                    AddClause("Điều 1. Nội dung hợp đồng",
                        $"Bên A đồng ý cho Bên B thuê phòng ký túc xá {ct.MaPhong} thuộc quản lý của trường theo các điều khoản ghi trong hợp đồng này.");

                    AddClause("Điều 2. Thời hạn thuê",
                        $"Thời hạn thuê được ghi trong hồ sơ: từ {(ct.NgayPhanBo.HasValue ? ct.NgayPhanBo.Value.ToString("dd/MM/yyyy") : "ngày phân bố")} đến {(ct.NgayHetHan.HasValue ? ct.NgayHetHan.Value.ToString("dd/MM/yyyy") : "ngày hết hạn")} hoặc theo thỏa thuận.");

                    AddClause("Điều 3. Giá thuê và phương thức thanh toán",
                        $"Giá thuê: {ct.TienPhong:N0} VND / tháng.\nTiền thế chân (nếu có): {ct.TienTheChan:N0} VND.\nTổng: {ct.TongTien:N0} VND.\nPhương thức: Tiền mặt hoặc chuyển khoản theo hướng dẫn của Bên A.");

                    AddClause("Điều 4. Quyền và nghĩa vụ của các bên",
                        "1. Bên A chịu trách nhiệm quản lý, đảm bảo cơ sở vật chất, an ninh và vệ sinh chung.\n" +
                        "2. Bên B có trách nhiệm thanh toán đúng hạn, giữ gìn tài sản và tuân thủ nội quy ký túc xá.\n" +
                        "3. Các sửa chữa, thay đổi lớn phải có sự đồng ý của Bên A; hư hỏng do Bên B gây ra phải bồi thường.");

                    AddClause("Điều 5. Chấm dứt hợp đồng",
                        "Hợp đồng chấm dứt khi hết hạn, khi hai bên thỏa thuận hoặc khi một bên vi phạm nghiêm trọng các điều khoản. Việc chấm dứt phải thông báo trước theo quy định.");

                    AddClause("Điều 6. Điều khoản chung",
                        "Mọi tranh chấp sẽ được giải quyết bằng thương lượng; nếu không được, sẽ được giải quyết theo pháp luật hiện hành.");

                    if (!string.IsNullOrWhiteSpace(ct.GhiChu))
                    {
                        AddClause("Ghi chú", ct.GhiChu);
                    }

                    // ===== Bảng thanh toán ngắn =====
                    doc.Add(new Paragraph("\n"));
                    Table payTable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 4, 2 })).UseAllAvailableWidth();
                    payTable.AddHeaderCell(new Cell().Add(new Paragraph("STT").SetFont(fontBold)).SetTextAlignment(TextAlignment.CENTER));
                    payTable.AddHeaderCell(new Cell().Add(new Paragraph("Nội dung").SetFont(fontBold)).SetTextAlignment(TextAlignment.CENTER));
                    payTable.AddHeaderCell(new Cell().Add(new Paragraph("Số tiền (VND)").SetFont(fontBold)).SetTextAlignment(TextAlignment.CENTER));

                    payTable.AddCell(new Cell().Add(new Paragraph("1").SetFont(font)).SetTextAlignment(TextAlignment.CENTER));
                    payTable.AddCell(new Cell().Add(new Paragraph("Tiền phòng (1 tháng)").SetFont(font)));
                    payTable.AddCell(new Cell().Add(new Paragraph($"{ct.TienPhong:N0}").SetFont(font)).SetTextAlignment(TextAlignment.RIGHT));

                    payTable.AddCell(new Cell().Add(new Paragraph("2").SetFont(font)).SetTextAlignment(TextAlignment.CENTER));
                    payTable.AddCell(new Cell().Add(new Paragraph("Tiền thế chân").SetFont(font)));
                    payTable.AddCell(new Cell().Add(new Paragraph($"{ct.TienTheChan:N0}").SetFont(font)).SetTextAlignment(TextAlignment.RIGHT));

                    payTable.AddCell(new Cell().SetBorder(Border.NO_BORDER));
                    payTable.AddCell(new Cell().Add(new Paragraph("TỔNG CỘNG").SetFont(fontBold)));
                    payTable.AddCell(new Cell().Add(new Paragraph($"{ct.TongTien:N0}").SetFont(fontBold)).SetTextAlignment(TextAlignment.RIGHT));

                    doc.Add(payTable);

                    // Tổng bằng chữ
                    string tongTienChu = ChuyenSoThanhChu(ct.TongTien);
                    doc.Add(new Paragraph($"\nTổng tiền (bằng chữ): {tongTienChu}").SetFont(fontItalic));

                    // ===== Ký kết =====
                    doc.Add(new Paragraph("\n"));
                    Table sign = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1 })).UseAllAvailableWidth();
                    sign.AddCell(new Cell().Add(new Paragraph("BÊN A").SetFont(fontBold).SetTextAlignment(TextAlignment.CENTER)).SetBorder(Border.NO_BORDER));
                    sign.AddCell(new Cell().Add(new Paragraph("BÊN B").SetFont(fontBold).SetTextAlignment(TextAlignment.CENTER)).SetBorder(Border.NO_BORDER));

                    sign.AddCell(new Cell().Add(new Paragraph("(Ký, đóng dấu, ghi rõ họ tên)\n\n\n").SetFont(font)).SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER));
                    sign.AddCell(new Cell().Add(new Paragraph("(Ký, ghi rõ họ tên)\n\n\n").SetFont(font)).SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER));

                    doc.Add(sign);

                    doc.Add(new Paragraph($"\nNgày lập hợp đồng: {ngayLap:dd/MM/yyyy}").SetFont(font).SetTextAlignment(TextAlignment.RIGHT));

                    doc.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xuất hợp đồng PDF: {ex.Message}", ex);
            }
        }

        // ================= Hàm chuyển số thành chữ (tiếng Việt, đơn vị: đồng) =================
        public static string ChuyenSoThanhChu(decimal number)
        {
            if (number == 0) return "Không đồng chẵn";
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
