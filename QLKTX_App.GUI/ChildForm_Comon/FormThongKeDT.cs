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
        public FormThongKeDT()
        {
            InitializeComponent();
        }
        private void LoadThongKe()
        {
            DateTime tuNgay = dtpTuNgay.Value;
            DateTime denNgay = dtpDenNgay.Value;

            // ===== Load hóa đơn phòng =====
            DataTable dtPhong = _phongBLL.GetAll();
            dgvPhong.DataSource = dtPhong;

            // Định dạng số cho dgvPhong (cột TongTien)
            if (dgvPhong.Columns.Contains("TongTien"))
            {
                dgvPhong.Columns["TienPhong"].DefaultCellStyle.Format = "N0"; // N0 = số nguyên, có dấu phẩy ngăn cách
                dgvPhong.Columns["TienPhong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPhong.Columns["TienTheChan"].DefaultCellStyle.Format = "N0"; // N0 = số nguyên, có dấu phẩy ngăn cách
                dgvPhong.Columns["TienTheChan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPhong.Columns["TongTien"].DefaultCellStyle.Format = "N0"; // N0 = số nguyên, có dấu phẩy ngăn cách
                dgvPhong.Columns["TongTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            // ===== Load hóa đơn dịch vụ =====
            DataTable dtDV = _dvBLL.LayDanhSachHoaDon();
            dgvDichVu.DataSource = dtDV;

            // Định dạng số cho các cột dịch vụ
            string[] colsDV = { "TienDien", "TienNuoc", "TienGuiXe", "TongTien" };
            foreach (string col in colsDV)
            {
                if (dgvDichVu.Columns.Contains(col))
                {
                    dgvDichVu.Columns[col].DefaultCellStyle.Format = "N0";
                    dgvDichVu.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }


            // ===== Cập nhật tổng doanh thu =====
            decimal tongPhong = 0, tongDV = 0;
            foreach (DataRow row in dtPhong.Rows)
                tongPhong += Convert.ToDecimal(row["TienPhong"]);

            foreach (DataRow row in dtDV.Rows)
                tongDV += Convert.ToDecimal(row["TongTien"]);


            lblTongDTPhong.Text = tongPhong.ToString("N0") + " đ";
            lblTongDTDichVu.Text = tongDV.ToString("N0") + " đ";

            lblDTDien.Text = _dvBLL.LayTongTienDien(tuNgay, denNgay).ToString("N0") + " đ";
            lblDTNuoc.Text = _dvBLL.LayTongTienNuoc(tuNgay, denNgay).ToString("N0") + " đ";
            lblDTGiuXe.Text = _dvBLL.LayTongTienXe(tuNgay, denNgay).ToString("N0") + " đ";
            lblTongDT.Text = (tongPhong + tongDV).ToString("N0") + " đ";

            // Vẽ chart doanh thu dịch vụ
            VeBieuDoTuHoaDon(dtDV);

            // Vẽ chart tỉ lệ dịch vụ
            VeBieuDoTyLeDichVu(dtDV);

        }

        private void FormThongKeDT_Load(object sender, EventArgs e)
        {
            dtpTuNgay.Value = new DateTime(DateTime.Now.Year, 1, 1);
            dtpDenNgay.Value = DateTime.Now;
            LoadThongKe();
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
            // Tính tổng theo từng loại dịch vụ
            decimal tongDien = dtDV.AsEnumerable().Sum(r => r.Field<decimal>("TienDien"));
            decimal tongNuoc = dtDV.AsEnumerable().Sum(r => r.Field<decimal>("TienNuoc"));
            decimal tongXe = dtDV.AsEnumerable().Sum(r => r.Field<decimal>("TienGuiXe"));

            chartPie.Series.Clear();
            Series s = new Series("Tỉ lệ dịch vụ")
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true,
                LabelFormat = "N0"
            };
            chartPie.Series.Add(s);

            // Thêm dữ liệu
            s.Points.AddXY("Điện", tongDien);
            s.Points.AddXY("Nước", tongNuoc);
            s.Points.AddXY("Gửi xe", tongXe);

            // Hiển thị % trong legend
            chartPie.Legends[0].Enabled = true;
            foreach (DataPoint p in s.Points)
            {
                p.LegendText = p.AxisLabel + " (#PERCENT{P0})";
            }
        }


    }
}
