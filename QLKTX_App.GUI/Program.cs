using QLKTX_App.DTO;
using QLKTX_App.Utilities;
using System;
using System.IO;
using System.Windows.Forms;

namespace QLKTX_App.GUI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Cấu hình
            if (!File.Exists("config.json"))
            {
                using (var f = new FormCauHinh())
                {
                    if (f.ShowDialog() != DialogResult.OK) return;
                }
            }

            // Login modal
            TaiKhoanModel tk = null;
            using (var login = new FormLogin())
            {
                if (login.ShowDialog() == DialogResult.OK)
                {
                    tk = login.TaiKhoanDaDangNhap;
                }
                else
                {
                    return; // nếu cancel hoặc login thất bại → thoát app
                }
            }

            // Mở form chính
            Form mainForm = null;
            if (tk.VaiTro == "Admin")
                mainForm = new FormAdmin(tk);
            else
                mainForm = new FormNhanVien(tk);

            if (mainForm != null)
                Application.Run(mainForm); // form chính duy nhất chạy
        }
    }
}
