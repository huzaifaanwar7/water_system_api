-- SQL Script for Water System Database Tables
-- ULTRA-ROBUST RESET SCRIPT

-- 1. DROP ALL INDEXES (to prevent "index already exists" errors if table drop fails)
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Deliveries_CustomerId' AND object_id = OBJECT_ID('Deliveries')) DROP INDEX IX_Deliveries_CustomerId ON Deliveries;
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Deliveries_Date' AND object_id = OBJECT_ID('Deliveries')) DROP INDEX IX_Deliveries_Date ON Deliveries;
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Invoices_CustomerId' AND object_id = OBJECT_ID('Invoices')) DROP INDEX IX_Invoices_CustomerId ON Invoices;
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Payments_CustomerId' AND object_id = OBJECT_ID('Payments')) DROP INDEX IX_Payments_CustomerId ON Payments;
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Users_Username' AND object_id = OBJECT_ID('Users')) DROP INDEX IX_Users_Username ON Users;
GO

-- 2. DROP ALL FOREIGN KEYS pointing TO or FROM our tables
DECLARE @sql NVARCHAR(MAX) = N'';
SELECT @sql += 'ALTER TABLE ' + QUOTENAME(SCHEMA_NAME(t.schema_id)) + '.' + QUOTENAME(t.name) + 
               ' DROP CONSTRAINT ' + QUOTENAME(f.name) + ';' + CHAR(13)
FROM sys.foreign_keys AS f
INNER JOIN sys.tables AS t ON f.parent_object_id = t.object_id
WHERE t.name IN ('Payments', 'Deliveries', 'Invoices', 'Customers', 'Users')
   OR OBJECT_NAME(f.referenced_object_id) IN ('Payments', 'Deliveries', 'Invoices', 'Customers', 'Users');

EXEC sp_executesql @sql;
GO

-- 3. DROP TABLES in order of dependency
-- Payments, Deliveries, Invoices depend on Customers.
-- Deliveries depends on Invoices.
IF OBJECT_ID('dbo.Payments', 'U') IS NOT NULL DROP TABLE dbo.Payments;
IF OBJECT_ID('dbo.Deliveries', 'U') IS NOT NULL DROP TABLE dbo.Deliveries;
IF OBJECT_ID('dbo.Invoices', 'U') IS NOT NULL DROP TABLE dbo.Invoices;
IF OBJECT_ID('dbo.Customers', 'U') IS NOT NULL DROP TABLE dbo.Customers;
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL DROP TABLE dbo.Users;
GO

-- 4. CREATE TABLES
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    Role NVARCHAR(50) DEFAULT 'Admin',
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

INSERT INTO Users (Username, Phone, Password, Role) 
VALUES ('huzaifa', '03450690300', 'huzaifa300', 'Admin');
GO

CREATE TABLE Customers (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20),
    Address NVARCHAR(500),
    Area NVARCHAR(100),
    Type NVARCHAR(50) DEFAULT 'home',
    Balance DECIMAL(18,2) DEFAULT 0,
    BottlesOut INT DEFAULT 0,
    OpeningBalance DECIMAL(18,2) DEFAULT 0,
    Notes NVARCHAR(MAX)
);
GO

CREATE TABLE Invoices (
    Id INT PRIMARY KEY IDENTITY(1,1),
    CustomerId INT NOT NULL,
    InvoiceDate DATETIME NOT NULL DEFAULT GETDATE(),
    DueDate DATETIME,
    TotalAmount DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(50) DEFAULT 'pending',
    CONSTRAINT FK_Invoices_Customers FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
);
GO

CREATE TABLE Deliveries (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Date DATETIME NOT NULL DEFAULT GETDATE(),
    CustomerId INT NOT NULL,
    Bottles19L INT DEFAULT 0,
    Bottles15L INT DEFAULT 0,
    Bottles05L INT DEFAULT 0,
    Empty19L INT DEFAULT 0,
    Empty15L INT DEFAULT 0,
    Empty05L INT DEFAULT 0,
    TotalAmount DECIMAL(18,2) NOT NULL,
    PaymentStatus NVARCHAR(50) DEFAULT 'pending',
    Notes NVARCHAR(MAX),
    InvoiceId INT,
    CONSTRAINT FK_Deliveries_Customers FOREIGN KEY (CustomerId) REFERENCES Customers(Id),
    CONSTRAINT FK_Deliveries_Invoices FOREIGN KEY (InvoiceId) REFERENCES Invoices(Id) ON DELETE SET NULL
);
GO

CREATE TABLE Payments (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Date DATETIME NOT NULL DEFAULT GETDATE(),
    CustomerId INT NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    Method NVARCHAR(50) DEFAULT 'cash',
    TxnId NVARCHAR(100),
    Notes NVARCHAR(MAX),
    CONSTRAINT FK_Payments_Customers FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
);
GO

-- 5. CREATE INDEXES
CREATE INDEX IX_Deliveries_CustomerId ON Deliveries(CustomerId);
CREATE INDEX IX_Deliveries_Date ON Deliveries(Date);
CREATE INDEX IX_Invoices_CustomerId ON Invoices(CustomerId);
CREATE INDEX IX_Payments_CustomerId ON Payments(CustomerId);
CREATE INDEX IX_Users_Username ON Users(Username);
GO
