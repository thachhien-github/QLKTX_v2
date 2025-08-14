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

namespace QLKTX_App.ChildForm_Admin
{
    public partial class FormQLNV : Form
    {
        private readonly NhanVienBLL bll = new NhanVienBLL();
        public FormQLNV()
        {
            InitializeComponent();
        }

        private void FormQLNV_Load(object sender, EventArgs e)
        {
            LoadNhanVien();
        }
        private void LoadNhanVien()
        {
            dgvListNV.DataSource = bll.GetAll();
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            bool isNew = bll.Save(
                txtMaNV.Text.Trim(),
                txtHoTen.Text.Trim(),
                radNam.Checked ? "Nam" : "Nữ",
                dtpNgaySinh.Value,
                txtSDT.Text.Trim(),
                txtEmail.Text.Trim(),
                txtTenDangNhap.Text.Trim()
            );

            MessageBox.Show(isNew ? "Thêm mới nhân viên thành công!" : "Cập nhật nhân viên thành công!",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadNhanVien();
            btnTiep_Click(null, null);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaNV.Text)) return;

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            bool deleted = bll.Delete(txtMaNV.Text.Trim());

            MessageBox.Show(deleted ? "Xóa nhân viên thành công!" : "Không thể xóa nhân viên đã có tài khoản!",
                "Thông báo", MessageBoxButtons.OK, deleted ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            LoadNhanVien();
            btnTiep_Click(null, null);
        }

        private void btnTiep_Click(object sender, EventArgs e)
        {
            txtMaNV.Clear();
            txtHoTen.Clear();
            txtSDT.Clear();
            txtEmail.Clear();
            txtTenDangNhap.Clear();
            txtSearch.Clear();
            dtpNgaySinh.Value = DateTime.Now;
            radNam.Checked = true;
            dgvListNV.ClearSelection();
            txtMaNV.ReadOnly = false;
            txtMaNV.Focus();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            dgvListNV.DataSource = bll.Search(txtSearch.Text.Trim());
        }

        private void dgvListNV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvListNV.Rows[e.RowIndex];
                txtMaNV.Text = row.Cells["MaNV"].Value?.ToString();
                txtHoTen.Text = row.Cells["HoTen"].Value?.ToString();
                dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
                txtSDT.Text = row.Cells["SoDienThoai"].Value?.ToString();
                txtEmail.Text = row.Cells["Email"].Value?.ToString();
                txtTenDangNhap.Text = row.Cells["TenDangNhap"].Value?.ToString();

                string gioiTinh = row.Cells["GioiTinh"].Value?.ToString();
                radNam.Checked = gioiTinh == "Nam";
                radNu.Checked = gioiTinh == "Nữ";

                txtMaNV.ReadOnly = true;
            }
        }
    }
}
        