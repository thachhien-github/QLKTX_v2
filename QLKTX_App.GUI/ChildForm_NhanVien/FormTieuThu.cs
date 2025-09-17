using QLKTX_App.BLL;
using QLKTX_App.DAL;
using QLKTX_App.DTO;
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

namespace QLKTX_App.ChildForm_NhanVien
{
    public partial class FormTieuThu : Form
    {
        private readonly ChiSoBLL _bll = new ChiSoBLL();

        public FormTieuThu()
        {
            InitializeComponent();
        }

        private void FormTieuThu_Load(object sender, EventArgs e)
        {
            LoadPhong();
            LoadThang();
            LoadNam();
            LoadData();

            cboPhong.SelectedIndexChanged += cboPhong_SelectedIndexChanged;
        }

        private void LoadPhong()
        {
            try
            {
                // load phòng trực tiếp từ DB
                string sql = "SELECT MaPhong FROM Phong";
                DBHelper db = new DBHelper();
                DataTable dt = db.ExecuteQuery(sql, false);

                cboPhong.DataSource = dt;
                cboPhong.DisplayMember = "MaPhong";
                cboPhong.ValueMember = "MaPhong";
                cboPhong.SelectedIndex = -1; // chưa chọn mặc định
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách phòng: " + ex.Message);
            }
        }

        private void LoadThang()
        {
            cboThang.Items.Clear();
            for (int i = 1; i <= 12; i++)
                cboThang.Items.Add(i);
            cboThang.SelectedItem = DateTime.Now.Month;
        }

        private void LoadNam()
        {
            cboNam.Items.Clear();
            for (int i = DateTime.Now.Year - 5; i <= DateTime.Now.Year + 1; i++)
                cboNam.Items.Add(i);
            cboNam.SelectedItem = DateTime.Now.Year;
        }

        private void LoadData()
        {
            dgvListChiSo.DataSource = _bll.GetAll();
            dgvListChiSo.ClearSelection();

            // ✅ Font to rõ ràng hơn
            dgvListChiSo.DefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            dgvListChiSo.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Regular);

            if (dgvListChiSo.Columns.Count > 0)
            {
                dgvListChiSo.Columns["MaPhong"].HeaderText = "Mã phòng";
                dgvListChiSo.Columns["Thang"].HeaderText = "Tháng";
                dgvListChiSo.Columns["Nam"].HeaderText = "Năm";
                dgvListChiSo.Columns["DienCu"].HeaderText = "Điện cũ";
                dgvListChiSo.Columns["DienMoi"].HeaderText = "Điện mới";
                dgvListChiSo.Columns["DienTieuThu"].HeaderText = "Điện tiêu thụ (kWh)";
                dgvListChiSo.Columns["NuocCu"].HeaderText = "Nước cũ";
                dgvListChiSo.Columns["NuocMoi"].HeaderText = "Nước mới";
                dgvListChiSo.Columns["NuocTieuThu"].HeaderText = "Nước tiêu thụ (m³)";

                // ✅ căn phải & format số
                string[] numberCols = { "DienCu", "DienMoi", "DienTieuThu", "NuocCu", "NuocMoi", "NuocTieuThu" };
                foreach (string col in numberCols)
                {
                    dgvListChiSo.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvListChiSo.Columns[col].DefaultCellStyle.Format = "N0";
                }
            }
        }

        private ChiSoModel GetModelFromForm()
        {
            if (cboPhong.SelectedValue == null)
                throw new Exception("Bạn chưa chọn phòng!");

            if (cboThang.SelectedItem == null)
                throw new Exception("Bạn chưa chọn tháng!");

            if (cboNam.SelectedItem == null)
                throw new Exception("Bạn chưa chọn năm!");

            return new ChiSoModel
            {
                MaPhong = cboPhong.SelectedValue.ToString(),
                Thang = Convert.ToInt32(cboThang.SelectedItem),
                Nam = Convert.ToInt32(cboNam.SelectedItem),
                DienCu = string.IsNullOrEmpty(txtDienCu.Text) ? 0 : int.Parse(txtDienCu.Text),
                DienMoi = string.IsNullOrEmpty(txtDienMoi.Text) ? 0 : int.Parse(txtDienMoi.Text),
                NuocCu = string.IsNullOrEmpty(txtNuocCu.Text) ? 0 : int.Parse(txtNuocCu.Text),
                NuocMoi = string.IsNullOrEmpty(txtNuocMoi.Text) ? 0 : int.Parse(txtNuocMoi.Text),
            };
        }

        private void SetFormFromModel(ChiSoModel cs)
        {
            cboPhong.SelectedValue = cs.MaPhong;
            cboThang.SelectedItem = cs.Thang;
            cboNam.SelectedItem = cs.Nam;
            txtDienCu.Text = cs.DienCu.ToString();
            txtDienMoi.Text = cs.DienMoi.ToString();
            txtNuocCu.Text = cs.NuocCu.ToString();
            txtNuocMoi.Text = cs.NuocMoi.ToString();
            txtDienTT.Text = cs.DienTieuThu.ToString();
            txtNuocTT.Text = cs.NuocTieuThu.ToString();
        }


        private void dgvListChiSo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvListChiSo.Rows[e.RowIndex];
                var cs = new ChiSoModel
                {
                    MaPhong = row.Cells["MaPhong"].Value.ToString(),
                    Thang = Convert.ToInt32(row.Cells["Thang"].Value),
                    Nam = Convert.ToInt32(row.Cells["Nam"].Value),
                    DienCu = Convert.ToInt32(row.Cells["DienCu"].Value),
                    DienMoi = Convert.ToInt32(row.Cells["DienMoi"].Value),
                    DienTieuThu = Convert.ToInt32(row.Cells["DienTieuThu"].Value),
                    NuocCu = Convert.ToInt32(row.Cells["NuocCu"].Value),
                    NuocMoi = Convert.ToInt32(row.Cells["NuocMoi"].Value),
                    NuocTieuThu = Convert.ToInt32(row.Cells["NuocTieuThu"].Value)
                };
                SetFormFromModel(cs);
            }
        }

        private void btnTinh_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtDienCu.Text, out int dienCu) &&
                int.TryParse(txtDienMoi.Text, out int dienMoi))
            {
                txtDienTT.Text = (dienMoi - dienCu).ToString();
            }

            if (int.TryParse(txtNuocCu.Text, out int nuocCu) &&
                int.TryParse(txtNuocMoi.Text, out int nuocMoi))
            {
                txtNuocTT.Text = (nuocMoi - nuocCu).ToString();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                var cs = GetModelFromForm();
                _bll.Them(cs);
                MessageBox.Show("Thêm chỉ số thành công!");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                var cs = GetModelFromForm();
                _bll.Sua(cs);
                MessageBox.Show("Cập nhật chỉ số thành công!");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (cboPhong.SelectedValue == null || cboThang.SelectedItem == null || cboNam.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn đầy đủ thông tin!");
                return;
            }

            try
            {
                string maPhong = cboPhong.SelectedValue.ToString();
                int thang = Convert.ToInt32(cboThang.SelectedItem);
                int nam = Convert.ToInt32(cboNam.SelectedItem);

                _bll.Xoa(maPhong, thang, nam);
                MessageBox.Show("Xóa thành công!");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnTiep_Click(object sender, EventArgs e)
        {
            txtDienCu.Clear();
            txtDienMoi.Clear();
            txtNuocCu.Clear();
            txtNuocMoi.Clear();
            txtDienTT.Clear();
            txtNuocTT.Clear();
            cboPhong.SelectedIndex = -1;

            // Gán tháng năm hiện tại
            int thang = DateTime.Now.Month;
            int nam = DateTime.Now.Year;

            cboThang.SelectedItem = thang;
            cboNam.SelectedItem = nam;

            dgvListChiSo.ClearSelection();

            txtDienCu.Focus();
        }

        private void cboPhong_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPhong.SelectedValue == null) return;

            string maPhong = cboPhong.SelectedValue.ToString();
            var csGanNhat = _bll.GetChiSoGanNhat(maPhong);

            if (csGanNhat != null)
            {
                // Gán chỉ số cũ = chỉ số mới của tháng trước
                txtDienCu.Text = csGanNhat.DienMoi.ToString();
                txtNuocCu.Text = csGanNhat.NuocMoi.ToString();

                // Xóa dữ liệu mới để người dùng nhập
                txtDienMoi.Clear();
                txtNuocMoi.Clear();
                txtDienTT.Clear();
                txtNuocTT.Clear();
            }
            else
            {
                // Nếu phòng chưa có dữ liệu -> set = 0
                txtDienCu.Text = "0";
                txtNuocCu.Text = "0";
                txtDienMoi.Clear();
                txtNuocMoi.Clear();
                txtDienTT.Clear();
                txtNuocTT.Clear();
            }
        }
    }
}
