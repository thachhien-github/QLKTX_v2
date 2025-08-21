using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Data;
using System.Drawing;
using System.IO;

namespace QLKTX_App.Utilities
{
    public static class ExcelExporter
    {
        public static void XuatBaoCaoExcel(DataTable data, string filePath)
        {
            try
            {
                // Khai báo license cho EPPlus 8.x
                ExcelPackage.License.SetNonCommercialPersonal("Thach Hien");

                using (var package = new ExcelPackage())
                {
                    var ws = package.Workbook.Worksheets.Add("Báo cáo");

                    int colCount = data.Columns.Count;

                    // ===== 1. TIÊU ĐỀ =====
                    ws.Cells["A1:I1"].Merge = true;
                    ws.Cells["A1"].Value = "BÁO CÁO DOANH THU CHI TIẾT";
                    ws.Cells["A1"].Style.Font.Size = 16;
                    ws.Cells["A1"].Style.Font.Bold = true;
                    ws.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    // ===== 2. HEADER =====
                    for (int i = 0; i < colCount; i++)
                    {
                        ws.Cells[3, i + 1].Value = data.Columns[i].ColumnName;
                        ws.Cells[3, i + 1].Style.Font.Bold = true;
                        ws.Cells[3, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[3, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        ws.Cells[3, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[3, i + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[3, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }

                    // ===== 3. DỮ LIỆU =====
                    int row = 4;
                    foreach (DataRow dr in data.Rows)
                    {
                        for (int c = 0; c < colCount; c++)
                        {
                            var cell = ws.Cells[row, c + 1];
                            var value = dr[c];
                            cell.Value = value;

                            string colName = data.Columns[c].ColumnName;

                            // Định dạng số cho các cột tiền
                            if (colName.Contains("tiền") || colName.Contains("Tiền"))
                            {
                                cell.Style.Numberformat.Format = "#,##0";
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }
                            // MSSV, Họ và tên -> căn trái
                            else if (colName.Contains("MSSV") || colName.Contains("Tên") || colName.Contains("Họ"))
                            {
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            }
                            // STT, Phòng, Số tháng -> căn giữa
                            else
                            {
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }

                            cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        }
                        row++;
                    }

                    // ===== 4. DÒNG TỔNG CỘNG =====
                    ws.Cells[row, colCount - 2].Value = "Tổng cộng:";
                    ws.Cells[row, colCount - 2].Style.Font.Bold = true;
                    ws.Cells[row, colCount - 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    // Tổng cộng ở cột Thành tiền (cột cuối cùng)
                    string lastColLetter = ws.Cells[1, colCount].Address.Substring(0, 1);
                    ws.Cells[row, colCount].Formula = $"SUM({lastColLetter}4:{lastColLetter}{row - 1})";
                    ws.Cells[row, colCount].Style.Font.Bold = true;
                    ws.Cells[row, colCount].Style.Numberformat.Format = "#,##0";
                    ws.Cells[row, colCount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[row, colCount].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    // ===== 5. AUTO FIT =====
                    ws.Cells.AutoFitColumns();

                    // Lưu file
                    package.SaveAs(new FileInfo(filePath));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi xuất Excel: " + ex.Message);
            }
        }
    }
}
