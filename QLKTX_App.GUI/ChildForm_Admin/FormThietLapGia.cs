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

namespace QLKTX_App.ChildForm_Admin
{
    public partial class FormThietLapGia : Form
    {
        private readonly LoaiPhongBLL _lpBLL = new LoaiPhongBLL();
        private readonly LoaiXeBLL _lxBLL = new LoaiXeBLL();
        private readonly GiaDienNuocBLL _gdnBLL = new GiaDienNuocBLL();

        public FormThietLapGia()
        {
            InitializeComponent();
        }

        private void FormThietLapGia_Load(object sender, EventArgs e)
        {
            LoadGiaPhong();
            LoadPhiGiuXe();
            LoadGiaDienNuoc();
            WireEvents();
        }

        private void WireEvents()
        {
            dgvGiaPhong.SelectionChanged += (s, e) => BindGiaPhongFromGrid();
            dgvPhiGiuXe.SelectionChanged += (s, e) => BindLoaiXeFromGrid();
            dgvGiaDienNuoc.SelectionChanged += (s, e) => BindGiaDienNuocFromGrid();
        }

        #region Giá phòng
        private void LoadGiaPhong()
        {
            var tb = _lpBLL.GetAll();
            dgvGiaPhong.DataSource = tb;
            dgvGiaPhong.ClearSelection();

            // 🔽 Fill combobox
            cboMaLoaiPhong.DropDownStyle = ComboBoxStyle.DropDown; // cho phép nhập thêm
            cboMaLoaiPhong.DisplayMember = "MaLoai";
            cboMaLoaiPhong.ValueMember = "MaLoai";
            cboMaLoaiPhong.DataSource = tb;
        }

        private void BindGiaPhongFromGrid()
        {
            if (dgvGiaPhong.CurrentRow == null) return;
            var r = dgvGiaPhong.CurrentRow;
            cboMaLoaiPhong.Text = r.Cells["MaLoai"].Value?.ToString();
            txtTenLoaiPhong.Text = r.Cells["TenLoai"].Value?.ToString();
            txtDGPhong.Text = r.Cells["GiaPhong"].Value?.ToString();
            txtSucChua.Text = r.Cells["SucChua"].Value?.ToString();
        }

        private bool TryGetGiaPhongInput(out LoaiPhongModel m, bool isUpdate = false)
        {
            m = null;
            string ma = cboMaLoaiPhong.Text.Trim();
            string ten = txtTenLoaiPhong.Text.Trim();
            string giaText = txtDGPhong.Text.Trim();
            string sucChuaText = txtSucChua.Text.Trim();

            if (string.IsNullOrWhiteSpace(ma))
            {
                MessageBox.Show("Mã loại phòng không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(ten))
            {
                MessageBox.Show("Tên loại phòng không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!decimal.TryParse(giaText, out decimal gia) || gia < 0)
            {
                MessageBox.Show("Đơn giá phòng không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!int.TryParse(sucChuaText, out int sucChua) || sucChua <= 0)
            {
                MessageBox.Show("Sức chứa phải là số nguyên dương!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            m = new LoaiPhongModel { MaLoai = ma, TenLoai = ten, GiaPhong = gia, SucChua = sucChua };
            return true;
        }


        private void btnLuuGiaPhong_Click(object sender, EventArgs e)
        {
            bool isUpdate = dgvGiaPhong.CurrentRow != null;
            if (!TryGetGiaPhongInput(out var m, isUpdate)) return;

            bool ok = isUpdate ? _lpBLL.Update(m) : _lpBLL.Insert(m);
            MessageBox.Show(ok
                ? (isUpdate ? "Cập nhật thành công!" : "Thêm thành công!")
                : "Lưu thất bại!",
                ok ? "Thông báo" : "Lỗi",
                MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (ok) LoadGiaPhong();
        }

        private void btnXoaPhong_Click(object sender, EventArgs e)
        {
            string ma = cboMaLoaiPhong.Text.Trim();
            if (string.IsNullOrWhiteSpace(ma))
            {
                MessageBox.Show("Vui lòng chọn Mã loại để xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show($"Bạn chắc muốn xóa loại phòng [{ma}] ?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bool ok = _lpBLL.Delete(ma);
                MessageBox.Show(ok ? "Xóa thành công!" : "Xóa thất bại! (Có thể đang được tham chiếu)",
                    ok ? "Thông báo" : "Lỗi", MessageBoxButtons.OK,
                    ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                if (ok) LoadGiaPhong();
            }
        }

        private void btnlammoiPhong_Click(object sender, EventArgs e)
        {
            cboMaLoaiPhong.SelectedIndex = -1;
            cboMaLoaiPhong.Text = "";
            txtTenLoaiPhong.Clear();
            txtDGPhong.Clear();
            txtSucChua.Clear();
            txtTenLoaiPhong.Focus();
        }

        #endregion

        #region Giá điện nước
        private void LoadGiaDienNuoc()
        {
            var g = _gdnBLL.GetCurrent();
            if (g == null) return;

            txtGiaDien.Text = g.GiaDien.ToString("N0");
            txtGiaNuoc.Text = g.GiaNuoc.ToString("N0");

            var tb = new DataTable();
            tb.Columns.Add("GiaDien", typeof(decimal));
            tb.Columns.Add("GiaNuoc", typeof(decimal));
            tb.Rows.Add(g.GiaDien, g.GiaNuoc);
            dgvGiaDienNuoc.DataSource = tb;
            dgvGiaDienNuoc.ClearSelection();

            // ✅ Style bảng
            dgvGiaDienNuoc.DefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            dgvGiaDienNuoc.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Regular);

            if (dgvGiaDienNuoc.Columns.Count > 0)
            {
                dgvGiaDienNuoc.Columns["GiaDien"].HeaderText = "Giá điện (VND/kWh)";
                dgvGiaDienNuoc.Columns["GiaNuoc"].HeaderText = "Giá nước (VND/m³)";

                // ✅ Định dạng số tiền
                dgvGiaDienNuoc.Columns["GiaDien"].DefaultCellStyle.Format = "N0";
                dgvGiaDienNuoc.Columns["GiaNuoc"].DefaultCellStyle.Format = "N0";

                // ✅ Căn phải cho đẹp
                dgvGiaDienNuoc.Columns["GiaDien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvGiaDienNuoc.Columns["GiaNuoc"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            // ✅ Tăng chiều cao dòng
            dgvGiaDienNuoc.RowTemplate.Height = 28;
        }


        private void BindGiaDienNuocFromGrid()
        {
            if (dgvGiaDienNuoc.CurrentRow == null) return;
            var r = dgvGiaDienNuoc.CurrentRow;
            txtGiaDien.Text = r.Cells["GiaDien"].Value?.ToString();
            txtGiaNuoc.Text = r.Cells["GiaNuoc"].Value?.ToString();
        }


        private void btnLuuGiaDN_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtGiaDien.Text.Trim(), out var giaDien) || giaDien < 0)
            {
                MessageBox.Show("Giá điện không hợp lệ!");
                return;
            }
            if (!decimal.TryParse(txtGiaNuoc.Text.Trim(), out var giaNuoc) || giaNuoc < 0)
            {
                MessageBox.Show("Giá nước không hợp lệ!");
                return;
            }

            int row = _gdnBLL.Update(new GiaDienNuocModel { GiaDien = giaDien, GiaNuoc = giaNuoc });
            MessageBox.Show(row > 0 ? "Cập nhật thành công!" : "Cập nhật thất bại!");
            LoadGiaDienNuoc();
        }

        private void btnlammoiDN_Click(object sender, EventArgs e)
        {
            LoadGiaDienNuoc();
        }
        #endregion

        #region Phí giữ xe
        private void LoadPhiGiuXe()
        {
            var tb = _lxBLL.GetAll();
            dgvPhiGiuXe.DataSource = tb;
            dgvPhiGiuXe.ClearSelection();

            // ✅ Style DataGridView
            dgvPhiGiuXe.DefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            dgvPhiGiuXe.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            dgvPhiGiuXe.RowTemplate.Height = 28;

            if (dgvPhiGiuXe.Columns.Contains("MaLoaiXe"))
                dgvPhiGiuXe.Columns["MaLoaiXe"].HeaderText = "Mã loại xe";
            if (dgvPhiGiuXe.Columns.Contains("TenLoai"))
                dgvPhiGiuXe.Columns["TenLoai"].HeaderText = "Tên loại xe";
            if (dgvPhiGiuXe.Columns.Contains("GiaGiuXe"))
            {
                dgvPhiGiuXe.Columns["GiaGiuXe"].HeaderText = "Phí gửi xe (VND/tháng)";
                dgvPhiGiuXe.Columns["GiaGiuXe"].DefaultCellStyle.Format = "N0";
                dgvPhiGiuXe.Columns["GiaGiuXe"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            // ✅ Fill combobox
            var tbCb = tb.Copy();
            var row = tbCb.NewRow();
            row["MaLoaiXe"] = "";
            row["TenLoai"] = "";
            row["GiaGiuXe"] = 0;
            tbCb.Rows.InsertAt(row, 0);

            cboMaLoaiXe.DisplayMember = "MaLoaiXe";
            cboMaLoaiXe.ValueMember = "MaLoaiXe";
            cboMaLoaiXe.DataSource = tbCb;
        }


        private void BindLoaiXeFromGrid()
        {
            if (dgvPhiGiuXe.CurrentRow == null) return;
            var r = dgvPhiGiuXe.CurrentRow;
            cboMaLoaiXe.SelectedValue = r.Cells["MaLoaiXe"].Value?.ToString();
            txtTenLoaiXe.Text = r.Cells["TenLoai"].Value?.ToString();
            txtDGXe.Text = r.Cells["GiaGiuXe"].Value?.ToString();
        }

        private bool TryGetLoaiXeInput(out LoaiXeModel m)
        {
            m = null;
            string ma = (cboMaLoaiXe.SelectedValue ?? cboMaLoaiXe.Text).ToString().Trim();
            string ten = txtTenLoaiXe.Text.Trim();

            if (!decimal.TryParse(txtDGXe.Text.Trim(), out decimal gia) || gia < 0)
            {
                MessageBox.Show("Giá giữ xe không hợp lệ!");
                return false;
            }
            if (string.IsNullOrWhiteSpace(ma) || string.IsNullOrWhiteSpace(ten))
            {
                MessageBox.Show("Mã loại / Tên loại không được trống!");
                return false;
            }

            m = new LoaiXeModel { MaLoaiXe = ma, TenLoai = ten, GiaGiuXe = gia };
            return true;
        }

        private void btnLuuGiaXe_Click(object sender, EventArgs e)
        {
            if (!TryGetLoaiXeInput(out var m)) return;
            int row = _lxBLL.Update(m);
            if (row == 0) row = _lxBLL.Insert(m);

            if (row > 0)
            {
                MessageBox.Show("Lưu thành công!");
                LoadPhiGiuXe();
            }
            else MessageBox.Show("Lưu thất bại!");
        }

        private void btnXoaXe_Click(object sender, EventArgs e)
        {
            string ma = (cboMaLoaiXe.SelectedValue ?? "").ToString();
            if (string.IsNullOrWhiteSpace(ma)) return;

            if (MessageBox.Show($"Xóa loại xe [{ma}] ?", "Xác nhận",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int row = _lxBLL.Delete(ma);
                MessageBox.Show(row > 0 ? "Xóa thành công!" : "Xóa thất bại!");
                LoadPhiGiuXe();
            }
        }

        private void btnlammoiXe_Click(object sender, EventArgs e)
        {
            cboMaLoaiXe.SelectedIndex = -1;
            txtTenLoaiXe.Clear();
            txtDGXe.Clear();
        }
        #endregion
    }
}