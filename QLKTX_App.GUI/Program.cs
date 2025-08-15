using System;
using System.Windows.Forms;
using QLKTX_App.DTO;

namespace QLKTX_App.GUI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormLogin());
        }

        public static void RunMainForm(TaiKhoanModel tk)
        {
            Form currentForm = Form.ActiveForm; // Get the currently active form  

            if (currentForm != null)
            {
                currentForm.Hide();
            }

            if (tk.VaiTro == "Admin")
            {
                FormAdmin form = new FormAdmin(tk); // truyền tk  
                form.ShowDialog();
            }
            else if (tk.VaiTro == "NhanVien")
            {
                FormNhanVien form = new FormNhanVien(tk); // truyền tk  
                form.ShowDialog();
            }

            if (currentForm != null)
            {
                currentForm.Close();
            }
        }
    }
}
