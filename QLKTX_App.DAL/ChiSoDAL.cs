using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using QLKTX_App.DTO;

namespace QLKTX_App.DAL
{
    public class ChiSoDAL
    {
        private readonly DBHelper db = new DBHelper();

        public List<ChiSoModel> GetAll()
        {
            var dt = db.ExecuteQuery("SELECT * FROM ChiSo", false);
            var list = new List<ChiSoModel>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new ChiSoModel
                {
                    MaPhong = row["MaPhong"].ToString(),
                    Thang = Convert.ToInt32(row["Thang"]),
                    Nam = Convert.ToInt32(row["Nam"]),
                    DienCu = Convert.ToInt32(row["DienCu"]),
                    DienMoi = Convert.ToInt32(row["DienMoi"]),
                    DienTieuThu = Convert.ToInt32(row["DienTieuThu"]),
                    NuocCu = Convert.ToInt32(row["NuocCu"]),
                    NuocMoi = Convert.ToInt32(row["NuocMoi"]),
                    NuocTieuThu = Convert.ToInt32(row["NuocTieuThu"])
                });
            }
            return list;
        }

        public int Them(ChiSoModel cs)
        {
            return db.ExecuteNonQuery("sp_ChiSo_Them", true,
                new SqlParameter("@MaPhong", cs.MaPhong),
                new SqlParameter("@Thang", cs.Thang),
                new SqlParameter("@Nam", cs.Nam),
                new SqlParameter("@DienCu", cs.DienCu),
                new SqlParameter("@DienMoi", cs.DienMoi),
                new SqlParameter("@NuocCu", cs.NuocCu),
                new SqlParameter("@NuocMoi", cs.NuocMoi));
        }

        public int Sua(ChiSoModel cs)
        {
            return db.ExecuteNonQuery("sp_ChiSo_Sua", true,
                new SqlParameter("@MaPhong", cs.MaPhong),
                new SqlParameter("@Thang", cs.Thang),
                new SqlParameter("@Nam", cs.Nam),
                new SqlParameter("@DienCu", cs.DienCu),
                new SqlParameter("@DienMoi", cs.DienMoi),
                new SqlParameter("@NuocCu", cs.NuocCu),
                new SqlParameter("@NuocMoi", cs.NuocMoi));
        }

        public int Xoa(string maPhong, int thang, int nam)
        {
            return db.ExecuteNonQuery("sp_ChiSo_Xoa", true,
                new SqlParameter("@MaPhong", maPhong),
                new SqlParameter("@Thang", thang),
                new SqlParameter("@Nam", nam));
        }
    }
}
