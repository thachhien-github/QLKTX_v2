using QLKTX_App.BLL;
using QLKTX_App.DTO;
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

namespace QLKTX_App.ChildForm_Comon
{
    public partial class FormThemPhong : Form
    {
        private readonly PhongBLL _phongBLL = new PhongBLL();
        private readonly TangBLL _tangBLL = new TangBLL();
        private readonly LoaiPhongBLL _loaiBLL = new LoaiPhongBLL();
        public FormThemPhong()
        {
            InitializeComponent();
        }

        #region phương thức load dữ liệu
        private void FormThemPhong_Load(object sender, EventArgs e)
        {
            LoadTang();
            LoadLoaiPhong();
            LoadTrangThai();
            LoadDSPhong();
        }

        private void LoadTang()
        {
            cboChonTang.DataSource = _tangBLL.GetAll();
            cboChonTang.DisplayMember = "TenTang";
            cboChonTang.ValueMember = "MaTang";
        }

        private void LoadLoaiPhong()
        {
            cboLoaiPhong.DataSource = _loaiBLL.GetAll();
            cboLoaiPhong.DisplayMember = "TenLoai";
            cboLoaiPhong.ValueMember = "MaLoai";
        }

        private void LoadTrangThai()
        {
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.Add("Trống");
            cboTrangThai.Items.Add("Đầy");
            cboTrangThai.Items.Add("Đang bảo trì");
            cboTrangThai.SelectedIndex = 0;
        }

        private void LoadDSPhong()
        {
            dgvListPhong.DataSource = _phongBLL.GetAll();
            dgvListPhong.ClearSelection();

            if (dgvListPhong.Columns.Count > 0)
            {
                // Đặt lại header
                dgvListPhong.Columns["MaPhong"].HeaderText = "Mã phòng";
                dgvListPhong.Columns["MaTang"].HeaderText = "Mã tầng";
                dgvListPhong.Columns["MaLoai"].HeaderText = "Mã loại";
                dgvListPhong.Columns["SoLuongToiDa"].HeaderText = "Số lượng tối đa";
                dgvListPhong.Columns["TrangThai"].HeaderText = "Trạng thái";

                // Căn giữa
                dgvListPhong.Columns["MaPhong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvListPhong.Columns["MaTang"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvListPhong.Columns["MaLoai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvListPhong.Columns["TrangThai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                // Format số cho cột "Số lượng tối đa"
                dgvListPhong.Columns["SoLuongToiDa"].DefaultCellStyle.Format = "N0";
                dgvListPhong.Columns["SoLuongToiDa"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                // Font
                dgvListPhong.DefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
                dgvListPhong.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
                dgvListPhong.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private PhongModel GetInput()
        {
            return new PhongModel
            {
                MaPhong = txtMaPhong.Text.Trim(),
                MaTang = cboChonTang.SelectedValue?.ToString(),   // ✔ lấy MaTang từ combobox
                MaLoai = cboLoaiPhong.SelectedValue?.ToString(),  // ✔ lấy MaLoai từ combobox
                SoLuongToiDa = (int)numSucChua.Value,
                TrangThai = cboTrangThai.SelectedItem?.ToString() ?? "Trống"
            };
        }
        #endregion

        #region sự kiện nút bấm
        private void btnThemTang_Click(object sender, EventArgs e)
        {
            if (_tangBLL.Insert(txtMaTang.Text.Trim(), txtTenTang.Text.Trim()))
            {
                MessageBox.Show("Thêm tầng thành công!");
                LoadTang();
            }
            else MessageBox.Show("Thêm tầng thất bại!");
        }

        private void btnXoaTang_Click(object sender, EventArgs e)
        {
            if (_tangBLL.Delete(txtMaTang.Text.Trim()))
            {
                MessageBox.Show("Xóa tầng thành công!");
                LoadTang();
            }
            else MessageBox.Show("Xóa tầng thất bại!");
        }

        private void btnLamMoiTang_Click(object sender, EventArgs e)
        {
            txtMaTang.Clear();
            txtTenTang.Clear();
            txtMaTang.Focus();
        }

        private void btnThemPhong_Click(object sender, EventArgs e)
        {
            var p = GetInput();
            if (string.IsNullOrWhiteSpace(p.MaPhong))
            {
                MessageBox.Show("Mã phòng không được để trống!");
                return;
            }

            if (_phongBLL.Insert(p))  // chỉ thêm mới
            {
                MessageBox.Show("Thêm phòng thành công!");
            }
            else
            {
                // Insert thất bại có thể do trùng mã => thử Update
                if (_phongBLL.Update(p))
                    MessageBox.Show("Phòng đã tồn tại, đã cập nhật thông tin!");
                else
                    MessageBox.Show("Thêm/Cập nhật thất bại!");
            }

            LoadDSPhong();
        }

        private void btnXoaPhong_Click(object sender, EventArgs e)
        {
            if (dgvListPhong.CurrentRow == null) return;

            string maPhong = dgvListPhong.CurrentRow.Cells["MaPhong"].Value.ToString();

            if (MessageBox.Show("Bạn có chắc muốn xóa phòng này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    if (_phongBLL.Delete(maPhong))
                    {
                        MessageBox.Show("Xóa thành công!");
                        LoadDSPhong();
                    }
                    else MessageBox.Show("Xóa thất bại!");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLamMoiPhong_Click(object sender, EventArgs e)
        {
            txtMaPhong.Clear();
            numSucChua.Value = 1;
            cboTrangThai.SelectedIndex = 0;
            if (cboChonTang.Items.Count > 0) cboChonTang.SelectedIndex = 0;
            if (cboLoaiPhong.Items.Count > 0) cboLoaiPhong.SelectedIndex = 0;
            txtMaPhong.Enabled = true;
        }

        #endregion

        private void dgvListPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // bỏ qua header

            var row = dgvListPhong.Rows[e.RowIndex];

            // Mã phòng
            txtMaPhong.Text = row.Cells["MaPhong"].Value?.ToString() ?? "";

            // Mã tầng
            if (row.Cells["MaTang"].Value != null)
                cboChonTang.SelectedValue = row.Cells["MaTang"].Value.ToString();

            // Mã loại phòng
            if (row.Cells["MaLoai"].Value != null)
                cboLoaiPhong.SelectedValue = row.Cells["MaLoai"].Value.ToString();

            // Sức chứa
            if (row.Cells["SoLuongToiDa"].Value != null && int.TryParse(row.Cells["SoLuongToiDa"].Value.ToString(), out int soLuong))
                numSucChua.Value = soLuong;
            else
                numSucChua.Value = numSucChua.Minimum; // fallback để tránh lỗi

            // Trạng thái
            if (row.Cells["TrangThai"].Value != null)
                cboTrangThai.SelectedItem = row.Cells["TrangThai"].Value.ToString();
            else
                cboTrangThai.SelectedIndex = -1;

            // Không cho sửa khóa chính
            txtMaPhong.Enabled = false;
        }
    }
}
