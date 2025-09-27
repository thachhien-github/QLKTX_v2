using System;
using System.Data;
using System.Data.SqlClient;

namespace QLKTX_App.DAL
{
    public class HopDongDAL
    {
        private readonly DBHelper db = new DBHelper();

        public DataTable GetAll()
        {
            string sql = @"
        SELECT 
            sv.MSSV, sv.HoTen, pb.MaPhong,
            lp.TenLoai AS LoaiPhong, lp.GiaPhong,
            pb.NgayPhanBo, pb.SoThang,
            DATEADD(MONTH, pb.SoThang, pb.NgayPhanBo) AS NgayHetHan,
            pb.MienTienPhong, pb.SoDotThu, pb.GhiChu
        FROM PhanBo pb
        JOIN SinhVien sv ON pb.MSSV = sv.MSSV
        JOIN Phong p ON pb.MaPhong = p.MaPhong
        JOIN LoaiPhong lp ON p.MaLoai = lp.MaLoai
        ORDER BY pb.NgayPhanBo DESC";

            return db.ExecuteQuery(sql, false);
        }

        public DataTable Search(string mssv, string maPhong, string trangThai)
        {
            string sql = @"
        SELECT 
            sv.MSSV, sv.HoTen, pb.MaPhong,
            lp.TenLoai AS LoaiPhong, lp.GiaPhong,
            pb.NgayPhanBo, pb.SoThang,
            DATEADD(MONTH, pb.SoThang, pb.NgayPhanBo) AS NgayHetHan,
            pb.MienTienPhong, pb.SoDotThu, pb.GhiChu
        FROM PhanBo pb
        JOIN SinhVien sv ON pb.MSSV = sv.MSSV
        JOIN Phong p ON pb.MaPhong = p.MaPhong
        JOIN LoaiPhong lp ON p.MaLoai = lp.MaLoai
        WHERE (sv.MSSV LIKE @MSSV OR @MSSV = '')
          AND (pb.MaPhong = @MaPhong OR @MaPhong = '')
        ";

            // lọc trạng thái
            if (trangThai == "Còn hạn")
                sql += " AND DATEADD(MONTH, pb.SoThang, pb.NgayPhanBo) >= GETDATE()";
            else if (trangThai == "Sắp hết hạn")
                sql += " AND DATEADD(MONTH, pb.SoThang, pb.NgayPhanBo) BETWEEN GETDATE() AND DATEADD(DAY,30,GETDATE())";
            else if (trangThai == "Hết hạn")
                sql += " AND DATEADD(MONTH, pb.SoThang, pb.NgayPhanBo) < GETDATE()";

            SqlParameter[] prms =
            {
                new SqlParameter("@MSSV", "%" + mssv + "%"),
                new SqlParameter("@MaPhong", maPhong ?? "")
            };

            return db.ExecuteQuery(sql, false, prms);
        }


        public bool GiaHan(string mssv, int soThang)
        {
            // Gia hạn hợp đồng mới nhất của sinh viên
            string sql = @"
        UPDATE PhanBo
        SET SoThang = SoThang + @SoThang
        WHERE MSSV = @MSSV
          AND NgayPhanBo = (SELECT MAX(NgayPhanBo) FROM PhanBo WHERE MSSV = @MSSV)";

            SqlParameter[] prms =
            {
                new SqlParameter("@SoThang", soThang),
                new SqlParameter("@MSSV", mssv)
            };
            return db.ExecuteNonQuery(sql, false, prms) > 0;
        }
    }
}
