using System;
using System.IO;
using System.Windows.Forms;
using QLKTX_App.DTO;
using QLKTX_App.Utilities;

namespace QLKTX_App
{
    public class MyAppContext : ApplicationContext
    {
        public MyAppContext()
        {
            StartApp();
        }

        private void StartApp()
        {
            if (!File.Exists("config.json"))
            {
                var f = new FormCauHinh();
                f.FormClosed += (s, e) =>
                {
                    if (AppConfig.CurrentConfig == null)
                        ExitThread();
                    else
                        ShowLogin();
                };
                f.Show();
            }
            else
            {
                ShowLogin();
            }
        }

        private void ShowLogin()
        {
            var login = new FormLogin();
            bool isLoginSuccess = false; // cờ kiểm soát

            login.LoginSucceeded += (s, tk) =>
            {
                isLoginSuccess = true; // đánh dấu là login ok

                // mở main form
                Form main = (tk.VaiTro == "Admin")
                    ? (Form)new FormAdmin(tk)
                    : new FormNhanVien(tk);

                main.FormClosed += (s2, e2) => ExitThread();
                main.Show();

                // đóng login (sẽ không thoát app vì có cờ isLoginSuccess = true)
                login.Close();
            };

            login.FormClosed += (s, e) =>
            {
                if (!isLoginSuccess) // chỉ thoát app nếu login thất bại hoặc bị đóng tay
                    ExitThread();
            };

            login.Show();
        }

    }
}
