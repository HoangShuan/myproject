use master
go
if  EXISTS (select name from sys.databases 
			where name = 'QLNhanVien' )
drop database QLNhanVien 
go
create database QLNhanVien
go
use QLNhanVien
go
create table PhongBan
( 
	MaPB nchar(10) PRIMARY KEY,
	TenPB nvarchar(50)
)

create table NhanVien
( 
	MaNV nchar(10),
	HoTen nvarchar(50),
	NgaySinh datetime,
	GioiTinh nchar(10),
	NgoaiNgu nvarchar(50),
	Luong money,
	MaPB nchar(10),
	constraint PK_NV primary key(MaNV, MaPB),
	constraint FK_NV_PB foreign key(MaPB)
		references PhongBan(MaPB)
)

insert into PhongBan values
(N'PB01', N'Kinh doanh'),
(N'PB02', N'Phát triển'),
(N'PB03', N'Kế toán'),
(N'PB04', N'Nhân sự');

insert into NhanVien values
(N'NV01', N'Trần Văn Thành', '2003-10-22', N'Nam', N'Anh', 10000000, N'PB02')

select * from PhongBan
select * from NhanVien

