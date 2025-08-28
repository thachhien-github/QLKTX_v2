using QLKTX_App.BLL;
using QLKTX_App.DAL;
using QLKTX_App.DTO;
using QLKTX_App.GUI.Util;
using QLKTX_App.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace QLKTX_App.ChildForm_NhanVien
{
    public partial class FormHoaDonDV : Form
    {
        private HoaDonDichVuBLL dvBLL = new HoaDonDichVuBLL();


        public FormHoaDonDV()
        {
            InitializeComponent();
        }

        private void FormHoaDonDV_Load(object sender, EventArgs e)
        {
            // Load phòng
            PhongBLL phongBLL = new PhongBLL();
            DataTable dtPhong = phongBLL.GetAll();
            cboPhong.DataSource = dtPhong;
            cboPhong.DisplayMember = "MaPhong";
            cboPhong.ValueMember = "MaPhong";
            cboPhong.SelectedIndex = -1;

            // Load tháng/năm (2 năm trước -> 1 năm sau)
            DataTable dtThangNam = new DataTable();
            dtThangNam.Columns.Add("Text");
            dtThangNam.Columns.Add("Value");

            for (int nam = DateTime.Now.Year - 2; nam <= DateTime.Now.Year + 1; nam++)
            {
                for (int thang = 1; thang <= 12; thang++)
                {
                    string text = $"{thang:D2}/{nam}";
                    string value = $"{thang:D2}-{nam}";
                    dtThangNam.Rows.Add(text, value);
                }
            }

            cboThangNam.DataSource = dtThangNam;
            cboThangNam.DisplayMember = "Text";
            cboThangNam.ValueMember = "Value";
            cboThangNam.SelectedIndex = -1;

            LoadDSHoaDon(); // load tất cả hóa đơn ban đầu
        }

        // Load chỉ số điện nước + xe
        private void LoadChiSoVaXe()
        {
            if (cboPhong.SelectedValue == null || cboThangNam.SelectedValue == null) return;

            string maPhong = cboPhong.SelectedValue.ToString();
            string[] parts = cboThangNam.SelectedValue.ToString().Split('-');
            int thang = int.Parse(parts[0]);
            int nam = int.Parse(parts[1]);

            try
            {
                DataTable cs = new HoaDonDichVuDAL().GetChiSoDienNuoc(maPhong, thang, nam);
                if (cs.Rows.Count > 0)
                {
                    txtCSdien.Text = cs.Rows[0]["DienTieuThu"].ToString();
                    txtCSnuoc.Text = cs.Rows[0]["NuocTieuThu"].ToString();
                }
                else
                {
                    txtCSdien.Text = "0";
                    txtCSnuoc.Text = "0";
                }

                int soXe = new HoaDonDichVuDAL().GetSoLuongXe(maPhong);
                txtSLxe.Text = soXe.ToString();
            }
            catch
            {
                txtCSdien.Text = "0";
                txtCSnuoc.Text = "0";
                txtSLxe.Text = "0";
            }
        }

        // Load danh sách hóa đơn
        private void LoadDSHoaDon()
        {
            int? thang = null, nam = null;
            if (cboThangNam.SelectedValue != null)
            {
                string[] parts = cboThangNam.SelectedValue.ToString().Split('-');
                thang = int.Parse(parts[0]);
                nam = int.Parse(parts[1]);
            }

            DataTable dt = dvBLL.LayDanhSachHoaDon(thang, nam);
            dgvListHD.DataSource = dt;

            if (dt != null && dt.Columns.Count > 0)
            {
                // Đặt header text
                var headers = new Dictionary<string, string>
                {
                    { "MaHD", "Mã HĐ" },
                    { "MaPhong", "Phòng" },
                    { "Thang", "Tháng" },
                    { "Nam", "Năm" },
                    { "NgayLap", "Ngày lập" },
                    { "DienTieuThu", "Điện (kWh)" },
                    { "NuocTieuThu", "Nước (m³)" },
                    { "SoLuongXe", "Xe (chiếc)" },
                    { "TienDien", "Tiền điện (VND)" },
                    { "TienNuoc", "Tiền nước (VND)" },
                    { "TienGuiXe", "Tiền xe (VND)" },
                    { "TongTien", "Tổng tiền (VND)" }
                };

                foreach (var kvp in headers)
                {
                    if (dgvListHD.Columns.Contains(kvp.Key))
                        dgvListHD.Columns[kvp.Key].HeaderText = kvp.Value;
                }

                // Format tiền tệ
                string[] tienCols = { "TienDien", "TienNuoc", "TienGuiXe", "TongTien" };
                foreach (string col in tienCols)
                {
                    if (dgvListHD.Columns.Contains(col))
                    {
                        dgvListHD.Columns[col].DefaultCellStyle.Format = "N0"; // định dạng 1,000
                        dgvListHD.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
            }

        }


        private void cboPhong_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadChiSoVaXe();
        }

        private void btnTaoHD_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboPhong.SelectedValue == null || cboThangNam.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn Phòng và Tháng/Năm!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string maPhong = cboPhong.SelectedValue.ToString();
                string[] parts = cboThangNam.SelectedValue.ToString().Split('-');
                int thang = int.Parse(parts[0]);
                int nam = int.Parse(parts[1]);

                // Không cần truyền giá từ form nữa
                HoaDonDichVuModel hd = dvBLL.TaoHoaDon(maPhong, thang, nam, dtpNgayLap.Value);

                MessageBox.Show(
                    $"Đã tạo hóa đơn {hd.MaHD} thành công!\n" +
                    $"Tiền điện: {hd.TienDien:N0} đ\n" +
                    $"Tiền nước: {hd.TienNuoc:N0} đ\n" +
                    $"Tiền xe: {hd.TienGuiXe:N0} đ\n" +
                    $"-------------------------\n" +
                    $"Tổng cộng: {hd.TongTien:N0} đ",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information
                );

                LoadDSHoaDon(); // refresh lại grid
            }
            catch (SqlException sqlEx)
            {
                // bắt lỗi THROW từ stored procedure
                MessageBox.Show(sqlEx.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboThangNam_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadChiSoVaXe();
        }

        private void btnXoaHD_Click(object sender, EventArgs e)
        {
            if (dgvListHD.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maHD = dgvListHD.CurrentRow.Cells["MaHD"].Value.ToString();

            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn xóa hóa đơn {maHD} không?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    dvBLL.XoaHoaDon(maHD);
                    MessageBox.Show($"Đã xóa hóa đơn {maHD} thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadDSHoaDon(); // refresh lại lưới
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnXuatHD_Click(object sender, EventArgs e)
        {
            if (dgvListHD.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để xuất.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataGridViewRow row = dgvListHD.SelectedRows[0];

                // Lấy giá DV từ DB
                var gia = dvBLL.GetGiaDichVu();

                var ct = new HoaDonDVPdfExporter.ChiTietDichVu
                {
                    MaPhong = row.Cells["MaPhong"].Value.ToString(),
                    ThangNam = row.Cells["Thang"].Value + "/" + row.Cells["Nam"].Value,
                    ChiSoDien = Convert.ToInt32(row.Cells["DienTieuThu"].Value),
                    ChiSoNuoc = Convert.ToInt32(row.Cells["NuocTieuThu"].Value),
                    TongXe = Convert.ToInt32(row.Cells["SoLuongXe"].Value),
                    GiaDien = gia.GiaDien,
                    GiaNuoc = gia.GiaNuoc,
                    GiaXe = gia.GiaXe,
                    TongTien = Convert.ToDecimal(row.Cells["TongTien"].Value),
                    NgayLap = Convert.ToDateTime(row.Cells["NgayLap"].Value) // ✅ thêm dòng này
                };


                using (SaveFileDialog sfd = new SaveFileDialog()
                {
                    Filter = "PDF file (*.pdf)|*.pdf",
                    FileName = $"HoaDonDV_{ct.MaPhong}_{ct.ThangNam.Replace("/", "-")}.pdf"
                })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        HoaDonDVPdfExporter.XuatHoaDonDichVu(ct, sfd.FileName);
                        MessageBox.Show("Xuất hóa đơn thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTiep_Click(object sender, EventArgs e)
        {
            dtpNgayLap.Value = DateTime.Now;
            cboThangNam.SelectedIndex = -1;
            cboPhong.SelectedIndex = -1;
            txtCSdien.Clear();
            txtCSnuoc.Clear();
            txtSLxe.Clear();
            dgvListHD.ClearSelection();
        }
    }
}
