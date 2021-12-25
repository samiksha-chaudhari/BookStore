USE BookStore;

create table Address
(
    AddressId int not null identity (1,1) primary key,
	Address varchar(600) not null,
	City varchar(50) not null,
	State varchar(50) not null,
	Type varchar(10) not null,
	UserId int
);

select * from Address;

create procedure SpAddUserAddress
        @Address varchar(600),
		@City varchar(50),
		@State varchar(50),
		@Type varchar(10),
		@UserId int
		
As 
Begin
   Insert into Address (
                Address,
				City,
				State,
				Type,
				UserId  )
		values (
		       @Address,
			   @City,
			   @State,
			   @Type,
			   @UserId);
End


create PROCEDURE UpdateUserAddress
(
@AddressID int,
@Address varchar(255),
@City varchar(50),
@State varchar(50),
@Type varchar(10),
@result int output
)
AS
BEGIN

       If exists(Select * from Address where AddressId=@AddressID)
	    begin
		  UPDATE Address
          SET 
		   Address= @Address, City = @City,
		   State=@State,
		   Type=@Type 
		 WHERE AddressId=@AddressID;
		 set @result=1;
		  end 
		  else
		  begin
		   set @result=0;
		  end
END 

