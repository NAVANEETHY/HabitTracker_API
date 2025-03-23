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

		set @jsonStr = JSON_MODIFY(@jsonStr, '$.UserID', @UserID)
		insert into Tbl_UserInfo(UserID, UserName, Email)
		select UserID, UserName, Email
		from openjson(@jsonStr)
		with(
			UserID int,
			UserName varchar(100),
			Email varchar(100)
		)
		select N'{"Status":"Success"}'
	end try
	begin catch
		select N'{"Error":"' + ERROR_MESSAGE() +'"}'
	end catch
end

go


use HabitDB
go

create or alter procedure spGetUserInfo @jsonStr nvarchar(500)
------------------------- Fetch User details from Tbl_UserInfo ------------------------
----------------------------- Author : Navaneeth Y ------------------------------
as
begin
	begin try
		select *
		from Tbl_UserInfo
		where Email = JSON_VALUE(@jsonStr, '$.Email')
		for json path, without_array_wrapper
	end try
	begin catch
		select N'{"Error":"' + ERROR_MESSAGE() +'"}'
	end catch
end