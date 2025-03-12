CREATE DATABASE LibraryManagement4;
GO

USE LibraryManagement4;
GO

-- Bảng SinhVien: Thông tin sinh viên
CREATE TABLE SinhVien (
    MaSinhVien INT PRIMARY KEY IDENTITY(1,1),
    Ten NVARCHAR(100) NOT NULL,
	PasswordHash NVARCHAR(255) NOT NULL,
    SoDienThoai NVARCHAR(15),
    Email NVARCHAR(100),
    NgaySinh DATE,
    Lop NVARCHAR(50)
);

-- Bảng sinh viên tự đăng ký đợi admin duyệt 
CREATE TABLE SinhVienRegister (
    MaSinhVien INT PRIMARY KEY IDENTITY(1,1),
    Ten NVARCHAR(100) NOT NULL,
	PasswordHash NVARCHAR(255) NOT NULL,
    SoDienThoai NVARCHAR(15),
    Email NVARCHAR(100),
    NgaySinh DATE,
    Lop NVARCHAR(50)
);

-- Bảng Admin: Thông tin quản trị viên
CREATE TABLE Admin (
    AdminID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    HoTen NVARCHAR(100),
    SoDienThoai NVARCHAR(15),
    Email NVARCHAR(100)
);

-- Bảng Thông báo cho học sinh
CREATE TABLE ThongBao(
    ThongBaoID INT PRIMARY KEY IDENTITY,
	ThongBaoCate NVARCHAR(100),
	ThongBaoName NVARCHAR(100),
	NoiDung NVARCHAR(MAX),

);

-- Bảng TheLoai: Lưu các thể loại sách
CREATE TABLE TheLoai (
    TheLoaiID INT PRIMARY KEY IDENTITY,
    TenTheLoai NVARCHAR(50) NOT NULL
);

-- Bảng Sach: Thông tin sách trong thư viện
CREATE TABLE Sach (
    SachID INT PRIMARY KEY IDENTITY,
    TheLoaiID INT FOREIGN KEY REFERENCES TheLoai(TheLoaiID) ON DELETE SET NULL,
    Title NVARCHAR(255) NOT NULL,
    NoiDung NVARCHAR(MAX),
    TacGia NVARCHAR(100),
    SoLuong INT DEFAULT 0,
    NamXuatBan INT,
    NhaXuatBan NVARCHAR(100)
);

-- Bảng YeuCauMuonSach: Lưu yêu cầu mượn sách của sinh viên
CREATE TABLE YeuCauMuonSach (
    MaYeuCau INT PRIMARY KEY IDENTITY,
    MaSinhVien INT FOREIGN KEY REFERENCES SinhVien(MaSinhVien) ON DELETE CASCADE,
    SachID INT FOREIGN KEY REFERENCES Sach(SachID) ON DELETE CASCADE,
    AdminID INT NULL, -- Admin xử lý yêu cầu
    NgayYeuCau DATE DEFAULT GETDATE(),
    TrangThai NVARCHAR(20) CHECK (TrangThai IN ('ChoDuyet', 'DaDuyet', 'TuChoi')) DEFAULT 'ChoDuyet',
    FOREIGN KEY (AdminID) REFERENCES Admin(AdminID) ON DELETE SET NULL
);

-- Bảng MuonSach: Lưu thông tin các yêu cầu mượn đã được duyệt
CREATE TABLE MuonSach (
    MaMuon INT PRIMARY KEY IDENTITY,
	MaSinhVien INT FOREIGN KEY REFERENCES SinhVien(MaSinhVien) ON DELETE CASCADE,
	SachID INT FOREIGN KEY REFERENCES Sach(SachID) ON DELETE CASCADE,
    NgayMuon DATE DEFAULT GETDATE(),
    NgayTra DATE,
    TrangThai NVARCHAR(20) CHECK (TrangThai IN ('DangMuon', 'DaTra', 'QuaHan')) DEFAULT 'DangMuon',
    AdminID INT NULL, -- Admin đã duyệt yêu cầu
    FOREIGN KEY (AdminID) REFERENCES Admin(AdminID) ON DELETE SET NULL
);

-- Bảng LogMuonSach: Lịch sử mượn trả sách
CREATE TABLE LogMuonSach (
    LogID INT PRIMARY KEY IDENTITY,
    MaMuon INT,
    NgayThucTra DATE,
    GhiChu NVARCHAR(255),
    FOREIGN KEY (MaMuon) REFERENCES MuonSach(MaMuon) ON DELETE CASCADE
);

Go
-- Thêm 3 Admin
INSERT INTO Admin (Username, PasswordHash, HoTen, SoDienThoai, Email)
VALUES 
    ('admin1', 'passwordhash1', N'Nguyễn Văn A', '0123456789', 'admin1@example.com'),
    ('admin2', 'passwordhash2', N'Lê Thị B', '0987654321', 'admin2@example.com'),
    ('admin3', 'passwordhash3', N'Trần Văn C', '0912345678', 'admin3@example.com');

-- Thêm 3 Sinh viên

Go

INSERT INTO SinhVien (Ten,PasswordHash, SoDienThoai, Email, NgaySinh, Lop)
VALUES 
    (N'Trần Minh Đức', '1' , '0931234567', 'duc.tran@example.com', '2001-01-15', 'CNTT01'),
    (N'Nguyễn Thị Hoa','1' , '0942345678', 'hoa.nguyen@example.com', '2002-03-20', 'CNTT02'),
    (N'Lê Văn Nam','1' , '0953456789', 'nam.le@example.com', '2001-05-10', 'CNTT03');


Go
-- Thêm thể loại sách
INSERT INTO TheLoai (TenTheLoai) 
VALUES 
    (N'Tiểu thuyết'), 
    (N'Khoa học'), 
    (N'Lịch sử'), 
    (N'Tâm lý học'), 
    (N'Công nghệ thông tin');

	Go
-- Thêm 10 sách với thông tin cụ thể
INSERT INTO Sach (TheLoaiID, Title, NoiDung, TacGia, SoLuong, NamXuatBan, NhaXuatBan)
VALUES
    (1, N'Chiến Binh Cầu Vồng', N'Câu chuyện về hành trình vượt khó của những đứa trẻ ở Indonesia.', N'Andrea Hirata', 5, 2005, N'NXB Văn Học'),
    (1, N'Tiếng Chim Hót Trong Bụi Mận Gai', N'Tình yêu và số phận của cô gái Maggie.', N'Colleen McCullough', 4, 1977, N'NXB Trẻ'),
    (2, N'Lược Sử Thời Gian', N'Tìm hiểu về vũ trụ từ góc nhìn vật lý.', N'Stephen Hawking', 6, 1988, N'NXB Khoa Học'),
    (2, N'Sapiens: Lược Sử Loài Người', N'Lịch sử và tiến hóa của con người.', N'Yuval Noah Harari', 7, 2011, N'NXB Tri Thức'),
    (3, N'Lịch Sử Thế Giới', N'Toàn cảnh lịch sử nhân loại từ cổ đại đến hiện đại.', N'H. G. Wells', 3, 1920, N'NXB Giáo Dục'),
    (3, N'Đông Tây Kim Cổ', N'Khám phá về các nền văn minh trên thế giới.', N'Nguyễn Hiến Lê', 2, 1973, N'NXB Văn Hóa'),
    (4, N'Thói Quen Tốt, Thói Quen Xấu', N'Tâm lý về việc hình thành và duy trì thói quen.', N'James Clear', 8, 2018, N'NXB Lao Động'),
    (4, N'Sức Mạnh Của Tư Duy Tích Cực', N'Cách phát triển tư duy tích cực và lạc quan.', N'Norman Vincent Peale', 6, 1952, N'NXB Văn Hóa'),
    (5, N'Lập Trình Python Cơ Bản', N'Hướng dẫn lập trình Python từ căn bản đến nâng cao.', N'Nguyễn Văn Tuấn', 10, 2020, N'NXB Khoa Học Kỹ Thuật'),
    (5, N'Trí Tuệ Nhân Tạo: Từ Sơ Khai Đến Hiện Tại', N'Lịch sử và tương lai của trí tuệ nhân tạo.', N'John McCarthy', 5, 2019, N'NXB Công Nghệ Thông Tin'),
	(1, 'The Rainbow Troops', 'The story of resilient children in Indonesia.', 'Andrea Hirata', 5, 2005, 'Literature Publishing'),
    (1, 'The Thorn Birds', 'The love and fate of Maggie, an Australian girl.', 'Colleen McCullough', 4, 1977, 'Youth Publishing'),
    (2, 'A Brief History of Time', 'An exploration of the universe from a physics perspective.', 'Stephen Hawking', 6, 1988, 'Science Publishing'),
    (2, 'Sapiens: A Brief History of Humankind', 'The history and evolution of humankind.', 'Yuval Noah Harari', 7, 2011, 'Knowledge Publishing'),
    (3, 'The History of the World', 'A broad history of humanity from ancient to modern times.', 'H.G. Wells', 3, 1920, 'Education Publishing'),
    (3, 'East and West Civilizations', 'An exploration of civilizations worldwide.', 'Nguyen Hien Le', 2, 1973, 'Cultural Publishing'),
    (4, 'Atomic Habits', 'The psychology of forming and maintaining habits.', 'James Clear', 8, 2018, 'Labor Publishing'),
    (4, 'The Power of Positive Thinking', 'Developing a positive and optimistic mindset.', 'Norman Vincent Peale', 6, 1952, 'Cultural Publishing'),
    (5, 'Python Programming Basics', 'A guide to Python programming from beginner to advanced.', 'Nguyen Van Tuan', 10, 2020, 'Science and Technology Publishing'),
    (5, 'Artificial Intelligence: Past and Present', 'The history and future of artificial intelligence.', 'John McCarthy', 5, 2019, 'IT Publishing'),
    (1, 'To Kill a Mockingbird', 'A novel about racial injustice in the American South.', 'Harper Lee', 7, 1960, 'Literature Publishing'),
    (2, 'Cosmos', 'An exploration of the universe and humanity’s place in it.', 'Carl Sagan', 8, 1980, 'Science Publishing'),
    (3, 'Guns, Germs, and Steel', 'A historical analysis of civilization development.', 'Jared Diamond', 6, 1997, 'Education Publishing'),
    (4, 'Thinking, Fast and Slow', 'Insights into human thought processes and decision-making.', 'Daniel Kahneman', 9, 2011, 'Psychology Publishing'),
    (5, 'Clean Code', 'A handbook of software craftsmanship.', 'Robert C. Martin', 12, 2008, 'Software Engineering Publishing');



