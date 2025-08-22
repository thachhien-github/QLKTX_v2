using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKTX_App.Utilities
{
    public static class CurrentUser
    {
        /// <summary>
        /// Mã nhân viên (hoặc admin) đang đăng nhập
        /// </summary>
        public static string MaNV { get; private set; }

        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        public static string Username { get; private set; }

        /// <summary>
        /// Họ tên nhân viên
        /// </summary>
        public static string HoTen { get; private set; }

        /// <summary>
        /// Vai trò (Admin/NhanVien/Khac)
        /// </summary>
        public static string VaiTro { get; private set; }

        /// <summary>
        /// Hàm khởi tạo khi đăng nhập thành công
        /// </summary>
        public static void SetUser(string maNV, string username, string hoTen, string vaiTro)
        {
            MaNV = maNV;
            Username = username;
            HoTen = hoTen;
            VaiTro = vaiTro;
        }

        /// <summary>
        /// Xóa thông tin khi đăng xuất
        /// </summary>
        public static void Clear()
        {
            MaNV = null;
            Username = null;
            HoTen = null;
            VaiTro = null;
        }

        /// <summary>
        /// Kiểm tra đã đăng nhập chưa
        /// </summary>
        public static bool IsLoggedIn => !string.IsNullOrEmpty(MaNV);
    }
}
