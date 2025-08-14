using FontAwesome.Sharp;
using QLKTX_App.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Documents;
using System.Windows.Forms;

namespace QLKTX_App
{
    public partial class FormLogin : Form
    {
        private TaiKhoanBLL _bll = new TaiKhoanBLL();

        public FormLogin()
        {
            InitializeComponent();

            //form
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
        }

        private void iconPictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            DataRow acc = _bll.DangNhap(txtUsername.Text.Trim(), txtPassword.Text.Trim());

            if (acc != null)
            {
                if (!(bool)acc["TrangThai"])
                {
                    MessageBox.Show("Tài khoản bị khóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string hoTen = acc["HoTen"] != DBNull.Value ? acc["HoTen"].ToString() : acc["TenDangNhap"].ToString();
                string role = acc["VaiTro"].ToString();

                if (role == "Admin")
                {
                    FormAdmin fm = new FormAdmin(hoTen);
                    this.Hide();
                    fm.FormClosed += (s, args) => this.Show();
                    fm.Show();
                }
                else if (role == "NhanVien")
                {
                    FormNhanVien fm = new FormNhanVien(hoTen);
                    this.Hide();
                    fm.FormClosed += (s, args) => this.Show();
                    fm.Show();
                }
                else
                {
                    MessageBox.Show("Vai trò không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu sai!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        private void chkHienMK_CheckedChanged(object sender, EventArgs e)
        {
            // Nếu được check, hiện mật khẩu
            txtPassword.UseSystemPasswordChar = !chkHienMK.Checked;
        }

        #region di chuyển và bo góc viền form

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panelTop_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void BoGocForm(int radius)
        {
            Rectangle bounds = new Rectangle(0, 0, this.Width, this.Height);
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90); // Top-left
            path.AddArc(bounds.Width - radius, 0, radius, radius, 270, 90); // Top-right
            path.AddArc(bounds.Width - radius, bounds.Height - radius, radius, radius, 0, 90); // Bottom-right
            path.AddArc(0, bounds.Height - radius, radius, radius, 90, 90); // Bottom-left
            path.CloseAllFigures();
            this.Region = new Region(path);
        }

        private void BoGocPanelDangNhap()
        {
            int radius = 30;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(panelLogin.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(panelLogin.Width - radius, panelLogin.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, panelLogin.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            panelLogin.Region = new Region(path);
        }

        private void FormLogin_Shown(object sender, EventArgs e)
        {
            BoGocPanelDangNhap();
            BoGocForm(30);
        }

        private void FormLogin_Resize(object sender, EventArgs e)
        {
            BoGocPanelDangNhap();
            BoGocForm(30);
        }
        #endregion
    }
}
