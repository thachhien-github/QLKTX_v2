using QLKTX_App.BLL;
using QLKTX_App.DAL;
using QLKTX_App.DTO;
using QLKTX_App.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace QLKTX_App.ChildForm_Comon
{
    public partial class FormThongKeDT : Form
    {
        private readonly ThanhToanPhongBLL _phongBLL = new ThanhToanPhongBLL();
        private readonly HoaDonDichVuBLL _dvBLL = new HoaDonDichVuBLL();

        private DataTable _dtPhong;
        private DataTable _dtDichVu;
        public FormThongKeDT()
        {
            InitializeComponent();
        }
        private void LoadThongKe()
        {
            DateTime tuNgay = dtpTuNgay.Value;
            DateTime denNgay = dtpDenNgay.Value;

            // ===== Hóa đơn phòng =====
            _dtPhong = _phongBLL.GetAll();
            dgvPhong.DataSource = _dtPhong;

            if (dgvPhong.Columns.Contains("TongTien"))
            {
                dgvPhong.Columns["TienPhong"].DefaultCellStyle.Format = "N0";
                dgvPhong.Columns["TienPhong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                dgvPhong.Columns["TienTheChan"].DefaultCellStyle.Format = "N0";
                dgvPhong.Columns["TienTheChan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                dgvPhong.Columns["TongTien"].DefaultCellStyle.Format = "N0";
                dgvPhong.Columns["TongTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            // ===== Hóa đơn dịch vụ =====
            _dtDichVu = _dvBLL.LayDanhSachHoaDon();
            dgvDichVu.DataSource = _dtDichVu;

            string[] colsDV = { "TienDien", "TienNuoc", "TienGuiXe", "TongTien" };
            foreach (string col in colsDV)
            {
                if (dgvDichVu.Columns.Contains(col))
                {
                    dgvDichVu.Columns[col].DefaultCellStyle.Format = "N0";
                    dgvDichVu.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }

            // ===== Tổng doanh thu =====
            decimal tongPhong = _dtPhong.AsEnumerable().Sum(r => r.Field<decimal>("TienPhong"));
            decimal tongDV = _dtDichVu.AsEnumerable().Sum(r => r.Field<decimal>("TongTien"));

            lblTongDTPhong.Text = tongPhong.ToString("N0") + " đ";
            lblTongDTDichVu.Text = tongDV.ToString("N0") + " đ";

            lblDTDien.Text = _dvBLL.LayTongTienDien(tuNgay, denNgay).ToString("N0") + " đ";
            lblDTNuoc.Text = _dvBLL.LayTongTienNuoc(tuNgay, denNgay).ToString("N0") + " đ";
            lblDTGiuXe.Text = _dvBLL.LayTongTienXe(tuNgay, denNgay).ToString("N0") + " đ";
            lblTongDT.Text = (tongPhong + tongDV).ToString("N0") + " đ";

            // Vẽ chart
            VeBieuDoTuHoaDon(_dtDichVu);
            VeBieuDoTyLeDichVu(_dtDichVu);
        }

        private void FormThongKeDT_Load(object sender, EventArgs e)
        {
            dtpTuNgay.Value = new DateTime(DateTime.Now.Year, 1, 1);
            dtpDenNgay.Value = DateTime.Now;
            LoadThongKe();

            // ===== Load dữ liệu vào cboThangNam =====
            cboThangNam.Items.Clear();
            cboThangNam.Items.Add("Tất cả");
            for (int y = DateTime.Now.Year - 2; y <= DateTime.Now.Year; y++) // lấy 3 năm gần đây
            {
                for (int m = 1; m <= 12; m++)
                {
                    cboThangNam.Items.Add($"{m:D2}/{y}");
                }
            }
            cboThangNam.SelectedIndex = 0; // mặc định "Tất cả"

            // ===== Load dữ liệu cboNamPhong =====
            cboNam.Items.Clear();
            cboNam.Items.Add("Tất cả");
            for (int y = DateTime.Now.Year - 5; y <= DateTime.Now.Year; y++) // 5 năm gần đây
            {
                cboNam.Items.Add(y.ToString());
            }
            cboNam.SelectedIndex = 0;

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            LoadThongKe();
        }

        private void VeBieuDoTuHoaDon(DataTable dtDV)
        {
            // Gom nhóm theo Tháng/Năm để tính tổng
            var data = dtDV.AsEnumerable()
                           .GroupBy(r => new { Thang = r.Field<int>("Thang"), Nam = r.Field<int>("Nam") })
                           .Select(g => new
                           {
                               Thang = $"{g.Key.Thang:D2}/{g.Key.Nam}",
                               DoanhThu = g.Sum(r => r.Field<decimal>("TongTien"))
                           })
                           .OrderBy(x => x.Thang)
                           .ToList();

            chartThang.Series.Clear();
            Series s = new Series("Doanh thu dịch vụ")
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true
            };

            chartThang.Series.Add(s);

            foreach (var item in data)
            {
                s.Points.AddXY(item.Thang, item.DoanhThu);
            }
        }

        private void VeBieuDoTyLeDichVu(DataTable dtDV)
        {
            decimal tongDien = dtDV.AsEnumerable().Sum(r => r.Field<decimal>("TienDien"));
            decimal tongNuoc = dtDV.AsEnumerable().Sum(r => r.Field<decimal>("TienNuoc"));
            decimal tongXe = dtDV.AsEnumerable().Sum(r => r.Field<decimal>("TienGuiXe"));

            chartPie.Series.Clear();
            chartPie.Titles.Clear();
            chartPie.Legends.Clear();

            // ===== Title =====
            chartPie.Titles.Add("Cơ cấu doanh thu dịch vụ");
            chartPie.Titles[0].Font = new Font("Segoe UI", 12, FontStyle.Bold);
            chartPie.Titles[0].ForeColor = Color.DarkBlue;

            // ===== Legend =====
            Legend legend = new Legend("Legend1");
            legend.Docking = Docking.Right;
            legend.Alignment = StringAlignment.Center;
            legend.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            chartPie.Legends.Add(legend);

            // ===== Series =====
            Series s = new Series("Tỉ lệ dịch vụ")
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
            };
            chartPie.Series.Add(s);

            // Dữ liệu
            s.Points.AddXY("Điện", tongDien);
            s.Points.AddXY("Nước", tongNuoc);
            s.Points.AddXY("Gửi xe", tongXe);

            // Cập nhật label và legend cho từng điểm
            foreach (DataPoint p in s.Points)
            {
                p.Label = "#PERCENT{P0}";   // chỉ hiển thị %
                p.LegendText = p.AxisLabel; // chỉ hiện tên trong chú thích
                p.LabelForeColor = Color.Black;
            }

            // ===== Hiệu ứng 3D =====
            chartPie.ChartAreas[0].Area3DStyle.Enable3D = true;
            chartPie.ChartAreas[0].Area3DStyle.Inclination = 40;
            chartPie.ChartAreas[0].Area3DStyle.Rotation = 20;
        }

        private void btnExportPhong_Click(object sender, EventArgs e)
        {
            if (_dtPhong == null || _dtPhong.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất báo cáo!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cboNam.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn Năm trước khi xuất báo cáo!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable dtPhongLoc;

            // ✅ Nếu chọn "Tất cả"
            if (cboNam.SelectedItem.ToString() == "Tất cả")
            {
                dtPhongLoc = _dtPhong.Copy();
            }
            else
            {
                int namChon;
                if (!int.TryParse(cboNam.SelectedItem.ToString(), out namChon))
                {
                    MessageBox.Show("Năm không hợp lệ, vui lòng chọn lại!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var rows = _dtPhong.AsEnumerable()
                    .Where(r => r.Field<DateTime>("NgayThu").Year == namChon);

                if (!rows.Any())
                {
                    MessageBox.Show($"Không có dữ liệu phòng trong năm {namChon}!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                dtPhongLoc = rows.CopyToDataTable();
            }

            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "Excel file|*.xlsx",
                FileName = $"BaoCao_Phong_{cboNam.SelectedItem}_{DateTime.Now:yyyyMMdd}.xlsx"
            })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ExcelExportPhong.XuatExcel(dtPhongLoc, sfd.FileName,
                            $"BÁO CÁO DOANH THU PHÒNG ({cboNam.SelectedItem})");
                        MessageBox.Show("Xuất báo cáo phòng thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xuất báo cáo: " + ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnExportDichVu_Click(object sender, EventArgs e)
        {
            if (_dtDichVu == null || _dtDichVu.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất báo cáo!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // ✅ Kiểm tra đã chọn Tháng/Năm chưa
            if (cboThangNam.SelectedIndex <= 0)
            {
                MessageBox.Show("Vui lòng chọn Tháng/Năm trước khi xuất báo cáo!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lọc dữ liệu theo cboThangNam
            string[] parts = cboThangNam.SelectedItem.ToString().Split('/');
            int thang = int.Parse(parts[0]);
            int nam = int.Parse(parts[1]);

            var rows = _dtDichVu.AsEnumerable()
                .Where(r =>
                    r.Field<int>("Thang") == thang &&
                    r.Field<int>("Nam") == nam
                );

            if (!rows.Any())
            {
                MessageBox.Show($"Không có dữ liệu dịch vụ trong tháng {thang}/{nam}!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataTable dtDVLoc = rows.CopyToDataTable();

            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "Excel file|*.xlsx",
                FileName = $"BaoCao_DichVu_{nam}{thang:D2}.xlsx"
            })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ExcelExportHoaDonDV.XuatExcel(dtDVLoc, sfd.FileName,
                            $"BÁO CÁO DOANH THU DỊCH VỤ THÁNG {thang}/{nam}");
                        MessageBox.Show("Xuất báo cáo dịch vụ thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xuất báo cáo: " + ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void cboThangNam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_dtDichVu == null || _dtDichVu.Rows.Count == 0) return;

            var dv = _dtDichVu.AsEnumerable();

            if (cboThangNam.SelectedIndex > 0) // Nếu khác "Tất cả"
            {
                string[] parts = cboThangNam.SelectedItem.ToString().Split('/');
                int thang = int.Parse(parts[0]);
                int nam = int.Parse(parts[1]);

                dv = dv.Where(r =>
                    r.Field<int>("Thang") == thang &&
                    r.Field<int>("Nam") == nam
                );
            }

            if (dv.Any())
            {
                DataTable dtFilter = dv.CopyToDataTable();
                dgvDichVu.DataSource = dtFilter;
                VeBieuDoTuHoaDon(dtFilter);
                VeBieuDoTyLeDichVu(dtFilter);

                // Tính lại tổng doanh thu dịch vụ
                decimal tongDV = dtFilter.AsEnumerable().Sum(r => r.Field<decimal>("TongTien"));
                lblTongDTDichVu.Text = tongDV.ToString("N0") + " đ";
            }
            else
            {
                dgvDichVu.DataSource = null;
                chartThang.Series.Clear();
                chartPie.Series.Clear();
                lblTongDTDichVu.Text = "0 đ";
            }
        }

        private void cboNam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_dtPhong == null || _dtPhong.Rows.Count == 0) return;

            var phong = _dtPhong.AsEnumerable();

            if (cboNam.SelectedIndex > 0) // khác "Tất cả"
            {
                int nam = int.Parse(cboNam.SelectedItem.ToString());
                phong = phong.Where(r => r.Field<DateTime>("NgayThu").Year == nam);
            }

            if (phong.Any())
            {
                DataTable dtFilter = phong.CopyToDataTable();
                dgvPhong.DataSource = dtFilter;

                // Tính lại tổng doanh thu phòng
                decimal tongPhong = dtFilter.AsEnumerable().Sum(r => r.Field<decimal>("TienPhong"));
                lblTongDTPhong.Text = tongPhong.ToString("N0") + " đ";
            }
            else
            {
                dgvPhong.DataSource = null;
                lblTongDTPhong.Text = "0 đ";
            }
        }
    }
}
