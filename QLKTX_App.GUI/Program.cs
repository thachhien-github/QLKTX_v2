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
            Application.Run(new FormCauHinh());
        }
    }
}
