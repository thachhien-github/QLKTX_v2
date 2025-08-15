namespace QLKTX_App.DTO
{
    public class TaiKhoanModel
    {
        // Tên đầy đủ của người dùng
        public string HoTen { get; set; }

        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string VaiTro { get; set; } // Admin hoặc NhanVien
        public bool TrangThai { get; set; } // true = hoạt động, false = khóa
    }
}
