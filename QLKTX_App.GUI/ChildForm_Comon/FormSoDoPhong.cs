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

namespace QLKTX_App.ChildForm_Comon
{
    public partial class FormSoDoPhong : Form
    {
        private readonly TangBLL _tangBLL = new TangBLL();
        private readonly PhongBLL _phongBLL = new PhongBLL();
        private readonly SinhVienBLL _svBLL = new SinhVienBLL();
        public FormSoDoPhong()
        {
            InitializeComponent();
        }

        private void FormSoDoPhong_Load(object sender, EventArgs e)
        {
            // Load combobox tầng
            cboTang.DataSource = _tangBLL.GetAll();
            cboTang.DisplayMember = "TenTang";
            cboTang.ValueMember = "MaTang";
            cboTang.SelectedIndex = -1;

            // Load tất cả phòng khi mở
            LoadPhongAll();
        }

        #region phương thức load dữ liệu
        // Load toàn bộ phòng
        private void LoadPhongAll()
        {
            flpPhong.Controls.Clear();
            DataTable dt = _phongBLL.GetAll();

            HienThiDanhSachPhong(dt);
        }

        // Load phòng theo tầng
        private void LoadPhongByTang(string maTang)
        {
            flpPhong.Controls.Clear();
            DataTable dt = _phongBLL.GetByTang(maTang);

            HienThiDanhSachPhong(dt);
        }

        // Hàm dùng chung để hiển thị danh sách phòng lên flpPhong
        private void HienThiDanhSachPhong(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                string maPhong = row["MaPhong"].ToString();
                int soLuongToiDa = Convert.ToInt32(row["SoLuongToiDa"]);

                Button btn = new Button
                {
                    Text = $"{maPhong}\n(Sức chứa: {soLuongToiDa})",
                    Tag = maPhong,
                    Width = 160,
                    Height = 120,
                    Margin = new Padding(8),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = Color.White,
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.Gray
                };

                // Bỏ viền khi hover
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = Color.DimGray;
                btn.FlatAppearance.MouseDownBackColor = Color.DarkGray;

                // Bo góc
                btn.Region = System.Drawing.Region.FromHrgn(
                    CreateRoundRectRgn(0, 0, btn.Width, btn.Height, 20, 20)
                );

                string trangThai = row["TrangThai"].ToString();
                if (trangThai == "Trống")
                    btn.BackColor = Color.LightGreen;
                else if (trangThai == "Đầy")
                    btn.BackColor = Color.DarkOrange;
                else
                    btn.BackColor = Color.Turquoise;

                btn.Click += BtnPhong_Click;
                flpPhong.Controls.Add(btn);
            }
        }




        #endregion
        // Sự kiện chọn phòng
        private void BtnPhong_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            string maPhong = btn.Tag.ToString();

            dgvSinhVien.DataSource = _svBLL.GetByPhong(maPhong);
            dgvSinhVien.Visible = true;
        }


        private void cboTang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTang.SelectedIndex == -1) return;

            string maTang = cboTang.SelectedValue.ToString();
            LoadPhongByTang(maTang);
        }

        #region tiện ích bo góc nút
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        #endregion
    }
}