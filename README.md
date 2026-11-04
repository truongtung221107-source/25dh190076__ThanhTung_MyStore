# MyStore - Website bán hàng (ASP.NET MVC)

## Cách chạy
1. Mở file MyStore.sln bằng Visual Studio (cần .NET Framework 4.8 và SQL Server LocalDB).
2. Tạo database:
   - Vào menu View > SQL Server Object Explorer
   - Chuột phải (localdb)\MSSQLLocalDB > New Query
   - Mở file MyStore_Full.sql, copy toàn bộ nội dung dán vào, bấm Execute
   - Chỉ chạy MỘT LẦN trên máy chưa có database MyStore
3. Nhấn F5 để chạy web.

## Tài khoản dùng thử
- Trang khách hàng: https://localhost:44357/
  - Đăng nhập: khach1 / 123456 (hoặc tự đăng ký tài khoản mới)
- Trang quản trị: https://localhost:44357/Admin
  - Đăng nhập: admin / 123456

## Chức năng
- Admin: danh mục, sản phẩm (upload ảnh, tìm kiếm, lọc giá, sắp xếp, phân trang), khách hàng, tài khoản Admin, thống kê
- Khách hàng: trang chủ, chi tiết sản phẩm, giỏ hàng, đăng ký/đăng nhập, thanh toán, đơn hàng của tôi