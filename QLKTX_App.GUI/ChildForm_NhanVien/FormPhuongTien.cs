using QLKTX_App.BLL;
using QLKTX_App.DAL;
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
using System.Windows.Controls;
using System.Windows.Forms;

namespace QLKTX_App.ChildForm_NhanVien
{
    public partial class FormPhuongTien : Form
    {
        private readonly TheXeBLL _bll = new TheXeBLL();
        public FormPhuongTien()
        {
            InitializeComponent();
            LoadData();
            LoadLoaiXe();
        }

        private void LoadLoaiXe()
        {
            cboLoaiXe.DataSource = _bll.GetLoaiXe();
            cboLoaiXe.DisplayMember = "TenLoai";
            cboLoaiXe.ValueMember = "MaLoaiXe";
            cboLoaiXe.SelectedIndex = -1;
        }

        private void LoadData()
        {
            dgvListTheXe.DataSource = _bll.GetAll();
            dgvListTheXe.ClearSelection();

            if (dgvListTheXe.Columns.Count > 0)
            {
                // Đặt lại header
                dgvListTheXe.Columns["MaThe"].HeaderText = "Mã thẻ";
                dgvListTheXe.Columns["MSSV"].HeaderText = "MSSV";
                dgvListTheXe.Columns["HoTen"].HeaderText = "Họ tên";
                dgvListTheXe.Columns["MaLoaiXe"].HeaderText = "Mã loại xe";
                dgvListTheXe.Columns["TenLoai"].HeaderText = "Loại xe";
                dgvListTheXe.Columns["BienSo"].HeaderText = "Biển số";
                dgvListTheXe.Columns["NgayDangKy"].HeaderText = "Ngày đăng ký";

                // Căn giữa mấy cột code
                dgvListTheXe.Columns["MaThe"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvListTheXe.Columns["MSSV"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvListTheXe.Columns["MaLoaiXe"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                // Cột ngày format dd/MM/yyyy
                dgvListTheXe.Columns["NgayDangKy"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvListTheXe.Columns["NgayDangKy"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                // Font
                dgvListTheXe.DefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
                dgvListTheXe.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
                dgvListTheXe.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private bool ValidateInput()
        {
            string maThe = txtMaThe.Text.Trim();
            string mssv = txtMSSV.Text.Trim();
            string bienSo = txtBienSo.Text.Trim();

            // Mã thẻ
            if (string.IsNullOrEmpty(maThe))
            {
                MessageBox.Show("Vui lòng nhập mã thẻ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // MSSV
            if (string.IsNullOrEmpty(mssv))
            {
                MessageBox.Show("Vui lòng nhập MSSV!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (mssv.Length != 10 || !mssv.All(char.IsDigit))
            {
                MessageBox.Show("MSSV phải gồm đúng 10 chữ số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Kiểm tra khóa học từ MSSV
            int khoaSV = int.Parse(mssv.Substring(0, 2));
            int khoaHienTai = DateTime.Now.Year % 100; // Ví dụ 2025 -> 25

            if (khoaSV > khoaHienTai)
            {
                MessageBox.Show($"Hiện tại mới có đến khóa {khoaHienTai}, chưa có khóa {khoaSV}!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (khoaSV < khoaHienTai - 2)
            {
                MessageBox.Show($"Sinh viên khóa {khoaSV} đã hết hạn đăng ký ở KTX!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Loại xe
            if (cboLoaiXe.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn loại xe!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Biển số
            if (string.IsNullOrEmpty(bienSo))
            {
                MessageBox.Show("Vui lòng nhập biển số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                var xe = new TheXeModel
                {
                    MaThe = txtMaThe.Text.Trim(),
                    MSSV = txtMSSV.Text.Trim(),
                    LoaiXe = cboLoaiXe.SelectedValue?.ToString(),
                    BienSo = txtBienSo.Text.Trim(),
                    NgayDangKy = dtpNgayDK.Value.Date
                };

                if (_bll.Them(xe))
                {
                    MessageBox.Show("Đăng ký phương tiện thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                    // 🔥 Highlight dòng vừa thêm
                    foreach (DataGridViewRow row in dgvListTheXe.Rows)
                    {
                        if (row.Cells["MaThe"].Value?.ToString() == xe.MaThe)
                        {
                            dgvListTheXe.ClearSelection();
                            row.Selected = true;
                            dgvListTheXe.CurrentCell = row.Cells[0]; // focus vào cột đầu tiên
                            dgvListTheXe.FirstDisplayedScrollingRowIndex = row.Index;
                            break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Đăng ký thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaThe.Text))
            {
                MessageBox.Show("Vui lòng nhập mã thẻ cần xóa!");
                return;
            }

            try
            {
                if (_bll.Xoa(txtMaThe.Text))
                {
                    MessageBox.Show("Xóa thành công!");
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnTiep_Click(object sender, EventArgs e)
        {
            txtMaThe.Clear();
            txtMSSV.Clear();
            txtBienSo.Clear();
            cboLoaiXe.SelectedIndex = -1;
            dtpNgayDK.Value = DateTime.Now;

            txtMSSV.ReadOnly = false; // cho phép nhập MSSV mới
            txtMSSV.Focus();
        }

        private void dgvListTheXe_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // bỏ qua header

            var row = dgvListTheXe.Rows[e.RowIndex];

            // Mã thẻ
            txtMaThe.Text = row.Cells["MaThe"].Value?.ToString() ?? "";

            // MSSV
            txtMSSV.Text = row.Cells["MSSV"].Value?.ToString() ?? "";
            txtMSSV.ReadOnly = true; // không cho sửa MSSV khi đã có thẻ

            // Loại xe
            if (row.Cells["MaLoaiXe"].Value != null)
                cboLoaiXe.SelectedValue = row.Cells["MaLoaiXe"].Value;

            // Biển số
            txtBienSo.Text = row.Cells["BienSo"].Value?.ToString() ?? "";

            // Ngày đăng ký
            if (row.Cells["NgayDangKy"].Value != null &&
                DateTime.TryParse(row.Cells["NgayDangKy"].Value.ToString(), out DateTime ngayDK))
            {
                dtpNgayDK.Value = ngayDK;
            }
            else
            {
                dtpNgayDK.Value = DateTime.Now; // fallback mặc định
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();
            DataTable dt = _bll.GetAll();
            DataView dv = dt.DefaultView;
            dv.RowFilter = $"MSSV LIKE '%{keyword}%' OR MaThe LIKE '%{keyword}%' OR BienSo LIKE '%{keyword}%'";
            dgvListTheXe.DataSource = dv.ToTable();
        }
    }
}
