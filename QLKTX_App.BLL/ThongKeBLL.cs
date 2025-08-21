using System;
using System.Collections.Generic;
using System.Data;
using QLKTX_App.DAL;
using QLKTX_App.DTO;

namespace QLKTX_App.BLL
{
    public class ThongKeBLL
    {
        private readonly ThongKeDAL _dal = new ThongKeDAL();

        public List<ThongKeModel> GetThongKe(DateTime tuNgay, DateTime denNgay)
        {
            DataTable dt = _dal.GetThongKeChiTiet(tuNgay, denNgay);
            var list = new List<ThongKeModel>();
            int stt = 1;

            foreach (DataRow r in dt.Rows)
            {
                list.Add(new ThongKeModel
                {
                    STT = stt++,
                    MaPhong = r["MaPhong"].ToString(),
                    MSSV = r["MSSV"].ToString(),
                    HoTen = r["HoTen"].ToString(),
                    LoaiPhong = r["LoaiPhong"].ToString(),
                    SoTienThang = Convert.ToDecimal(r["SoTienThang"]),
                    SoThang = Convert.ToInt32(r["SoThang"]),
                    ThanhTien = Convert.ToDecimal(r["ThanhTien"]),
                    GhiChu = r["GhiChu"].ToString()
                });
            }
            return list;
        }

        public (decimal Phong, decimal Dien, decimal Nuoc, decimal GiuXe) GetTongHopDoanhThu(DateTime tuNgay, DateTime denNgay)
        {
            DataTable dt = _dal.GetTongHopDoanhThu(tuNgay, denNgay);
            if (dt.Rows.Count == 0) return (0, 0, 0, 0);

            var r = dt.Rows[0];
            return (
                Convert.ToDecimal(r["DoanhThuPhong"]),
                Convert.ToDecimal(r["DoanhThuDien"]),
                Convert.ToDecimal(r["DoanhThuNuoc"]),
                Convert.ToDecimal(r["DoanhThuGiuXe"])
            );
        }
    }
}
