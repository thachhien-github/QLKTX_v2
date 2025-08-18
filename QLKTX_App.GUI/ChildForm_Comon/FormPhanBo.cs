using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
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
    public partial class FormPhanBo : Form
    {
        private readonly PhanBoBLL _pbBLL = new PhanBoBLL();
        private readonly string _mssv;
        private readonly string _hoten;
        private object dgvListHopDong;

        public FormPhanBo(string mssv, string hoten) // Constructor updated to accept parameters
        {
            InitializeComponent(); // Corrected method call
            _mssv = mssv; // Assigning values to fields
            _hoten = hoten;
        }

        private void FormPhanBo_Load(object sender, EventArgs e) // Corrected method signature
        {
            txtMSSV.Text = _mssv;
            txtHoTen.Text = _hoten;
            LoadPhong();
            LoadPhanBo();

            // Gắn sự kiện CellClick
            dgvHopDong.CellClick += dgvHopDong_CellClick;
        }

        private void LoadPhong()
        {
            // Lấy danh sách phòng từ BLL
            var tb = _pbBLL.GetAllPhong(); 
            cboMaPhong.DisplayMember = "MaPhong";
            cboMaPhong.ValueMember = "MaPhong";
            cboMaPhong.DataSource = tb;
        }

        private void LoadPhanBo()
        {
            dgvHopDong.DataSource = _pbBLL.GetBySinhVien(_mssv);
        }

        private PhanBoModel GetInput()
        {
            return new PhanBoModel
            {
                MSSV = _mssv,
                MaPhong = cboMaPhong.SelectedValue?.ToString(),
                SoThang = (int)nmuSoThang.Value,
                NgayPhanBo = dtpNgayPhanBo.Value,
                MienTienPhong = chkMienTienPhong.Checked,
                SoDotThu = string.IsNullOrWhiteSpace(txtSoDotThu.Text) ? 1 : int.Parse(txtSoDotThu.Text),
                GhiChu = txtGhiChu.Text.Trim()
            };
        }

        private void btnPhanBo_Click(object sender, EventArgs e) // Corrected method signature
        {
            if (cboMaPhong.SelectedValue == null || string.IsNullOrWhiteSpace(cboMaPhong.SelectedValue.ToString()))
            {
                MessageBox.Show("Vui lòng chọn phòng trước khi phân bổ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (nmuSoThang.Value <= 0)
            {
                MessageBox.Show("Số tháng phải lớn hơn 0!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var pb = new PhanBoModel
            {
                MSSV = _mssv,
                MaPhong = cboMaPhong.SelectedValue.ToString(),
                SoThang = (int)nmuSoThang.Value,
                NgayPhanBo = dtpNgayPhanBo.Value,
                MienTienPhong = chkMienTienPhong.Checked,
                SoDotThu = string.IsNullOrWhiteSpace(txtSoDotThu.Text) ? 1 : int.Parse(txtSoDotThu.Text),
                GhiChu = txtGhiChu.Text.Trim()
            };

            if (_pbBLL.Insert(pb))
            {
                MessageBox.Show("Phân bổ thành công!");
                LoadPhanBo();
            }
            else
            {
                MessageBox.Show("Phân bổ thất bại!");
            }
        }

        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            this.Close(); // đóng form, quay lại FormQLSV
        }

        private void btnXuatHD_Click(object sender, EventArgs e)
        {
            if (dgvHopDong is DataGridView dgv && dgv.CurrentRow != null)
            {
                // Lấy DataRow từ dòng hiện tại
                var r = ((DataRowView)dgv.CurrentRow.DataBoundItem).Row;

                using (SaveFileDialog sfd = new SaveFileDialog { Filter = "PDF|*.pdf" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        // Gọi ExportHopDongPDF với DataRow hiện tại
                        HopDongExporter.ExportHopDongPDF(r, sfd.FileName);
                        MessageBox.Show("Xuất hợp đồng PDF thành công!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn dòng để xuất hợp đồng!");
            }
        }

        private void dgvHopDong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // bỏ qua tiêu đề

            DataGridViewRow row = dgvHopDong.Rows[e.RowIndex];

            cboMaPhong.SelectedValue = row.Cells["MaPhong"].Value?.ToString();
            nmuSoThang.Value = Convert.ToInt32(row.Cells["SoThang"].Value);
            dtpNgayPhanBo.Value = Convert.ToDateTime(row.Cells["NgayPhanBo"].Value);
            chkMienTienPhong.Checked = Convert.ToBoolean(row.Cells["MienTienPhong"].Value);
            txtSoDotThu.Text = row.Cells["SoDotThu"].Value?.ToString();
            txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString();
        }
    }
}
