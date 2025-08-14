/* ============================================================
   KÝ TÚC XÁ – TRIỂN KHAI CƠ SỞ DỮ LIỆU & NGHIỆP VỤ
   Tác giả: Thạch Hiền - 2421160052 - C24A.TH2
   ============================================================ */

/* ============================================================
   PHẦN 1 – TẠO DATABASE & CÁC BẢNG CHÍNH CHO HỆ THỐNG KÝ TÚC XÁ
   ============================================================ */

-------------------------------------------
-- 0) TẠO DATABASE NẾU CHƯA CÓ (Phương án A – SQL tự quản lý file)
-------------------------------------------
IF DB_ID('KTX_Database_C24TH2') IS NULL
BEGIN
    PRINT N'>>> Tạo mới CSDL KTX_Database_C24TH2...';
    CREATE DATABASE KTX_Database_C24TH2;
END
GO

-------------------------------------------
-- 1) SỬ DỤNG DATABASE
-------------------------------------------

/* ============================================================
   2) BẢNG HỆ THỐNG & NHÂN SỰ
   ============================================================ */
IF OBJECT_ID('dbo.TaiKhoan','U') IS NULL
CREATE TABLE dbo.TaiKhoan (
    TenDangNhap VARCHAR(50)  NOT NULL PRIMARY KEY,
    MatKhau     VARCHAR(100) NOT NULL,
    VaiTro      NVARCHAR(20) NOT NULL CHECK (VaiTro IN (N'Admin', N'NhanVien')),
    TrangThai   BIT NOT NULL DEFAULT(1) -- 1: hoạt động, 0: khóa
);

IF OBJECT_ID('dbo.NhanVien','U') IS NULL
CREATE TABLE dbo.NhanVien (
    MaNV        VARCHAR(10)  NOT NULL PRIMARY KEY,
    HoTen       NVARCHAR(100) NOT NULL,
    GioiTinh    NVARCHAR(10)  NULL,
    NgaySinh    DATE          NULL,
    SDT         VARCHAR(20)   NULL,
    Email       VARCHAR(100)  NULL,
    TenDangNhap VARCHAR(50)   NULL UNIQUE,
    CONSTRAINT FK_NhanVien_TaiKhoan FOREIGN KEY (TenDangNhap) REFERENCES dbo.TaiKhoan(TenDangNhap)
);
GO

/* ============================================================
   3) BẢNG SINH VIÊN
   ============================================================ */
IF OBJECT_ID('dbo.SinhVien','U') IS NULL
CREATE TABLE dbo.SinhVien (
    MSSV     NVARCHAR(10)  NOT NULL PRIMARY KEY,
    HoTen    NVARCHAR(100) NOT NULL,
    GioiTinh NVARCHAR(10)  NULL,
    NgaySinh DATE          NULL,
    SDT      NVARCHAR(20)  NULL,
    DiaChi   NVARCHAR(200) NULL
);
GO

/* ============================================================
   4) NHÀ/PHÒNG: TẦNG, LOẠI PHÒNG, PHÒNG
   ============================================================ */
IF OBJECT_ID('dbo.Tang','U') IS NULL
CREATE TABLE dbo.Tang (
    MaTang  VARCHAR(10)  NOT NULL PRIMARY KEY,
    TenTang NVARCHAR(50) NOT NULL
);

IF OBJECT_ID('dbo.LoaiPhong','U') IS NULL
CREATE TABLE dbo.LoaiPhong (
    MaLoai  VARCHAR(10)  NOT NULL PRIMARY KEY,
    TenLoai NVARCHAR(50) NOT NULL,      -- ví dụ: "Phòng 5 người"
    SucChua INT         NOT NULL CHECK (SucChua BETWEEN 1 AND 20),
    DonGia  MONEY       NOT NULL CHECK (DonGia >= 0) -- giá phòng/tháng
);

IF OBJECT_ID('dbo.Phong','U') IS NULL
CREATE TABLE dbo.Phong (
    MaPhong       VARCHAR(10)  NOT NULL PRIMARY KEY,
    MaTang        VARCHAR(10)  NOT NULL,
    MaLoai        VARCHAR(10)  NOT NULL,
    SoLuongToiDa  INT          NOT NULL CHECK (SoLuongToiDa > 0),
    TrangThai     NVARCHAR(30) NOT NULL DEFAULT(N'Trống'), -- Trống/Còn chỗ/Đầy
    CONSTRAINT FK_Phong_Tang      FOREIGN KEY (MaTang) REFERENCES dbo.Tang(MaTang),
    CONSTRAINT FK_Phong_LoaiPhong FOREIGN KEY (MaLoai) REFERENCES dbo.LoaiPhong(MaLoai)
);
GO

/* ============================================================
   5) PHÂN BỔ (HỢP ĐỒNG) & THANH TOÁN TIỀN PHÒNG
   ============================================================ */
IF OBJECT_ID('dbo.PhanBo','U') IS NULL
CREATE TABLE dbo.PhanBo (
    MSSV           NVARCHAR(10) NOT NULL,
    MaPhong        VARCHAR(10)  NOT NULL,
    NgayPhanBo     DATE         NOT NULL,
    SoThang        INT          NOT NULL CHECK (SoThang BETWEEN 1 AND 60),
    GhiChu         NVARCHAR(200) NULL,
    MienTienPhong  BIT NOT NULL DEFAULT(0),                 -- 1 = miễn tiền phòng
    SoDotThu       TINYINT NOT NULL DEFAULT(1) CHECK (SoDotThu IN (1,2)), -- thu 1 hoặc 2 đợt
    CONSTRAINT PK_PhanBo PRIMARY KEY (MSSV, MaPhong),
    CONSTRAINT FK_PhanBo_SV    FOREIGN KEY (MSSV)   REFERENCES dbo.SinhVien(MSSV),
    CONSTRAINT FK_PhanBo_Phong FOREIGN KEY (MaPhong) REFERENCES dbo.Phong(MaPhong)
);

IF OBJECT_ID('dbo.ThanhToanPhong','U') IS NULL
CREATE TABLE dbo.ThanhToanPhong (
    ID          INT IDENTITY(1,1) PRIMARY KEY,
    MSSV        NVARCHAR(10) NOT NULL,
    MaPhong     VARCHAR(10)  NOT NULL,
    SoThangThu  INT          NOT NULL CHECK (SoThangThu > 0),
    NgayThu     DATE         NOT NULL,
    NguoiThu    VARCHAR(10)  NULL, -- MaNV
    GhiChu      NVARCHAR(200) NULL,
    CONSTRAINT FK_TTP_SV    FOREIGN KEY (MSSV)   REFERENCES dbo.SinhVien(MSSV),
    CONSTRAINT FK_TTP_Phong FOREIGN KEY (MaPhong) REFERENCES dbo.Phong(MaPhong)
);
GO

/* ============================================================
   6) GIỮ XE
   ============================================================ */
IF OBJECT_ID('dbo.LoaiXe','U') IS NULL
CREATE TABLE dbo.LoaiXe (
    MaLoaiXe VARCHAR(10) NOT NULL PRIMARY KEY,
    TenLoai  NVARCHAR(50) NOT NULL,
    GiaGiuXe MONEY NOT NULL CHECK (GiaGiuXe >= 0)
);

IF OBJECT_ID('dbo.TheXe','U') IS NULL
CREATE TABLE dbo.TheXe (
    MaThe      VARCHAR(10)  NOT NULL PRIMARY KEY,
    MSSV       NVARCHAR(10) NOT NULL,
    MaLoaiXe   VARCHAR(10)  NOT NULL,
    BienSo     NVARCHAR(20) NOT NULL UNIQUE,
    NgayDangKy DATE         NOT NULL,
    CONSTRAINT FK_TheXe_SV FOREIGN KEY (MSSV)     REFERENCES dbo.SinhVien(MSSV),
    CONSTRAINT FK_TheXe_LX FOREIGN KEY (MaLoaiXe) REFERENCES dbo.LoaiXe(MaLoaiXe)
);
GO

/* ============================================================
   7) GIÁ ĐIỆN/NƯỚC & CHỈ SỐ
   ============================================================ */
IF OBJECT_ID('dbo.GiaDienNuoc','U') IS NULL
CREATE TABLE dbo.GiaDienNuoc (
    ID      CHAR(1) NOT NULL CONSTRAINT PK_GiaDN PRIMARY KEY CHECK (ID='1'),
    GiaDien MONEY NOT NULL CHECK (GiaDien >= 0),
    GiaNuoc MONEY NOT NULL CHECK (GiaNuoc >= 0)
);

IF NOT EXISTS (SELECT 1 FROM dbo.GiaDienNuoc WHERE ID='1')
BEGIN
    INSERT INTO dbo.GiaDienNuoc(ID, GiaDien, GiaNuoc)
    VALUES('1', 2909, 21300); -- giá mặc định
END

IF OBJECT_ID('dbo.ChiSo','U') IS NULL
CREATE TABLE dbo.ChiSo (
    MaPhong VARCHAR(10) NOT NULL,
    Thang   INT NOT NULL CHECK (Thang BETWEEN 1 AND 12),
    Nam     INT NOT NULL CHECK (Nam >= 2000),
    DienCu  INT NOT NULL CHECK (DienCu  >= 0),
    DienMoi INT NOT NULL CHECK (DienMoi >= 0),
    DienTieuThu AS (CASE WHEN DienMoi >= DienCu THEN DienMoi - DienCu ELSE NULL END) PERSISTED,
    NuocCu  INT NOT NULL CHECK (NuocCu  >= 0),
    NuocMoi INT NOT NULL CHECK (NuocMoi >= 0),
    NuocTieuThu AS (CASE WHEN NuocMoi >= NuocCu THEN NuocMoi - NuocCu ELSE NULL END) PERSISTED,
    CONSTRAINT PK_ChiSo PRIMARY KEY (MaPhong, Thang, Nam),
    CONSTRAINT FK_ChiSo_Phong FOREIGN KEY (MaPhong) REFERENCES dbo.Phong(MaPhong)
);
GO

/* ============================================================
   8) HÓA ĐƠN & CHI TIẾT HÓA ĐƠN
   ============================================================ */
IF OBJECT_ID('dbo.HoaDon','U') IS NULL
CREATE TABLE dbo.HoaDon (
    MaHD     VARCHAR(20) NOT NULL PRIMARY KEY,
    MaPhong  VARCHAR(10) NOT NULL,
    ThangNam CHAR(6)     NOT NULL, -- MMYYYY
    NgayLap  DATE        NOT NULL,
    TongTien MONEY       NOT NULL,
    CONSTRAINT UQ_HD_PhongThang UNIQUE (MaPhong, ThangNam),
    CONSTRAINT FK_HD_Phong FOREIGN KEY (MaPhong) REFERENCES dbo.Phong(MaPhong)
);

IF OBJECT_ID('dbo.ChiTietHoaDon','U') IS NULL
CREATE TABLE dbo.ChiTietHoaDon (
    MaChiTietHD INT IDENTITY(1,1) PRIMARY KEY,
    MaHD        VARCHAR(20)  NOT NULL,
    MSSV        NVARCHAR(10) NOT NULL,
    TienPhong   MONEY NOT NULL,
    TienDien    MONEY NOT NULL,
    TienNuoc    MONEY NOT NULL,
    TienGuiXe   MONEY NOT NULL,
    TongTien    AS (TienPhong + TienDien + TienNuoc + TienGuiXe) PERSISTED,
    CONSTRAINT FK_CTHD_HD FOREIGN KEY (MaHD) REFERENCES dbo.HoaDon(MaHD),
    CONSTRAINT FK_CTHD_SV FOREIGN KEY (MSSV) REFERENCES dbo.SinhVien(MSSV)
);
GO

--------------------------------------------------------------------------------
-- END PHẦN 1
--------------------------------------------------------------------------------


/* ============================================================
   PHẦN 2 (FULL) - LOGIC: CRUD + Nghiệp vụ
   - Tương thích với schema PHẦN 1
   - Mỗi proc có kiểm tra, THROW lỗi rõ cho app C#
   ============================================================ */

--------------------------------------------------------------------------------
-- 2.1 CRUD NHÓM HỆ THỐNG: TaiKhoan, NhanVien
-- (mã lỗi: 60000-61999)
--------------------------------------------------------------------------------

CREATE OR ALTER PROCEDURE dbo.sp_TaiKhoan_Them
    @TenDangNhap VARCHAR(50),
    @MatKhau VARCHAR(100),
    @VaiTro NVARCHAR(20),
    @TrangThai BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS(SELECT 1 FROM dbo.TaiKhoan WHERE TenDangNhap=@TenDangNhap)
        THROW 60001, N'Tài khoản đã tồn tại.', 1;
    IF @VaiTro NOT IN (N'Admin',N'NhanVien')
        THROW 60002, N'Vai trò không hợp lệ.', 1;
    INSERT INTO dbo.TaiKhoan (TenDangNhap,MatKhau,VaiTro,TrangThai)
    VALUES (@TenDangNhap,@MatKhau,@VaiTro,@TrangThai);
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_TaiKhoan_Sua
    @TenDangNhap VARCHAR(50),
    @VaiTro NVARCHAR(20),
    @TrangThai BIT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.TaiKhoan WHERE TenDangNhap=@TenDangNhap)
        THROW 60003, N'Không tìm thấy tài khoản.', 1;
    IF @VaiTro NOT IN (N'Admin',N'NhanVien')
        THROW 60002, N'Vai trò không hợp lệ.', 1;
    UPDATE dbo.TaiKhoan SET VaiTro=@VaiTro, TrangThai=@TrangThai WHERE TenDangNhap=@TenDangNhap;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_TaiKhoan_DoiMatKhau
    @TenDangNhap VARCHAR(50),
    @MatKhauMoi VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.TaiKhoan WHERE TenDangNhap=@TenDangNhap)
        THROW 60003, N'Không tìm thấy tài khoản.', 1;
    UPDATE dbo.TaiKhoan SET MatKhau=@MatKhauMoi WHERE TenDangNhap=@TenDangNhap;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_TaiKhoan_Xoa
    @TenDangNhap VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.TaiKhoan WHERE TenDangNhap=@TenDangNhap)
        THROW 60003, N'Không tìm thấy tài khoản.', 1;
    IF EXISTS(SELECT 1 FROM dbo.NhanVien WHERE TenDangNhap=@TenDangNhap)
        THROW 60004, N'Tài khoản đang được gán cho nhân viên; không thể xóa.', 1;
    DELETE FROM dbo.TaiKhoan WHERE TenDangNhap=@TenDangNhap;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_TaiKhoan_CapNhatTrangThai
    @TenDangNhap VARCHAR(50),
    @TrangThai BIT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.TaiKhoan WHERE TenDangNhap=@TenDangNhap)
        THROW 60003, N'Không tìm thấy tài khoản.', 1;
    UPDATE dbo.TaiKhoan SET TrangThai=@TrangThai WHERE TenDangNhap=@TenDangNhap;
END
GO

-- NhanVien
CREATE OR ALTER PROCEDURE dbo.sp_NhanVien_Them
    @MaNV VARCHAR(10),
    @HoTen NVARCHAR(100),
    @GioiTinh NVARCHAR(10) = NULL,
    @NgaySinh DATE = NULL,
    @SDT VARCHAR(20) = NULL,
    @Email VARCHAR(100) = NULL,
    @TenDangNhap VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS(SELECT 1 FROM dbo.NhanVien WHERE MaNV=@MaNV)
        THROW 61001, N'Nhân viên đã tồn tại.', 1;
    IF @TenDangNhap IS NOT NULL AND NOT EXISTS(SELECT 1 FROM dbo.TaiKhoan WHERE TenDangNhap=@TenDangNhap)
        THROW 61002, N'Tên đăng nhập không tồn tại.', 1;
    INSERT INTO dbo.NhanVien (MaNV,HoTen,GioiTinh,NgaySinh,SDT,Email,TenDangNhap)
    VALUES(@MaNV,@HoTen,@GioiTinh,@NgaySinh,@SDT,@Email,@TenDangNhap);
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_NhanVien_Sua
    @MaNV VARCHAR(10),
    @HoTen NVARCHAR(100),
    @GioiTinh NVARCHAR(10) = NULL,
    @NgaySinh DATE = NULL,
    @SDT VARCHAR(20) = NULL,
    @Email VARCHAR(100) = NULL,
    @TenDangNhap VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.NhanVien WHERE MaNV=@MaNV)
        THROW 61003, N'Không tìm thấy nhân viên.', 1;
    IF @TenDangNhap IS NOT NULL AND NOT EXISTS(SELECT 1 FROM dbo.TaiKhoan WHERE TenDangNhap=@TenDangNhap)
        THROW 61002, N'Tên đăng nhập không tồn tại.', 1;
    UPDATE dbo.NhanVien
    SET HoTen=@HoTen, GioiTinh=@GioiTinh, NgaySinh=@NgaySinh, SDT=@SDT, Email=@Email, TenDangNhap=@TenDangNhap
    WHERE MaNV=@MaNV;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_NhanVien_Xoa
    @MaNV VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.NhanVien WHERE MaNV=@MaNV)
        THROW 61003, N'Không tìm thấy nhân viên.', 1;
    IF EXISTS(SELECT 1 FROM dbo.ThanhToanPhong WHERE NguoiThu=@MaNV)
        THROW 61004, N'Nhân viên đã có chứng từ thu, không thể xóa.', 1;
    DELETE FROM dbo.NhanVien WHERE MaNV=@MaNV;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_NhanVien_GoTaiKhoan
    @MaNV VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.NhanVien WHERE MaNV=@MaNV)
        THROW 61003, N'Không tìm thấy nhân viên.', 1;
    UPDATE dbo.NhanVien SET TenDangNhap=NULL WHERE MaNV=@MaNV;
END
GO

--------------------------------------------------------------------------------
-- 2.2 CRUD DANH MỤC & PHÒNG: Tang, LoaiPhong, Phong
-- (mã lỗi: 62000-63999)
--------------------------------------------------------------------------------

CREATE OR ALTER PROCEDURE dbo.sp_Tang_Them
    @MaTang VARCHAR(10),
    @TenTang NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS(SELECT 1 FROM dbo.Tang WHERE MaTang=@MaTang)
        THROW 62001, N'Tầng đã tồn tại.', 1;
    INSERT INTO dbo.Tang (MaTang,TenTang) VALUES (@MaTang,@TenTang);
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_Tang_Sua
    @MaTang VARCHAR(10),
    @TenTang NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.Tang WHERE MaTang=@MaTang)
        THROW 62002, N'Không tìm thấy tầng.', 1;
    UPDATE dbo.Tang SET TenTang=@TenTang WHERE MaTang=@MaTang;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_Tang_Xoa
    @MaTang VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.Tang WHERE MaTang=@MaTang)
        THROW 62002, N'Không tìm thấy tầng.', 1;
    IF EXISTS(SELECT 1 FROM dbo.Phong WHERE MaTang=@MaTang)
        THROW 62003, N'Không thể xóa tầng vì tồn tại phòng thuộc tầng này.', 1;
    DELETE FROM dbo.Tang WHERE MaTang=@MaTang;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_LoaiPhong_Them
    @MaLoai VARCHAR(10),
    @TenLoai NVARCHAR(50),
    @SucChua INT,
    @DonGia MONEY
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS(SELECT 1 FROM dbo.LoaiPhong WHERE MaLoai=@MaLoai)
        THROW 63001, N'Loại phòng đã tồn tại.', 1;
    IF @SucChua IS NULL OR @SucChua <= 0
        THROW 63002, N'Sức chứa phải > 0.', 1;
    IF @DonGia IS NULL OR @DonGia < 0
        THROW 63003, N'Đơn giá không hợp lệ.', 1;
    INSERT INTO dbo.LoaiPhong (MaLoai,TenLoai,SucChua,DonGia)
    VALUES(@MaLoai,@TenLoai,@SucChua,@DonGia);
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_LoaiPhong_Sua
    @MaLoai VARCHAR(10),
    @TenLoai NVARCHAR(50),
    @SucChua INT,
    @DonGia MONEY
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.LoaiPhong WHERE MaLoai=@MaLoai)
        THROW 63004, N'Không tìm thấy loại phòng.', 1;
    IF @SucChua IS NULL OR @SucChua <= 0
        THROW 63002, N'Sức chứa phải > 0.', 1;
    IF @DonGia IS NULL OR @DonGia < 0
        THROW 63003, N'Đơn giá không hợp lệ.', 1;
    UPDATE dbo.LoaiPhong SET TenLoai=@TenLoai, SucChua=@SucChua, DonGia=@DonGia WHERE MaLoai=@MaLoai;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_LoaiPhong_Xoa
    @MaLoai VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.LoaiPhong WHERE MaLoai=@MaLoai)
        THROW 63004, N'Không tìm thấy loại phòng.', 1;
    IF EXISTS(SELECT 1 FROM dbo.Phong WHERE MaLoai=@MaLoai)
        THROW 63005, N'Không thể xóa loại phòng vì còn phòng thuộc loại này.', 1;
    DELETE FROM dbo.LoaiPhong WHERE MaLoai=@MaLoai;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_Phong_Them
    @MaPhong VARCHAR(10),
    @MaTang VARCHAR(10),
    @MaLoai VARCHAR(10),
    @SoLuongToiDa INT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS(SELECT 1 FROM dbo.Phong WHERE MaPhong=@MaPhong)
        THROW 64001, N'Phòng đã tồn tại.', 1;
    IF NOT EXISTS(SELECT 1 FROM dbo.Tang WHERE MaTang=@MaTang)
        THROW 64002, N'Tầng không tồn tại.', 1;
    IF NOT EXISTS(SELECT 1 FROM dbo.LoaiPhong WHERE MaLoai=@MaLoai)
        THROW 64003, N'Loại phòng không tồn tại.', 1;
    IF @SoLuongToiDa IS NULL OR @SoLuongToiDa <= 0
        THROW 64004, N'Số lượng tối đa phải > 0.', 1;
    INSERT INTO dbo.Phong (MaPhong,MaTang,MaLoai,SoLuongToiDa,TrangThai)
    VALUES(@MaPhong,@MaTang,@MaLoai,@SoLuongToiDa,N'Trống');
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_Phong_Sua
    @MaPhong VARCHAR(10),
    @MaTang VARCHAR(10),
    @MaLoai VARCHAR(10),
    @SoLuongToiDa INT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.Phong WHERE MaPhong=@MaPhong)
        THROW 64005, N'Không tìm thấy phòng.', 1;
    IF NOT EXISTS(SELECT 1 FROM dbo.Tang WHERE MaTang=@MaTang)
        THROW 64002, N'Tầng không tồn tại.', 1;
    IF NOT EXISTS(SELECT 1 FROM dbo.LoaiPhong WHERE MaLoai=@MaLoai)
        THROW 64003, N'Loại phòng không tồn tại.', 1;
    IF @SoLuongToiDa IS NULL OR @SoLuongToiDa <= 0
        THROW 64004, N'Số lượng tối đa phải > 0.', 1;

    DECLARE @SoNguoi INT = (SELECT COUNT(*) FROM dbo.PhanBo WHERE MaPhong=@MaPhong);
    IF @SoLuongToiDa < @SoNguoi
        THROW 64006, N'Không thể giảm sức chứa nhỏ hơn số người đang ở.', 1;

    UPDATE dbo.Phong SET MaTang=@MaTang, MaLoai=@MaLoai, SoLuongToiDa=@SoLuongToiDa WHERE MaPhong=@MaPhong;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_Phong_CapNhatTrangThai
    @MaPhong VARCHAR(10),
    @TrangThai NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.Phong WHERE MaPhong=@MaPhong)
        THROW 64005, N'Không tìm thấy phòng.', 1;
    UPDATE dbo.Phong SET TrangThai=@TrangThai WHERE MaPhong=@MaPhong;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_Phong_Xoa
    @MaPhong VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.Phong WHERE MaPhong=@MaPhong)
        THROW 64005, N'Không tìm thấy phòng.', 1;
    IF EXISTS(SELECT 1 FROM dbo.PhanBo WHERE MaPhong=@MaPhong)
        THROW 64007, N'Không thể xóa phòng vì có sinh viên đã phân.', 1;
    IF EXISTS(SELECT 1 FROM dbo.ChiSo WHERE MaPhong=@MaPhong)
        THROW 64008, N'Không thể xóa phòng vì có chỉ số điện nước.', 1;
    IF EXISTS(SELECT 1 FROM dbo.HoaDon WHERE MaPhong=@MaPhong)
        THROW 64009, N'Không thể xóa phòng vì đã phát sinh hóa đơn.', 1;
    DELETE FROM dbo.Phong WHERE MaPhong=@MaPhong;
END
GO

--------------------------------------------------------------------------------
-- 2.3 CRUD PHÂN BỔ & THANH TOÁN (sửa/hoàn thiện logic)
-- (mã lỗi: 65000-66999)
--------------------------------------------------------------------------------

-- Lưu ý: bảng PhanBo trong PHẦN1 dùng cột NgayPhanBo (không phải NgayVao)
CREATE OR ALTER PROCEDURE dbo.sp_PhanBo_Them
    @MSSV NVARCHAR(10),
    @MaPhong VARCHAR(10),
    @NgayPhanBo DATE,
    @SoThang INT,
    @SoDotThu TINYINT = 1,
    @MienTienPhong BIT = 0,
    @GhiChu NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.SinhVien WHERE MSSV=@MSSV)
        THROW 65001, N'Sinh viên không tồn tại.', 1;
    IF NOT EXISTS(SELECT 1 FROM dbo.Phong WHERE MaPhong=@MaPhong)
        THROW 65002, N'Phòng không tồn tại.', 1;
    IF @SoThang IS NULL OR @SoThang <= 0
        THROW 65005, N'Số tháng hợp đồng phải > 0.', 1;
    IF @SoDotThu NOT IN (1,2)
        THROW 65006, N'Số đợt thu chỉ là 1 hoặc 2.', 1;

    -- Kiểm tra sinh viên đã có hợp đồng khác chồng lên thời gian này không
    IF EXISTS(
        SELECT 1 FROM dbo.PhanBo pb
        WHERE pb.MSSV = @MSSV
          AND DATEADD(MONTH, pb.SoThang, pb.NgayPhanBo) > @NgayPhanBo
    )
        THROW 65009, N'Sinh viên đang có hợp đồng còn hiệu lực, không thể phân thêm.', 1;

    -- Kiểm tra sức chứa phòng
    DECLARE @SucChua INT = (SELECT SoLuongToiDa FROM dbo.Phong WHERE MaPhong=@MaPhong);
    DECLARE @DangO INT = (SELECT COUNT(*) FROM dbo.PhanBo WHERE MaPhong=@MaPhong);
    IF @DangO >= @SucChua
        THROW 65003, N'Phòng đã đầy.', 1;

    INSERT INTO dbo.PhanBo (MSSV,MaPhong,NgayPhanBo,SoThang,GhiChu,MienTienPhong,SoDotThu)
    VALUES (@MSSV,@MaPhong,@NgayPhanBo,@SoThang,@GhiChu,@MienTienPhong,@SoDotThu);
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_PhanBo_Sua
    @MSSV NVARCHAR(10),
    @MaPhong VARCHAR(10),
    @NgayPhanBo DATE,
    @SoThang INT,
    @SoDotThu TINYINT = 1,
    @MienTienPhong BIT = 0,
    @GhiChu NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.PhanBo WHERE MSSV=@MSSV AND MaPhong=@MaPhong)
        THROW 65007, N'Không tìm thấy phân bổ.', 1;
    IF @SoThang IS NULL OR @SoThang <= 0
        THROW 65005, N'Số tháng hợp đồng phải > 0.', 1;
    IF @SoDotThu NOT IN (1,2)
        THROW 65006, N'Số đợt thu chỉ là 1 hoặc 2.', 1;

    -- Kiểm tra không sửa để tạo trùng hợp đồng khác (đơn giản: không cho sửa MSSV/MaPhong)
    UPDATE dbo.PhanBo
    SET NgayPhanBo=@NgayPhanBo, SoThang=@SoThang, SoDotThu=@SoDotThu, MienTienPhong=@MienTienPhong, GhiChu=@GhiChu
    WHERE MSSV=@MSSV AND MaPhong=@MaPhong;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_PhanBo_Xoa
    @MSSV NVARCHAR(10),
    @MaPhong VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.PhanBo WHERE MSSV=@MSSV AND MaPhong=@MaPhong)
        THROW 65007, N'Không tìm thấy phân bổ.', 1;
    IF EXISTS(SELECT 1 FROM dbo.ThanhToanPhong WHERE MSSV=@MSSV AND MaPhong=@MaPhong)
        THROW 65008, N'Không thể xóa phân bổ vì đã có thanh toán.', 1;
    DELETE FROM dbo.PhanBo WHERE MSSV=@MSSV AND MaPhong=@MaPhong;
END
GO

-- ThanhToanPhong: table in Part1 has ID as PK (identity)
CREATE OR ALTER PROCEDURE dbo.sp_ThanhToanPhong_Them
    @MSSV NVARCHAR(10),
    @MaPhong VARCHAR(10),
    @SoThangThu INT,
    @NgayThu DATE,
    @NguoiThu VARCHAR(10),
    @GhiChu NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.PhanBo WHERE MSSV=@MSSV AND MaPhong=@MaPhong)
        THROW 66001, N'Sinh viên chưa phân bổ phòng này.', 1;
    IF @SoThangThu IS NULL OR @SoThangThu <= 0
        THROW 66002, N'Số tháng thu phải > 0.', 1;
    IF @NguoiThu IS NOT NULL AND NOT EXISTS(SELECT 1 FROM dbo.NhanVien WHERE MaNV=@NguoiThu)
        THROW 66003, N'Người thu không tồn tại.', 1;

    DECLARE @SoThangHopDong INT = (SELECT SoThang FROM dbo.PhanBo WHERE MSSV=@MSSV AND MaPhong=@MaPhong);
    DECLARE @DaThu INT = ISNULL((SELECT SUM(SoThangThu) FROM dbo.ThanhToanPhong WHERE MSSV=@MSSV AND MaPhong=@MaPhong),0);
    IF @DaThu + @SoThangThu > @SoThangHopDong
        THROW 66005, N'Thanh toán vượt quá số tháng hợp đồng.', 1;

    INSERT INTO dbo.ThanhToanPhong (MSSV,MaPhong,SoThangThu,NgayThu,NguoiThu,GhiChu)
    VALUES(@MSSV,@MaPhong,@SoThangThu,@NgayThu,@NguoiThu,@GhiChu);
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_ThanhToanPhong_Sua
    @ID INT,
    @SoThangThu INT,
    @NgayThu DATE,
    @NguoiThu VARCHAR(10),
    @GhiChu NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.ThanhToanPhong WHERE ID=@ID)
        THROW 66004, N'Không tìm thấy chứng từ.', 1;
    IF @SoThangThu IS NULL OR @SoThangThu <= 0
        THROW 66002, N'Số tháng thu phải > 0.', 1;
    IF @NguoiThu IS NOT NULL AND NOT EXISTS(SELECT 1 FROM dbo.NhanVien WHERE MaNV=@NguoiThu)
        THROW 66003, N'Người thu không tồn tại.', 1;

    -- kiểm tra không vượt hợp đồng sau khi cập nhật tổng
    DECLARE @MSSV NVARCHAR(10) = (SELECT MSSV FROM dbo.ThanhToanPhong WHERE ID=@ID);
    DECLARE @MaPhong VARCHAR(10) = (SELECT MaPhong FROM dbo.ThanhToanPhong WHERE ID=@ID);

    DECLARE @SoThangHopDong INT = (SELECT SoThang FROM dbo.PhanBo WHERE MSSV=@MSSV AND MaPhong=@MaPhong);
    DECLARE @DaThuKhac INT = ISNULL((SELECT SUM(SoThangThu) FROM dbo.ThanhToanPhong WHERE MSSV=@MSSV AND MaPhong=@MaPhong AND ID<>@ID),0);

    IF @DaThuKhac + @SoThangThu > @SoThangHopDong
        THROW 66005, N'Cập nhật làm vượt quá số tháng hợp đồng.', 1;

    UPDATE dbo.ThanhToanPhong
    SET SoThangThu=@SoThangThu, NgayThu=@NgayThu, NguoiThu=@NguoiThu, GhiChu=@GhiChu
    WHERE ID=@ID;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_ThanhToanPhong_Xoa
    @ID INT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.ThanhToanPhong WHERE ID=@ID)
        THROW 66004, N'Không tìm thấy chứng từ.', 1;
    DELETE FROM dbo.ThanhToanPhong WHERE ID=@ID;
END
GO

--------------------------------------------------------------------------------
-- 2.4 CRUD GIỮ XE, GIÁ ĐIỆN NƯỚC, CHỈ SỐ
-- (mã lỗi 67000-70999)
--------------------------------------------------------------------------------

CREATE OR ALTER PROCEDURE dbo.sp_LoaiXe_Them
    @MaLoaiXe VARCHAR(10),
    @TenLoai NVARCHAR(50),
    @GiaGiuXe MONEY
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS(SELECT 1 FROM dbo.LoaiXe WHERE MaLoaiXe=@MaLoaiXe)
        THROW 67001, N'Loại xe đã tồn tại.', 1;
    IF @GiaGiuXe IS NULL OR @GiaGiuXe < 0
        THROW 67002, N'Giá giữ xe không hợp lệ.', 1;
    INSERT INTO dbo.LoaiXe (MaLoaiXe,TenLoai,GiaGiuXe) VALUES(@MaLoaiXe,@TenLoai,@GiaGiuXe);
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_LoaiXe_Sua
    @MaLoaiXe VARCHAR(10),
    @TenLoai NVARCHAR(50),
    @GiaGiuXe MONEY
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.LoaiXe WHERE MaLoaiXe=@MaLoaiXe)
        THROW 67003, N'Không tìm thấy loại xe.', 1;
    IF @GiaGiuXe IS NULL OR @GiaGiuXe < 0
        THROW 67002, N'Giá giữ xe không hợp lệ.', 1;
    UPDATE dbo.LoaiXe SET TenLoai=@TenLoai, GiaGiuXe=@GiaGiuXe WHERE MaLoaiXe=@MaLoaiXe;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_LoaiXe_Xoa
    @MaLoaiXe VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.LoaiXe WHERE MaLoaiXe=@MaLoaiXe)
        THROW 67003, N'Không tìm thấy loại xe.', 1;
    IF EXISTS(SELECT 1 FROM dbo.TheXe WHERE MaLoaiXe=@MaLoaiXe)
        THROW 67004, N'Không thể xóa loại xe vì đang có thẻ thuộc loại này.', 1;
    DELETE FROM dbo.LoaiXe WHERE MaLoaiXe=@MaLoaiXe;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_TheXe_Them
    @MaThe VARCHAR(10),
    @MSSV NVARCHAR(10),
    @MaLoaiXe VARCHAR(10),
    @BienSo NVARCHAR(20),
    @NgayDangKy DATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS(SELECT 1 FROM dbo.TheXe WHERE MaThe=@MaThe)
        THROW 68001, N'Thẻ xe đã tồn tại.', 1;
    IF NOT EXISTS(SELECT 1 FROM dbo.SinhVien WHERE MSSV=@MSSV)
        THROW 68002, N'Sinh viên không tồn tại.', 1;
    IF NOT EXISTS(SELECT 1 FROM dbo.LoaiXe WHERE MaLoaiXe=@MaLoaiXe)
        THROW 68003, N'Loại xe không tồn tại.', 1;
    IF EXISTS(SELECT 1 FROM dbo.TheXe WHERE BienSo=@BienSo)
        THROW 68005, N'Biển số đã tồn tại.', 1;
    INSERT INTO dbo.TheXe (MaThe,MSSV,MaLoaiXe,BienSo,NgayDangKy)
    VALUES(@MaThe,@MSSV,@MaLoaiXe,@BienSo,@NgayDangKy);
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_TheXe_Sua
    @MaThe VARCHAR(10),
    @MaLoaiXe VARCHAR(10),
    @BienSo NVARCHAR(20),
    @NgayDangKy DATE
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.TheXe WHERE MaThe=@MaThe)
        THROW 68004, N'Không tìm thấy thẻ xe.', 1;
    IF NOT EXISTS(SELECT 1 FROM dbo.LoaiXe WHERE MaLoaiXe=@MaLoaiXe)
        THROW 68003, N'Loại xe không tồn tại.', 1;
    -- kiểm tra biển số đã dùng bởi thẻ khác
    IF EXISTS(SELECT 1 FROM dbo.TheXe WHERE BienSo=@BienSo AND MaThe<>@MaThe)
        THROW 68005, N'Biển số đã tồn tại.', 1;
    UPDATE dbo.TheXe SET MaLoaiXe=@MaLoaiXe, BienSo=@BienSo, NgayDangKy=@NgayDangKy WHERE MaThe=@MaThe;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_TheXe_Xoa
    @MaThe VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.TheXe WHERE MaThe=@MaThe)
        THROW 68004, N'Không tìm thấy thẻ xe.', 1;
    DELETE FROM dbo.TheXe WHERE MaThe=@MaThe;
END
GO

-- GiaDienNuoc: in PHẦN1 table has ID char(1) primary; but we also keep historical rows? earlier schema used only ID='1' entry.
-- Here we assume table may have multiple records with NgayApDung (if you've adjusted schema).
-- But per PHẦN1 we created single-row table ID='1'. To be safe, create procs that update that row.

CREATE OR ALTER PROCEDURE dbo.sp_GiaDienNuoc_CapNhat
    @GiaDien MONEY,
    @GiaNuoc MONEY
AS
BEGIN
    SET NOCOUNT ON;
    IF @GiaDien IS NULL OR @GiaNuoc IS NULL OR @GiaDien < 0 OR @GiaNuoc < 0
        THROW 69001, N'Giá điện/nước không hợp lệ.', 1;
    IF EXISTS(SELECT 1 FROM dbo.GiaDienNuoc WHERE ID='1')
    BEGIN
        UPDATE dbo.GiaDienNuoc SET GiaDien=@GiaDien, GiaNuoc=@GiaNuoc WHERE ID='1';
    END
    ELSE
    BEGIN
        INSERT INTO dbo.GiaDienNuoc (ID,GiaDien,GiaNuoc) VALUES('1',@GiaDien,@GiaNuoc);
    END
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_GiaDienNuoc_Lay
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM dbo.GiaDienNuoc;
END
GO

-- ChiSo: schema in PHẦN1 uses (MaPhong,Thang,Nam,DienCu,DienMoi,NuocCu,NuocMoi)
CREATE OR ALTER PROCEDURE dbo.sp_ChiSo_Them
    @MaPhong VARCHAR(10),
    @Thang INT,
    @Nam INT,
    @DienCu INT,
    @DienMoi INT,
    @NuocCu INT,
    @NuocMoi INT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.Phong WHERE MaPhong=@MaPhong)
        THROW 70001, N'Phòng không tồn tại.', 1;
    IF @Thang NOT BETWEEN 1 AND 12
        THROW 70002, N'Tháng không hợp lệ.', 1;
    IF @Nam < 2000
        THROW 70003, N'Năm không hợp lệ.', 1;
    IF EXISTS(SELECT 1 FROM dbo.ChiSo WHERE MaPhong=@MaPhong AND Thang=@Thang AND Nam=@Nam)
        THROW 70004, N'Chỉ số cho tháng này đã tồn tại.', 1;
    IF @DienMoi < @DienCu OR @NuocMoi < @NuocCu
        THROW 70005, N'Chỉ số mới phải >= chỉ số cũ.', 1;

    -- optional continuity check: if previous month exists, DienCu must equal previous DienMoi
    DECLARE @prevDienMoi INT, @prevNuocMoi INT;
    SELECT TOP 1 @prevDienMoi = DienMoi, @prevNuocMoi = NuocMoi
    FROM dbo.ChiSo
    WHERE MaPhong=@MaPhong
      AND (Nam < @Nam OR (Nam=@Nam AND Thang < @Thang))
    ORDER BY Nam DESC, Thang DESC;

    IF @prevDienMoi IS NOT NULL AND @DienCu <> @prevDienMoi
        THROW 70006, N'ChiSo.DienCu phải bằng DienMoi của tháng trước.', 1;
    IF @prevNuocMoi IS NOT NULL AND @NuocCu <> @prevNuocMoi
        THROW 70007, N'ChiSo.NuocCu phải bằng NuocMoi của tháng trước.', 1;

    INSERT INTO dbo.ChiSo (MaPhong,Thang,Nam,DienCu,DienMoi,NuocCu,NuocMoi)
    VALUES(@MaPhong,@Thang,@Nam,@DienCu,@DienMoi,@NuocCu,@NuocMoi);
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_ChiSo_Sua
    @MaPhong VARCHAR(10),
    @Thang INT,
    @Nam INT,
    @DienCu INT,
    @DienMoi INT,
    @NuocCu INT,
    @NuocMoi INT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.ChiSo WHERE MaPhong=@MaPhong AND Thang=@Thang AND Nam=@Nam)
        THROW 70008, N'Không tìm thấy chỉ số.', 1;
    IF @DienMoi < @DienCu OR @NuocMoi < @NuocCu
        THROW 70005, N'Chỉ số mới phải >= chỉ số cũ.', 1;

    -- (Optional) check continuity like insert
    UPDATE dbo.ChiSo
    SET DienCu=@DienCu, DienMoi=@DienMoi, NuocCu=@NuocCu, NuocMoi=@NuocMoi
    WHERE MaPhong=@MaPhong AND Thang=@Thang AND Nam=@Nam;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_ChiSo_Xoa
    @MaPhong VARCHAR(10),
    @Thang INT,
    @Nam INT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.ChiSo WHERE MaPhong=@MaPhong AND Thang=@Thang AND Nam=@Nam)
        THROW 70008, N'Không tìm thấy chỉ số.', 1;
    DELETE FROM dbo.ChiSo WHERE MaPhong=@MaPhong AND Thang=@Thang AND Nam=@Nam;
END
GO

--------------------------------------------------------------------------------
-- 2.5 CRUD SinhVien (đảm bảo có, mã lỗi: 72000-72999)
--------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_SinhVien_Them
    @MSSV NVARCHAR(10),
    @HoTen NVARCHAR(100),
    @GioiTinh NVARCHAR(10) = NULL,
    @NgaySinh DATE = NULL,
    @SDT NVARCHAR(20) = NULL,
    @DiaChi NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS(SELECT 1 FROM dbo.SinhVien WHERE MSSV=@MSSV)
        THROW 72001, N'Sinh viên đã tồn tại.', 1;
    INSERT INTO dbo.SinhVien (MSSV,HoTen,GioiTinh,NgaySinh,SDT,DiaChi)
    VALUES(@MSSV,@HoTen,@GioiTinh,@NgaySinh,@SDT,@DiaChi);
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_SinhVien_Sua
    @MSSV NVARCHAR(10),
    @HoTen NVARCHAR(100),
    @GioiTinh NVARCHAR(10) = NULL,
    @NgaySinh DATE = NULL,
    @SDT NVARCHAR(20) = NULL,
    @DiaChi NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.SinhVien WHERE MSSV=@MSSV)
        THROW 72002, N'Không tìm thấy sinh viên.', 1;
    UPDATE dbo.SinhVien SET HoTen=@HoTen, GioiTinh=@GioiTinh, NgaySinh=@NgaySinh, SDT=@SDT, DiaChi=@DiaChi WHERE MSSV=@MSSV;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_SinhVien_Xoa
    @MSSV NVARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.SinhVien WHERE MSSV=@MSSV)
        THROW 72002, N'Không tìm thấy sinh viên.', 1;
    IF EXISTS(SELECT 1 FROM dbo.PhanBo WHERE MSSV=@MSSV)
        THROW 72003, N'Không thể xóa sinh viên vì đã phân phòng/have history.', 1;
    IF EXISTS(SELECT 1 FROM dbo.TheXe WHERE MSSV=@MSSV)
        THROW 72004, N'Không thể xóa sinh viên vì có thẻ xe.', 1;
    DELETE FROM dbo.SinhVien WHERE MSSV=@MSSV;
END
GO

--------------------------------------------------------------------------------
-- 2.6 CRUD HoaDon & ChiTietHoaDon (mã lỗi 73000-73999)
--------------------------------------------------------------------------------
-- Lưu ý: HoaDon.MaHD là string do bạn thiết kế; ChiTietHoaDon references HoaDon

CREATE OR ALTER PROCEDURE dbo.sp_HoaDon_Them
    @MaHD VARCHAR(50),
    @MaPhong VARCHAR(10),
    @Thang INT,
    @Nam INT,
    @NgayLap DATE,
    @TongTien MONEY
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS(SELECT 1 FROM dbo.HoaDon WHERE MaHD=@MaHD)
        THROW 73001, N'Mã hóa đơn đã tồn tại.', 1;
    DECLARE @ThangNam CHAR(6) = RIGHT('00' + CAST(@Thang AS VARCHAR(2)),2) + CAST(@Nam AS VARCHAR(4));
    IF EXISTS(SELECT 1 FROM dbo.HoaDon WHERE MaPhong=@MaPhong AND ThangNam=@ThangNam)
        THROW 73002, N'Phòng đã có hóa đơn tháng này.', 1;
    IF NOT EXISTS(SELECT 1 FROM dbo.Phong WHERE MaPhong=@MaPhong)
        THROW 73003, N'Phòng không tồn tại.', 1;
    INSERT INTO dbo.HoaDon (MaHD,MaPhong,ThangNam,NgayLap,TongTien)
    VALUES(@MaHD,@MaPhong,@ThangNam,@NgayLap,@TongTien);
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_HoaDon_Xoa
    @MaHD VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.HoaDon WHERE MaHD=@MaHD)
        THROW 73004, N'Không tìm thấy hóa đơn.', 1;
    IF EXISTS(SELECT 1 FROM dbo.ChiTietHoaDon WHERE MaHD=@MaHD)
        THROW 73005, N'Không thể xóa hóa đơn vì đã có chi tiết.', 1;
    DELETE FROM dbo.HoaDon WHERE MaHD=@MaHD;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_ChiTietHoaDon_Them
    @MaHD VARCHAR(50),
    @MSSV NVARCHAR(10),
    @TienPhong MONEY,
    @TienDien MONEY,
    @TienNuoc MONEY,
    @TienGuiXe MONEY
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.HoaDon WHERE MaHD=@MaHD)
        THROW 73006, N'Hóa đơn không tồn tại.', 1;
    IF NOT EXISTS(SELECT 1 FROM dbo.SinhVien WHERE MSSV=@MSSV)
        THROW 73007, N'Sinh viên không tồn tại.', 1;
    INSERT INTO dbo.ChiTietHoaDon (MaHD,MSSV,TienPhong,TienDien,TienNuoc,TienGuiXe)
    VALUES(@MaHD,@MSSV,@TienPhong,@TienDien,@TienNuoc,@TienGuiXe);
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_ChiTietHoaDon_Xoa
    @MaChiTietHD INT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.ChiTietHoaDon WHERE MaChiTietHD=@MaChiTietHD)
        THROW 73008, N'Không tìm thấy chi tiết hóa đơn.', 1;
    DELETE FROM dbo.ChiTietHoaDon WHERE MaChiTietHD=@MaChiTietHD;
END
GO

--------------------------------------------------------------------------------
-- 2.7 Các thủ tục nghiệp vụ tiện ích (lấy báo cáo, tóm tắt)
-- (mã lỗi 74000-74999)
--------------------------------------------------------------------------------

-- Lấy báo cáo tiền phòng theo hợp đồng (view thực tế sẽ tạo ở phần 3)
CREATE OR ALTER PROCEDURE dbo.sp_BaoCaoTienPhong
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM dbo.PhanBo pb
    JOIN dbo.SinhVien sv ON sv.MSSV = pb.MSSV
    JOIN dbo.Phong p ON p.MaPhong = pb.MaPhong
    JOIN dbo.LoaiPhong lp ON lp.MaLoai = p.MaLoai;
END
GO

-- Lấy báo cáo dịch vụ tháng (điện, nước, giữ xe) - cơ bản
CREATE OR ALTER PROCEDURE dbo.sp_BaoCaoDichVu_Thang
    @Thang INT,
    @Nam INT
AS
BEGIN
    SET NOCOUNT ON;
    IF @Thang NOT BETWEEN 1 AND 12
        THROW 74001, N'Tháng không hợp lệ.', 1;
    DECLARE @GiaDien MONEY = (SELECT GiaDien FROM dbo.GiaDienNuoc WHERE ID='1');
    DECLARE @GiaNuoc MONEY = (SELECT GiaNuoc FROM dbo.GiaDienNuoc WHERE ID='1');
    IF @GiaDien IS NULL OR @GiaNuoc IS NULL
        THROW 74002, N'Chưa cấu hình giá điện/nước.', 1;

    -- tính điện/nước chia đều cho số người trong phòng tháng đó
    ;WITH cs AS (
        SELECT MaPhong, DienMoi - DienCu AS DienTT, NuocMoi - NuocCu AS NuocTT
        FROM dbo.ChiSo WHERE Thang=@Thang AND Nam=@Nam
    ), ds AS (
        SELECT pb.MaPhong, pb.MSSV
        FROM dbo.PhanBo pb
        WHERE DATEFROMPARTS(@Nam,@Thang,1) BETWEEN pb.NgayPhanBo AND DATEADD(MONTH,pb.SoThang,pb.NgayPhanBo)
    ), cnt AS (
        SELECT MaPhong, COUNT(*) AS SoNguoi FROM ds GROUP BY MaPhong
    )
    SELECT d.MaPhong, d.MSSV, sv.HoTen,
        CAST((c.DienTT * @GiaDien) / NULLIF(cnt.SoNguoi,0) AS MONEY) AS TienDien,
        CAST((c.NuocTT * @GiaNuoc) / NULLIF(cnt.SoNguoi,0) AS MONEY) AS TienNuoc,
        ISNULL(lx.GiaGiuXe,0) AS TienGuiXe
    FROM ds d
    JOIN cnt ON cnt.MaPhong=d.MaPhong
    JOIN cs c ON c.MaPhong = d.MaPhong
    JOIN dbo.SinhVien sv ON sv.MSSV = d.MSSV
    LEFT JOIN dbo.TheXe tx ON tx.MSSV = d.MSSV
    LEFT JOIN dbo.LoaiXe lx ON lx.MaLoaiXe = tx.MaLoaiXe
    ORDER BY d.MaPhong, d.MSSV;
END
GO

--------------------------------------------------------------------------------
-- END PHẦN 2
--------------------------------------------------------------------------------




/* ============================================================

   PHẦN 3 – View, Trigger, Stored Procedure nghiệp vụ, Seed dữ liệu

   ============================================================ */

/* ============================================================
   PHẦN 3.1 – VIEW BÁO CÁO
   Gồm:
     - v_BaoCaoTienPhong
     - v_BaoCaoDienNuoc
     - v_BaoCaoGiuXe
   ============================================================ */

------------------------------------------------------------
-- 3.1.1: Báo cáo tiền phòng
-- Quy ước ghi chú:
--   'Miễn' = miễn tiền phòng (MienTienPhong = 1)
--   'R'    = thanh toán đủ tiền phòng theo hợp đồng
--   'R4'   = thanh toán trước 4 tháng (còn lại chưa thanh toán)
--   'R5'   = thanh toán trước 5 tháng (còn lại chưa thanh toán)
------------------------------------------------------------
CREATE OR ALTER VIEW dbo.v_BaoCaoTienPhong
AS
SELECT 
    pb.MSSV,
    sv.HoTen,
    pb.MaPhong,
    lp.TenLoaiPhong,
    lp.GiaPhong,
    pb.SoThang,
    pb.SoDotThu,
    pb.MienTienPhong,
    ISNULL(SUM(tt.SoThangThu), 0) AS TongThangDaThu,
    CASE 
        WHEN pb.MienTienPhong = 1 THEN N'Miễn'
        WHEN ISNULL(SUM(tt.SoThangThu), 0) >= pb.SoThang THEN N'R'
        WHEN ISNULL(SUM(tt.SoThangThu), 0) * 2 = pb.SoThang THEN N'R' + CAST(ISNULL(SUM(tt.SoThangThu), 0) AS NVARCHAR)
        ELSE N'Chưa đủ'
    END AS GhiChu,
    (CASE 
        WHEN pb.MienTienPhong = 1 THEN 0
        ELSE (lp.GiaPhong / NULLIF(lp.SoLuongToiDa, 0)) * pb.SoThang
    END) AS TongTienPhong
FROM dbo.PhanBo pb
JOIN dbo.SinhVien sv ON pb.MSSV = sv.MSSV
JOIN dbo.Phong p ON pb.MaPhong = p.MaPhong
JOIN dbo.LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong
LEFT JOIN dbo.ThanhToanPhong tt ON pb.MSSV = tt.MSSV AND pb.MaPhong = tt.MaPhong
GROUP BY pb.MSSV, sv.HoTen, pb.MaPhong, lp.TenLoaiPhong, lp.GiaPhong, lp.SoLuongToiDa, pb.SoThang, pb.SoDotThu, pb.MienTienPhong;
GO


------------------------------------------------------------
-- 3.1.2: Báo cáo điện, nước
------------------------------------------------------------
CREATE OR ALTER VIEW dbo.v_BaoCaoDienNuoc
AS
SELECT 
    cs.MaPhong,
    p.MaLoaiPhong,
    lp.TenLoaiPhong,
    cs.ThangNam,
    cs.ChiSoDien,
    cs.ChiSoNuoc,
    gdn.GiaDien,
    gdn.GiaNuoc,
    (cs.ChiSoDien * gdn.GiaDien) AS TienDien,
    (cs.ChiSoNuoc * gdn.GiaNuoc) AS TienNuoc
FROM dbo.ChiSo cs
JOIN dbo.Phong p ON cs.MaPhong = p.MaPhong
JOIN dbo.LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong
CROSS APPLY (
    SELECT TOP 1 GiaDien, GiaNuoc
    FROM dbo.GiaDienNuoc
    WHERE NgayApDung <= CAST('01/' + cs.ThangNam AS DATE)
    ORDER BY NgayApDung DESC
) gdn;
GO


------------------------------------------------------------
-- 3.1.3: Báo cáo giữ xe
------------------------------------------------------------
CREATE OR ALTER VIEW dbo.v_BaoCaoGiuXe
AS
SELECT 
    tx.MaThe,
    sv.MSSV,
    sv.HoTen,
    lx.TenLoai AS LoaiXe,
    lx.GiaThang,
    tx.NgayDK,
    DATEDIFF(MONTH, tx.NgayDK, GETDATE()) + 1 AS SoThangSuDung,
    (DATEDIFF(MONTH, tx.NgayDK, GETDATE()) + 1) * lx.GiaThang AS TongTienGiuXe
FROM dbo.TheXe tx
JOIN dbo.SinhVien sv ON tx.MSSV = sv.MSSV
JOIN dbo.LoaiXe lx ON tx.MaLoaiXe = lx.MaLoaiXe;
GO


/* ============================================================
   PHẦN 3.2 – TRIGGER
   ============================================================ */

------------------------------------------------------------
-- 3.2.1: Trigger cập nhật trạng thái phòng
--  Quy ước:
--    TrangThaiPhong = 0 → Còn trống
--    TrangThaiPhong = 1 → Đang sử dụng
------------------------------------------------------------
CREATE OR ALTER TRIGGER dbo.TR_PhanBo_UpdatePhongStatus
ON dbo.PhanBo
AFTER INSERT, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Cập nhật trạng thái cho các phòng có sinh viên mới vào
    UPDATE p
    SET p.TrangThaiPhong = 1
    FROM dbo.Phong p
    WHERE p.MaPhong IN (SELECT MaPhong FROM inserted);

    -- Cập nhật trạng thái cho các phòng có sinh viên vừa rời đi (nếu hết người)
    UPDATE p
    SET p.TrangThaiPhong = 0
    FROM dbo.Phong p
    WHERE p.MaPhong IN (SELECT MaPhong FROM deleted)
      AND NOT EXISTS (
          SELECT 1 FROM dbo.PhanBo pb
          WHERE pb.MaPhong = p.MaPhong
      );
END
GO


------------------------------------------------------------
-- 3.2.2: Trigger kiểm tra chỉ số điện/nước hợp lệ
--  Không cho phép chỉ số mới < chỉ số tháng trước
------------------------------------------------------------
CREATE OR ALTER TRIGGER dbo.TR_ChiSo_CheckHopLe
ON dbo.ChiSo
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM inserted i
        CROSS APPLY (
            SELECT TOP 1 cs_prev.ChiSoDien, cs_prev.ChiSoNuoc
            FROM dbo.ChiSo cs_prev
            WHERE cs_prev.MaPhong = i.MaPhong
              AND cs_prev.ThangNam < i.ThangNam
            ORDER BY cs_prev.ThangNam DESC
        ) prev
        WHERE i.ChiSoDien < prev.ChiSoDien
           OR i.ChiSoNuoc < prev.ChiSoNuoc
    )
    BEGIN
        RAISERROR (N'Chỉ số điện hoặc nước mới không được nhỏ hơn tháng trước.', 16, 1);
    END
END
GO


/* ============================================================
   PHẦN 3.3 – STORED PROCEDURE NGHIỆP VỤ
   ============================================================ */

------------------------------------------------------------
-- 3.3.1: Tạo hóa đơn tiền phòng
--   Chia đều tiền phòng cho từng sinh viên
------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_TaoHoaDonPhong
    @MaPhong VARCHAR(10),
    @ThangNam CHAR(7) -- 'MM/YYYY'
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @GiaPhong MONEY, @SoNguoi INT;

    SELECT @GiaPhong = lp.GiaPhong,
           @SoNguoi = COUNT(pb.MSSV)
    FROM dbo.Phong p
    JOIN dbo.LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong
    JOIN dbo.PhanBo pb ON p.MaPhong = pb.MaPhong
    WHERE p.MaPhong = @MaPhong
    GROUP BY lp.GiaPhong;

    IF @SoNguoi = 0
        THROW 72001, N'Phòng không có sinh viên để tính tiền.', 1;

    DECLARE @TienMoiSV MONEY = @GiaPhong / @SoNguoi;

    -- Tạo hóa đơn tổng
    DECLARE @MaHD VARCHAR(20) = CONCAT('HD', @MaPhong, REPLACE(@ThangNam,'/',''));
    IF EXISTS (SELECT 1 FROM dbo.HoaDon WHERE MaHD = @MaHD)
        THROW 72002, N'Hóa đơn tiền phòng đã tồn tại.', 1;

    INSERT INTO dbo.HoaDon (MaHD, MaPhong, ThangNam, TongTien)
    VALUES (@MaHD, @MaPhong, @ThangNam, @GiaPhong);

    -- Thêm chi tiết hóa đơn cho từng sinh viên
    INSERT INTO dbo.ChiTietHoaDon (MaHD, MSSV, LoaiPhi, SoTien)
    SELECT @MaHD, pb.MSSV, N'Tiền phòng', @TienMoiSV
    FROM dbo.PhanBo pb
    WHERE pb.MaPhong = @MaPhong;
END
GO


------------------------------------------------------------
-- 3.3.2: Tạo hóa đơn điện nước
--   Tiền = chỉ số * giá, chia đều cho từng sinh viên
------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_TaoHoaDonDienNuoc
    @MaPhong VARCHAR(10),
    @ThangNam CHAR(7)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ChiSoDien INT, @ChiSoNuoc INT, @GiaDien MONEY, @GiaNuoc MONEY, @SoNguoi INT;

    SELECT @ChiSoDien = cs.ChiSoDien,
           @ChiSoNuoc = cs.ChiSoNuoc
    FROM dbo.ChiSo cs
    WHERE cs.MaPhong = @MaPhong AND cs.ThangNam = @ThangNam;

    IF @ChiSoDien IS NULL OR @ChiSoNuoc IS NULL
        THROW 73001, N'Chưa có chỉ số điện/nước cho tháng này.', 1;

    SELECT TOP 1 @GiaDien = GiaDien, @GiaNuoc = GiaNuoc
    FROM dbo.GiaDienNuoc
    WHERE NgayApDung <= CAST('01/' + @ThangNam AS DATE)
    ORDER BY NgayApDung DESC;

    SELECT @SoNguoi = COUNT(MSSV) FROM dbo.PhanBo WHERE MaPhong = @MaPhong;
    IF @SoNguoi = 0
        THROW 73002, N'Phòng không có sinh viên.', 1;

    DECLARE @TienDien MONEY = @ChiSoDien * @GiaDien;
    DECLARE @TienNuoc MONEY = @ChiSoNuoc * @GiaNuoc;

    DECLARE @MaHD VARCHAR(20) = CONCAT('HD_DN', @MaPhong, REPLACE(@ThangNam,'/',''));
    IF EXISTS (SELECT 1 FROM dbo.HoaDon WHERE MaHD = @MaHD)
        THROW 73003, N'Hóa đơn điện nước đã tồn tại.', 1;

    INSERT INTO dbo.HoaDon (MaHD, MaPhong, ThangNam, TongTien)
    VALUES (@MaHD, @MaPhong, @ThangNam, @TienDien + @TienNuoc);

    INSERT INTO dbo.ChiTietHoaDon (MaHD, MSSV, LoaiPhi, SoTien)
    SELECT @MaHD, pb.MSSV, N'Tiền điện', @TienDien / @SoNguoi
    FROM dbo.PhanBo pb WHERE pb.MaPhong = @MaPhong;

    INSERT INTO dbo.ChiTietHoaDon (MaHD, MSSV, LoaiPhi, SoTien)
    SELECT @MaHD, pb.MSSV, N'Tiền nước', @TienNuoc / @SoNguoi
    FROM dbo.PhanBo pb WHERE pb.MaPhong = @MaPhong;
END
GO


------------------------------------------------------------
-- 3.3.3: Tạo hóa đơn giữ xe
--   Lấy từ bảng TheXe, tính theo số tháng sử dụng
------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_TaoHoaDonGiuXe
    @MSSV NVARCHAR(10),
    @ThangNam CHAR(7)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @GiaThang MONEY, @NgayDK DATE;
    SELECT @GiaThang = lx.GiaThang, @NgayDK = tx.NgayDK
    FROM dbo.TheXe tx
    JOIN dbo.LoaiXe lx ON tx.MaLoaiXe = lx.MaLoaiXe
    WHERE tx.MSSV = @MSSV;

    IF @GiaThang IS NULL
        THROW 74001, N'Sinh viên chưa đăng ký giữ xe.', 1;

    DECLARE @SoThang INT = DATEDIFF(MONTH, @NgayDK, CAST('01/' + @ThangNam AS DATE)) + 1;
    IF @SoThang <= 0
        THROW 74002, N'Thời gian tính phí giữ xe không hợp lệ.', 1;

    DECLARE @TienXe MONEY = @GiaThang;

    DECLARE @MaHD VARCHAR(20) = CONCAT('HD_XE', @MSSV, REPLACE(@ThangNam,'/',''));
    IF EXISTS (SELECT 1 FROM dbo.HoaDon WHERE MaHD = @MaHD)
        THROW 74003, N'Hóa đơn giữ xe đã tồn tại.', 1;

    INSERT INTO dbo.HoaDon (MaHD, MaPhong, ThangNam, TongTien)
    VALUES (@MaHD, NULL, @ThangNam, @TienXe);

    INSERT INTO dbo.ChiTietHoaDon (MaHD, MSSV, LoaiPhi, SoTien)
    VALUES (@MaHD, @MSSV, N'Giữ xe', @TienXe);
END
GO


------------------------------------------------------------
-- 3.3.4: Xuất hóa đơn chi tiết cho một sinh viên
------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_XuatHoaDonSinhVien
    @MSSV NVARCHAR(10),
    @ThangNam CHAR(7)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        hd.MaHD,
        hd.ThangNam,
        cthd.LoaiPhi,
        cthd.SoTien
    FROM dbo.ChiTietHoaDon cthd
    JOIN dbo.HoaDon hd ON cthd.MaHD = hd.MaHD
    WHERE cthd.MSSV = @MSSV
      AND hd.ThangNam = @ThangNam;
END
GO


/* ============================================================
   PHẦN 3.4 – SEED DỮ LIỆU MẶC ĐỊNH
   ============================================================ */

------------------------------------------------------------
-- 3.4.1: Nhân viên & Tài khoản Admin mặc định
------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.NhanVien WHERE MaNV = 'AD001')
BEGIN
    INSERT INTO dbo.NhanVien (MaNV, HoTen, GioiTinh, NgaySinh, SDT, Email, TenDangNhap)
    VALUES ('AD001', N'Quản Trị Viên', N'Nam', '1999-01-01', NULL, NULL, 'admin');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.TaiKhoan WHERE TenDangNhap = 'admin')
BEGIN
    INSERT INTO dbo.TaiKhoan (TenDangNhap, MatKhau, MaNV, VaiTro)
    VALUES ('admin', 'admin', 'AD001', N'Admin');
END
GO


------------------------------------------------------------
-- 3.4.2: Tầng mẫu
------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.Tang)
BEGIN
    INSERT INTO dbo.Tang (MaTang, TenTang) VALUES
    ('T1', N'Tầng 1'),
    ('T2', N'Tầng 2'),
    ('T3', N'Tầng 3'),
    ('T4', N'Tầng 4');
END
GO


------------------------------------------------------------
-- 3.4.3: Loại phòng mẫu
------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.LoaiPhong)
BEGIN
    INSERT INTO dbo.LoaiPhong (MaLoaiPhong, TenLoaiPhong, GiaPhong, SoLuongToiDa) VALUES
    ('P5N', N'Phòng 5 người', 1800000, 5),
    ('P4N', N'Phòng 4 người', 1600000, 4);
END
GO


------------------------------------------------------------
-- 3.4.4: Giá điện nước mẫu
------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.GiaDienNuoc)
BEGIN
    INSERT INTO dbo.GiaDienNuoc (NgayApDung, GiaDien, GiaNuoc)
    VALUES ('2025-01-01', 2909, 21300);
END
GO


------------------------------------------------------------
-- 3.4.5: Loại xe mẫu
------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.LoaiXe)
BEGIN
    INSERT INTO dbo.LoaiXe (MaLoaiXe, TenLoai, GiaThang) VALUES
    ('XD', N'Xe đạp', 50000),
    ('XM', N'Xe máy', 100000);
END
GO


/* ============================================================
   RÀNG BUỘC KIỂM TRA GIÁ PHẢI > 0
   ============================================================ */

-- 1. Giá điện/nước
ALTER TABLE dbo.GiaDienNuoc
ADD CONSTRAINT CK_GiaDienNuoc_Positive
CHECK (GiaDien > 0 AND GiaNuoc > 0);
GO

-- 2. Giá phòng
ALTER TABLE dbo.LoaiPhong
ADD CONSTRAINT CK_LoaiPhong_Gia_Positive
CHECK (GiaPhong > 0);
GO

-- 3. Giá giữ xe
ALTER TABLE dbo.LoaiXe
ADD CONSTRAINT CK_LoaiXe_Gia_Positive
CHECK (GiaThang > 0);
GO
