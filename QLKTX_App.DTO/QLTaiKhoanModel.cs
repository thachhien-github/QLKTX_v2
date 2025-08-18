namespace QLKTX_App.DTO
{
    public class ALTaiKhoanModel
    {
        public string MaNV { get; set; }          // <-- thêm
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string VaiTro { get; set; }
        public bool TrangThai { get; set; }
        public string HoTen { get; set; }

        // tiện cho hiển thị
        public string TrangThaiText => TrangThai ? "Còn hoạt động" : "Bị khóa";
    }
}
