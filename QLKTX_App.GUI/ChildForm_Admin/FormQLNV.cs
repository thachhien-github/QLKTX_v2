using Org.BouncyCastle.Asn1.X500;
using QLKTX_App.BLL;
using QLKTX_App.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheArtOfDevHtmlRenderer.Adapters;

namespace QLKTX_App.ChildForm_Admin
{
    public partial class FormQLNV : Form
    {
        private readonly NhanVienBLL bll = new NhanVienBLL();
        private bool isInsert = true;

        public FormQLNV()
        {
            InitializeComponent();
        }

        #region phương thức load dữ liệu
        private void LoadData()
        {
            dgvListNV.DataSource = bll.GetAll();
            dgvListNV.ClearSelection();
            ResetForm();

            // ✅ Font to, rõ ràng
            dgvListNV.DefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            dgvListNV.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Regular);

            // ✅ Đặt lại tiêu đề cột
            if (dgvListNV.Columns.Count > 0)
            {
                dgvListNV.Columns["MaNV"].HeaderText = "Mã NV";
                dgvListNV.Columns["HoTen"].HeaderText = "Họ tên";
                dgvListNV.Columns["GioiTinh"].HeaderText = "Giới tính";
                dgvListNV.Columns["NgaySinh"].HeaderText = "Ngày sinh";
                dgvListNV.Columns["SDT"].HeaderText = "Số ĐT";
                dgvListNV.Columns["Email"].HeaderText = "Email";

                // ✅ Căn giữa cột giới tính & ngày sinh
                dgvListNV.Columns["GioiTinh"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvListNV.Columns["NgaySinh"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                // ✅ Căn phải số điện thoại cho đẹp
                dgvListNV.Columns["SDT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            // ✅ Tăng chiều cao dòng cho dễ nhìn
            dgvListNV.RowTemplate.Height = 28;
        }


        private void ResetForm()
        {
            txtMaNV.Clear();
            txtHoTen.Clear();
            radNam.Checked = true;
            dtpNgaySinh.Value = DateTime.Now;
            txtSDT.Clear();
            txtEmail.Clear();
            txtMaNV.Enabled = true;
            isInsert = true;
        }

        #endregion

        private void FormQLNV_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        #region sự kiện nút bấm
        private void btnDangKy_Click(object sender, EventArgs e)
        {
            string maNV = txtMaNV.Text.Trim();
            string hoTen = txtHoTen.Text.Trim();
            string gioiTinh = radNam.Checked ? "Nam" : "Nữ";
            DateTime ngaySinh = dtpNgaySinh.Value;
            string sdt = txtSDT.Text.Trim();
            string email = txtEmail.Text.Trim();

            string result;
            if (isInsert)
            {
                result = bll.Insert(maNV, hoTen, gioiTinh, ngaySinh, sdt, email);
            }
            else
            {
                result = bll.Update(maNV, hoTen, gioiTinh, ngaySinh, sdt, email);
            }

            if (result.Contains("thành công"))
                MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

            LoadData();
            ResetForm();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvListNV.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên để xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maNV = dgvListNV.SelectedRows[0].Cells["MaNV"].Value.ToString();

            try
            {
                var result = bll.Delete(maNV);

                if (result.Contains("thành công"))
                {
                    MessageBox.Show(result, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(result, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                // Bắt lỗi từ SQL (ví dụ: ràng buộc FK, THROW trong SP)
                MessageBox.Show($"Lỗi SQL: {ex.Message}",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Lỗi khác
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            LoadData();
            ResetForm();
        }

        private void btnTiep_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();
            DataTable dt = bll.GetAll();
            DataView dv = dt.DefaultView;
            dv.RowFilter = $"MaNV LIKE '%{keyword}%' OR HoTen LIKE '%{keyword}%'";
            dgvListNV.DataSource = dv.ToTable();
        }

        private void dgvListNV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // bỏ qua header

            DataGridViewRow row = dgvListNV.Rows[e.RowIndex];

            // Mã NV
            txtMaNV.Text = row.Cells["MaNV"].Value?.ToString() ?? "";

            // Họ tên
            txtHoTen.Text = row.Cells["HoTen"].Value?.ToString() ?? "";

            // Giới tính
            string gt = row.Cells["GioiTinh"].Value?.ToString();
            radNam.Checked = gt == "Nam";
            radNu.Checked = gt == "Nữ";

            // Ngày sinh
            if (row.Cells["NgaySinh"].Value != null &&
                DateTime.TryParse(row.Cells["NgaySinh"].Value.ToString(), out DateTime ns))
            {
                dtpNgaySinh.Value = ns;
            }
            else
            {
                dtpNgaySinh.Value = DateTime.Now;
            }

            // SĐT
            txtSDT.Text = row.Cells["SDT"].Value?.ToString() ?? "";

            // Email
            txtEmail.Text = row.Cells["Email"].Value?.ToString() ?? "";

            // Tên đăng nhập (nếu bạn cần bật lại)
            //txtTenDangNhap.Text = row.Cells["TenDangNhap"].Value?.ToString() ?? "";

            // Không cho sửa khóa chính
            txtMaNV.Enabled = false;
            isInsert = false;
        }
        #endregion
    }
}
        