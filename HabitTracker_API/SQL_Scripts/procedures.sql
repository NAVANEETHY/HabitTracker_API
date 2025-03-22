use HabitDB
go

create or alter procedure spInsertUser @jsonStr nvarchar(500)
------------------------- Insert a new user into Tbl_UserInfo ------------------------
-------------------------------- Author : Navaneeth Y --------------------------------
as
begin
	begin try
		declare @UserID int
		while 1=1
		begin
			set @UserID = cast(floor(rand() * (9999 - 1000 + 1)) + 1000 as int)
			if(select count(1) from Tbl_UserInfo where UserID = 0) = 0
				break
		end

		set @jsonStr = JSON_MODIFY(@jsonStr, '$.userId', @UserID)
		insert into Tbl_UserInfo(UserID, UserName, Email)
		select userId, userName, email
		from openjson(@jsonStr)
		with(
			userId int,
			userName varchar(100),
			email varchar(100)
		)
		select N'{"status":"success"}'
	end try
	begin catch
		select N'{"error":"' + ERROR_MESSAGE() +'"}'
	end catch
end

go