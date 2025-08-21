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
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
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
                    MessageBox.Show("Đăng ký thành công!");
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
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
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string kw = txtTimKiem.Text.Trim();
            if (string.IsNullOrEmpty(kw))
            {
                LoadData();
                return;
            }
            dgvListTheXe.DataSource = _bll.Search(kw);
        }

        private void dgvListTheXe_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvListTheXe.Rows[e.RowIndex];
                txtMaThe.Text = row.Cells["MaThe"].Value.ToString();
                txtMSSV.Text = row.Cells["MSSV"].Value.ToString();
                cboLoaiXe.SelectedValue = row.Cells["MaLoaiXe"].Value;
                txtBienSo.Text = row.Cells["BienSo"].Value.ToString();
                dtpNgayDK.Value = Convert.ToDateTime(row.Cells["NgayDangKy"].Value);
            }
        }
    }
}
