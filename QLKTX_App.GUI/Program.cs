using QLKTX_App.Utilities;
using System;
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

            bool needConfig = true;

            // B1: Load config nếu có
            if (AppConfig.LoadConfig() && !string.IsNullOrEmpty(AppConfig.ConnString))
            {
                // B2: Test kết nối
                if (DbHelper.TestConnection(AppConfig.ConnString))
                {
                    needConfig = false;
                }
            }

            // B3: Nếu chưa config hoặc connect fail -> mở FormCauHinh
            if (needConfig)
            {
                using (var f = new FormCauHinh())
                {
                    if (f.ShowDialog() != DialogResult.OK) return;

                    // Sau khi lưu config từ FormCauHinh thì test lại
                    if (!DbHelper.TestConnection(AppConfig.ConnString))
                    {
                        MessageBox.Show("Không thể kết nối tới CSDL. Thoát ứng dụng.",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            // B4: Chạy form chính
            Application.Run(new FormLogin());
        }
    }
}
