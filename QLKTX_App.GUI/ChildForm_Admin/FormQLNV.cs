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
        public FormQLNV()
        {
            InitializeComponent();
        }

        private void FormQLNV_Load(object sender, EventArgs e)
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
            
        }

        private void dgvListNV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
    }
}
        