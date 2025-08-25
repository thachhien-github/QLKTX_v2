using System;
using System.Data;
using System.Data.SqlClient;
using QLKTX_App.Utilities;

namespace QLKTX_App.DAL
{
    public class ThongKeDAL
    {
        private readonly DBHelper db = new DBHelper();

        public DataSet ThongKeTongHop(DateTime tuNgay, DateTime denNgay, int nam)
        {
            DataTable dataTable = db.ExecuteQuery("sp_ThongKe_TongHop", true,
                new SqlParameter("@TuNgay", tuNgay),
                new SqlParameter("@DenNgay", denNgay),
                new SqlParameter("@Nam", nam));

            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            return dataSet;
        }


    }
}
