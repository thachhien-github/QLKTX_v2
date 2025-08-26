using FontAwesome.Sharp;
using QLKTX_App.ChildForm_Admin;
using QLKTX_App.ChildForm_Comon;
using QLKTX_App.ChildForm_NhanVien;
using QLKTX_App.DTO;
using QLKTX_App.GUI.ChildForm_Admin;
using QLKTX_App.GUI.ChildForm_Comon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;

namespace QLKTX_App
{
    public partial class FormAdmin : Form
    {   //fields
        private IconButton currentBtn;
        private Form currentChildForm;


        //constructor
        private string hoTen; // lưu tên người đăng nhập
        private TaiKhoanModel tk;

        public FormAdmin(TaiKhoanModel tk)
        {
            InitializeComponent();
            this.tk = tk;
            this.hoTen = tk.HoTen; // lưu tên người dùng

            lblChao.Text = GetLoiChaoTheoThoiGian(tk.HoTen); // hiển thị lời chào
            customizeDesign();

            //form
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
        }

        #region menu ẩn hiện
        private void customizeDesign()
        {
            panelPhong.Visible = false;
            panelThietLap.Visible = false;          
        }

        private void hideMenu()
        {
            if (panelPhong.Visible == true)
                panelPhong.Visible = false;
            if (panelThietLap.Visible == true)
                panelThietLap.Visible = false;
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



        #region Properties
        private struct RGBColors
        {
            //public static Color color1 = Color.FromArgb(172, 126, 241);
            //public static Color color2 = Color.FromArgb(249, 118, 176);
            //public static Color color3 = Color.FromArgb(253, 138, 114);
            //public static Color color4 = Color.FromArgb(95, 77, 221);
            //public static Color color5 = Color.FromArgb(249, 88, 155);
            public static Color color6 = Color.FromArgb(1, 19, 73);
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
                currentBtn.BackColor = Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(19)))), ((int)(((byte)(102)))));
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

        //Click logo reset SideBar
        private void btnHome_Click(object sender, EventArgs e)
        {
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

        #region Kéo thả form
        //Drag Form
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panelTop_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        #endregion

        private void FormAdmin_Load(object sender, EventArgs e)
        {
            string loiChao = GetLoiChaoTheoThoiGian(hoTen);
            lblChao.Text = loiChao;

            if (WindowState == FormWindowState.Maximized)
            {
                btnMaximize.IconChar = FontAwesome.Sharp.IconChar.WindowRestore;
            }
            else
            {
                btnMaximize.IconChar = FontAwesome.Sharp.IconChar.WindowMaximize;
            }
        }

        #region 3 nút điều khiển form
        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát ứng dụng?","Xác nhận thoát",
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

        #region sidebar
        //menu tài khoản
        private void btnTaiKhoan_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color6);
            OpenChildForm(new FormQLTK());
        }

        //reset chọn khi nhấn vào logo
        private void btnHome_Click_1(object sender, EventArgs e)
        {
            currentChildForm.Close();
            Reset();
            hideMenu();
        }

        //menu nhân viên
        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color6);
            OpenChildForm(new FormQLNV());
        }

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

        //menu thống kê
        private void btnThongKe_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color6);
            OpenChildForm(new FormThongKeDT());
        }

        private void btnTKdien_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color6);
        }

        private void btnTKnuoc_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color6);
        }

        private void btnTKgiuxe_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color6);
        }

        private void btnXuatFile_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color6);
        }

        //menu thiếp lập
        private void btnThiepLap_Click(object sender, EventArgs e)
        {
            showMenu(panelThietLap);
        }

        private void btnGia_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color6);
            OpenChildForm(new FormThietLapGia());
        }

        private void btnLogOut_Click_1(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color6);
            if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?",
                "Đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                FormLogin.LastLoginTaiKhoan = null; // báo cho Program là logout
                this.Close(); // đóng form chính → Program quay lại login
            }
        }

        private void btnQLHopDong_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color6);
            OpenChildForm(new FormHopDong());
        }
        #endregion

        private void btnNapDLexcel_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color6);
            OpenChildForm(new FormImportDB());
        }
    }
}
