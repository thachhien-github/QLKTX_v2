using QLKTX_App.DTO;
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

            bool daMoLogin = false;
            TaiKhoanModel tk = null;

            // Nếu chưa có config.json thì mở FormCauHinh
            if (!File.Exists("config.json"))
            {
                using (var f = new FormCauHinh())
                {
                    if (f.ShowDialog() != DialogResult.OK) return;

                    // FormCauHinh đã tự mở FormLogin rồi
                    daMoLogin = true;
                }
            }

            // Nếu FormLogin chưa mở (khi đã có config.json) thì mở ở đây
            if (!daMoLogin)
            {
                using (var login = new FormLogin())
                {
                    if (login.ShowDialog() == DialogResult.OK)
                    {
                        tk = login.TaiKhoanDaDangNhap;
                    }
                }
            }
            else
            {
                // Nếu FormLogin đã mở trong FormCauHinh thì lấy tk từ đó
                if (FormLogin.LastLoginTaiKhoan != null)
                    tk = FormLogin.LastLoginTaiKhoan;
            }

            if (tk == null) return;

            // Chạy form chính
            if (tk.VaiTro == "Admin")
                Application.Run(new FormAdmin(tk));
            else
                Application.Run(new FormNhanVien(tk));
        }
    }
}
