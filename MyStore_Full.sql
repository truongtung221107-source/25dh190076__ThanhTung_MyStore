-- MyStore: tao database, bang va du lieu mau (da gom cac cot cua phan dat hang)
-- Chay file nay MOT LAN tren may chua co database MyStore
USE [master]
GO
CREATE DATABASE [MyStore]
GO
USE [MyStore]
GO

CREATE TABLE [dbo].[User] (
    [Username] NVARCHAR(255) NOT NULL PRIMARY KEY,
    [Password] NCHAR(50) NOT NULL,
    [UserRole] NCHAR(1) NOT NULL   -- A: admin, C: customer
);
GO
CREATE TABLE [dbo].[Customer] (
    [CustomerID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [CustomerName] NVARCHAR(MAX) NOT NULL,
    [CustomerPhone] NVARCHAR(15) NOT NULL,
    [CustomerEmail] NVARCHAR(MAX) NULL,
    [CustomerAddress] NVARCHAR(MAX) NULL,
    [Username] NVARCHAR(255) NOT NULL,
    CONSTRAINT [FK_User_Customer] FOREIGN KEY ([Username]) REFERENCES [dbo].[User]([Username])
);
GO
CREATE TABLE [dbo].[Category] (
    [CategoryID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [CategoryName] NVARCHAR(MAX) NOT NULL
);
GO
CREATE TABLE [dbo].[Product] (
    [ProductID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [CategoryID] INT NOT NULL,
    [ProductName] NVARCHAR(MAX) NOT NULL,
    [ProductDescription] NVARCHAR(MAX) NOT NULL,
    [ProductPrice] DECIMAL(18,2) NOT NULL,
    [ProductImage] NVARCHAR(MAX) NULL,
    [Quantity] INT NOT NULL DEFAULT 50,
    CONSTRAINT [FK_Pro_Category] FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[Category]([CategoryID])
);
GO
CREATE TABLE [dbo].[Order] (
    [OrderID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [CustomerID] INT NOT NULL,
    [OrderDate] DATE NOT NULL,
    [TotalAmount] DECIMAL(18,2) NOT NULL,
    [PaymentStatus] NVARCHAR(MAX) NULL,
    [ShippingAddress] NVARCHAR(MAX) NOT NULL,
    [DeliveryMethod] NVARCHAR(100) NULL,
    [PaymentMethod] NVARCHAR(100) NULL,
    FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customer]([CustomerID])
);
GO
CREATE TABLE [dbo].[OrderDetail] (
    [ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [ProductID] INT NOT NULL,
    [OrderID] INT NOT NULL,
    [Quantity] INT NOT NULL,
    [UnitPrice] DECIMAL(18,2) NOT NULL,
    [TotalPrice] AS ([Quantity] * [UnitPrice]) PERSISTED,
    FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product]([ProductID]),
    FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order]([OrderID])
);
GO

-- DU LIEU MAU
INSERT [dbo].[User] ([Username],[Password],[UserRole]) VALUES (N'admin', N'123456', N'A');
INSERT [dbo].[User] ([Username],[Password],[UserRole]) VALUES (N'khach1', N'123456', N'C');
GO
SET IDENTITY_INSERT [dbo].[Category] ON
INSERT [dbo].[Category] ([CategoryID],[CategoryName]) VALUES (1, N'Điện thoại');
INSERT [dbo].[Category] ([CategoryID],[CategoryName]) VALUES (2, N'Laptop');
INSERT [dbo].[Category] ([CategoryID],[CategoryName]) VALUES (3, N'Phụ kiện');
SET IDENTITY_INSERT [dbo].[Category] OFF
GO
SET IDENTITY_INSERT [dbo].[Customer] ON
INSERT [dbo].[Customer] ([CustomerID],[CustomerName],[CustomerPhone],[CustomerEmail],[CustomerAddress],[Username]) VALUES (1, N'Nguyễn Văn A', N'0901234567', N'a@gmail.com', N'TP.HCM', N'khach1');
SET IDENTITY_INSERT [dbo].[Customer] OFF
GO
SET IDENTITY_INSERT [dbo].[Product] ON
INSERT [dbo].[Product] ([ProductID],[CategoryID],[ProductName],[ProductDescription],[ProductPrice],[ProductImage]) VALUES (1, 1, N'iPhone 15', N'Điện thoại Apple', 20000000, N'~/Content/images/ip1.jpg');
INSERT [dbo].[Product] ([ProductID],[CategoryID],[ProductName],[ProductDescription],[ProductPrice],[ProductImage]) VALUES (2, 1, N'Samsung Galaxy S24', N'Điện thoại Samsung', 18000000, N'~/Content/images/ss.jpg');
INSERT [dbo].[Product] ([ProductID],[CategoryID],[ProductName],[ProductDescription],[ProductPrice],[ProductImage]) VALUES (3, 2, N'MacBook Air M2', N'Laptop Apple', 25000000, N'~/Content/images/macbook.jpg');
INSERT [dbo].[Product] ([ProductID],[CategoryID],[ProductName],[ProductDescription],[ProductPrice],[ProductImage]) VALUES (4, 3, N'Tai nghe Bluetooth', N'Tai nghe không dây', 500000, N'~/Content/images/tainghe.jpg');
INSERT [dbo].[Product] ([ProductID],[CategoryID],[ProductName],[ProductDescription],[ProductPrice],[ProductImage]) VALUES (5, 1, N'IPHONE 18', N'Điện thoại thông minh đời mới', 20000000, N'~/Content/images/ip.jpg');
SET IDENTITY_INSERT [dbo].[Product] OFF
GO