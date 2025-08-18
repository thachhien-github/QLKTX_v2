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
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvListSV.Rows[e.RowIndex];

            txtMSSV.Text = row.Cells["MSSV"].Value?.ToString();
            txtHoTen.Text = row.Cells["HoTen"].Value?.ToString();
            dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
            string gt = row.Cells["GioiTinh"].Value?.ToString();
            radNam.Checked = gt == "Nam";
            radNu.Checked = gt == "Nữ";
            txtSDT.Text = row.Cells["SDT"].Value?.ToString();
            txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString();
        }
    }
}
