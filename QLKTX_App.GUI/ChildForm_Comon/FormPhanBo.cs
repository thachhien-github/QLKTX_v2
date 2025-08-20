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
        private readonly PhongBLL _phongBLL = new PhongBLL();
        private readonly SinhVienBLL _svBLL = new SinhVienBLL();
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
            var tb = _phongBLL.GetAll();   // ✅ Lấy danh sách phòng từ BLL Phong
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
            if (cboMaPhong.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn phòng trước khi phân bổ!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 🔎 Kiểm tra sinh viên đã có hợp đồng còn hiệu lực chưa
            if (_pbBLL.CheckDangO(_mssv))
            {
                MessageBox.Show("Sinh viên này đang có hợp đồng còn hiệu lực, không thể phân bổ thêm!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maPhong = cboMaPhong.SelectedValue.ToString();
            int soLuongToiDa = _phongBLL.GetAll().Select($"MaPhong='{maPhong}'")[0].Field<int>("SoLuongToiDa");
            int dangO = _svBLL.GetByPhong(maPhong).Rows.Count;

            if (dangO >= soLuongToiDa)
            {
                MessageBox.Show("Phòng đã đầy, không thể phân bổ thêm!");
                return;
            }

            var pb = GetInput();

            if (_pbBLL.Insert(pb))
            {
                MessageBox.Show("Phân bổ thành công!");
                LoadPhanBo();
                _phongBLL.CapNhatTrangThai(maPhong);
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
