USE BookStore;

CREATE TABLE CART
(
	CartID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	BookId INT NOT NULL,
	UserId INT NOT NULL,
	Quantity INT NOT NULL
);

ALTER TABLE [CART] ADD CONSTRAINT CART_BookId_FK
FOREIGN KEY (BookId) REFERENCES [Books](BookId)

ALTER TABLE [CART] ADD CONSTRAINT CART_UserId_FK
FOREIGN KEY (UserId) REFERENCES [RegUser](UserId)




CREATE PROC spAddCart
	@BookId INT,
	@UserId INT,
	@cart INT = NULL OUTPUT
AS
BEGIN
	IF EXISTS(SELECT * FROM [CART] WHERE BookId = @BookId AND UserId = @UserId)
		SET @cart = 1;
	ELSE
	BEGIN
		IF EXISTS(SELECT * FROM [Books] WHERE BookId=@BookId)
		BEGIN
			INSERT INTO [CART](BookId, UserId, Quantity)
			VALUES (@BookId, @UserId,1)
			SET @cart = 2;
		END
		ELSE
			SET @cart = NULL;
	END
END


CREATE PROC spDeleteCart
	@CartID INT,
	@cart INT =NULL OUTPUT
AS
BEGIN
	IF EXISTS(SELECT * FROM [CART] WHERE CartID = @CartID)
	BEGIN
		DELETE FROM [CART] WHERE CartID = @CartID
		SET @cart = 1;
	END
	ELSE
	BEGIN
		SET @cart = NULL;
	END
END



CREATE PROC spUpdateCart
	@CartID INT,
	@Quantity INT,
	@cart INT = NULL OUTPUT
AS
BEGIN
	IF EXISTS(SELECT * FROM [CART] WHERE CartID = @CartID)
	BEGIN
			SET @cart = 1;
			UPDATE CART
			SET
				Quantity = @Quantity
			WHERE
				CartID = @CartID;
		END
		ELSE
		BEGIN
			SET @cart = NULL;
		END
END



CREATE PROC spGetCart
	@UserId INT
AS
BEGIN
	SELECT
		c.CartID,
		c.BookId,
		c.UserId,
		b.BookName,
		b.AuthorName,
		b.BookDescription,
		b.BookImage ,
		b.Quantity,
		b.Price,
		b.DiscountPrice,
		b.Rating,
		b.RatingCount
	FROM [CART] AS c
	LEFT JOIN [Books] AS b ON c.BookId = B.BookId
	WHERE C.UserId = @UserId
END