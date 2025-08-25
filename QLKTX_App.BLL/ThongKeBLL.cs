using QLKTX_App.DAL;
using System;
using System.Data;

namespace QLKTX_App.BLL
{
    public class ThongKeBLL
    {
        private readonly ThongKeDAL dal = new ThongKeDAL();

        public DataSet ThongKeTongHop(DateTime tuNgay, DateTime denNgay, int nam)
        {
            return dal.ThongKeTongHop(tuNgay, denNgay, nam);
        }
    }
}
