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

            // fill combobox
            var tbCb = tb.Copy();
            var row = tbCb.NewRow();
            row["MaLoai"] = "";
            row["TenLoai"] = "";
            row["GiaPhong"] = 0;
            row["SucChua"] = 0;
            tbCb.Rows.InsertAt(row, 0);

            cboMaLoaiPhong.DisplayMember = "MaLoai";
            cboMaLoaiPhong.ValueMember = "MaLoai";
            cboMaLoaiPhong.DataSource = tbCb;
        }

        private void BindGiaPhongFromGrid()
        {
            if (dgvGiaPhong.CurrentRow == null) return;
            var r = dgvGiaPhong.CurrentRow;
            cboMaLoaiPhong.SelectedValue = r.Cells["MaLoai"].Value?.ToString();
            txtTenLoaiPhong.Text = r.Cells["TenLoai"].Value?.ToString();
            txtDGPhong.Text = r.Cells["GiaPhong"].Value?.ToString();
            txtSucChua.Text = r.Cells["SucChua"].Value?.ToString();
        }

        private bool TryGetGiaPhongInput(out LoaiPhongModel m, bool allowEmptyMa = true)
        {
            m = null;
            string ma = (cboMaLoaiPhong.SelectedValue ?? "").ToString().Trim();
            string ten = txtTenLoaiPhong.Text.Trim();
            string giaText = txtDGPhong.Text.Trim();
            string sucChuaText = txtSucChua.Text.Trim();

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

            if (string.IsNullOrEmpty(ma))
            {
                if (!allowEmptyMa)
                {
                    MessageBox.Show("Vui lòng chọn Mã loại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                ma = Microsoft.VisualBasic.Interaction.InputBox("Nhập mã loại phòng (ví dụ: LP1):", "Thêm loại phòng");
                if (string.IsNullOrWhiteSpace(ma))
                    return false;
            }

            if (string.IsNullOrWhiteSpace(ten))
            {
                MessageBox.Show("Tên loại phòng không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            m = new LoaiPhongModel { MaLoai = ma, TenLoai = ten, GiaPhong = gia, SucChua = sucChua };
            return true;
        }


        private void btnLuuGiaPhong_Click(object sender, EventArgs e)
        {
            bool isUpdate = !string.IsNullOrWhiteSpace((cboMaLoaiPhong.SelectedValue ?? "").ToString());

            if (!TryGetGiaPhongInput(out var m, allowEmptyMa: !isUpdate)) return;

            bool ok = isUpdate ? _lpBLL.Update(m) : _lpBLL.Insert(m);
            if (ok)
            {
                MessageBox.Show(isUpdate ? "Cập nhật loại phòng thành công!" : "Thêm loại phòng thành công!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadGiaPhong();
            }
            else
            {
                MessageBox.Show("Lưu thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaPhong_Click(object sender, EventArgs e)
        {
            string ma = (cboMaLoaiPhong.SelectedValue ?? "").ToString();
            if (string.IsNullOrWhiteSpace(ma))
            {
                MessageBox.Show("Vui lòng chọn Mã loại để xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show($"Bạn chắc muốn xóa loại phòng [{ma}] ?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bool ok = _lpBLL.Delete(ma);
                if (ok)
                {
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadGiaPhong();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại! (Có thể đang được tham chiếu)", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnlammoiPhong_Click(object sender, EventArgs e)
        {
            cboMaLoaiPhong.SelectedIndex = 0;
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

            txtGiaDien.Text = g.GiaDien.ToString();
            txtGiaNuoc.Text = g.GiaNuoc.ToString();

            var tb = new DataTable();
            tb.Columns.Add("GiaDien", typeof(decimal));
            tb.Columns.Add("GiaNuoc", typeof(decimal));
            tb.Rows.Add(g.GiaDien, g.GiaNuoc);
            dgvGiaDienNuoc.DataSource = tb;
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

            cboMaLoaiXe.DisplayMember = "MaLoaiXe";
            cboMaLoaiXe.ValueMember = "MaLoaiXe";
            cboMaLoaiXe.DataSource = tb.Copy();
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