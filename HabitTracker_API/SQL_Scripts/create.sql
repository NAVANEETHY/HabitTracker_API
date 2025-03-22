
create database HabitDB
go

use HabitDB
go

create table Tbl_UserInfo(
	UserID int not null,
	UserName varchar(100) not null,
	Email varchar(100) not null unique,
	constraint Pk_Tbl_UserInfo primary key clustered(
		UserID
	)
)

go
