use HabitDB
go

create or alter procedure spAddHabit @jsonStr nvarchar(500), @status int out, @statusMsg nvarchar(200) out
------------------------- Insert a new habit into Tbl_Habits -------------------------
-------------------------------- Author : Navaneeth Y --------------------------------
as
begin
	begin try
		insert into Tbl_Habits(UserId, Task, TOD, Days, Duration)
		select * from openjson(@jsonStr)
		with(
			UserId int,
			Task varchar(200),
			TOD int,
			Days varchar(7),
			Duration int
		)
		select @status = 1, @statusMsg = N'Success'
	end try
	begin catch
		select @status = 0, @statusMsg = N'DB Error - ' + ERROR_MESSAGE()
	end catch
end

go