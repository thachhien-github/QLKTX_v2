using QLKTX_App.BLL;
using QLKTX_App.DAL;
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
using static iText.Commons.Utils.PlaceHolderTextUtil;

namespace QLKTX_App.ChildForm_Admin
{
    public partial class FormQLTK : Form
    {
        private readonly QLTaiKhoanBLL bll = new QLTaiKhoanBLL();
        private bool isInsert = true;

        public FormQLTK()
        {
            InitializeComponent();
        }

        #region phương thức load dữ liệu
        private void LoadData()
        {
            dgvListTK.DataSource = bll.GetAll();
            dgvListTK.ClearSelection();
            ResetForm();

            // ✅ Font to, rõ ràng
            dgvListTK.DefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            dgvListTK.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Regular);

            if (dgvListTK.Columns.Count > 0)
            {
                // ✅ Đặt lại tiêu đề cột
                dgvListTK.Columns["MaNV"].HeaderText = "Mã NV";
                dgvListTK.Columns["HoTen"].HeaderText = "Họ tên";
                dgvListTK.Columns["TenDangNhap"].HeaderText = "Tên đăng nhập";
                dgvListTK.Columns["MatKhau"].HeaderText = "Mật khẩu";
                dgvListTK.Columns["TrangThai"].HeaderText = "Trạng thái";
                dgvListTK.Columns["VaiTro"].HeaderText = "Vai trò";

                // ✅ Căn giữa cho đẹp
                dgvListTK.Columns["MaNV"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvListTK.Columns["TrangThai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvListTK.Columns["VaiTro"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // ✅ Căn trái cho họ tên và tên đăng nhập (đọc dễ hơn)
                dgvListTK.Columns["HoTen"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvListTK.Columns["TenDangNhap"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }

            // ✅ Tăng chiều cao dòng
            dgvListTK.RowTemplate.Height = 28;
        }


        private void LoadCboTrangThai()
        {
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.Add("Còn hoạt động");
            cboTrangThai.Items.Add("Bị khóa");
        }

        private void LoadCboVaiTro()
        {
            cboVaiTro.Items.Clear();
            cboVaiTro.Items.Add("Admin");
            cboVaiTro.Items.Add("NhanVien");
        }

        private void ResetForm()
        {
            txtTenDangNhap.Clear();
            txtMatKhau.Clear();
            cboMaNV.SelectedIndex = -1;  // chỉ load khi bấm nút thêm mới
            cboTrangThai.SelectedIndex = -1;
            cboVaiTro.SelectedIndex = -1;
            txtHoTen.Clear();   // chỉ load khi chọn nhân viên
            txtTenDangNhap.Enabled = true;
            isInsert = true;
        }

        private void LoadCboMaNV()
        {
            string query = @"
        SELECT NV.MaNV, NV.HoTen
        FROM NhanVien NV
        WHERE NOT EXISTS (SELECT 1 FROM TaiKhoan TK WHERE TK.MaNV = NV.MaNV)";
            DataTable dt = new DBHelper().ExecuteQuery(query, false);

            cboMaNV.DataSource = dt;
            cboMaNV.DisplayMember = "MaNV";
            cboMaNV.ValueMember = "MaNV";
            cboMaNV.SelectedIndex = -1;
        }

        #endregion

        private void FormQLTK_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadCboTrangThai();
            LoadCboVaiTro();
            LoadCboMaNV();
        }
        

        #region sự kiện nút bấm
        private void dgvListTK_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvListTK.Rows[e.RowIndex];
                txtTenDangNhap.Text = row.Cells["TenDangNhap"].Value.ToString();
                txtHoTen.Text = row.Cells["HoTen"].Value.ToString();
                txtMatKhau.Text = row.Cells["MatKhau"].Value.ToString();
                cboMaNV.Text = row.Cells["MaNV"].Value.ToString();
                cboTrangThai.Text = row.Cells["TrangThai"].Value.ToString();
                cboVaiTro.Text = row.Cells["VaiTro"].Value.ToString();

                txtTenDangNhap.Enabled = false;
                isInsert = false;
            }
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            // ==============================
            //  Kiểm tra dữ liệu nhập
            // ==============================
            if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
            {
                MessageBox.Show("Tên đăng nhập không được để trống!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
            {
                MessageBox.Show("Mật khẩu không được để trống!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return;
            }

            if (string.IsNullOrEmpty(cboTrangThai.Text))
            {
                MessageBox.Show("Chưa chọn trạng thái!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTrangThai.Focus();
                return;
            }

            if (string.IsNullOrEmpty(cboVaiTro.Text))
            {
                MessageBox.Show("Chưa chọn vai trò!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboVaiTro.Focus();
                return;
            }

            if (string.IsNullOrEmpty(cboMaNV.Text))
            {
                MessageBox.Show("Chưa chọn nhân viên!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboMaNV.Focus();
                return;
            }

            // ==============================
            //  Lấy dữ liệu từ form
            // ==============================
            string tenDN = txtTenDangNhap.Text.Trim();
            string mk = txtMatKhau.Text.Trim();
            bool trangThai = (cboTrangThai.Text == "Còn hoạt động");
            string vaiTro = cboVaiTro.Text.Trim();
            string maNV;

            string result;

            if (isInsert)
            {
                // Insert -> lấy MaNV từ ComboBox
                maNV = cboMaNV.SelectedValue.ToString();
                result = bll.Insert(maNV, tenDN, mk, trangThai, vaiTro);
            }
            else
            {
                // Update -> lấy MaNV từ DataGridView (dòng đang chọn)
                if (dgvListTK.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn tài khoản cần cập nhật!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                maNV = dgvListTK.SelectedRows[0].Cells["MaNV"].Value.ToString();
                result = bll.Update(maNV, tenDN, mk, trangThai, vaiTro);
            }

            // ==============================
            //  Kết quả trả về
            // ==============================
            if (result.Contains("thành công"))
                MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

            LoadData();
            ResetForm();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvListTK.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản để xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maNV = dgvListTK.SelectedRows[0].Cells["MaNV"].Value.ToString();

            // ✅ Hỏi trước khi xóa
            var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa tài khoản của nhân viên [" + maNV + "] không?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

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
            catch (SqlException ex)
            {
                if (ex.Number == 547) // Lỗi khóa ngoại
                {
                    MessageBox.Show("Tài khoản đang gắn với nhân viên, không thể xóa!",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Lỗi SQL: " + ex.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            dv.RowFilter = $"TenDangNhap LIKE '%{keyword}%' OR HoTen LIKE '%{keyword}%'";
            dgvListTK.DataSource = dv.ToTable();
        }

        private void cboMaNV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaNV.SelectedItem != null)
            {
                DataRowView drv = cboMaNV.SelectedItem as DataRowView;
                if (drv != null)
                    txtHoTen.Text = drv["HoTen"].ToString();
            }
        }
        #endregion

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();
            DataTable dt = bll.GetAll();
            DataView dv = dt.DefaultView;
            dv.RowFilter = $"TenDangNhap LIKE '%{keyword}%' OR HoTen LIKE '%{keyword}%'";
            dgvListTK.DataSource = dv.ToTable();
        }
    }
}
