USE [master]
GO
IF (EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = N'DailyBytesDB'OR name = N'DailyBytesDB')))
DROP DATABASE DailyBytesDB

CREATE DATABASE DailyBytesDB
GO

USE DailyBytesDB
GO


IF OBJECT_ID('Users') IS NOT NULL
	DROP TABLE Users
GO

IF OBJECT_ID('Articles') IS NOT NULL
	DROP TABLE Articles
GO

IF OBJECT_ID('Categories') IS NOT NULL
	DROP TABLE Categories
GO

IF OBJECT_ID('Memberships') IS NOT NULL
	DROP TABLE Memberships
GO

CREATE TABLE Users(
	[UserId] TINYINT IDENTITY PRIMARY KEY,
	[FirstName] VARCHAR(100) NOT NULL CHECK(FirstName NOT LIKE('%[^A-Za-z]%')),
	[LastName] VARCHAR(100) NOT NULL CHECK(LastName NOT LIKE('%[^A-Za-z]%')),
	[EmailId] VARCHAR(100) UNIQUE NOT NULL,
	[UserName] VARCHAR(100) UNIQUE NOT NULL,
	[Password] VARCHAR(50) NOT NULL,
	[Gender] VARCHAR(10) NOT NULL,
	[ContactNumber] NUMERIC(10) NOT NULL,
	[DateOfBirth] DATE NOT NULL,
	[Address] VARCHAR(255) NOT NULL
);
GO

CREATE TABLE Categories(
	[CategoryId] TINYINT IDENTITY PRIMARY KEY,
	[CategoryName] VARCHAR(100) NOT NULL
);
GO

CREATE TABLE Articles(
	[ArticleId] TINYINT IDENTITY PRIMARY KEY,
	[HeadLine] VARCHAR(200) NOT NULL,
	[SubHeading] VARCHAR(300) NOT NULL,
	[CategoryId] TINYINT NOT NULL FOREIGN KEY REFERENCES Categories(CategoryId),
	[Content] NVARCHAR(MAX) NOT NULL,
	[CreatedDate] DATETIME DEFAULT GETDATE()
);
GO

CREATE TABLE Memberships(
	[MembershipId] TINYINT IDENTITY PRIMARY KEY,
	[MembershipName] VARCHAR(50) NOT NULL,
	[Price] NUMERIC(5,2) NOT NULL,
	[DurationTime] INT NOT NULL
);
GO

INSERT INTO Users VALUES
('Ajay','Rathod','ajay.rathod01@gmail.com','ajayrathods','Ajay12345','Male',9325914451,'2004-12-04','Pune, Maharashtra'),
('suchitra','mahato','suchitra.mahato@gmail.com','suchitraa','suchitra12345','Female',8745662345,'2002-01-01','West Bangal'),
('Praveen','Sharma','praveen.sharma@gmail.com','praveens','Praveen1234','Male',9876283210,'1990-05-15','Jamshedpur'),
('Jane','Doe','jane.doe@gmail.com','janedoe','Jane1234','Female',9876543210,'1990-05-15','123 Main st, Anytown'),
('John','Smith','john.smith@gmail.com','jsmith','John1234','Male',8765432109,'1985-11-20','456, Oak Ave'),
('Alice','Johnson','alice.johnson@gmail.com','alicej','Alice1234','Female',7654321098,'1998-01-01','789 Pine Ln, Cityplace'),
('Bob','Williams','bob.williams@gmail.com','bobw','Bob1234','Male',6543210987,'2001-07-25','123 Main st, Anytown'),
('Charlie','Brown','charlie.brown@gmail.com','charlieb','Charlie1234','Male',5432109876,'1950-10-02','The Doghouse, USA')
;
GO

INSERT INTO Categories VALUES
('Politics'),
('Business'),
('Education'),
('Sports');
GO

-- 1. Politics Article (ID 1)

INSERT INTO Articles (HeadLine, SubHeading, CategoryId, Content)

VALUES (

    'Understanding the New Electoral Reforms Bill',

    'A deep analysis of the proposed changes and their potential impact on voting rights.',

    1,

    'The recent tabling of the Electoral Reforms Bill has sparked national debate. Key provisions include mandatory voter ID laws and changes to campaign finance regulations. This article breaks down the complex legal language and outlines the arguments for and against the legislation...'

);

-- 2. Business Article (ID 2)

INSERT INTO Articles (HeadLine, SubHeading, CategoryId, Content)

VALUES (

    'Q3 Tech Market Report: Mixed Signals for Startups',

    'While established giants show stable growth, early-stage funding rounds face significant headwinds.',

    2,

    'The third quarter of the fiscal year presented a challenging environment for the tech sector. Layoffs continued in some large firms, yet the demand for specialized AI talent remains strong. We examine the venture capital landscape and forecast trends for the next six months...'

);

-- 3. Education Article (ID 3)

INSERT INTO Articles (HeadLine, SubHeading, CategoryId, Content)

VALUES (

    'The Rise of Personalized Learning in K-12',

    'How technology is enabling customized educational pathways for every student.',

    3,

    'Personalized learning tailors content, pace, and support to meet the individual needs of students. From adaptive testing software to project-based learning, this pedagogical approach aims to maximize student engagement and outcomes. We look at successful implementation stories across the nation...'

);

-- 4. Sports Article (ID 4)

INSERT INTO Articles (HeadLine, SubHeading, CategoryId, Content)

VALUES (

    'Record Broken at National Track Championship',

    'Local athlete, Sarah Chen, sets a new benchmark in the 400-meter dash.',

    4,

    'In a stunning display of speed and endurance, Sarah Chen shattered the long-standing record at the National Track Championship on Saturday. Her time of 49.88 seconds was the highlight of the weekend, signaling a potential new star for the upcoming international games...'

);

INSERT INTO Memberships VALUES('Standard',299,30)
INSERT INTO Memberships VALUES('Premium',599,30)
GO

SELECT * FROM Users

SELECT * FROM Articles

SELECT * FROM Categories

SELECT * FROM Memberships
GO

CREATE FUNCTION ufn_ValidateUserCredentials
(
	@EmailId VARCHAR(50),
	@Password VARCHAR(50)
)
RETURNS INT
AS
BEGIN
		IF EXISTS(
		SELECT 1 FROM Users WHERE EmailId=@EmailId AND Password=@Password)
		RETURN 1;
	RETURN 0;

END
GO

CREATE PROCEDURE usp_RegisterUser(
	@FirstName VARCHAR(100),
	@LastName VARCHAR(100),
	@EmailId VARCHAR(100),
	@UserName VARCHAR(100),
	@Password VARCHAR(50) ,
	@Gender VARCHAR(10) ,
	@ContactNumber NUMERIC(10),
	@DateOfBirth DATE,
	@Address VARCHAR(255)
)
AS
BEGIN
	DECLARE @ReturnValue INT
	BEGIN TRY
	 -- Check if username already exists
   IF EXISTS (SELECT 1 FROM Users WHERE EmailId = @EmailId)
   BEGIN
       RETURN -1;
   END
   ELSE
	BEGIN
   -- Insert user record
   INSERT INTO Users
   (
       FirstName, LastName, EmailId, UserName, [Password],
       Gender, ContactNumber, DateOfBirth, [Address]
   )
   VALUES
   (
       @FirstName, @LastName, @EmailId, @UserName, @Password,
       @Gender, @ContactNumber, @DateOfBirth, @Address
   );
   RETURN 1;
	END
   END TRY
   BEGIN CATCH
	RETURN -99
   END CATCH
END
GO

CREATE FUNCTION dbo.fn_GetArticles()
RETURNS TABLE
AS
RETURN
(
    SELECT
        ArticleId,
        HeadLine,
        SubHeading,
        CategoryId,
        Content,
        CreatedDate
    FROM Articles
);
GO

CREATE FUNCTION fn_GetArticleById
(
	@ArticleId TINYINT
)
RETURNS TABLE
AS
RETURN
(
	SELECT * FROM Articles WHERE ArticleId = @ArticleId
);
GO


---------------------------------SPRINT 1------------------------------------------

CREATE TABLE ArticleReports (
    ReportId INT IDENTITY PRIMARY KEY,
    ArticleId TINYINT NOT NULL,
    UserId TINYINT NOT NULL,
    Reason VARCHAR(100) NOT NULL,
    Comments VARCHAR(255),
    ReportedDate DATETIME DEFAULT GETDATE(),
    Status VARCHAR(20) DEFAULT 'Pending',

    FOREIGN KEY (ArticleId) REFERENCES Articles(ArticleId),
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

GO
CREATE PROCEDURE usp_ReportArticle
(
    @ArticleId TINYINT,
    @UserId TINYINT,
    @Reason VARCHAR(100),
    @Comments VARCHAR(255) = NULL
)
AS
BEGIN
    BEGIN TRY
        INSERT INTO ArticleReports
        (ArticleId, UserId, Reason, Comments)
        VALUES
        (@ArticleId, @UserId, @Reason, @Comments);

        -- Notification trigger (logical)
        -- This will be used by backend to notify editor
        RETURN 1;
    END TRY
    BEGIN CATCH
        RETURN -1;
    END CATCH
END
GO



--Scaffold-DbContext -Connection "Data Source =(localdb)\MSSQLLocalDB;Initial Catalog=DailyBytesDB;Integrated Security=true" -Provider Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -f
----------------------------------------------
        --    ADD TO FAVORITE
----------------------------------------------



CREATE TABLE Favorite
(
    [ArticleId] TINYINT 
        CONSTRAINT fk_Favorite_ArticleId 
        REFERENCES Articles(ArticleId) NOT NULL,

    [EmailId] VARCHAR(100) 
        CONSTRAINT fk_Favorite_EmailId 
        REFERENCES Users(EmailId) NOT NULL,

    CONSTRAINT pk_Favorite_ArticleId_EmailId 
        PRIMARY KEY (ArticleId, EmailId)
);
GO

CREATE PROCEDURE usp_AddArticleToFavorite
(
    @ArticleId TINYINT,
    @EmailId VARCHAR(100)
)
AS
BEGIN
    DECLARE @retval INT = -1;

    BEGIN TRY
        -- If article already exists in Favorite
        IF EXISTS (
            SELECT 1 
            FROM Favorite 
            WHERE ArticleId = @ArticleId 
              AND EmailId = @EmailId
        )
        BEGIN
            -- Already exists
            SET @retval = 2;
            SELECT @retval;
            RETURN;
        END

        -- Insert new favorite
        INSERT INTO Favorite (ArticleId, EmailId)
        VALUES (@ArticleId, @EmailId);

        -- Successfully added
        SET @retval = 1;
        SELECT @retval;
    END TRY
    BEGIN CATCH
        -- Error
        SET @retval = -99;
        SELECT @retval;
    END CATCH
END
GO



CREATE PROCEDURE usp_RemoveArticleFromFavorite
(
    @ArticleId TINYINT,
    @EmailId VARCHAR(100)
)
AS
BEGIN
    DECLARE @retval INT = -1;

    BEGIN TRY
        IF EXISTS (
            SELECT 1 
            FROM Favorite 
            WHERE ArticleId = @ArticleId 
              AND EmailId = @EmailId
        )
        BEGIN
            DELETE FROM Favorite
            WHERE ArticleId = @ArticleId
              AND EmailId = @EmailId;

            -- Removed successfully
            SET @retval = 1;
        END
        ELSE
        BEGIN
            -- Record not found
            SET @retval = 0;
        END

        SELECT @retval;
    END TRY
    BEGIN CATCH
        SET @retval = -99;
        SELECT @retval;
    END CATCH
END
GO


CREATE FUNCTION dbo.ufn_CheckFavoriteArticle
(
    @ArticleId TINYINT,
    @EmailId VARCHAR(100)
)
RETURNS BIT
AS
BEGIN
    DECLARE @Result BIT = 0;

    IF EXISTS (
        SELECT 1
        FROM Favorite
        WHERE ArticleId = @ArticleId
          AND EmailId = @EmailId
    )
        SET @Result = 1;

    RETURN @Result;
END;
GO





CREATE FUNCTION ufn_FetchFavoriteArticleByEmailId
(
    @EmailId VARCHAR(100)
)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        f.ArticleId,
        a.HeadLine,
        a.SubHeading,
        a.Content,
        a.CreatedDate,
        c.CategoryName
    FROM Favorite f
    JOIN Articles a ON f.ArticleId = a.ArticleId
    JOIN Categories c ON a.CategoryId = c.CategoryId
    WHERE f.EmailId = @EmailId
);
GO




INSERT INTO Favorite VALUES (1, 'alice.johnson@gmail.com');
INSERT INTO Favorite VALUES (4, 'alice.johnson@gmail.com');
GO

EXEC usp_AddArticleToFavorite 1, 'praveen.sharma@gmail.com';
GO

SELECT * 
FROM ufn_FetchFavoriteArticleByEmailId('praveen.sharma@gmail.com');

GO

SELECT * FROM Favorite;



EXEC usp_AddArticleToFavorite 
    @ArticleId = 1,
    @EmailId = 'ajay.rathod01@gmail.com';


SELECT dbo.ufn_CheckFavoriteArticle(1,'ajay.rathod01@gmail.com');

EXEC usp_RemoveArticleFromFavorite 
    @ArticleId = 1,
    @EmailId = 'ajay.rathod01@gmail.com';


CREATE TABLE FavouriteArticles
(
    FavouriteId BIGINT IDENTITY(1,1) PRIMARY KEY,
    UserId TINYINT NOT NULL,
    ArticleId TINYINT NOT NULL,
    CONSTRAINT FK_Fav_User FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_Fav_Article FOREIGN KEY (ArticleId)REFERENCES Articles(ArticleId),
    CONSTRAINT UQ_User_Article UNIQUE (UserId, ArticleId)
);
GO
INSERT INTO FavouriteArticles (UserId, ArticleId)VALUES (1, 1);
INSERT INTO FavouriteArticles (UserId, ArticleId)VALUES (2, 2);
INSERT INTO FavouriteArticles (UserId, ArticleId)VALUES (1, 3);
INSERT INTO FavouriteArticles (UserId, ArticleId)VALUES (2, 1);


SELECT * FROM FavouriteArticles;
GO
--------------------------------------------------
--      Stored Procedure for add to favourites
---------------------------------------------------
CREATE PROCEDURE usp_AddArticleToFavourite
(
    @UserId BIGINT,
    @ArticleId BIGINT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO FavouriteArticles (UserId, ArticleId)
        VALUES (@UserId, @ArticleId);

        RETURN 1;
    END TRY
    BEGIN CATCH
        RETURN -99; 
    END CATCH
END
GO
DECLARE @Result INT;
------------------------------------------------------
---- Add to favourite testing
------------------------------------------------------
EXEC @Result = usp_AddArticleToFavourite
    @UserId = 3,
    @ArticleId = 1;

SELECT @Result AS Result;   -- Expect: 1