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
INSERT [dbo].[Category] ([CategoryID],[CategoryName]) VALUES (4, N'GamingGear');
SET IDENTITY_INSERT [dbo].[Category] OFF
GO
SET IDENTITY_INSERT [dbo].[Customer] ON
INSERT [dbo].[Customer] ([CustomerID],[CustomerName],[CustomerPhone],[CustomerEmail],[CustomerAddress],[Username]) VALUES (1, N'Nguyễn Văn A', N'0901234567', N'a@gmail.com', N'TP.HCM', N'khach1');
SET IDENTITY_INSERT [dbo].[Customer] OFF
GO
SET IDENTITY_INSERT [dbo].[Product] ON
INSERT [dbo].[Product] ([ProductID],[CategoryID],[ProductName],[ProductDescription],[ProductPrice],[ProductImage],[Quantity]) VALUES (1, 1, N'iPhone 15', N'Điện thoại Apple', 20000000.00, N'~/Content/images/ip1.jpg', 50);
INSERT [dbo].[Product] ([ProductID],[CategoryID],[ProductName],[ProductDescription],[ProductPrice],[ProductImage],[Quantity]) VALUES (2, 1, N'Samsung Galaxy S24', N'Điện thoại Samsung', 18000000.00, N'~/Content/images/ss.jpg', 50);
INSERT [dbo].[Product] ([ProductID],[CategoryID],[ProductName],[ProductDescription],[ProductPrice],[ProductImage],[Quantity]) VALUES (3, 2, N'MacBook Air M2', N'Laptop Apple', 25000000.00, N'~/Content/images/macbook.jpg', 50);
INSERT [dbo].[Product] ([ProductID],[CategoryID],[ProductName],[ProductDescription],[ProductPrice],[ProductImage],[Quantity]) VALUES (5, 1, N'IPHONE 18', N'Điên thoại thông mình đời mới.Có nhiều loại công năng đặc dị.', 20000000.00, N'~/Content/images/ip.jpg', 50);
INSERT [dbo].[Product] ([ProductID],[CategoryID],[ProductName],[ProductDescription],[ProductPrice],[ProductImage],[Quantity]) VALUES (6, 3, N'Dây sạc nhanh Iphone', N'Sạc nhanh không nóng máy,bảo hành dài hạn 1 đổi 1 .', 300000.00, N'~/Content/images/daysac.jpg', 0);
INSERT [dbo].[Product] ([ProductID],[CategoryID],[ProductName],[ProductDescription],[ProductPrice],[ProductImage],[Quantity]) VALUES (7, 4, N'Bộ PC GAMING BỂ CÁ MID TOWER LP03', N'PC Văn Phòng - Xem Phim LP03 là dòng sản phẩm mà Long Phát Computer muốn giới thiệu tới người dùng phân khúc văn phòng phổ thông. Mang trong mình thiết kế đơn giản, hướng tới hiệu năng khi sử dụng, PC Văn Phòng - Xem Phim được chắt lọc từ các linh kiện chính hãng, đáp ứng tốt nhu cầu của người tiêu dùng, mà không cần phải bỏ ra quá nhiều chi phí.    ', 10000000.00, N'~/Content/images/pc.jpg', 50);
INSERT [dbo].[Product] ([ProductID],[CategoryID],[ProductName],[ProductDescription],[ProductPrice],[ProductImage],[Quantity]) VALUES (8, 4, N'Combo Ghế Gaming Extreme Zero V1 + Bàn Gaming Z-Desk Mặt Kính Cường Lực Z4 5.00 trên 5', N'Bàn Gaming Mặt Kính Cường Lực Z4 được thiết kế độc đáo với từng chi tiết đơn giản, tinh tế mang lại không gian cực đẹp cho góc game của bạn. Sản phẩm có thiết kế đơn giản với 1 thanh khung vuông để đỡ mặt bàn, 2 chân hình chữ Z tạo thế vững chãi, một thanh ngang để cố định cân bằng cho 2 chân chống, và 1 tấm kính cường lực dày làm mặt bàn.', 4000000.00, N'~/Content/images/banghepc.jpg', 20);
INSERT [dbo].[Product] ([ProductID],[CategoryID],[ProductName],[ProductDescription],[ProductPrice],[ProductImage],[Quantity]) VALUES (9, 3, N'Tai nghe Bluetooth', N'Tai nghe thế hệ mới', 5000000.00, N'~/Content/images/tainghe.jpg', 3);
SET IDENTITY_INSERT [dbo].[Product] OFF
GO