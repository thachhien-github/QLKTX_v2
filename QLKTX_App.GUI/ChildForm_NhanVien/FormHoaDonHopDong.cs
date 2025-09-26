using QLKTX_App.BLL;
using QLKTX_App.DAL;
using QLKTX_App.DTO;
using QLKTX_App.GUI.Util;
using QLKTX_App.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLKTX_App.GUI.ChildForm_NhanVien
{
    public partial class FormHoaDonHopDong : Form
    {
        private readonly ThanhToanPhongBLL ttpBLL = new ThanhToanPhongBLL();
        private readonly PhanBoBLL phanBoBLL = new PhanBoBLL();
        public FormHoaDonHopDong()
        {
            InitializeComponent();
            LoadDanhSachHoaDon();
            LoadDanhSachPhong();
        }

        private void LoadDanhSachPhong()
        {
            cboPhong.DataSource = phanBoBLL.GetAllPhong();
            cboPhong.DisplayMember = "MaPhong";
            cboPhong.ValueMember = "MaPhong";
            cboPhong.SelectedIndex = -1;
        }

        private void LoadDanhSachHoaDon()
        {
            dgvListHD.DataSource = ttpBLL.GetAll();
            dgvListHD.ClearSelection();

            // Định nghĩa header text
            var headers = new Dictionary<string, string>
            {
                { "MaHD", "Mã HĐ" },
                { "MSSV", "MSSV" },
                { "MaPhong", "Phòng" },
                { "SoThangThu", "Số tháng" },
                { "NgayThu", "Ngày thu" },
                { "TienPhong", "Tiền phòng (VND)" },
                { "TienTheChan", "Tiền Thế chân (VND)" },
                { "TongTien", "Tổng tiền (VND)" },
                { "GhiChu", "Ghi chú" }
            };

            foreach (var kvp in headers)
            {
                if (dgvListHD.Columns.Contains(kvp.Key))
                    dgvListHD.Columns[kvp.Key].HeaderText = kvp.Value;
            }

            // Các cột tiền tệ
            string[] tienCols = { "TienPhong", "TienTheChan", "TongTien" };

            foreach (string col in tienCols)
            {
                if (dgvListHD.Columns.Contains(col))
                {
                    dgvListHD.Columns[col].DefaultCellStyle.Format = "N0"; // 1,000 định dạng
                    dgvListHD.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }

            if (dgvListHD.Columns.Contains("GhiChu"))
            {
                dgvListHD.Columns["GhiChu"].DisplayIndex = dgvListHD.Columns.Count - 1;
            }

        }

        private void LoadThongTinHoaDon()
        {
            if (string.IsNullOrWhiteSpace(txtMSSV.Text) || cboPhong.SelectedIndex == -1)
                return;

            string mssv = txtMSSV.Text.Trim();
            string maPhong = cboPhong.SelectedValue.ToString();

            DataTable dt = phanBoBLL.GetChiTietPhanBo(mssv, maPhong);

            if (dt.Rows.Count > 0)
            {
                int soThang = Convert.ToInt32(dt.Rows[0]["SoThang"]);
                decimal giaPhong = Convert.ToDecimal(dt.Rows[0]["GiaPhong"]);
                int sucChua = Convert.ToInt32(dt.Rows[0]["SucChua"]);

                nmuSoThang.Value = soThang;

                decimal tienPhong = soThang * (giaPhong / sucChua);
                decimal tienTheChan = 300000;
                decimal tongTien = tienPhong + tienTheChan;

                txtHoTen.Text = dt.Rows[0]["HoTen"].ToString();   // <<-- load họ tên
                txtGhiChu.Text = dt.Rows[0]["GhiChu"].ToString(); // <<-- load ghi chú

                txtTienPhong.Text = tienPhong.ToString("N0");
                txtTienTheChan.Text = tienTheChan.ToString("N0");
                txtTongTien.Text = tongTien.ToString("N0");
            }
            else
            {
                MessageBox.Show("Không tìm thấy phân bổ cho sinh viên này!", "Thông báo");
            }
        }

        private void cboPhong_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadThongTinHoaDon();
        }

        private void txtMSSV_Leave(object sender, EventArgs e)
        {
            LoadThongTinHoaDon();
        }

        private void btnTaoHD_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMSSV.Text) || cboPhong.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng nhập MSSV và chọn phòng!", "Thông báo");
                return;
            }

            ThanhToanPhongModel ttp = new ThanhToanPhongModel
            {
                MSSV = txtMSSV.Text.Trim(),
                MaPhong = cboPhong.SelectedValue.ToString(),
                SoThangThu = (int)nmuSoThang.Value,
                NgayThu = dtpNgayLap.Value,
                NguoiThu = CurrentUser.MaNV,
                GhiChu = txtGhiChu.Text,   // ✅ lưu từ phân bổ
                TienPhong = decimal.Parse(txtTienPhong.Text.Replace(".", "")),
                TienTheChan = decimal.Parse(txtTienTheChan.Text.Replace(".", "")),
                TongTien = decimal.Parse(txtTongTien.Text.Replace(".", ""))
            };


            int kq = ttpBLL.Insert(ttp);
            if (kq > 0)
            {
                MessageBox.Show("Tạo hóa đơn thành công!", "Thông báo");
                LoadDanhSachHoaDon();
            }
            else
            {
                MessageBox.Show("Tạo hóa đơn thất bại!", "Lỗi");
            }
        }

        private void btnXoaHD_Click(object sender, EventArgs e)
        {
            if (dgvListHD.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn để xóa!", "Thông báo");
                return;
            }

            int id = Convert.ToInt32(dgvListHD.SelectedRows[0].Cells["ID"].Value);

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa hóa đơn này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int kq = ttpBLL.Delete(id);
                if (kq > 0)
                {
                    MessageBox.Show("Xóa hóa đơn thành công!", "Thông báo");
                    LoadDanhSachHoaDon();
                }
                else
                {
                    MessageBox.Show("Xóa hóa đơn thất bại!", "Lỗi");
                }
            }
        }

        private void btnXuatHD_Click(object sender, EventArgs e)
        {
            if (dgvListHD.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn để xuất!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Lấy dữ liệu từ row đang chọn
                DataGridViewRow row = dgvListHD.SelectedRows[0];
                string maHD = row.Cells["ID"].Value.ToString();
                string mssv = row.Cells["MSSV"].Value.ToString();
                string maPhong = row.Cells["MaPhong"].Value.ToString();
                DateTime ngayLap = Convert.ToDateTime(row.Cells["NgayThu"].Value);

                // Lấy họ tên SV từ Phân Bổ (hoặc JOIN)
                DataTable dt = phanBoBLL.GetChiTietPhanBo(mssv, maPhong);
                string hoTen = dt.Rows.Count > 0 ? dt.Rows[0]["HoTen"].ToString() : "Không rõ";

                decimal tienPhong = Convert.ToDecimal(row.Cells["TienPhong"].Value);
                decimal tienTheChan = Convert.ToDecimal(row.Cells["TienTheChan"].Value);

                var chiTiet = new HoaDonHDPdfExporter.ChiTietHopDong
                {
                    MSSV = mssv,
                    HoTen = hoTen,
                    MaPhong = maPhong,
                    TienPhong = tienPhong,
                    TienTheChan = tienTheChan,
                    NguoiThu = CurrentUser.HoTen
                };

                string thangNam = ngayLap.ToString("MM-yyyy");

                // Dùng SaveFileDialog để lưu trực tiếp file PDF
                using (SaveFileDialog sfd = new SaveFileDialog()
                {
                    Filter = "PDF file (*.pdf)|*.pdf",
                    FileName = $"HD_{mssv}_{maPhong}_{thangNam}.pdf"
                })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = sfd.FileName;
                        HoaDonHDPdfExporter.XuatHoaDonHopDong(maHD, ngayLap, chiTiet, filePath);

                        MessageBox.Show("Xuất hóa đơn thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // ✅ Mở file sau khi xuất
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                        {
                            FileName = filePath,
                            UseShellExecute = true
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất hóa đơn: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void dgvListHD_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // bỏ qua header

            DataGridViewRow row = dgvListHD.Rows[e.RowIndex];

            // MSSV
            txtMSSV.Text = row.Cells["MSSV"].Value?.ToString() ?? "";

            // Phòng
            if (row.Cells["MaPhong"].Value != null)
                cboPhong.SelectedValue = row.Cells["MaPhong"].Value.ToString();

            // Số tháng
            if (row.Cells["SoThangThu"].Value != null &&
                int.TryParse(row.Cells["SoThangThu"].Value.ToString(), out int soThang))
                nmuSoThang.Value = soThang;
            else
                nmuSoThang.Value = nmuSoThang.Minimum;

            // Ngày thu
            if (row.Cells["NgayThu"].Value != null &&
                DateTime.TryParse(row.Cells["NgayThu"].Value.ToString(), out DateTime ngayThu))
                dtpNgayLap.Value = ngayThu;
            else
                dtpNgayLap.Value = DateTime.Now;

            // Tiền phòng
            txtTienPhong.Text = row.Cells["TienPhong"].Value?.ToString() ?? "";

            // Tiền thế chân
            txtTienTheChan.Text = row.Cells["TienTheChan"].Value?.ToString() ?? "";

            // Tổng tiền
            txtTongTien.Text = row.Cells["TongTien"].Value?.ToString() ?? "";

            // Ghi chú
            txtGhiChu.Text = row.Cells["GhiChu"].Value != null && row.Cells["GhiChu"].Value != DBNull.Value
                ? row.Cells["GhiChu"].Value.ToString()
                : "";
        }

        private void btnTiep_Click(object sender, EventArgs e)
        {
            cboPhong.SelectedIndex = -1;
            txtMSSV.Clear();
            txtHoTen.Clear();
            nmuSoThang.Value = nmuSoThang.Minimum;
            dtpNgayLap.Value = DateTime.Now;
            txtGhiChu.Clear();
            txtTienPhong.Clear();
            txtTienTheChan.Clear();
            txtTongTien.Clear();
            dgvListHD.ClearSelection();
        }
    }
}
