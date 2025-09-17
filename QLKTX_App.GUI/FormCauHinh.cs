using QLKTX_App.BLL;
using QLKTX_App.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLKTX_App
{
    public partial class FormCauHinh : Form
    {
        private readonly CauHinhBLL _bll = new CauHinhBLL();
        public FormCauHinh()
        {
            InitializeComponent();
            LoadConfig();
        }
        private void LoadConfig()
        {
            if (AppConfig.LoadConfig() && AppConfig.CurrentConfig != null)
            {
                txtServer.Text = AppConfig.CurrentConfig.Server;
                txtDatabase.Text = AppConfig.CurrentConfig.Database;
                txtUser.Text = AppConfig.CurrentConfig.User;
                txtPassword.Text = AppConfig.CurrentConfig.Password;
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            var cfg = GetConfigFromForm();
            bool ok = _bll.TestConnection(cfg);
            MessageBox.Show(ok ? "Kết nối thành công!" : "❌ Kết nối thất bại!",
                "Thông báo", MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var cfg = GetConfigFromForm();

            // 1. Test kết nối DB
            if (!_bll.TestConnection(cfg))
            {
                // 2. Nếu không vào được DB, thử kết nối chỉ server
                var cfgServerOnly = new DbConfig
                {
                    Server = cfg.Server,
                    User = cfg.User,
                    Password = cfg.Password,
                    Database = "master"
                };

                if (!_bll.TestConnection(cfgServerOnly))
                {
                    MessageBox.Show("Không thể kết nối tới SQL Server.\nVui lòng kiểm tra thông tin.",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 3. Hỏi người dùng có muốn tạo DB mới không "yes"
                if (MessageBox.Show("Không tìm thấy cơ sở dữ liệu. Bạn có muốn tạo mới?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string sqlPath = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "DatabaseScripts",
                        "KTX_Database_C24TH2.sql"
                    );

                    // Kiểm tra file .sql có tồn tại không
                    if (!File.Exists(sqlPath))
                    {
                        MessageBox.Show($"Không tìm thấy file script:\n{sqlPath}",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Tạo DB
                    if (!_bll.CreateDatabase(cfg, sqlPath))
                    {
                        MessageBox.Show("Lỗi khi tạo cơ sở dữ liệu!",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    MessageBox.Show("Tạo cơ sở dữ liệu thành công!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    return;
                }

            }

            // 4. Lưu cấu hình
            if (_bll.SaveConfig(cfg))
            {
                MessageBox.Show("Lưu cấu hình thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;

                //Load vào FormLogin
                this.Hide();
                var loginForm = new FormLogin();
                loginForm.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Lỗi khi lưu cấu hình!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) => Close();

        private DbConfig GetConfigFromForm()
        {
            return new DbConfig
            {
                Server = txtServer.Text.Trim(),
                Database = txtDatabase.Text.Trim(),
                User = txtUser.Text.Trim(),
                Password = txtPassword.Text.Trim()
            };
        }

        private void lblHuongDan_Click(object sender, EventArgs e)
        {
            string huongDan = @"Quy trình cấu hình kết nối
1. Nhập thông tin SQL Server:
    - Server: Tên máy chủ SQL (ví dụ: .\SQLEXPRESS hoặc localhost)
    - Database: Tên cơ sở dữ liệu (ví dụ: QLKTX)
    - User: Tài khoản đăng nhập SQL (ví dụ: sa)
    - Password: Mật khẩu của tài khoản
2. Bấm 'Test' để kiểm tra kết nối.
3. Nếu thành công, bấm 'Save' để lưu cấu hình.
    Nếu cơ sở dữ liệu chưa tồn tại:
    - Chương trình sẽ hỏi bạn có muốn tạo mới hay không.
    - Chọn Yes để tạo Database mới.";
            MessageBox.Show(huongDan, "Hướng dẫn cấu hình",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void lblHuongDan_MouseEnter(object sender, EventArgs e)
        {
            lblHuongDan.ForeColor = Color.Blue;
            lblHuongDan.Font = new Font(lblHuongDan.Font, FontStyle.Underline);
        }

        private void lblHuongDan_MouseLeave(object sender, EventArgs e)
        {
            lblHuongDan.ForeColor = Color.Gray;
            lblHuongDan.Font = new Font(lblHuongDan.Font, FontStyle.Italic);
        }
    }
}
