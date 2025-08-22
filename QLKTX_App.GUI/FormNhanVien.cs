using FontAwesome.Sharp;
using QLKTX_App.ChildForm_Comon;
using QLKTX_App.ChildForm_NhanVien;
using QLKTX_App.DTO;
using QLKTX_App.GUI.ChildForm_Comon;
using QLKTX_App.GUI.ChildForm_NhanVien;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QLKTX_App
{
    public partial class FormNhanVien : Form
    {
        //fields
        private IconButton currentBtn;
        private Form currentChildForm;


        private string hoTen;
        private TaiKhoanModel tk;

        public FormNhanVien(TaiKhoanModel tk)
        {
            InitializeComponent();
            this.tk = tk;
            this.hoTen = tk.HoTen;

            // Hiển thị lời chào
            lblChao.Text = GetLoiChaoTheoThoiGian(tk.HoTen);
            customizeDesign();

            //form
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
        }

        #region Properties
        private struct RGBColors
        {
            public static Color color6 = Color.FromArgb(26, 19, 73);
        }

        private void ActivateButton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                DisableButton();
                //Button 
                currentBtn = (IconButton)senderBtn;
                currentBtn.BackColor = Color.LightGray;
                currentBtn.ForeColor = color;
                currentBtn.TextAlign = ContentAlignment.MiddleCenter;
                currentBtn.IconColor = color;
                currentBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
                currentBtn.ImageAlign = ContentAlignment.MiddleRight;
                //Icon Current Child Form
                iconCurrentChildForm.IconChar = currentBtn.IconChar;
                iconCurrentChildForm.IconColor = color;
            }
        }

        private void DisableButton()
        {
            if (currentBtn != null)
            {
                currentBtn.BackColor = Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(19)))), ((int)(((byte)(73)))));
                currentBtn.ForeColor = Color.White;
                currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentBtn.IconColor = Color.White;
                currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentBtn.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }

        private void OpenChildForm(Form childForm)
        {
            if (currentChildForm != null)
            {
                //Mở Form khi
                currentChildForm.Close();
            }
            currentChildForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelDesktop.Controls.Add(childForm);
            panelDesktop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            lblTitleChildForm.Text = childForm.Text;
        }

        private void Reset()
        {
            DisableButton();
            iconCurrentChildForm.IconChar = IconChar.HomeLg;
            iconCurrentChildForm.IconColor = iconCurrentChildForm.IconColor;
            lblTitleChildForm.Text = "Home";
        }

        private string GetLoiChaoTheoThoiGian(string hoTen)
        {
            string loiChao = "Chào " + hoTen;
            DateTime now = DateTime.Now;
            if (now.Hour < 12)
            {
                loiChao = "Chào buổi sáng, " + hoTen;
            }
            else if (now.Hour < 18)
            {
                loiChao = "Chào buổi chiều, " + hoTen;
            }
            else
            {
                loiChao = "Chào buổi tối, " + hoTen;
            }
            return loiChao;
        }

        #endregion

        private void FormNhanVien_Load(object sender, EventArgs e)
        {
            lblChao.Text = GetLoiChaoTheoThoiGian(hoTen);

            btnMaximize.IconChar = (WindowState == FormWindowState.Maximized)
                ? IconChar.WindowRestore
                : IconChar.WindowMaximize;
        }
        
        private void btnLogOut_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color6);

            if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?",
         "Đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                FormLogin.LastLoginTaiKhoan = null; // báo cho Program là logout
                this.Close(); // đóng form chính → Program quay lại login
            }
        }

        #region nút điều khiển form
        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát ứng dụng?", "Xác nhận thoát",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                FormBorderStyle = FormBorderStyle.None;
                MaximumSize = Screen.FromHandle(this.Handle).WorkingArea.Size;
                Location = Screen.FromHandle(this.Handle).WorkingArea.Location;
                WindowState = FormWindowState.Maximized;

                btnMaximize.IconChar = FontAwesome.Sharp.IconChar.WindowRestore;
            }
            else
            {
                MaximumSize = Size.Empty; // bỏ giới hạn
                WindowState = FormWindowState.Normal;
                btnMaximize.IconChar = FontAwesome.Sharp.IconChar.WindowMaximize;
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        #endregion

        #region sự kiện kéo thả form
        private void panelTop_MouseDown_1(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        //Drag Form
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        #endregion

        #region ẩn hiện menu

        private void customizeDesign()
        {
            panelPhong.Visible = false;
            panelHoaDon.Visible = false;
        }

        private void hideMenu()
        {
            if (panelPhong.Visible == true)
                panelPhong.Visible = false;

            if (panelHoaDon.Visible == true)
                panelHoaDon.Visible = false;
        }

        private void showMenu(Panel Menu)
        {
            if (Menu.Visible == false)
            {
                hideMenu();
                Menu.Visible = true;
            }
            else
            {
                Menu.Visible = false;
            }
        }
        #endregion

        #region sự kiện nút điều hướng menu
        //menu sinh viên
        private void btnSinhVien_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color6);
            OpenChildForm(new FormQLSV());
        }

        //menu phòng
        private void btnPhong_Click(object sender, EventArgs e)
        {
            showMenu(panelPhong);
        }

        private void btnSoDoPhong_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color6);
            OpenChildForm(new FormSoDoPhong());
        }

        private void btnThemPhong_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color6);
            OpenChildForm(new FormThemPhong());
        }

        //menu phương tiện
        private void btnPhuongTien_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color6);
            OpenChildForm(new FormPhuongTien());
        }

        //menu chỉ số điện nước
        private void btnCSdiennuoc_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color6);
            OpenChildForm(new FormTieuThu());
        }

        //menu hóa đơn
        private void btnQLHD_Click(object sender, EventArgs e)
        {
            showMenu(panelHoaDon);       
        }

        //menu thống kê
        private void btnThongKe_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color6);
            OpenChildForm(new FormThongKeDT());
        }

        //Click logo reset SideBar
        private void btnHome_Click_1(object sender, EventArgs e)
        {
            currentChildForm.Close();
            Reset();
            hideMenu();
        }

        private void btnQLHopDong_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color6);
            OpenChildForm(new FormHopDong());
        }

        // Sự kiện nút con trong menu hóa đơn
        private void btnHDDichVu_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color6);
            OpenChildForm(new FormHoaDonDV());
        }

        private void btnHDHopDong_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color6);
            OpenChildForm(new FormHoaDonHopDong());
        }
        #endregion
    }
}
