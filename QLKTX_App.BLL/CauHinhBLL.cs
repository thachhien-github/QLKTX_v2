using QLKTX_App.DAL;
using QLKTX_App.Utilities;

namespace QLKTX_App.BLL
{
    public class CauHinhBLL
    {
        private readonly CauHinhDAL _dal = new CauHinhDAL();

        public bool TestConnection(DbConfig cfg) => _dal.TestConnection(cfg);

        public bool CreateDatabase(DbConfig cfg, string sqlPath) => _dal.CreateDatabase(cfg, sqlPath);

        public bool SaveConfig(DbConfig cfg) => AppConfig.SaveConfig(cfg);
    }
}
