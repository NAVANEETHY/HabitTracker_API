use HabitDB
go

create or alter procedure spInsertHabit @jsonStr nvarchar(max)
as
----------------- Author : Navaneeth Y ------------------------
--------------- Procedure to insert Habits --------------------
begin
	begin try
		declare @UserID int = JSON_VALUE(@jsonStr, '$.UserID')
		if(select count(1) from Tbl_Users where UserID = @UserID) = 1
		begin
			begin transaction
				insert into Tbl_Habits(UserID, Task, TimeOfDay, Duration)
				select UserID, Task, TimeOfDay, Duration 
				from openjson(@jsonStr)
				with(
					UserID int,
					Task varchar(100),
					TimeOfDay tinyint,
					Duration int
				)
			
				declare @TaskID int 
				select @TaskID = MAX(TaskID) from Tbl_Habits where UserID = @UserID
				declare @dayColumns nvarchar(500) = JSON_QUERY(@jsonStr, '$.Days')
				set @dayColumns = REPLACE(REPLACE(REPLACE(@dayColumns, ']', ''), '[', ''), '"', '')
				set @dayColumns = REPLACE(@dayColumns, ',', '=1,') + '=1'

				insert into Tbl_Habits_Repeat(UserID, TaskID)
				values(@UserID, @TaskID)

				insert into Tbl_Habits_Status(UserID, TaskID)
				values(@UserID, @TaskID)

				insert into Tbl_Habits_History(UserID, TaskID)
				values(@UserID, @TaskID)

				declare @sqlScript nvarchar(1000) = N'update Tbl_Habits_Repeat '
												  + 'set ' + @dayColumns
												  + 'where UserID = @UserID and TaskID = @TaskID'
				exec sp_executesql @sqlScript, N'@UserID int, @TaskID int', @UserID, @TaskID
			commit transaction
			select 'Success' as Message for json path, without_array_wrapper
		end
		else
		begin
			select 'UserID doesn''t exist' as Error for json path, without_array_wrapper
		end
	end try
	begin catch
		rollback transaction
		select ERROR_MESSAGE() as Error for json path, without_array_wrapper
	end catch
end
