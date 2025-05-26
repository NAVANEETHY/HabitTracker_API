
create database HabitDB
go

use HabitDB
go

create table Tbl_Users(
	UserID int not null,
	UserName varchar(100) not null,
	Email varchar(100) not null unique,
	constraint Pk_Tbl_UserInfo primary key clustered(
		UserID
	)
)

go


create table Tbl_Habits(
	UserID int not null,
	TaskID int identity(1,1) not null,
	Task varchar(100) not null,
	TimeOfDay tinyint not null,
	Duration int not null,
	CreatedDate date default getdate(),
	DeletedDate date null,
	constraint Pk_Tbl_Habits primary key clustered(
		UserID,
		TaskID
	)
)

go


create table Tbl_Habits_Repeat(
	UserID int not null,
	TaskID int not null,
	Monday bit default 0 not null,
	Tuesday bit default 0 not null,
	Wednesday bit default 0 not null,
	Thursday bit default 0 not null,
	Friday bit default 0 not null,
	Saturday bit default 0 not null,
	Sunday bit default 0 not null,
	constraint Pk_Tbl_Habits_Repeat primary key clustered(
		UserID,
		TaskID
	)
)

go


create table Tbl_Habits_Status(
	UserID int not null,
	TaskID int not null,
	IsCompleted bit default 0 not null,
	IsSkipped bit default 0 not null,
	constraint Pk_Tbl_Habits_Status primary key clustered(
		UserID,
		TaskID
	)
)

go


create Table Tbl_Habits_History(
	UserID int not null,
	TaskID int not null,
	MissedDate date null,
	SkippedDate date null
)

go