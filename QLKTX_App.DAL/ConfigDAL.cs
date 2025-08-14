using QLKTX_App.Utilities;

namespace QLKTX_App.DAL
{
    public static class ConfigDAL
    {
        /// <summary>
        /// Lấy chuỗi kết nối từ AppConfig (đã lưu trong file JSON sau khi cấu hình)
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionString()
        {
            // Đảm bảo đã load config
            if (!AppConfig.LoadConfig())
                return string.Empty;

            return AppConfig.ConnectionString;
        }
    }
}
