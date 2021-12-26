use BookStore;

create table Wishlist
(
	WishlistId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	BookId INT NOT NULL,
	UserId INT NOT NULL
);


ALTER TABLE [Wishlist] ADD CONSTRAINT Wishlist_BookId_FK
FOREIGN KEY (BookId) REFERENCES [Books](BookId)

ALTER TABLE [Wishlist] ADD CONSTRAINT Wishlist_UserId_FK
FOREIGN KEY (UserId) REFERENCES [RegUser](UserId)



CREATE PROC spAddWishlist
	@BookId INT,
	@UserId INT,
	@wishlist INT = NULL OUTPUT
AS
BEGIN 
	IF EXISTS(SELECT * FROM [Wishlist] WHERE BookId = @BookId AND UserId = @UserId)
		SET @wishlist = 1;
	ELSE
	BEGIN
		IF EXISTS(SELECT * FROM [Books] WHERE BookId = @BookId)
		BEGIN
			INSERT INTO [Wishlist](BookId, UserId)
			VALUES (@BookId, @UserId)
			SET @wishlist = 2;
		END
		ELSE
			SET @wishlist = NULL;
	END
END