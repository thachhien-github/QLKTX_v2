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


        private void btnDangKy_Click(object sender, EventArgs e)
        {
            var sv = new SinhVienModel
            {
                MSSV = txtMSSV.Text.Trim(),
                HoTen = txtHoTen.Text.Trim(),
                NgaySinh = dtpNgaySinh.Value,
                GioiTinh = radNam.Checked ? "Nam" : "Nữ",
                SDT = txtSDT.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim()
            };

            if (_svBLL.Insert(sv))
            {
                MessageBox.Show("Thêm sinh viên thành công!");
                LoadSinhVien();
            }
            else MessageBox.Show("Thêm thất bại!");
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string mssv = txtMSSV.Text.Trim();
            if (_svBLL.Delete(mssv))
            {
                MessageBox.Show("Xóa thành công!");
                LoadSinhVien();
            }
            else MessageBox.Show("Xóa thất bại!");
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
            if (row.Cells["NgaySinh"].Value != null && DateTime.TryParse(row.Cells["NgaySinh"].Value.ToString(), out DateTime ns))
            {
                dtpNgaySinh.Value = ns;
            }
            else
            {
                dtpNgaySinh.Value = DateTime.Now; // gán mặc định để tránh crash
            }

            // Giới tính
            string gt = row.Cells["GioiTinh"].Value?.ToString();
            radNam.Checked = gt == "Nam";
            radNu.Checked = gt == "Nữ";

            // SĐT
            txtSDT.Text = row.Cells["SDT"].Value?.ToString() ?? "";

            // Địa chỉ
            txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString() ?? "";
        }
    }
}
