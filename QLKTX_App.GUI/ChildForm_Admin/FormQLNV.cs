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
                MessageBox.Show("⚠️ Vui lòng chọn nhân viên để xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string maNV = dgvListNV.SelectedRows[0].Cells["MaNV"].Value.ToString();
            var result = bll.Delete(maNV);

            if (result.Contains("thành công"))
                MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvListNV.Rows[e.RowIndex];
                txtMaNV.Text = row.Cells["MaNV"].Value.ToString();
                txtHoTen.Text = row.Cells["HoTen"].Value.ToString();
                string gt = row.Cells["GioiTinh"].Value.ToString();
                radNam.Checked = gt == "Nam";
                radNu.Checked = gt == "Nữ";
                dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
                txtSDT.Text = row.Cells["SDT"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                //txtTenDangNhap.Text = row.Cells["TenDangNhap"].Value.ToString();

                txtMaNV.Enabled = false; // Không cho sửa MaNV khi cập nhật
                isInsert = false;
            }
        }
        #endregion
    }
}
        