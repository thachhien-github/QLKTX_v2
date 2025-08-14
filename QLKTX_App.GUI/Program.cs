using QLKTX_App.Utilities;
using System;
using System.Windows.Forms;

namespace QLKTX_App
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 1. Tải config.json nếu có
            bool configOk = AppConfig.LoadConfig() && AppConfig.CurrentConfig != null;

            // 2. Nếu chưa có config hoặc kết nối thất bại → mở FormCauHinh
            if (!configOk || !TestDbConnection())
            {
                using (var f = new FormCauHinh())
                {
                    var result = f.ShowDialog();
                    if (result != DialogResult.OK)
                    {
                        // Người dùng hủy cấu hình → thoát chương trình
                        return;
                    }
                }

                // Sau khi cấu hình xong → load lại config
                if (!AppConfig.LoadConfig() || AppConfig.CurrentConfig == null || !TestDbConnection())
                {
                    MessageBox.Show("❌ Không thể kết nối CSDL sau khi cấu hình. Ứng dụng sẽ thoát.",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // 3. Mở FormLogin
            Application.Run(new FormLogin());
        }

        // Hàm kiểm tra kết nối nhanh
        private static bool TestDbConnection()
        {
            try
            {
                using (var conn = new System.Data.SqlClient.SqlConnection(AppConfig.CurrentConfig.GetConnectionString()))
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
