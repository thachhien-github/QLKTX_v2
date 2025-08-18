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

namespace QLKTX_App.GUI.ChildForm_Comon
{
    public partial class FormHopDong : Form
    {
        private readonly HopDongBLL _hdBLL = new HopDongBLL();
        public FormHopDong()
        {
            InitializeComponent();
        }

        private void FormHopDong_Load(object sender, EventArgs e)
        {
            LoadHopDong();
        }

        private void LoadHopDong()
        {
            dgvListHopDong.DataSource = _hdBLL.Search("", "", "");
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string mssv = txtMSSV.Text.Trim();
            string phong = cboPhong.SelectedValue?.ToString() ?? "";
            string trangThai = cboTrangThai.Text.Trim();

            dgvListHopDong.DataSource = _hdBLL.Search(mssv, phong, trangThai);
        }

        private void btnGiaHan_Click(object sender, EventArgs e)
        {
            if (dgvListHopDong.CurrentRow == null) return;
            int maPhanBo = Convert.ToInt32(dgvListHopDong.CurrentRow.Cells["MaPhanBo"].Value);

            if (_hdBLL.GiaHan(maPhanBo, 3)) // mặc định +3 tháng
            {
                MessageBox.Show("Gia hạn thành công!");
                LoadHopDong();
            }
            else MessageBox.Show("Gia hạn thất bại!");
        }

        private void btnXuatHopDong_Click(object sender, EventArgs e)
        {
            if (dgvListHopDong.CurrentRow == null) return;

            var r = ((DataRowView)dgvListHopDong.CurrentRow.DataBoundItem).Row;

            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "Text|*.txt" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    HopDongExporter.ExportHopDong(r, sfd.FileName);
                    MessageBox.Show("Xuất hợp đồng thành công!");
                }
            }
        }
    }
}
