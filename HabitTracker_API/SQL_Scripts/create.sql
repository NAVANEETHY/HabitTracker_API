
create database HabitDB
go

use HabitDB
go

create table Tbl_Habits(
	UserId int not null,
	Task varchar(200) not null,
	TOD int not null,
	Days varchar(7) not null,
	Duration int,
	IsCompleted tinyint default 0 not null,
	IsSkipped tinyint default 0 not null,
	Color nvarchar(20),
	CONSTRAINT Pk_Tbl_Habits PRIMARY KEY CLUSTERED (
		UserId,
		Task
	)
)
