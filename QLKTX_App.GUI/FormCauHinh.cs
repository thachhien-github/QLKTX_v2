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
            var cfg = new DbConfig
            {
                Server = txtServer.Text,
                Database = txtDatabase.Text,
                User = txtUser.Text,
                Password = txtPassword.Text
            };

            MessageBox.Show(_bll.TestConnection(cfg) ?
                "✅ Kết nối thành công!" :
                "❌ Kết nối thất bại!", "Thông báo",
                MessageBoxButtons.OK,
                _bll.TestConnection(cfg) ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var cfg = new DbConfig
            {
                Server = txtServer.Text,
                Database = txtDatabase.Text,
                User = txtUser.Text,
                Password = txtPassword.Text
            };

            if (!_bll.TestConnection(cfg))
            {
                if (MessageBox.Show("Không thể kết nối, bạn có muốn tạo DB mới?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string sqlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DatabaseScripts", "KTX_Database_C24TH2.sql");
                    if (!_bll.CreateDatabase(cfg, sqlPath))
                    {
                        MessageBox.Show("❌ Lỗi khi tạo DB!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else return;
            }

            _bll.SaveConfig(cfg);
            MessageBox.Show("💾 Lưu cấu hình thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) => Close();
    }
}
