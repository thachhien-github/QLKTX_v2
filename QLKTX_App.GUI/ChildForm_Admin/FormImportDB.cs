using ClosedXML.Excel;
using QLKTX_App.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLKTX_App.GUI.ChildForm_Admin
{
    public partial class FormImportDB : Form
    {
        private DBConnect _dbConnect;
        public FormImportDB()
        {
            InitializeComponent();
            _dbConnect = new DBConnect(); // đọc config.json
        }

        private void FormImportDB_Load(object sender, EventArgs e)
        {
            cboTable.Items.AddRange(new string[]
            {
                "SinhVien", "Phong", "Tang", "LoaiPhong", "NhanVien",
                "TaiKhoan", "PhanBo", "TheXe", "LoaiXe",
                "ChiSo", "GiaDienNuoc", "HoaDon", "ThanhToanPhong"
            });
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Excel Files|*.xlsx";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = ofd.FileName;
                    DataTable dt = ReadExcelWithClosedXML(ofd.FileName);
                    dgvTable.DataSource = dt;
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboTable.Text))
            {
                MessageBox.Show("Vui lòng chọn bảng!");
                return;
            }

            if (string.IsNullOrEmpty(txtFilePath.Text))
            {
                MessageBox.Show("Vui lòng chọn file Excel!");
                return;
            }

            DataTable dt = dgvTable.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để import!");
                return;
            }

            try
            {
                using (SqlConnection conn = _dbConnect.GetConnection())
                {
                    conn.Open();

                    // ⚡⚡ Xóa toàn bộ dữ liệu trong bảng trước khi import
                    string deleteSql = $"DELETE FROM {cboTable.Text}";
                    using (SqlCommand delCmd = new SqlCommand(deleteSql, conn))
                    {
                        delCmd.ExecuteNonQuery();
                    }

                    // Import lại dữ liệu từ Excel
                    foreach (DataRow row in dt.Rows)
                    {
                        string sql = BuildInsertSql(cboTable.Text, row);
                        if (!string.IsNullOrEmpty(sql))
                        {
                            using (SqlCommand cmd = new SqlCommand(sql, conn))
                            {
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }

                MessageBox.Show("Xóa dữ liệu cũ và import thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi import dữ liệu: " + ex.Message);
            }
        }


        private DataTable ReadExcelWithClosedXML(string filePath)
        {
            DataTable dt = new DataTable();

            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1); // lấy sheet đầu tiên
                bool firstRow = true;

                foreach (var row in worksheet.RowsUsed())
                {
                    if (firstRow)
                    {
                        foreach (var cell in row.Cells())
                            dt.Columns.Add(cell.Value.ToString());
                        firstRow = false;
                    }
                    else
                    {
                        dt.Rows.Add(row.Cells().Select(c => c.Value.ToString()).ToArray());
                    }
                }
            }

            return dt;
        }

        private string BuildInsertSql(string table, DataRow row)
        {
            try
            {
                switch (table)
                {
                    case "SinhVien":
                        return $@"
                    INSERT INTO SinhVien (MSSV, HoTen, GioiTinh, NgaySinh, SDT, DiaChi)
                    VALUES (N'{row["MSSV"]}', N'{row["HoTen"]}', N'{row["GioiTinh"]}',
                            '{row["NgaySinh"]}', '{row["SDT"]}', N'{row["DiaChi"]}')";

                    case "Phong":
                        return $@"
                    INSERT INTO Phong (MaPhong, MaTang, MaLoai, SoLuongToiDa, TrangThai)
                    VALUES (N'{row["MaPhong"]}', N'{row["MaTang"]}', N'{row["MaLoai"]}',
                            {row["SoLuongToiDa"]}, N'{row["TrangThai"]}')";

                    case "Tang":
                        return $@"
                    INSERT INTO Tang (MaTang, TenTang)
                    VALUES (N'{row["MaTang"]}', N'{row["TenTang"]}')";

                    case "LoaiPhong":
                        return $@"
                    INSERT INTO LoaiPhong (MaLoai, TenLoai, GiaPhong, SucChua)
                    VALUES (N'{row["MaLoai"]}', N'{row["TenLoai"]}', {row["GiaPhong"]}, {row["SucChua"]})";

                    case "NhanVien":
                        return $@"
                    INSERT INTO NhanVien (MaNV, HoTen, SDT, ChucVu)
                    VALUES (N'{row["MaNV"]}', N'{row["HoTen"]}', '{row["SDT"]}', N'{row["ChucVu"]}')";

                    case "TaiKhoan":
                        return $@"
                    INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, MaNV)
                    VALUES (N'{row["TenDangNhap"]}', N'{row["MatKhau"]}', N'{row["VaiTro"]}', N'{row["MaNV"]}')";

                    case "PhanBo":
                        return $@"
                    INSERT INTO PhanBo (MSSV, MaPhong, NgayVao, SoThang, SoDotThu, MienTienPhong)
                    VALUES (N'{row["MSSV"]}', N'{row["MaPhong"]}', '{row["NgayVao"]}',
                            {row["SoThang"]}, {row["SoDotThu"]}, {row["MienTienPhong"]})";

                    case "TheXe":
                        return $@"
                    INSERT INTO TheXe (MaThe, MSSV, MaLoaiXe, BienSo, NgayDangKy)
                    VALUES (N'{row["MaThe"]}', N'{row["MSSV"]}', N'{row["MaLoaiXe"]}', 
                            N'{row["BienSo"]}', '{row["NgayDangKy"]}')";

                    case "LoaiXe":
                        return $@"
                    INSERT INTO LoaiXe (MaLoaiXe, TenLoai, GiaGiuXe)
                    VALUES (N'{row["MaLoaiXe"]}', N'{row["TenLoai"]}', {row["GiaGiuXe"]})";

                    case "ChiSo":
                        return $@"
                    INSERT INTO ChiSo (MaPhong, Thang, Nam, DienCu, DienMoi, NuocCu, NuocMoi)
                    VALUES (N'{row["MaPhong"]}', {row["Thang"]}, {row["Nam"]},
                            {row["DienCu"]}, {row["DienMoi"]}, {row["NuocCu"]}, {row["NuocMoi"]})";

                    case "GiaDienNuoc":
                        return $@"
                    INSERT INTO GiaDienNuoc (ID, GiaDien, GiaNuoc)
                    VALUES (N'{row["ID"]}', {row["GiaDien"]}, {row["GiaNuoc"]})";

                    case "HoaDon":
                        return $@"
                    INSERT INTO HoaDon (MaHD, MaPhong, ThangNam, NgayLap, TongTien)
                    VALUES (N'{row["MaHD"]}', N'{row["MaPhong"]}', N'{row["ThangNam"]}', 
                            '{row["NgayLap"]}', {row["TongTien"]})";

                    case "ThanhToanPhong":
                        return $@"
                    INSERT INTO ThanhToanPhong (ID, MSSV, MaPhong, SoThangThu, NgayThu)
                    VALUES (N'{row["ID"]}', N'{row["MSSV"]}', N'{row["MaPhong"]}', 
                            {row["SoThangThu"]}, '{row["NgayThu"]}')";

                    default:
                        return null;
                }
            }
            catch
            {
                return null;
            }
        }

    }
}
