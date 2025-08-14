using System;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

namespace QLKTX_App.DAL
{
    public class CauHinhDAL
    {
        public bool TestConnection(string connStr)
        {
            try
            {
                using (var cn = new SqlConnection(connStr))
                {
                    cn.Open();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CreateDatabase(string masterConn, string dbConn, string sqlFilePath)
        {
            try
            {
                string script = File.ReadAllText(sqlFilePath);
                var batches = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

                using (var cn = new SqlConnection(masterConn))
                {
                    cn.Open();
                    foreach (var batch in batches)
                    {
                        if (string.IsNullOrWhiteSpace(batch)) continue;
                        using (var cmd = cn.CreateCommand())
                        {
                            cmd.CommandText = batch;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                using (var cn = new SqlConnection(dbConn))
                {
                    cn.Open();
                    using (var cmd = cn.CreateCommand())
                    {
                        cmd.CommandText =
                            @"IF NOT EXISTS (SELECT 1 FROM TaiKhoan WHERE TenDangNhap='admin')
                              INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThai)
                              VALUES ('admin','admin',N'Admin',1)";
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
