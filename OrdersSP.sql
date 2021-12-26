create table Orders
(
         OrderId int not null identity (1,1) primary key,
		 UserId int,
		 AddressId int,
	     BookId int,
		 Price int,
		 Quantity int
);

ALTER TABLE [Orders] ADD CONSTRAINT Orders_BookId_FK
FOREIGN KEY (BookId) REFERENCES [Books](BookId)

ALTER TABLE [Orders] ADD CONSTRAINT Orders_UserId_FK
FOREIGN KEY (UserId) REFERENCES [RegUser](UserId)

ALTER TABLE [Orders] ADD CONSTRAINT Orders_AddressId_FK
FOREIGN KEY (AddressId) REFERENCES [Address](AddressId)


CREATE PROC spAddOrder
	@BookId INT,
	@UserId INT,
	@AddressId INT,
	@Quantity INT,
	@order INT =NULL OUTPUT
AS
BEGIN
	IF EXISTS(SELECT * FROM[Books] WHERE BookId = @BookId)
	BEGIN
		IF EXISTS(SELECT * FROM[Books] WHERE BookId = @BookId AND Quantity < @Quantity)
		BEGIN
			SET @order = 3;
		END
		ELSE IF EXISTS(SELECT * FROM[Books] WHERE BookId = @BookId AND Quantity >= @Quantity)
		BEGIN
			DECLARE @unitPrice INT;
			DECLARE @available INT;
			SELECT @unitPrice = DiscountPrice, @available = Quantity FROM [Books] WHERE BookId = @BookId;

			INSERT INTO [Orders]
			VALUES(@UserId, @AddressId, @BookId, (@unitPrice*@Quantity), @Quantity);

			UPDATE [Books]
			SET
				Quantity = (@available - @Quantity)
			WHERE
				BookId = @BookId;

			DELETE FROM [CART] WHERE BookId = @BookId AND UserId = @UserId;
			SET @order = 2;
		END
		ELSE
			SET @order = 1;
	END
	ELSE
	BEGIN
		SET @order = NULL;
	END
END



CREATE PROC spGetOrder
	@UserId INT
AS
BEGIN
	SELECT * FROM [Orders] WHERE UserId = @UserId
END