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
            cboTrangThai.Items.AddRange(new string[] { "", "Còn hạn", "Sắp hết hạn", "Hết hạn" });
            LoadHopDong();
        }

        private void LoadHopDong()
        {
            dgvListHopDong.DataSource = _hdBLL.GetAll();
            FormatGiaPhong();
        }

        private void FormatGiaPhong()
        {
            if (dgvListHopDong.Columns.Contains("GiaPhong"))
            {
                dgvListHopDong.Columns["GiaPhong"].DefaultCellStyle.Format = "N0"; // số nguyên với dấu phân tách hàng nghìn
                dgvListHopDong.Columns["GiaPhong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string mssv = txtMSSV.Text.Trim();
            string phong = cboPhong.Text.Trim();
            string trangThai = cboTrangThai.Text.Trim();

            dgvListHopDong.DataSource = _hdBLL.Search(mssv, phong, trangThai);
        }

        private void btnGiaHan_Click(object sender, EventArgs e)
        {
            if (dgvListHopDong.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn hợp đồng cần gia hạn!");
                return;
            }

            string mssv = dgvListHopDong.CurrentRow.Cells["MSSV"].Value.ToString();

            // hỏi số tháng muốn gia hạn
            string input = Microsoft.VisualBasic.Interaction.InputBox("Nhập số tháng muốn gia hạn:", "Gia hạn hợp đồng", "3");
            if (!int.TryParse(input, out int soThang) || soThang <= 0)
            {
                MessageBox.Show("Số tháng không hợp lệ!");
                return;
            }

            // xác nhận
            if (MessageBox.Show($"Bạn có chắc muốn gia hạn hợp đồng của sinh viên {mssv} thêm {soThang} tháng?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            if (_hdBLL.GiaHan(mssv, soThang))
            {
                MessageBox.Show("Gia hạn thành công!");
                // reload dữ liệu với filter hiện tại
                btnTimKiem_Click(null, null);
            }
            else
            {
                MessageBox.Show("Gia hạn thất bại!");
            }
        }

        private void btnXuatHopDong_Click(object sender, EventArgs e)
        {
            if (dgvListHopDong.CurrentRow == null) return;

            var r = ((DataRowView)dgvListHopDong.CurrentRow.DataBoundItem).Row;

            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "PDF|*.pdf" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    HopDongExporter.ExportHopDongPDF(r, sfd.FileName);
                    MessageBox.Show("Xuất hợp đồng PDF thành công!");
                }
            }
        }
    }
}
