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
        public FormQLTK()
        {
            InitializeComponent();
        }

        private void FormQLTK_Load(object sender, EventArgs e)
        {
           
        }

        private void dgvListTK_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            
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
            
        }
    }
}
