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
    public partial class FormQLSV : Form
    {
        private readonly SinhVienBLL _svBLL = new SinhVienBLL();
        private readonly PhongBLL _phongBLL = new PhongBLL();
        private readonly PhanBoBLL _pbBLL = new PhanBoBLL();
        private bool isEditMode = false;


        public FormQLSV()
        {
            InitializeComponent();
        }

        private void FormQLSV_Load(object sender, EventArgs e)
        {
            LoadSinhVien();
            // Gắn sự kiện CellClick
            dgvListSV.CellClick += dgvListSV_CellClick;
        }

        private void LoadSinhVien()
        {
            dgvListSV.DataSource = _svBLL.GetAll();
            dgvListSV.ClearSelection();

            if (dgvListSV.Columns.Count > 0)
            {
                // Đặt lại tên cột
                dgvListSV.Columns["MSSV"].HeaderText = "MSSV";
                dgvListSV.Columns["HoTen"].HeaderText = "Họ tên";
                dgvListSV.Columns["GioiTinh"].HeaderText = "Giới tính";
                dgvListSV.Columns["NgaySinh"].HeaderText = "Ngày sinh";
                dgvListSV.Columns["SDT"].HeaderText = "SĐT";
                dgvListSV.Columns["DiaChi"].HeaderText = "Địa chỉ";

                // Format ngày
                dgvListSV.Columns["NgaySinh"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvListSV.Columns["NgaySinh"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                // Font + căn giữa header
                dgvListSV.DefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
                dgvListSV.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
                dgvListSV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Căn giữa cho MSSV & Giới tính
                dgvListSV.Columns["MSSV"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvListSV.Columns["GioiTinh"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvListSV.Columns["SDT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        private bool ValidateInput()
        {
            string mssv = txtMSSV.Text.Trim();
            string hoten = txtHoTen.Text.Trim();
            string sdt = txtSDT.Text.Trim();

            // Kiểm tra MSSV
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

            // Tính khóa hiện tại theo năm hệ thống
            int namHienTai = DateTime.Now.Year;       // ví dụ 2025
            int khoaHienTai = namHienTai % 100;       // => 25
            int khoaSV = int.Parse(mssv.Substring(0, 2));

            if (khoaSV > khoaHienTai)
            {
                MessageBox.Show($"Hiện tại mới đến khóa {khoaHienTai}, chưa có khóa {khoaSV}!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (khoaSV < khoaHienTai - 2)
            {
                MessageBox.Show($"Sinh viên khóa {khoaSV} đã hết hạn đăng ký ở KTX!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Kiểm tra họ tên
            if (string.IsNullOrEmpty(hoten))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Kiểm tra SĐT
            if (!string.IsNullOrEmpty(sdt))
            {
                if (!sdt.All(char.IsDigit))
                {
                    MessageBox.Show("Số điện thoại chỉ được nhập số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (sdt.Length < 9 || sdt.Length > 11)
                {
                    MessageBox.Show("Số điện thoại phải từ 9 đến 11 chữ số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            return true;
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu trước
            if (!ValidateInput())
                return;

            var sv = new SinhVienModel
            {
                MSSV = txtMSSV.Text.Trim(),
                HoTen = txtHoTen.Text.Trim(),
                NgaySinh = dtpNgaySinh.Value,
                GioiTinh = radNam.Checked ? "Nam" : "Nữ",
                SDT = txtSDT.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim()
            };

            bool success = false;

            if (isEditMode) // đang cập nhật
            {
                if (_svBLL.Update(sv))
                {
                    MessageBox.Show("Cập nhật sinh viên thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    success = true;
                }
                else
                {
                    MessageBox.Show("Cập nhật sinh viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else // đang thêm mới
            {
                if (_svBLL.Insert(sv))
                {
                    MessageBox.Show("Thêm sinh viên thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    success = true;
                }
                else
                {
                    MessageBox.Show("Thêm sinh viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (success)
            {
                LoadSinhVien();

                // Tìm và highlight dòng MSSV vừa thêm/sửa
                foreach (DataGridViewRow row in dgvListSV.Rows)
                {
                    if (row.Cells["MSSV"].Value?.ToString() == sv.MSSV)
                    {
                        dgvListSV.ClearSelection();
                        row.Selected = true;
                        dgvListSV.CurrentCell = row.Cells[0]; // focus vào cột đầu tiên
                        break;
                    }
                }
            }

            // Reset lại sau khi lưu
            isEditMode = false;
            txtMSSV.Enabled = true;

            btnLamMoi_Click(sender, e); // gọi hàm làm mới
        }

        private void btnPhanBo_Click(object sender, EventArgs e)
        {
            if (dgvListSV.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để phân bổ!");
                return;
            }

            string mssv = dgvListSV.CurrentRow.Cells["MSSV"].Value.ToString();
            string hoten = dgvListSV.CurrentRow.Cells["HoTen"].Value.ToString();

            // Mở FormPhanBo, truyền MSSV vào
            using (var f = new FormPhanBo(mssv, hoten))
            {
                f.ShowDialog();
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMSSV.Clear();
            txtHoTen.Clear();
            txtSDT.Clear();
            txtDiaChi.Clear();
            radNam.Checked = true;

            txtMSSV.Enabled = true; // cho phép nhập MSSV
        }

        private void dgvListSV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // bỏ qua header

            DataGridViewRow row = dgvListSV.Rows[e.RowIndex];

            // MSSV
            txtMSSV.Text = row.Cells["MSSV"].Value?.ToString() ?? "";

            // Họ tên
            txtHoTen.Text = row.Cells["HoTen"].Value?.ToString() ?? "";

            // Ngày sinh
            if (row.Cells["NgaySinh"].Value != null &&
                DateTime.TryParse(row.Cells["NgaySinh"].Value.ToString(), out DateTime ns))
            {
                dtpNgaySinh.Value = ns;
            }
            else
            {
                dtpNgaySinh.Value = DateTime.Now; // gán mặc định
            }

            // Giới tính
            string gt = row.Cells["GioiTinh"].Value?.ToString();
            radNam.Checked = gt == "Nam";
            radNu.Checked = gt == "Nữ";

            // SĐT
            txtSDT.Text = row.Cells["SDT"].Value?.ToString() ?? "";

            // Địa chỉ
            txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString() ?? "";

            // 🔥 Đánh dấu là đang sửa
            isEditMode = true;
            txtMSSV.Enabled = false; // không cho sửa MSSV vì nó là khóa chính
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();
            DataTable dt = _svBLL.GetAll();
            DataView dv = dt.DefaultView;
            dv.RowFilter = $"MSSV LIKE '%{keyword}%' OR HoTen LIKE '%{keyword}%'";
            dgvListSV.DataSource = dv.ToTable();
        }
    }
}
