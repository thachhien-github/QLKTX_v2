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

            while (true) // Vòng lặp đảm bảo logout quay lại login
            {
                TaiKhoanModel tk = null;

                // --- Nếu chưa có config.json thì mở FormCauHinh ---
                if (!File.Exists("config.json"))
                {
                    using (var f = new FormCauHinh())
                    {
                        if (f.ShowDialog() != DialogResult.OK)
                            return; // Người dùng hủy cấu hình thì thoát

                        // Sau khi cấu hình, FormLogin đã được mở
                        tk = FormLogin.LastLoginTaiKhoan;
                    }
                }
                else
                {
                    // --- Mở FormLogin ---
                    using (var login = new FormLogin())
                    {
                        if (login.ShowDialog() != DialogResult.OK)
                            return; // Người dùng thoát login thì thoát app

                        tk = login.TaiKhoanDaDangNhap;
                    }
                }

                if (tk == null) return;

                // --- Chạy form chính theo vai trò ---
                if (tk.VaiTro == "Admin")
                {
                    Application.Run(new FormAdmin(tk));
                }
                else
                {
                    Application.Run(new FormNhanVien(tk));
                }

                // --- Kiểm tra: Logout hay Exit ---
                if (FormLogin.LastLoginTaiKhoan == null)
                {
                    // Người dùng logout → quay lại login
                    continue;
                }
                else
                {
                    // Người dùng exit hẳn → thoát chương trình
                    break;
                }
            }
        }
    }
}
