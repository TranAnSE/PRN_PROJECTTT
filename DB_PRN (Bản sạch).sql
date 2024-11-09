USE [master]
GO

/*******************************************************************************
   Drop database if it exists
********************************************************************************/
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'[QLCUAHANG]')
BEGIN
	ALTER DATABASE [QLCUAHANG] SET OFFLINE WITH ROLLBACK IMMEDIATE;
	ALTER DATABASE [QLCUAHANG] SET ONLINE;
	DROP DATABASE [QLCUAHANG];
END

GO

CREATE DATABASE [QLCUAHANG]
GO

USE [QLCUAHANG]
GO

/*******************************************************************************
	Drop tables if exists
*******************************************************************************/
DECLARE @sql nvarchar(MAX) 
SET @sql = N'' 

SELECT @sql = @sql + N'ALTER TABLE ' + QUOTENAME(KCU1.TABLE_SCHEMA) 
    + N'.' + QUOTENAME(KCU1.TABLE_NAME) 
    + N' DROP CONSTRAINT ' -- + QUOTENAME(rc.CONSTRAINT_SCHEMA)  + N'.'  -- not in MS-SQL
    + QUOTENAME(rc.CONSTRAINT_NAME) + N'; ' + CHAR(13) + CHAR(10) 
FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS RC 

INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KCU1 
    ON KCU1.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG  
    AND KCU1.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA 
    AND KCU1.CONSTRAINT_NAME = RC.CONSTRAINT_NAME 

EXECUTE(@sql) 

GO
DECLARE @sql2 NVARCHAR(max)=''

SELECT @sql2 += ' Drop table ' + QUOTENAME(TABLE_SCHEMA) + '.'+ QUOTENAME(TABLE_NAME) + '; '
FROM   INFORMATION_SCHEMA.TABLES
WHERE  TABLE_TYPE = 'BASE TABLE'

Exec Sp_executesql @sql2 
GO 

-- Create Tables
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserRole NVARCHAR(20),
    Name NVARCHAR(MAX),
    Address NVARCHAR(MAX),
    Phone VARCHAR(20),
    Email NVARCHAR(200),
    UserName NVARCHAR(100),
    Password NVARCHAR(MAX),
    Avatar IMAGE,
    ManagerId INT,
    Salary INT,
    SalaryDate DATE
);

CREATE TABLE Customer (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(MAX),
    Address NVARCHAR(MAX),
    Phone VARCHAR(20),
    Email NVARCHAR(200),
    Point INT
);

CREATE TABLE Supplier (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(MAX),
    Address NVARCHAR(MAX),
    Phone VARCHAR(20),
    Email NVARCHAR(MAX)
);

CREATE TABLE Product (
    Barcode VARCHAR(20) PRIMARY KEY,
    Title NVARCHAR(MAX),
    Image IMAGE,
    Type NVARCHAR(50),
    ProductionSite VARCHAR(20)
);

CREATE TABLE InputInfo (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    InputDate DATE,
    UserId INT,
    SupplierId INT,
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (SupplierId) REFERENCES Supplier(Id)
);

CREATE TABLE Consignment (
    InputInfoId INT,
    ProductId VARCHAR(20),
    Stock INT,
    ManufacturingDate DATE,
    ExpiryDate DATE,
    InputPrice INT,
    OutputPrice INT,
    Discount FLOAT,
    InStock INT,
    CONSTRAINT PK_Consignment PRIMARY KEY (InputInfoId, ProductId),
    CONSTRAINT FK_InputInfo FOREIGN KEY (InputInfoId) REFERENCES InputInfo(Id),
    CONSTRAINT FK_Product FOREIGN KEY (ProductId) REFERENCES Product(Barcode),
    CONSTRAINT CHK_Date CHECK (ManufacturingDate < ExpiryDate)
);

CREATE TABLE Bill (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    BillDate DATETIME,
    CustomerId INT,
    UserId INT,
    Price INT,
    Discount INT,
    FOREIGN KEY (CustomerId) REFERENCES Customer(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE BillDetail (
    BillId INT,
    ProductId VARCHAR(20),
    Quantity INT,
    TotalPrice INT,
    CONSTRAINT PK_BillDetail PRIMARY KEY (BillId, ProductId),
    FOREIGN KEY (BillId) REFERENCES Bill(Id),
    FOREIGN KEY (ProductId) REFERENCES Product(Barcode)
);

CREATE TABLE BlockVoucher (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ReleaseName VARCHAR(50),
    TypeVoucher INT,
    ParValue INT,
    StartDate DATE,
    FinishDate DATE
);

CREATE TABLE Voucher (
    Code VARCHAR(30) PRIMARY KEY,
    Status INT,
    BlockId INT,
    FOREIGN KEY (BlockId) REFERENCES BlockVoucher(Id)
);

CREATE TABLE SalaryBill (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SalaryBillDate DATE,
    UserId INT,
    TotalMoney INT,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE Report (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(50) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    SubmittedAt SMALLDATETIME NOT NULL,
    RepairCost INT,
    StartDate SMALLDATETIME,
    FinishDate SMALLDATETIME,
    StaffId INT NOT NULL,
    Image IMAGE,
    FOREIGN KEY (StaffId) REFERENCES Users(Id)
);
-- Create Indexes
CREATE UNIQUE NONCLUSTERED INDEX Idx_UserAccount_NotNull 
ON Users(UserName) WHERE UserName IS NOT NULL;

CREATE UNIQUE NONCLUSTERED INDEX Idx_UserEmail_NotNull 
ON Users(Email) WHERE Email IS NOT NULL;

CREATE UNIQUE NONCLUSTERED INDEX Idx_UserPhone_NotNull 
ON Users(Phone) WHERE Phone IS NOT NULL;

CREATE UNIQUE NONCLUSTERED INDEX Idx_CustomerEmail_NotNull 
ON Customer(Email) WHERE Email IS NOT NULL;

CREATE UNIQUE NONCLUSTERED INDEX Idx_CustomerPhone_NotNull 
ON Customer(Phone) WHERE Phone IS NOT NULL;
