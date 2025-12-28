# Football Manager - Hệ thống quản lý sân bóng đá mini

## Tổng quan
Đây là phiên bản ASP.NET Core MVC được chuyển đổi từ project React/TypeScript gốc, giữ nguyên giao diện và tính năng.

## Tính năng chính

### Cho khách hàng:
- **Trang chủ**: Hiển thị danh sách sân bóng với tìm kiếm và lọc theo giá
- **Chi tiết sân**: Xem thông tin chi tiết sân bóng và đặt sân
- **Đăng ký/Đăng nhập**: Hệ thống xác thực người dùng
- **Quản lý booking**: Xem lịch đặt sân, hủy booking

### Cho admin:
- **Dashboard**: Thống kê tổng quan (số sân, booking, khách hàng, doanh thu)
- **Quản lý sân bóng**: Xem danh sách sân bóng
- **Quản lý booking**: Xác nhận, hủy, hoàn thành booking
- **Quản lý khách hàng**: Xem danh sách khách hàng

## Công nghệ sử dụng
- **Backend**: ASP.NET Core 9.0 MVC
- **Database**: SQL Server với Entity Framework Core
- **Authentication**: ASP.NET Core Identity
- **Frontend**: Bootstrap 5, Font Awesome, jQuery
- **UI**: Giao diện responsive tương tự React version

## Cấu trúc project
```
FootballManagerMVC/
├── Controllers/           # MVC Controllers
│   ├── HomeController.cs
│   ├── BookingController.cs
│   └── AdminController.cs
├── Models/               # Data Models
│   ├── ApplicationUser.cs
│   ├── Field.cs
│   ├── Booking.cs
│   └── ViewModels/
├── Views/                # Razor Views
│   ├── Home/
│   ├── Booking/
│   ├── Admin/
│   └── Shared/
├── Areas/Identity/       # Identity Pages
│   └── Pages/Account/
├── Data/                 # Database Context
│   └── ApplicationDbContext.cs
└── wwwroot/             # Static files
```

## Cài đặt và chạy

### Yêu cầu hệ thống:
- .NET 9.0 SDK
- SQL Server hoặc SQL Server LocalDB

### Các bước chạy:

1. **Cài đặt dependencies:**
```bash
dotnet restore
```

2. **Cập nhật connection string** trong `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FootballManagerDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

3. **Tạo database:**
```bash
dotnet ef database update
```
Hoặc database sẽ được tạo tự động khi chạy lần đầu.

4. **Chạy ứng dụng:**
```bash
dotnet run
```

5. **Truy cập ứng dụng:**
- URL: https://localhost:5001 hoặc http://localhost:5029
- Admin account: admin@footballmanager.vn / Admin123!

## Dữ liệu mẫu
Project đã được cấu hình với:
- 3 sân bóng mẫu với thông tin chi tiết
- Tài khoản admin mặc định
- Roles: Admin và Customer

## Tính năng đã hoàn thành
✅ Chuyển đổi hoàn toàn từ React sang ASP.NET Core MVC
✅ Giữ nguyên giao diện và UX như bản gốc
✅ Hệ thống đăng ký/đăng nhập với Identity
✅ Quản lý sân bóng và booking
✅ Admin dashboard với thống kê
✅ Responsive design với Bootstrap
✅ Seed data tự động

## Lưu ý
- Project hiện tại có một số lỗi build liên quan đến Razor Pages cần được sửa
- Cần hoàn thiện thêm validation và error handling
- Có thể mở rộng thêm tính năng báo cáo, thanh toán online

## Liên hệ
Nếu có vấn đề gì trong quá trình chạy project, vui lòng liên hệ để được hỗ trợ.