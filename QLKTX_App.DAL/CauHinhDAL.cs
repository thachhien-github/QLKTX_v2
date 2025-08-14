using System;
using System.Data.SqlClient;
using System.IO;
using QLKTX_App.Utilities;

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
                // 1. Tạo database trống (nếu chưa tồn tại)
                var createDbQuery = $"IF DB_ID('{cfg.Database}') IS NULL CREATE DATABASE [{cfg.Database}]";
                using (var conn = new SqlConnection(cfg.GetConnectionString(false)))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(createDbQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                // 2. Nếu có file script thì chạy nó
                if (File.Exists(sqlScriptPath))
                {
                    var script = File.ReadAllText(sqlScriptPath);

                    // Thay tên database mặc định trong script thành tên người dùng nhập
                    script = script.Replace("KTX_Database_C24TH2", cfg.Database);

                    // Chia script theo "GO" để chạy từng batch (tránh lỗi)
                    var batches = script.Split(new[] { "GO", "go", "Go" }, StringSplitOptions.RemoveEmptyEntries);

                    using (var conn = new SqlConnection(cfg.GetConnectionString()))
                    {
                        conn.Open();
                        foreach (var batch in batches)
                        {
                            if (string.IsNullOrWhiteSpace(batch)) continue;
                            using (var cmd = new SqlCommand(batch, conn))
                            {
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi tạo DB: " + ex.Message);
                return false;
            }
        }
    }
}
