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
        private readonly ThongKeBLL _bll = new ThongKeBLL();
        private List<ThongKeModel> _data = new List<ThongKeModel>();
        public FormThongKeDT()
        {
            InitializeComponent();
            chartThang.Series.Clear();
            chartPie.Series.Clear();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgay = dtpDenNgay.Value.Date;

            _data = _bll.GetThongKe(tuNgay, denNgay);

            if (_data == null || !_data.Any())
            {
                dgvThongKe.DataSource = null;
                chartThang.Series.Clear();
                chartPie.Series.Clear();
                lblTongDT.Text = "0 đ";
                MessageBox.Show("Không có dữ liệu trong khoảng thời gian này!", "Thông báo");
                return;
            }

            // load dữ liệu chi tiết
            dgvThongKe.DataSource = _data;

            // tính tổng doanh thu
            decimal tong = _data.Sum(x => x.ThanhTien);
            lblTongDT.Text = $"{tong:N0} đ";

            // load chart cột
            LoadChartBar(_data);

            // load chart tròn
            LoadChartPie(tuNgay, denNgay);

            // load combobox tháng
            LoadComboBoxThang(_data);
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (_data == null || !_data.Any())
            {
                MessageBox.Show("Không có dữ liệu để xuất!");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "Excel|*.xlsx" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var dt = ToDataTable(_data);
                    ExcelExporter.XuatBaoCaoExcel(dt, sfd.FileName);
                    MessageBox.Show("Xuất Excel thành công!");
                }
            }
        }

        private void LoadChart(List<ThongKeModel> data)
        {
            chartThang.Series.Clear();

            var series = new Series("Doanh thu")
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true
            };

            // Nhóm theo phòng để thống kê doanh thu
            var group = data.GroupBy(x => x.MaPhong)
                            .Select(g => new
                            {
                                Phong = g.Key,
                                TongTien = g.Sum(x => x.ThanhTien)
                            })
                            .OrderBy(g => g.Phong);

            foreach (var item in group)
            {
                series.Points.AddXY(item.Phong, item.TongTien);
            }

            chartThang.Series.Add(series);
            chartThang.ChartAreas[0].AxisX.Title = "Phòng";
            chartThang.ChartAreas[0].AxisY.Title = "Doanh thu (VNĐ)";
        }

        private void LoadComboBoxThang(List<ThongKeModel> data)
        {
            var dsThang = data
                .GroupBy(x => x.SoThang)
                .Select(g => g.Key)
                .OrderBy(x => x)
                .ToList();

            cboLocThangNam.DataSource = dsThang;
            cboLocThangNam.SelectedIndex = -1; // chưa chọn
        }

        private void LoadChartBar(List<ThongKeModel> data)
        {
            chartThang.Series.Clear();

            var series = new Series("Doanh thu")
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true
            };

            var group = data.GroupBy(x => x.MaPhong)
                            .Select(g => new
                            {
                                Phong = g.Key,
                                TongTien = g.Sum(x => x.ThanhTien)
                            })
                            .OrderBy(g => g.Phong);

            foreach (var item in group)
            {
                series.Points.AddXY(item.Phong, item.TongTien);
            }

            chartThang.Series.Add(series);
            chartThang.ChartAreas[0].AxisX.Title = "Phòng";
            chartThang.ChartAreas[0].AxisY.Title = "Doanh thu (VNĐ)";
        }

        private void LoadChartPie(DateTime tuNgay, DateTime denNgay)
        {
            chartPie.Series.Clear();

            var tongHop = _bll.GetTongHopDoanhThu(tuNgay, denNgay);

            var series = new Series("Cơ cấu doanh thu")
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true
            };

            series.Points.AddXY("Phòng", tongHop.Phong);
            series.Points.AddXY("Điện", tongHop.Dien);
            series.Points.AddXY("Nước", tongHop.Nuoc);
            series.Points.AddXY("Giữ xe", tongHop.GiuXe);

            chartPie.Series.Add(series);
        }


        private DataTable ToDataTable(List<ThongKeModel> list)
        {
            var dt = new DataTable();
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("Phòng", typeof(string));
            dt.Columns.Add("MSSV", typeof(string));
            dt.Columns.Add("Họ và tên", typeof(string));
            dt.Columns.Add("Loại phòng", typeof(string));
            dt.Columns.Add("Số tiền/tháng", typeof(decimal));
            dt.Columns.Add("Số tháng", typeof(int));
            dt.Columns.Add("Thành tiền", typeof(decimal));
            dt.Columns.Add("Ghi chú", typeof(string));

            foreach (var item in list)
            {
                dt.Rows.Add(item.STT, item.MaPhong, item.MSSV, item.HoTen,
                            item.LoaiPhong, item.SoTienThang, item.SoThang,
                            item.ThanhTien, item.GhiChu);
            }
            return dt;
        }

        private void cboLocThangNam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLocThangNam.SelectedIndex >= 0 && _data.Any())
            {
                int thang = Convert.ToInt32(cboLocThangNam.SelectedItem);
                var loc = _data.Where(x => x.SoThang == thang).ToList();
                dgvThongKe.DataSource = loc;

                decimal tong = loc.Sum(x => x.ThanhTien);
                lblTongDT.Text = $"{tong:N0} đ";
            }
        }
    }
}
