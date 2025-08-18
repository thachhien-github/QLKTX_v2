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

namespace QLKTX_App.Utilities
{
    public static class HopDongExporter
    {
        // Helper: Lấy giá trị cột an toàn
        private static string GetValue(DataRow r, string colName)
        {
            if (r.Table.Columns.Contains(colName) && r[colName] != DBNull.Value)
                return r[colName].ToString();
            return "";
        }

        public static void ExportHopDongPDF(DataRow r, string filePath)
        {
            // Kiểm tra và tạo thư mục nếu chưa có
            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (PdfWriter writer = new PdfWriter(filePath))
            using (PdfDocument pdf = new PdfDocument(writer))
            {
                Document doc = new Document(pdf);

                var fontPath = @"C:\Windows\Fonts\tahoma.ttf"; // font Unicode
                var fontBold = PdfFontFactory.CreateFont(fontPath, iText.IO.Font.PdfEncodings.IDENTITY_H);
                var font = PdfFontFactory.CreateFont(fontPath, iText.IO.Font.PdfEncodings.IDENTITY_H);


                // Tiêu đề
                Paragraph title = new Paragraph("HỢP ĐỒNG KÝ TÚC XÁ")
                    .SetFont(fontBold)
                    .SetFontSize(16)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(20);
                doc.Add(title);

                // Tạo bảng 2 cột
                Table table = new Table(UnitValue.CreatePercentArray(new float[] { 30, 70 }))
                    .UseAllAvailableWidth();

                void AddCell(string label, string value)
                {
                    table.AddCell(new Cell().Add(new Paragraph(label).SetFont(fontBold)).SetBorder(Border.NO_BORDER));
                    table.AddCell(new Cell().Add(new Paragraph(value).SetFont(font)).SetBorder(Border.NO_BORDER));
                }

                AddCell("MSSV:", GetValue(r, "MSSV"));
                AddCell("Họ tên:", GetValue(r, "HoTen"));
                AddCell("Phòng:", GetValue(r, "MaPhong"));
                AddCell("Loại phòng:", GetValue(r, "LoaiPhong"));

                // Giá phòng định dạng số
                string giaPhongStr = GetValue(r, "GiaPhong");
                AddCell("Giá phòng:", string.IsNullOrEmpty(giaPhongStr) ? "" : string.Format("{0:N0} ₫", Convert.ToDecimal(giaPhongStr)));

                // Ngày phân bổ
                string ngayPhanBoStr = GetValue(r, "NgayPhanBo");
                AddCell("Ngày phân bổ:", string.IsNullOrEmpty(ngayPhanBoStr) ? "" : Convert.ToDateTime(ngayPhanBoStr).ToString("dd/MM/yyyy"));

                AddCell("Số tháng:", GetValue(r, "SoThang"));

                // Miễn tiền phòng (Boolean)
                string mienTien = GetValue(r, "MienTienPhong");
                AddCell("Miễn tiền phòng:", (mienTien == "True" || mienTien == "1") ? "Có" : "Không");

                AddCell("Số đợt thu:", GetValue(r, "SoDotThu"));
                AddCell("Ghi chú:", GetValue(r, "GhiChu"));

                doc.Add(table);

                // Footer
                Paragraph footer = new Paragraph("\nXác nhận của Ban Quản Lý và Sinh viên.")
                    .SetFont(font)
                    .SetTextAlignment(TextAlignment.CENTER);
                doc.Add(footer);

                doc.Close();
            }
        }
    }
}
