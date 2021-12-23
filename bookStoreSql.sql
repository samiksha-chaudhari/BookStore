create database BookStore;
use BookStore;

Create table RegUser
(
UserId int IDENTITY(1,1) NOT NULL,
UserName varchar(50) NOT NULL,
Email varchar(50) NOT NULL,
PhoneNo varchar(12) NOT NULL,
Password varchar(50) NOT NULL
)

Create procedure usp_AddUser
(   
    @UserName VARCHAR(50),
    @Email VARCHAR(50),   
    @PhoneNo VARCHAR(12),   
	@Password VARCHAR(50) 
)   
as 
Begin    
    Insert into RegUser (UserName,Email,PhoneNo,Password)    
	Values (@UserName,@Email,@PhoneNo, @Password)    
End



CREATE PROC spUserLogin
	@Email VARCHAR(50),
	@Password VARCHAR(50),
	@user INT = NULL OUTPUT
AS
BEGIN
	IF EXISTS(SELECT * FROM RegUser WHERE Email=@Email)
	BEGIN 
		IF EXISTS(SELECT * FROM RegUser WHERE Email=@Email AND Password=@Password)
		BEGIN
			SET @user = 2;
		END
		ELSE
		BEGIN
			SET @user = 1;
		END
	END
	ELSE
	BEGIN
		SET @user = NULL;
	END
END


CREATE PROC spUserForget
	@Email VARCHAR(50),
	@user INT = NULL OUTPUT
AS
BEGIN
	IF EXISTS(SELECT * FROM RegUser WHERE Email=@Email)
	BEGIN 
		SELECT @user =UserId FROM RegUser WHERE Email=@Email;
	END
	ELSE
	BEGIN
		SET @user = NULL;
	END
END


CREATE PROC spUserReset
	@UserId INT,
	@Password VARCHAR(50)
AS
BEGIN
	UPDATE RegUser
	SET
		Password = @Password
	WHERE
		UserId = @UserId		
END