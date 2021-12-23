USE BookStore;

create table Books
(
	BookId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	BookName VARCHAR(255) NOT NULL,
	AuthorName VARCHAR(255) NOT NULL,
	BookDescription VARCHAR(255) NOT NULL,
	BookImage VARCHAR(255) NOT NULL,
	Quantity INT NOT NULL,
	Price INT NOT NULL,
	DiscountPrice INT NOT NULL,
	Rating INT,
	RatingCount INT
);

CREATE PROC spAddBook
	@BookName VARCHAR(255),
	@AuthorName VARCHAR(255),
	@BookDescription VARCHAR(255),
	@BookImage VARCHAR(255),
	@Quantity INT,
	@Price INT,
	@DiscountPrice INT,
	@Rating INT,
	@RatingCount INT,
	@book INT = NULL OUTPUT
AS
BEGIN
	IF EXISTS(SELECT * FROM [Books] WHERE BookName = @BookName )
		SET @book=NULL;
	ELSE
		INSERT INTO [Books](BookName, AuthorName, BookDescription, BookImage, Quantity, Price, DiscountPrice, Rating, RatingCount)
		VALUES(@BookName, @AuthorName, @BookDescription, @BookImage, @Quantity, @Price, @DiscountPrice, @Rating, @RatingCount)
		SET @book = SCOPE_IDENTITY();
END



CREATE PROC spGetBook
AS
BEGIN 
	SELECT * FROM [Books]
END



CREATE PROC spGetSpecificBook
	@BookId INT
AS
BEGIN 
	SELECT * FROM [Books]
	WHERE BookId = @BookId
END



CREATE PROC spUpdateBook
    @BookId INT,
	@BookName VARCHAR(255),
	@AuthorName VARCHAR(255),
	@BookDescription VARCHAR(255),
	@BookImage VARCHAR(255),
	@Quantity INT,
	@Price INT,
	@DiscountPrice INT,
	@book INT = NULL OUTPUT
AS
BEGIN
	IF EXISTS(SELECT * FROM [Books] WHERE BookId = @BookId )
	BEGIN
		SET @book = @BookId
		UPDATE [Books]
		SET
			BookName = CASE WHEN @BookName='' THEN BookName ELSE @BookName END,
			AuthorName = CASE WHEN @AuthorName='' THEN AuthorName ELSE @AuthorName END, 
			BookDescription= CASE WHEN @BookDescription='' THEN BookDescription ELSE @BookDescription END, 
			BookImage =CASE WHEN @BookImage='' THEN BookImage ELSE @BookImage END, 
			Quantity = @Quantity, 
			Price = CASE WHEN @Price='0' THEN Price ELSE @Price END, 
			DiscountPrice = CASE WHEN @DiscountPrice='0' THEN DiscountPrice ELSE @DiscountPrice END
		WHERE
			BookId = @BookId;
	END
	ELSE
	BEGIN
		SET @book =NULL;
	END
END



CREATE PROC spDeleteBook
	@BookId INT,
	@book INT = NULL OUTPUT
AS
BEGIN
	IF EXISTS(SELECT * FROM [Books] WHERE BookId = @BookId)
	BEGIN
		DELETE FROM [Books] WHERE BookId = @BookId
		SET @book=1;
	END
	ELSE
	BEGIN
		SET @book = NULL;
	END
END