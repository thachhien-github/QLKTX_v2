using QLKTX_App.BLL;
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
        }

        private void LoadPhong()
        {
            // TODO: load phòng từ DB (bạn có thể viết DAL riêng hoặc dùng chung)
            // ví dụ: select MaPhong from Phong
            cboPhong.Items.Clear();
            cboPhong.Items.AddRange(new string[] { "P101", "P102", "P201" }); // demo
        }

        private void LoadThang()
        {
            cboThang.Items.Clear();
            for (int i = 1; i <= 12; i++)
                cboThang.Items.Add(i);
        }

        private void LoadNam()
        {
            cboNam.Items.Clear();
            for (int i = DateTime.Now.Year - 5; i <= DateTime.Now.Year + 1; i++)
                cboNam.Items.Add(i);
        }

        private void LoadData()
        {
            dgvListChiSo.DataSource = _bll.GetAll();
        }

        private ChiSoModel GetModelFromForm()
        {
            return new ChiSoModel
            {
                MaPhong = cboPhong.SelectedItem?.ToString(),
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
            cboPhong.SelectedItem = cs.MaPhong;
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
            if (cboPhong.SelectedItem == null || cboThang.SelectedItem == null || cboNam.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn đầy đủ thông tin!");
                return;
            }

            try
            {
                string maPhong = cboPhong.SelectedItem.ToString();
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
    }
}
