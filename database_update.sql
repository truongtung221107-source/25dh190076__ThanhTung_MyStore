-- Chay SAU file database2024 de cap nhat bang Order va OrderDetail
ALTER TABLE [dbo].[Order] ADD DeliveryMethod NVARCHAR(100) NULL;
ALTER TABLE [dbo].[Order] ADD PaymentMethod NVARCHAR(100) NULL;
EXEC sp_rename 'dbo.[Order].AddressDelivery', 'ShippingAddress', 'COLUMN';
ALTER TABLE [dbo].[OrderDetail] ADD TotalPrice AS (Quantity * UnitPrice) PERSISTED;