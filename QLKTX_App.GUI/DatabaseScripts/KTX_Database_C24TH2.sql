-- 1. Tạo bảng Nhân Viên
IF OBJECT_ID('dbo.NhanVien','U') IS NULL
CREATE TABLE dbo.NhanVien (
    MaNV        VARCHAR(10)   NOT NULL PRIMARY KEY,
    HoTen       NVARCHAR(100) NOT NULL,
    GioiTinh    NVARCHAR(10)  NULL,
    NgaySinh    DATE          NULL,
    SDT         VARCHAR(20)   NULL,
    Email       VARCHAR(100)  NULL
);
GO

-- 2. Tạo bảng Tài Khoản
IF OBJECT_ID('dbo.TaiKhoan','U') IS NULL
CREATE TABLE dbo.TaiKhoan (
    MaNV        VARCHAR(10)   NOT NULL,  
    TenDangNhap VARCHAR(50)   NOT NULL,  
    MatKhau     VARCHAR(100)  NOT NULL,  
    VaiTro      NVARCHAR(20)  NOT NULL CHECK (VaiTro IN (N'Admin', N'NhanVien')), 
    TrangThai   BIT NOT NULL DEFAULT(1), 

    CONSTRAINT PK_TaiKhoan PRIMARY KEY (MaNV),                 
    CONSTRAINT UQ_TaiKhoan_TenDangNhap UNIQUE (TenDangNhap),   
    CONSTRAINT FK_TaiKhoan_NhanVien FOREIGN KEY (MaNV) 
        REFERENCES dbo.NhanVien(MaNV) ON DELETE CASCADE
);
GO

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

IF OBJECT_ID('dbo.Tang','U') IS NULL
CREATE TABLE dbo.Tang (
    MaTang  VARCHAR(10)  NOT NULL PRIMARY KEY,
    TenTang NVARCHAR(50) NOT NULL
);

IF OBJECT_ID('dbo.LoaiPhong','U') IS NULL
CREATE TABLE dbo.LoaiPhong (
    MaLoai  VARCHAR(10)  NOT NULL PRIMARY KEY,
    TenLoai NVARCHAR(50) NOT NULL,
    SucChua INT         NOT NULL CHECK (SucChua BETWEEN 1 AND 20),
    GiaPhong MONEY      NOT NULL CHECK (GiaPhong > 0)
);

IF OBJECT_ID('dbo.Phong','U') IS NULL
CREATE TABLE dbo.Phong (
    MaPhong       VARCHAR(10)  NOT NULL PRIMARY KEY,
    MaTang        VARCHAR(10)  NOT NULL,
    MaLoai        VARCHAR(10)  NOT NULL,
    SoLuongToiDa  INT          NOT NULL CHECK (SoLuongToiDa > 0),
    TrangThai     NVARCHAR(30) NOT NULL DEFAULT(N'Trống'), 
    CONSTRAINT FK_Phong_Tang      FOREIGN KEY (MaTang) REFERENCES dbo.Tang(MaTang),
    CONSTRAINT FK_Phong_LoaiPhong FOREIGN KEY (MaLoai) REFERENCES dbo.LoaiPhong(MaLoai)
);
GO

IF OBJECT_ID('dbo.PhanBo','U') IS NULL
CREATE TABLE dbo.PhanBo (
    MSSV           NVARCHAR(10) NOT NULL,
    MaPhong        VARCHAR(10)  NOT NULL,
    NgayPhanBo     DATE         NOT NULL,
    SoThang        INT          NOT NULL CHECK (SoThang BETWEEN 1 AND 60),
    GhiChu         NVARCHAR(200) NULL,
    MienTienPhong  BIT NOT NULL DEFAULT(0),                 
    SoDotThu       TINYINT NOT NULL DEFAULT(1) CHECK (SoDotThu IN (1,2)), 
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
    NgayThu     DATE         NOT NULL DEFAULT GETDATE(),
    NguoiThu    VARCHAR(10)  NULL, 
    GhiChu      NVARCHAR(200) NULL,

    TienPhong   MONEY NOT NULL,
    TienTheChan MONEY NOT NULL DEFAULT 300000,
    TongTien    AS (TienPhong + TienTheChan) PERSISTED,

    CONSTRAINT FK_TTP_SV    FOREIGN KEY (MSSV)   REFERENCES dbo.SinhVien(MSSV),
    CONSTRAINT FK_TTP_Phong FOREIGN KEY (MaPhong) REFERENCES dbo.Phong(MaPhong)
);
GO

IF OBJECT_ID('dbo.LoaiXe','U') IS NULL
CREATE TABLE dbo.LoaiXe (
    MaLoaiXe VARCHAR(10) NOT NULL PRIMARY KEY,
    TenLoai  NVARCHAR(50) NOT NULL,
    GiaGiuXe MONEY NOT NULL CHECK (GiaGiuXe > 0)
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

IF OBJECT_ID('dbo.GiaDienNuoc','U') IS NULL
CREATE TABLE dbo.GiaDienNuoc (
    ID      CHAR(1) NOT NULL CONSTRAINT PK_GiaDN PRIMARY KEY CHECK (ID='1'),
    GiaDien MONEY NOT NULL CHECK (GiaDien > 0),
    GiaNuoc MONEY NOT NULL CHECK (GiaNuoc > 0)
);

IF NOT EXISTS (SELECT 1 FROM dbo.GiaDienNuoc WHERE ID='1')
BEGIN
    INSERT INTO dbo.GiaDienNuoc(ID, GiaDien, GiaNuoc)
    VALUES('1', 2909, 21300); 
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

IF OBJECT_ID('dbo.HoaDon','U') IS NULL
CREATE TABLE dbo.HoaDon (
    MaHD        VARCHAR(20) NOT NULL PRIMARY KEY,
    MaPhong     VARCHAR(10) NOT NULL,
    Thang       INT NOT NULL CHECK (Thang BETWEEN 1 AND 12),
    Nam         INT NOT NULL CHECK (Nam >= 2000),
    NgayLap     DATE NOT NULL DEFAULT (GETDATE()),
    DienTieuThu INT  NOT NULL CHECK (DienTieuThu >= 0),
    NuocTieuThu INT  NOT NULL CHECK (NuocTieuThu >= 0),
    SoLuongXe   INT  NOT NULL CHECK (SoLuongXe >= 0),
    TienDien    MONEY NOT NULL,
    TienNuoc    MONEY NOT NULL,
    TienGuiXe   MONEY NOT NULL,
    TongTien    AS (TienDien + TienNuoc + TienGuiXe) PERSISTED,
    CONSTRAINT UQ_HD_PhongThang UNIQUE (MaPhong, Thang, Nam),
    CONSTRAINT FK_HD_Phong FOREIGN KEY (MaPhong) REFERENCES dbo.Phong(MaPhong)
);
GO



CREATE OR ALTER PROCEDURE dbo.sp_TaiKhoan_Them
    @MaNV        VARCHAR(10),
    @TenDangNhap VARCHAR(50),
    @MatKhau     VARCHAR(100),
    @VaiTro      NVARCHAR(20),
    @TrangThai   BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiểm tra nhân viên đã có tài khoản chưa
    IF EXISTS(SELECT 1 FROM dbo.TaiKhoan WHERE MaNV = @MaNV)
    BEGIN
        RAISERROR(N'❌ Nhân viên này đã có tài khoản!', 16, 1);
        RETURN;
    END

    -- Kiểm tra trùng tên đăng nhập
    IF EXISTS(SELECT 1 FROM dbo.TaiKhoan WHERE TenDangNhap = @TenDangNhap)
    BEGIN
        RAISERROR(N'❌ Tên đăng nhập đã tồn tại!', 16, 1);
        RETURN;
    END

    -- Kiểm tra vai trò hợp lệ
    IF @VaiTro NOT IN (N'Admin', N'NhanVien')
    BEGIN
        RAISERROR(N'❌ Vai trò không hợp lệ!', 16, 1);
        RETURN;
    END

    -- Thêm mới
    INSERT INTO dbo.TaiKhoan (MaNV, TenDangNhap, MatKhau, VaiTro, TrangThai)
    VALUES (@MaNV, @TenDangNhap, @MatKhau, @VaiTro, @TrangThai);
END
GO


CREATE OR ALTER PROCEDURE dbo.sp_TaiKhoan_Sua
    @MaNV        VARCHAR(10),
    @TenDangNhap VARCHAR(50),
    @MatKhau     VARCHAR(100),
    @VaiTro      NVARCHAR(20),
    @TrangThai   BIT
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiểm tra tồn tại
    IF NOT EXISTS (SELECT 1 FROM dbo.TaiKhoan WHERE MaNV = @MaNV)
    BEGIN
        RAISERROR(N'❌ Không tìm thấy tài khoản.', 16, 1);
        RETURN;
    END

    -- Kiểm tra vai trò hợp lệ
    IF @VaiTro NOT IN (N'Admin', N'NhanVien')
    BEGIN
        RAISERROR(N'❌ Vai trò không hợp lệ.', 16, 1);
        RETURN;
    END

    -- Cập nhật
    UPDATE dbo.TaiKhoan
    SET TenDangNhap = @TenDangNhap,
        MatKhau     = @MatKhau,
        VaiTro      = @VaiTro,
        TrangThai   = @TrangThai
    WHERE MaNV = @MaNV;
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
    @MaNV VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    -- 1. Kiểm tra tài khoản có tồn tại không
    IF NOT EXISTS (SELECT 1 FROM dbo.TaiKhoan WHERE MaNV = @MaNV)
    BEGIN
        RAISERROR(N'Không tìm thấy tài khoản.', 16, 1);
        RETURN;
    END;

    -- 2. Kiểm tra nhân viên có gắn tài khoản không
    IF EXISTS (SELECT 1 FROM dbo.NhanVien WHERE MaNV = @MaNV)
    BEGIN
        RAISERROR(N'Tài khoản đang được gắn cho nhân viên; không thể xóa.', 16, 1);
        RETURN;
    END;

    -- 3. Xóa tài khoản
    DELETE FROM dbo.TaiKhoan WHERE MaNV = @MaNV;
END;
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

CREATE OR ALTER PROCEDURE dbo.sp_NhanVien_Them
    @MaNV VARCHAR(10),
    @HoTen NVARCHAR(100),
    @GioiTinh NVARCHAR(10) = NULL,
    @NgaySinh DATE = NULL,
    @SDT VARCHAR(20) = NULL,
    @Email VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiểm tra trùng mã NV
    IF EXISTS(SELECT 1 FROM dbo.NhanVien WHERE MaNV = @MaNV)
    BEGIN
        RAISERROR(N'Nhân viên đã tồn tại.', 16, 1);
        RETURN;
    END;

    -- Thêm nhân viên
    INSERT INTO dbo.NhanVien(MaNV, HoTen, GioiTinh, NgaySinh, SDT, Email)
    VALUES(@MaNV, @HoTen, @GioiTinh, @NgaySinh, @SDT, @Email);
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_NhanVien_Sua
    @MaNV VARCHAR(10),
    @HoTen NVARCHAR(100),
    @GioiTinh NVARCHAR(10) = NULL,
    @NgaySinh DATE = NULL,
    @SDT VARCHAR(20) = NULL,
    @Email VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiểm tra tồn tại
    IF NOT EXISTS(SELECT 1 FROM dbo.NhanVien WHERE MaNV = @MaNV)
    BEGIN
        RAISERROR(N'❌ Không tìm thấy nhân viên.', 16, 1);
        RETURN;
    END;

    -- Cập nhật
    UPDATE dbo.NhanVien
    SET HoTen = @HoTen,
        GioiTinh = @GioiTinh,
        NgaySinh = @NgaySinh,
        SDT = @SDT,
        Email = @Email
    WHERE MaNV = @MaNV;

    PRINT N'✅ Cập nhật nhân viên thành công!';
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_NhanVien_Xoa
    @MaNV VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiểm tra tồn tại
    IF NOT EXISTS(SELECT 1 FROM dbo.NhanVien WHERE MaNV = @MaNV)
    BEGIN
        RAISERROR(N'❌ Nhân viên không tồn tại.', 16, 1);
        RETURN;
    END;

    -- Nếu nhân viên đang có tài khoản thì KHÔNG cho xóa
    IF EXISTS(SELECT 1 FROM dbo.TaiKhoan WHERE MaNV = @MaNV)
    BEGIN
        RAISERROR(N'❌ Không thể xóa nhân viên vì đang có tài khoản.', 16, 1);
        RETURN;
    END;

    DELETE FROM dbo.NhanVien WHERE MaNV = @MaNV;

    PRINT N'✅ Xóa nhân viên thành công!';
END;
GO


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
    @GiaPhong MONEY
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS(SELECT 1 FROM dbo.LoaiPhong WHERE MaLoai=@MaLoai)
        THROW 63001, N'Loại phòng đã tồn tại.', 1;
    IF @SucChua IS NULL OR @SucChua <= 0
        THROW 63002, N'Sức chứa phải > 0.', 1;
    IF @GiaPhong IS NULL OR @GiaPhong <= 0
        THROW 63003, N'Giá phòng không hợp lệ.', 1;
    INSERT INTO dbo.LoaiPhong (MaLoai,TenLoai,SucChua,GiaPhong)
    VALUES(@MaLoai,@TenLoai,@SucChua,@GiaPhong);
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

CREATE OR ALTER PROCEDURE sp_Phong_CapNhatTrangThai
    @MaPhong VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SoLuongHienTai INT, @SoLuongToiDa INT;

    -- Đếm số sinh viên hiện tại trong phòng qua bảng PhanBo
    SELECT @SoLuongHienTai = COUNT(*)
    FROM PhanBo
    WHERE MaPhong = @MaPhong;

    -- Lấy số lượng tối đa từ bảng Phong
    SELECT @SoLuongToiDa = SoLuongToiDa
    FROM Phong
    WHERE MaPhong = @MaPhong;

    -- Cập nhật trạng thái phòng
    IF @SoLuongHienTai = 0
        UPDATE Phong SET TrangThai = N'Trống' WHERE MaPhong = @MaPhong;
    ELSE IF @SoLuongHienTai >= @SoLuongToiDa
        UPDATE Phong SET TrangThai = N'Đầy' WHERE MaPhong = @MaPhong;
    ELSE
        UPDATE Phong SET TrangThai = N'Còn chỗ' WHERE MaPhong = @MaPhong;
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

    IF EXISTS(
        SELECT 1 FROM dbo.PhanBo pb
        WHERE pb.MSSV = @MSSV
          AND DATEADD(MONTH, pb.SoThang, pb.NgayPhanBo) > @NgayPhanBo
    )
        THROW 65009, N'Sinh viên đang có hợp đồng còn hiệu lực, không thể phân thêm.', 1;

    DECLARE @SucChua INT = (SELECT SoLuongToiDa FROM dbo.Phong WHERE MaPhong=@MaPhong);
    DECLARE @DangO INT = (SELECT COUNT(*) FROM dbo.PhanBo WHERE MaPhong=@MaPhong);
    IF @DangO >= @SucChua
        THROW 65003, N'Phòng đã đầy.', 1;

    INSERT INTO dbo.PhanBo (MSSV,MaPhong,NgayPhanBo,SoThang,GhiChu,MienTienPhong,SoDotThu)
    VALUES (@MSSV,@MaPhong,@NgayPhanBo,@SoThang,@GhiChu,@MienTienPhong,@SoDotThu);
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_PhanBo_Sua
    @MSSV NVARCHAR(10),       -- Sinh viên
    @MaPhong VARCHAR(10),     -- Phòng
    @NgayPhanBo DATE,         -- Ngày bắt đầu phân bổ
    @SoThang INT,             -- Thời gian thuê (tháng)
    @SoDotThu TINYINT = 1,    -- 1 hoặc 2
    @MienTienPhong BIT = 0,   -- Miễn phí hay không
    @GhiChu NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- 1. Kiểm tra tồn tại phân bổ
    IF NOT EXISTS (SELECT 1 FROM dbo.PhanBo WHERE MSSV=@MSSV AND MaPhong=@MaPhong)
    BEGIN
        THROW 65010, N'Không tìm thấy phân bổ để sửa.', 1;
    END

    -- 2. Kiểm tra hợp lệ của tham số
    IF @SoThang IS NULL OR @SoThang <= 0
        THROW 65011, N'Số tháng hợp đồng phải > 0.', 1;
    IF @SoDotThu NOT IN (1,2)
        THROW 65012, N'Số đợt thu chỉ được là 1 hoặc 2.', 1;

    -- 3. Cập nhật phân bổ
    UPDATE dbo.PhanBo
    SET NgayPhanBo    = @NgayPhanBo,
        SoThang       = @SoThang,
        SoDotThu      = @SoDotThu,
        MienTienPhong = @MienTienPhong,
        GhiChu        = @GhiChu
    WHERE MSSV=@MSSV AND MaPhong=@MaPhong;

    PRINT N'Cập nhật phân bổ thành công!';
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_PhanBo_ChuyenPhong
    @MSSV NVARCHAR(10),
    @MaPhongCu VARCHAR(10),
    @MaPhongMoi VARCHAR(10),
    @NgayPhanBo DATE,
    @SoThang INT,
    @SoDotThu TINYINT = 1,
    @MienTienPhong BIT = 0,
    @GhiChu NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- 1. Kiểm tra phân bổ cũ có tồn tại
    IF NOT EXISTS (SELECT 1 FROM dbo.PhanBo WHERE MSSV=@MSSV AND MaPhong=@MaPhongCu)
    BEGIN
        THROW 65020, N'❌ Không tìm thấy phân bổ ở phòng cũ.', 1;
    END

    -- 2. Kiểm tra phòng mới có tồn tại
    IF NOT EXISTS (SELECT 1 FROM dbo.Phong WHERE MaPhong=@MaPhongMoi)
    BEGIN
        THROW 65021, N'❌ Phòng mới không tồn tại.', 1;
    END

    -- 3. Xóa phân bổ cũ
    DELETE FROM dbo.PhanBo WHERE MSSV=@MSSV AND MaPhong=@MaPhongCu;

    -- 4. Thêm phân bổ mới
    INSERT INTO dbo.PhanBo(MSSV, MaPhong, NgayPhanBo, SoThang, SoDotThu, MienTienPhong, GhiChu)
    VALUES(@MSSV, @MaPhongMoi, @NgayPhanBo, @SoThang, @SoDotThu, @MienTienPhong, @GhiChu);

    PRINT N'✅ Chuyển phòng thành công!';
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

    -- Kiểm tra phân bổ
    IF NOT EXISTS(SELECT 1 FROM dbo.PhanBo WHERE MSSV=@MSSV AND MaPhong=@MaPhong)
        THROW 66001, N'Sinh viên chưa phân bổ phòng này.', 1;

    -- Kiểm tra số tháng
    IF @SoThangThu IS NULL OR @SoThangThu <= 0
        THROW 66002, N'Số tháng thu phải > 0.', 1;

    -- Kiểm tra người thu
    IF @NguoiThu IS NOT NULL AND NOT EXISTS(SELECT 1 FROM dbo.NhanVien WHERE MaNV=@NguoiThu)
        THROW 66003, N'Người thu không tồn tại.', 1;

    -- Số tháng hợp đồng
    DECLARE @SoThangHopDong INT = (SELECT SoThang FROM dbo.PhanBo WHERE MSSV=@MSSV AND MaPhong=@MaPhong);
    DECLARE @DaThu INT = ISNULL((SELECT SUM(SoThangThu) FROM dbo.ThanhToanPhong WHERE MSSV=@MSSV AND MaPhong=@MaPhong),0);

    IF @DaThu + @SoThangThu > @SoThangHopDong
        THROW 66005, N'Thanh toán vượt quá số tháng hợp đồng.', 1;

    -- Lấy giá phòng & sức chứa
    DECLARE @GiaPhong MONEY, @SucChua INT;
    SELECT @GiaPhong = lp.GiaPhong, @SucChua = lp.SucChua
    FROM dbo.Phong p JOIN dbo.LoaiPhong lp ON p.MaLoai=lp.MaLoai
    WHERE p.MaPhong=@MaPhong;

    -- Tính tiền phòng cho sinh viên này
    DECLARE @TienPhong MONEY = (@GiaPhong / @SucChua) * @SoThangThu;

    -- Thêm bản ghi
    INSERT INTO dbo.ThanhToanPhong (MSSV,MaPhong,SoThangThu,NgayThu,NguoiThu,GhiChu,TienPhong)
    VALUES(@MSSV,@MaPhong,@SoThangThu,@NgayThu,@NguoiThu,@GhiChu,@TienPhong);
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

CREATE OR ALTER PROCEDURE dbo.sp_HoaDon_Them
    @MaHD VARCHAR(20),
    @MaPhong VARCHAR(10),
    @Thang INT,
    @Nam INT,
    @NgayLap DATE = NULL,
    @DienTieuThu INT,
    @NuocTieuThu INT,
    @SoLuongXe INT,
    @TienDien MONEY,
    @TienNuoc MONEY,
    @TienGuiXe MONEY
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS(SELECT 1 FROM dbo.HoaDon WHERE MaHD=@MaHD)
        THROW 73001, N'Mã hóa đơn đã tồn tại.', 1;

    IF EXISTS(SELECT 1 FROM dbo.HoaDon WHERE MaPhong=@MaPhong AND Thang=@Thang AND Nam=@Nam)
        THROW 73002, N'Phòng đã có hóa đơn tháng này.', 1;

    IF NOT EXISTS(SELECT 1 FROM dbo.Phong WHERE MaPhong=@MaPhong)
        THROW 73003, N'Phòng không tồn tại.', 1;

    IF @NgayLap IS NULL SET @NgayLap = GETDATE();

    INSERT INTO dbo.HoaDon(MaHD,MaPhong,Thang,Nam,NgayLap,
                           DienTieuThu,NuocTieuThu,SoLuongXe,
                           TienDien,TienNuoc,TienGuiXe)
    VALUES(@MaHD,@MaPhong,@Thang,@Nam,@NgayLap,
           @DienTieuThu,@NuocTieuThu,@SoLuongXe,
           @TienDien,@TienNuoc,@TienGuiXe);
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_HoaDon_Xoa
    @MaHD VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.HoaDon WHERE MaHD=@MaHD)
        THROW 73004, N'Không tìm thấy hóa đơn.', 1;

    DELETE FROM dbo.HoaDon WHERE MaHD=@MaHD;
END
GO


/* ==========================================
   SP: Thống kê TỔNG HỢP Doanh thu
   ========================================== */
IF OBJECT_ID('dbo.sp_ThongKe_TongHop','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_ThongKe_TongHop;
GO

CREATE PROCEDURE dbo.sp_ThongKe_TongHop
    @TuNgay DATE,
    @DenNgay DATE,
    @Nam INT
AS
BEGIN
    SET NOCOUNT ON;

    /* ================== 1. Doanh thu TIỀN PHÒNG ================== */
    SELECT 
        ISNULL(SUM(TongTien), 0) AS DoanhThuPhong
    INTO #tmpPhong
    FROM dbo.ThanhToanPhong
    WHERE NgayThu BETWEEN @TuNgay AND @DenNgay;

    /* ================== 2. Doanh thu DỊCH VỤ ================== */
    SELECT 
        ISNULL(SUM(TienDien), 0)  AS DoanhThuDien,
        ISNULL(SUM(TienNuoc), 0)  AS DoanhThuNuoc,
        ISNULL(SUM(TienGuiXe), 0) AS DoanhThuXe
    INTO #tmpDichVu
    FROM dbo.HoaDon
    WHERE NgayLap BETWEEN @TuNgay AND @DenNgay;

    /* ================== 3. Doanh thu THEO THÁNG ================== */
    SELECT 
        m.Thang,
        ISNULL(SUM(m.DoanhThu), 0) AS DoanhThu
    INTO #tmpThang
    FROM
    (
        SELECT 
            MONTH(NgayThu) AS Thang, 
            SUM(TongTien) AS DoanhThu
        FROM dbo.ThanhToanPhong
        WHERE YEAR(NgayThu) = @Nam
        GROUP BY MONTH(NgayThu)

        UNION ALL

        SELECT 
            Thang, 
            SUM(TienDien + TienNuoc + TienGuiXe) AS DoanhThu
        FROM dbo.HoaDon
        WHERE Nam = @Nam
        GROUP BY Thang
    ) m
    GROUP BY m.Thang
    ORDER BY m.Thang;

    /* ========== Xuất kết quả ra 3 ResultSet ========== */
    SELECT * FROM #tmpPhong;
    SELECT * FROM #tmpDichVu;
    SELECT * FROM #tmpThang;

    /* Xóa bảng tạm */
    DROP TABLE #tmpPhong, #tmpDichVu, #tmpThang;
END
GO


-- 1. Nhân viên & Tài khoản Admin
IF NOT EXISTS (SELECT 1 FROM dbo.NhanVien WHERE MaNV = 'AD001')
BEGIN
    INSERT INTO dbo.NhanVien (MaNV, HoTen, GioiTinh, NgaySinh, SDT, Email)
    VALUES ('AD001', N'Quản Trị Viên', N'Nam', '1999-01-01', NULL, NULL);
END

IF NOT EXISTS (SELECT 1 FROM dbo.TaiKhoan WHERE TenDangNhap = 'admin')
BEGIN
    INSERT INTO dbo.TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThai, MaNV)
    VALUES ('admin', 'admin', N'Admin', 1, 'AD001');
END
GO


/* =============================
   TRIGGER CẬP NHẬT TRẠNG THÁI PHÒNG
   ============================= */
CREATE OR ALTER TRIGGER dbo.TR_PhanBo_UpdatePhongStatus
ON dbo.PhanBo
AFTER INSERT, DELETE, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH Affected AS (
        SELECT MaPhong FROM inserted
        UNION
        SELECT MaPhong FROM deleted
    ),
    SoLuong AS (
        SELECT 
            p.MaPhong,
            COUNT(pb.MSSV) AS SoSV,
            lp.SucChua
        FROM dbo.Phong p
        JOIN dbo.LoaiPhong lp ON p.MaLoai = lp.MaLoai
        LEFT JOIN dbo.PhanBo pb ON p.MaPhong = pb.MaPhong
        WHERE p.MaPhong IN (SELECT MaPhong FROM Affected)
        GROUP BY p.MaPhong, lp.SucChua
    )
    UPDATE p
    SET p.TrangThai = CASE 
                        WHEN s.SoSV = 0 THEN N'Trống'
                        WHEN s.SoSV < s.SucChua THEN N'Còn chỗ'
                        ELSE N'Đầy'
                      END
    FROM dbo.Phong p
    JOIN SoLuong s ON p.MaPhong = s.MaPhong;
END
GO



/* =============================
   TRIGGER KIỂM TRA CHỈ SỐ ĐIỆN NƯỚC
   ============================= */
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
            SELECT TOP 1 cs_prev.DienMoi, cs_prev.NuocMoi
            FROM dbo.ChiSo cs_prev
            WHERE cs_prev.MaPhong = i.MaPhong
              AND (cs_prev.Nam < i.Nam OR (cs_prev.Nam = i.Nam AND cs_prev.Thang < i.Thang))
            ORDER BY cs_prev.Nam DESC, cs_prev.Thang DESC
        ) prev
        WHERE i.DienMoi < prev.DienMoi
           OR i.NuocMoi < prev.NuocMoi
    )
    BEGIN
        RAISERROR (N'Chỉ số điện hoặc nước mới không được nhỏ hơn tháng trước.', 16, 1);
        ROLLBACK TRANSACTION;
    END
END
GO


/* =============================
   VIEW BÁO CÁO TIỀN PHÒNG
   ============================= */
CREATE OR ALTER VIEW dbo.v_BaoCaoTienPhong
AS
SELECT 
    pb.MSSV,
    sv.HoTen,
    pb.MaPhong,
    lp.TenLoai,
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
        ELSE (lp.GiaPhong / NULLIF(lp.SucChua, 0)) * pb.SoThang
    END) AS TongTienPhong
FROM dbo.PhanBo pb
JOIN dbo.SinhVien sv ON pb.MSSV = sv.MSSV
JOIN dbo.Phong p ON pb.MaPhong = p.MaPhong
JOIN dbo.LoaiPhong lp ON p.MaLoai = lp.MaLoai
LEFT JOIN dbo.ThanhToanPhong tt ON pb.MSSV = tt.MSSV AND pb.MaPhong = tt.MaPhong
GROUP BY pb.MSSV, sv.HoTen, pb.MaPhong, lp.TenLoai, lp.GiaPhong, lp.SucChua, pb.SoThang, pb.SoDotThu, pb.MienTienPhong;
GO


/* =============================
   VIEW BÁO CÁO ĐIỆN NƯỚC
   ============================= */
CREATE OR ALTER VIEW dbo.v_BaoCaoDienNuoc
AS
SELECT 
    cs.MaPhong,
    p.MaLoai,
    lp.TenLoai,
    cs.Thang,
    cs.Nam,
    cs.DienMoi,
    cs.NuocMoi,
    gdn.GiaDien,
    gdn.GiaNuoc,
    cs.DienTieuThu * gdn.GiaDien AS TienDien,
    cs.NuocTieuThu * gdn.GiaNuoc AS TienNuoc
FROM dbo.ChiSo cs
JOIN dbo.Phong p ON cs.MaPhong = p.MaPhong
JOIN dbo.LoaiPhong lp ON p.MaLoai = lp.MaLoai
CROSS JOIN (SELECT GiaDien, GiaNuoc FROM dbo.GiaDienNuoc WHERE ID='1') gdn;
GO


/* =============================
   VIEW BÁO CÁO GIỮ XE
   ============================= */
CREATE OR ALTER VIEW dbo.v_BaoCaoGiuXe
AS
SELECT 
    tx.MaThe,
    sv.MSSV,
    sv.HoTen,
    p.MaPhong,
    lx.TenLoai AS LoaiXe,
    lx.GiaGiuXe,
    tx.NgayDangKy,
    DATEDIFF(MONTH, tx.NgayDangKy, GETDATE()) + 1 AS SoThangSuDung,
    (DATEDIFF(MONTH, tx.NgayDangKy, GETDATE()) + 1) * lx.GiaGiuXe AS TongTienGiuXe
FROM dbo.TheXe tx
JOIN dbo.SinhVien sv ON tx.MSSV = sv.MSSV
JOIN dbo.PhanBo pb ON sv.MSSV = pb.MSSV
JOIN dbo.Phong p ON pb.MaPhong = p.MaPhong
JOIN dbo.LoaiXe lx ON tx.MaLoaiXe = lx.MaLoaiXe;
GO


