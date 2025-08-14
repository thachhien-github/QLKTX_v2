using QLKTX_App.DAL;
using QLKTX_App.Utilities;
using System;
using System.IO;

namespace QLKTX_App.BLL
{
    public class CauHinhBLL
    {
        private readonly CauHinhDAL _dal = new CauHinhDAL();

        public bool TestConnection(DbConfig cfg)
        {
            return _dal.TestConnection(AppConfig.BuildConn(cfg));
        }

        public bool SaveConfig(DbConfig cfg)
        {
            AppConfig.SaveConfig(cfg);
            return true;
        }

        public bool CreateDatabase(DbConfig cfg, string sqlFilePath)
        {
            string masterConn = AppConfig.BuildConn(new DbConfig
            {
                Server = cfg.Server,
                Database = "master",
                User = cfg.User,
                Password = cfg.Password
            });

            return _dal.CreateDatabase(masterConn, AppConfig.BuildConn(cfg), sqlFilePath);
        }
    }
}
