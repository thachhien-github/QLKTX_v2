
using QLKTX_App.BLL;
using QLKTX_App.DTO;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QLKTX_App
{
    public partial class FormLogin : Form
    {
        private TaiKhoanBLL _bll;
        public TaiKhoanModel TaiKhoanDaDangNhap { get; private set; }
        public static TaiKhoanModel LastLoginTaiKhoan { get; set; }
        public FormLogin()
        {
            InitializeComponent();
            _bll = new TaiKhoanBLL();

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
            string ten = txtUsername.Text.Trim();
            string mk = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(ten) || string.IsNullOrEmpty(mk))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TaiKhoanModel tk = _bll.KiemTraDangNhap(ten, mk);

            if (tk != null)
            {
                if (!tk.TrangThai)
                {
                    MessageBox.Show("Tài khoản đã bị khóa!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show($"Đăng nhập thành công! Xin chào {tk.VaiTro}", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                TaiKhoanDaDangNhap = tk;
                LastLoginTaiKhoan = tk; // gán vào biến static để Program lấy

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu sai!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
