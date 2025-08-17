using QLKTX_App.Utilities;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace QLKTX_App.DAL
{
    public class CauHinhDAL
    {
        public bool TestConnection(DbConfig cfg, bool includeDatabase = true)
        {
            try
            {
                using (var conn = new SqlConnection(cfg.GetConnectionString(includeDatabase)))
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

        public bool CreateDatabase(DbConfig cfg, string sqlScriptPath)
        {
            try
            {
                // 1. Kiểm tra file script
                if (!File.Exists(sqlScriptPath))
                {
                    MessageBox.Show($"Không tìm thấy file script SQL:\n{sqlScriptPath}",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                string connMaster = cfg.GetConnectionString(false); // false = kết nối master

                // 2. Tạo DB nếu chưa tồn tại
                using (var conn = new SqlConnection(connMaster))
                {
                    conn.Open();
                    string checkDbQuery = $"SELECT database_id FROM sys.databases WHERE Name = '{cfg.Database}'";
                    using (var cmd = new SqlCommand(checkDbQuery, conn))
                    {
                        if (cmd.ExecuteScalar() == null)
                        {
                            string createDbQuery = $"CREATE DATABASE [{cfg.Database}]";
                            using (var createCmd = new SqlCommand(createDbQuery, conn))
                            {
                                createCmd.ExecuteNonQuery();
                            }
                            MessageBox.Show($"✅ Đã tạo database mới: {cfg.Database}",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }

                // 3. Đọc và xử lý script
                string script = File.ReadAllText(sqlScriptPath);

                // Xóa comment nhiều dòng /* ... */
                script = Regex.Replace(script, @"/\*.*?\*/", "", RegexOptions.Singleline);

                // Xóa comment 1 dòng --
                script = Regex.Replace(script, @"--.*?$", "", RegexOptions.Multiline);

                // Xóa lệnh USE
                script = Regex.Replace(script, @"USE\s+\[?\w+\]?\s*;?", "", RegexOptions.IgnoreCase);

                // Xóa CREATE DATABASE (dù có [] hay không)
                script = Regex.Replace(script, @"CREATE\s+DATABASE\s+\[?\w+\]?\s*;?", "", RegexOptions.IgnoreCase);

                // Thay tên DB cứng bằng tên mới (hỗ trợ cả [Tên])
                script = Regex.Replace(script, @"(\[?)KTX_Database_C24TH2(\]?)", $"[{cfg.Database}]", RegexOptions.IgnoreCase);

                // Thêm USE vào đầu script để đảm bảo context đúng
                script = $"USE [{cfg.Database}]\nGO\n" + script;

                // 4. Tách batch theo GO (có thể kèm comment phía sau)
                var batches = Regex.Split(script, @"^\s*GO\s*(?:--.*)?$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

                // 5. Chạy từng batch trên kết nối master (script đã tự USE DB)
                using (var conn = new SqlConnection(connMaster))
                {
                    conn.Open();
                    foreach (var batch in batches)
                    {
                        if (string.IsNullOrWhiteSpace(batch)) continue;

                        try
                        {
                            using (var cmd = new SqlCommand(batch, conn))
                            {
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch (Exception batchEx)
                        {
                            MessageBox.Show(
                                $"❌ Lỗi khi chạy batch SQL:\n---Batch Start---\n{batch}\n---Batch End---\n\nChi tiết: {batchEx.Message}",
                                "Lỗi SQL",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi tạo database: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
