using QLKTX_App.BLL;
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

namespace QLKTX_App.ChildForm_Admin
{
    public partial class FormQLTK : Form
    {
        private QLTaiKhoanBLL bll = new QLTaiKhoanBLL();
        public FormQLTK()
        {
            InitializeComponent();
        }

        private void FormQLTK_Load(object sender, EventArgs e)
        {
            cboTrangThai.Items.Add("Đang hoạt động");
            cboTrangThai.Items.Add("Bị khóa");
            cboTrangThai.SelectedIndex = 0;

            LoadTaiKhoan();
        }
        private void LoadTaiKhoan()
        {
            dgvListTK.DataSource = bll.GetAll();
        }

        private void dgvListTK_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvListTK.Rows[e.RowIndex];

                txtTenDangNhap.Text = row.Cells["TenDangNhap"].Value?.ToString();
                txtMatKhau.Text = row.Cells["MatKhau"].Value?.ToString();
                txtMaNV.Text = row.Cells["MaNV"].Value?.ToString();
                txtHoTen.Text = row.Cells["HoTen"].Value?.ToString();

                cboTrangThai.SelectedIndex = (row.Cells["TrangThai"].Value?.ToString() == "Đang hoạt động") ? 0 : 1;

                txtTenDangNhap.ReadOnly = true;
                txtMatKhau.ReadOnly = true;
                txtMaNV.ReadOnly = true;
                txtHoTen.ReadOnly = true;
            }
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            try
            {
                string tenDangNhap = txtTenDangNhap.Text.Trim();
                string matKhau = txtMatKhau.Text.Trim();
                string maNV = txtMaNV.Text.Trim();
                int trangThai = cboTrangThai.SelectedIndex == 0 ? 1 : 0;

                if (bll.Save(tenDangNhap, matKhau, maNV, trangThai))
                {
                    MessageBox.Show("Lưu tài khoản thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadTaiKhoan();
                    btnTiep_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string user = txtTenDangNhap.Text.Trim();
            if (string.IsNullOrEmpty(user)) return;

            if (MessageBox.Show("Xóa tài khoản này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (bll.Delete(user))
                {
                    MessageBox.Show("Xóa thành công!");
                    LoadTaiKhoan();
                    btnTiep_Click(null, null);
                }
                else
                {
                    MessageBox.Show("Xóa thất bại!");
                }
            }
        }

        private void btnTiep_Click(object sender, EventArgs e)
        {
            txtTenDangNhap.Clear();
            txtMatKhau.Clear();
            txtMaNV.Clear();
            txtHoTen.Clear();
            txtSearch.Clear();
            cboTrangThai.SelectedIndex = 0;
            dgvListTK.ClearSelection();
            txtTenDangNhap.ReadOnly = false;
            txtMatKhau.ReadOnly = false;
            txtMaNV.ReadOnly = false;
            txtHoTen.ReadOnly = false;
            txtTenDangNhap.Focus();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            dgvListTK.DataSource = bll.Search(txtSearch.Text.Trim());
        }
    }
}
