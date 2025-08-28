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
            var dt = _pbBLL.GetBySinhVien(_mssv);

            if (dt == null || dt.Rows.Count == 0)
            {
                dgvHopDong.DataSource = null;
                return;
            }

            bool hetHan = true;

            foreach (DataRow row in dt.Rows)
            {
                DateTime ngayPB = row.Field<DateTime>("NgayPhanBo");
                int soThang = row.Field<int>("SoThang");
                DateTime ngayHetHan = ngayPB.AddMonths(soThang);

                if (ngayHetHan > DateTime.Now) // ✅ còn hiệu lực
                {
                    hetHan = false;
                    break;
                }
            }

            if (hetHan)
            {
                // ❌ tất cả hợp đồng của MSSV này đều hết hạn
                if (_pbBLL.DeleteByMSSV(_mssv))
                {
                    MessageBox.Show("Hợp đồng đã hết hạn, phân bổ của sinh viên đã được xóa!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                dgvHopDong.DataSource = null;
            }
            else
            {
                // ✅ chỉ hiển thị hợp đồng còn hạn
                var dtConHan = dt.AsEnumerable()
                    .Where(r => r.Field<DateTime>("NgayPhanBo").AddMonths(r.Field<int>("SoThang")) > DateTime.Now)
                    .CopyToDataTable();

                dgvHopDong.DataSource = dtConHan;
            }
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

        private void dgvHopDong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // bỏ qua tiêu đề

            DataGridViewRow row = dgvHopDong.Rows[e.RowIndex];

            // Mã phòng
            if (row.Cells["MaPhong"].Value != null)
                cboMaPhong.SelectedValue = row.Cells["MaPhong"].Value.ToString();

            // Số tháng
            if (row.Cells["SoThang"].Value != null &&
                int.TryParse(row.Cells["SoThang"].Value.ToString(), out int soThang))
                nmuSoThang.Value = soThang;
            else
                nmuSoThang.Value = nmuSoThang.Minimum;

            // Ngày phân bổ
            if (row.Cells["NgayPhanBo"].Value != null &&
                DateTime.TryParse(row.Cells["NgayPhanBo"].Value.ToString(), out DateTime ngayPB))
                dtpNgayPhanBo.Value = ngayPB;
            else
                dtpNgayPhanBo.Value = DateTime.Now;

            // Miễn tiền phòng
            if (row.Cells["MienTienPhong"].Value != null)
            {
                chkMienTienPhong.Checked = row.Cells["MienTienPhong"].Value.ToString() == "1";
            }
            else
            {
                chkMienTienPhong.Checked = false;
            }

            // Số đợt thu
            txtSoDotThu.Text = row.Cells["SoDotThu"].Value?.ToString() ?? "";

            // Ghi chú
            txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString() ?? "";
        }
    }
}
