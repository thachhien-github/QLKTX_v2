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

        public FormPhanBo(string mssv, string hoten) // Constructor updated to accept parameters
        {
            InitializeComponent(); // Corrected method call
            _mssv = mssv;
            _hoten = hoten;
        }

        private void FormPhanBo_Load(object sender, EventArgs e) // Corrected method signature
        {
            txtMSSV.Text = _mssv;
            txtHoTen.Text = _hoten;
            LoadPhong();
            LoadPhanBo();

            // Gắn sự kiện CellClick (nếu chưa gắn trong Designer)
            dgvHopDong.CellClick -= dgvHopDong_CellClick;
            dgvHopDong.CellClick += dgvHopDong_CellClick;

            // Mặc định ngày phân bổ: 1/8 năm hiện tại
            dtpNgayPhanBo.Value = new DateTime(DateTime.Now.Year, 8, 1);

            // Tự tính số tháng còn lại theo khóa (2 chữ số đầu MSSV)
            if (!string.IsNullOrWhiteSpace(_mssv) && _mssv.Length >= 2)
            {
                if (int.TryParse(_mssv.Substring(0, 2), out int khoa2))
                {
                    int khoa = 2000 + khoa2;
                    int namHienTai = DateTime.Now.Year;
                    int namHocThu = namHienTai - khoa + 1;

                    if (namHocThu == 1 || namHocThu == 2)
                        nmuSoThang.Value = 10;
                    else if (namHocThu == 3)
                        nmuSoThang.Value = 5;
                    else
                        nmuSoThang.Value = 0;
                }
            }
        }

        private void LoadPhong()
        {
            var tb = _phongBLL.GetAll();
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

            // Lọc chỉ hợp đồng còn hiệu lực
            try
            {
                var rowsConHan = dt.AsEnumerable()
                    .Where(r => r.Field<DateTime>("NgayPhanBo").AddMonths(r.Field<int>("SoThang")) > DateTime.Now);

                if (!rowsConHan.Any())
                {
                    // Toàn bộ hết hạn => xóa phân bổ (logic bạn đã có)
                    if (_pbBLL.DeleteByMSSV(_mssv))
                    {
                        MessageBox.Show("Hợp đồng đã hết hạn, phân bổ của sinh viên đã được xóa!",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    dgvHopDong.DataSource = null;
                }
                else
                {
                    dgvHopDong.DataSource = rowsConHan.CopyToDataTable();
                }
            }
            catch (Exception ex)
            {
                // phòng trường hợp CopyToDataTable lỗi
                dgvHopDong.DataSource = dt;
                System.Diagnostics.Debug.WriteLine("LoadPhanBo error: " + ex.Message);
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

            // Kiểm tra số tháng
            if (nmuSoThang.Value <= 0)
            {
                string khoa = _mssv.Substring(0, 2);
                MessageBox.Show($"Sinh viên khóa C{khoa} đã hết hạn ở ký túc xá.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Kiểm tra sinh viên đã có hợp đồng còn hiệu lực chưa
            if (_pbBLL.CheckDangO(_mssv))
            {
                MessageBox.Show("Sinh viên này đang có hợp đồng còn hiệu lực, không thể phân bổ thêm!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maPhong = cboMaPhong.SelectedValue.ToString();

            // Lấy số lượng tối đa và số người đang ở (dựa trên phân bổ còn hiệu lực)
            int soLuongToiDa = _phongBLL.GetSoLuongToiDa(maPhong);
            int dangO = _pbBLL.CountActiveByRoom(maPhong);

            // Debug log — giúp kiểm tra khi test
            System.Diagnostics.Debug.WriteLine($"[PhanBo] MaPhong={maPhong}, DangO={dangO}, SoLuongToiDa={soLuongToiDa}");

            if (dangO >= soLuongToiDa)
            {
                MessageBox.Show("Phòng đã đầy, không thể phân bổ thêm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // Cập nhật trạng thái phòng để giao diện luôn chính xác
                try { _phongBLL.RefreshTrangThai(maPhong); } catch { /* ignore nếu không có method */ }
                return;
            }

            var pb = GetInput();

            try
            {
                // Insert trả về int (stored proc) — 1 là thành công
                int res = _pbBLL.Insert(pb);
                if (res > 0)
                {
                    MessageBox.Show("Phân bổ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPhanBo();
                    // cập nhật trạng thái phòng (cả phòng mới và có thể phòng cũ)
                    try { _phongBLL.RefreshTrangThai(maPhong); } catch { /* ignore nếu không có method */ }
                }
                else
                {
                    MessageBox.Show("Phân bổ thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException sqlEx)
            {
                // Nếu SP RAISERROR thì SqlException sẽ ném về
                MessageBox.Show(sqlEx.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // refresh trạng thái phòng để sync
                try { _phongBLL.RefreshTrangThai(maPhong); } catch { }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            this.Close(); // đóng form, quay lại FormQLSV
        } 

        private void dgvHopDong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

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

            // Miễn tiền phòng (hỗ trợ cả "1"/"0" và "True"/"False")
            if (row.Cells["MienTienPhong"].Value != null)
            {
                var v = row.Cells["MienTienPhong"].Value.ToString().Trim().ToLower();
                chkMienTienPhong.Checked = (v == "1" || v == "true" || v == "x" || v == "yes");
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

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvHopDong.CurrentRow == null)
                {
                    MessageBox.Show("Vui lòng chọn một hợp đồng để sửa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string maPhongCu = dgvHopDong.CurrentRow.Cells["MaPhong"].Value.ToString();
                var pb = GetInput();

                bool result = false;

                // Nếu người dùng chọn phòng mới khác phòng cũ → kiểm tra số lượng trước
                if (pb.MaPhong != maPhongCu)
                {
                    int soLuongToiDa = _phongBLL.GetAll().Select($"MaPhong='{pb.MaPhong}'")[0].Field<int>("SoLuongToiDa");
                    int dangO = _svBLL.GetByPhong(pb.MaPhong).Rows.Count;

                    if (dangO >= soLuongToiDa)
                    {
                        MessageBox.Show("❌ Phòng mới đã đầy, không thể chuyển sinh viên vào!",
                            "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Nếu phòng còn chỗ → cho phép chuyển
                    result = _pbBLL.ChuyenPhong(pb, maPhongCu);
                }
                else
                {
                    // Nếu không đổi phòng → vẫn có thể sửa thông tin khác
                    result = _pbBLL.Update(pb);
                }

                if (result)
                {
                    MessageBox.Show("✅ Cập nhật phân bổ thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPhanBo();
                    try { _phongBLL.RefreshTrangThai(maPhongCu); } catch { }
                    try { _phongBLL.RefreshTrangThai(pb.MaPhong); } catch { }
                }
                else
                {
                    MessageBox.Show("❌ Cập nhật thất bại!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvHopDong.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một hợp đồng để xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string mssv = dgvHopDong.CurrentRow.Cells["MSSV"].Value.ToString();
            string maPhong = dgvHopDong.CurrentRow.Cells["MaPhong"].Value.ToString();

            var confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa phân bổ của SV {mssv} tại phòng {maPhong}?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    if (_pbBLL.Delete(mssv, maPhong))
                    {
                        MessageBox.Show("Xóa phân bổ thành công!");
                        LoadPhanBo();
                        try { _phongBLL.RefreshTrangThai(maPhong); } catch { }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
