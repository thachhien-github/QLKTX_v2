using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Data;
using System.Drawing;
using System.IO;

namespace QLKTX_App.Utilities
{
    public static class ExcelExportHoaDonDV
    {
        public static void XuatExcel(DataTable data, string filePath, string title)
        {
            try
            {
                ExcelPackage.License.SetNonCommercialPersonal("Thach Hien");

                using (var package = new ExcelPackage())
                {
                    var ws = package.Workbook.Worksheets.Add("DanhSachHoaDon");

                    // ===== 1. TIÊU ĐỀ =====
                    ws.Cells[1, 1].Value = title;
                    ws.Cells[1, 1].Style.Font.Size = 16;
                    ws.Cells[1, 1].Style.Font.Bold = true;
                    ws.Cells[1, 1].Style.Font.Color.SetColor(Color.DarkBlue);
                    ws.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[1, 1, 1, data.Columns.Count].Merge = true;

                    // HEADER
                    for (int i = 0; i < data.Columns.Count; i++)
                    {
                        ws.Cells[3, i + 1].Value = data.Columns[i].ColumnName;
                        ws.Cells[3, i + 1].Style.Font.Bold = true;
                        ws.Cells[3, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[3, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        ws.Cells[3, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[3, i + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[3, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }

                    // DATA
                    for (int r = 0; r < data.Rows.Count; r++)
                    {
                        for (int c = 0; c < data.Columns.Count; c++)
                        {
                            ws.Cells[r + 4, c + 1].Value = data.Rows[r][c];
                            string colName = data.Columns[c].ColumnName.ToLower();

                            if (colName.Contains("tien") || colName.Contains("tong"))
                            {
                                ws.Cells[r + 4, c + 1].Style.Numberformat.Format = "#,##0";
                                ws.Cells[r + 4, c + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }
                            else if (colName.Contains("ghi"))
                            {
                                ws.Cells[r + 4, c + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            }
                            else if (colName.Contains("stt") || colName.Contains("phong"))
                            {
                                ws.Cells[r + 4, c + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                            else
                            {
                                ws.Cells[r + 4, c + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            }

                            ws.Cells[r + 4, c + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        }
                    }

                    // TỔNG CỘNG
                    int totalRow = data.Rows.Count + 4;
                    ws.Cells[totalRow, 1].Value = "Tổng cộng";
                    ws.Cells[totalRow, 1, totalRow, data.Columns.Count - 4].Merge = true;
                    ws.Cells[totalRow, 1].Style.Font.Bold = true;
                    ws.Cells[totalRow, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[totalRow, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    for (int col = 2; col <= data.Columns.Count; col++)
                    {
                        string colName = data.Columns[col - 1].ColumnName.ToLower();
                        if (colName.Contains("tien") || colName.Contains("tong"))
                        {
                            string colLetter = ws.Cells[1, col].Address.Substring(0, 1);
                            ws.Cells[totalRow, col].Formula = $"SUM({colLetter}4:{colLetter}{data.Rows.Count + 3})";
                            ws.Cells[totalRow, col].Style.Font.Bold = true;
                            ws.Cells[totalRow, col].Style.Numberformat.Format = "#,##0";
                            ws.Cells[totalRow, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            ws.Cells[totalRow, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        }
                    }

                    ws.Cells.AutoFitColumns();
                    package.SaveAs(new FileInfo(filePath));
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Lỗi khi xuất Excel: " + ex.Message);
            }
        }
    }
}
