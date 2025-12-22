# Walleto - Backend  
**Hệ thống quản lý tài chính cá nhân & đầu tư thông minh (All-in-one Personal Finance)**

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-15-blue.svg)
![Redis](https://img.shields.io/badge/Redis-7-red.svg)
![Status](https://img.shields.io/badge/status-in%20development-yellowgreen)

> Walleto là một ứng dụng đa nền tảng giúp người dùng quản lý thu chi, ngân sách, mục tiêu tiết kiệm, danh mục đầu tư (Crypto/Stock), tương tác cộng đồng tài chính và tư vấn bởi trợ lý AI thông minh.

Đây là repository **Backend** được xây dựng theo kiến trúc Clean Architecture hiện đại, bảo mật cao và dễ mở rộng.

## 🚀 Tính năng chính

### Quản lý tài chính cá nhân
- Ngân sách chi tiêu (Budget) với theo dõi tiến độ
- Ghi chép giao dịch thu/chi tự động cập nhật số dư
- Mục tiêu tiết kiệm với lịch sử đóng góp và tính tiến độ (%)

### Đầu tư & Danh mục
- Quản lý quỹ đầu tư cá nhân
- Theo dõi tài sản Crypto/Stock
- Tính toán lợi nhuận/lỗ theo phương pháp **Average Cost**
- Tích hợp API giá real-time (CoinGecko/Binance sẵn sàng mở rộng)

### Trợ lý AI thông minh
- Tích hợp **Google Gemini** làm trợ lý tài chính "Walleto"
- Context-aware: tự động tổng hợp dữ liệu ngân sách, giao dịch, đầu tư để đưa ra lời khuyên cá nhân hóa
- Bảo mật nghiêm ngặt: không lộ ID, token, password trong prompt

### Mạng xã hội tài chính
- Đăng bài chia sẻ kèm **Snapshot** dữ liệu tài chính (JSON)
- Kiểm duyệt bài đăng bởi Admin
- Like, đánh giá sao, bình luận (chỉ sửa trong 30 phút)
- Kết bạn & chat realtime qua SignalR

### Thanh toán & Premium
- Tích hợp **ZaloPay** với xác thực HMAC-SHA256
- Quản lý gói dịch vụ (Basic → Premium → VIP)
- Tự động cấp quyền khi thanh toán thành công

### Bảo mật & Xác thực
- JWT Access + Refresh Token (HttpOnly Cookie + Redis)
- OTP xác thực email (TTL 2 phút)
- Blacklist token khi logout
- Mã hóa mật khẩu bằng BCrypt

### Realtime
- SignalR cho thông báo, tin nhắn mới, duyệt bài đăng

## 🏗️ Kiến trúc hệ thống

```text
Dự án tuân thủ nghiêm ngặt Clean Architecture (Onion Architecture):

PersonalFinanceSys/
├── PersonalFinanceSys.Domain
│   └── Entities, Value Objects
│
├── PersonalFinanceSys.Application
│   └── Use Cases, Interfaces, DTOs, Handlers
│
├── PersonalFinanceSys.Infrastructure
│   └── EF Core, Repositories, External Services
│       (Google Gemini, ZaloPay, SignalR)
│
└── PersonalFinanceSys.Api
    └── Controllers, Middleware, Program.cs
```

### Các pattern quan trọng
- Repository Pattern
- Handler / Use Case Pattern (Thin Controller)
- Data Aggregation cho AI
- AutoMapper cho DTO mapping
- Standardized ApiResponse<T>

## 🛠️ Công nghệ sử dụng

| Layer           | Technology                                                                 |
|-----------------|----------------------------------------------------------------------------|
| Runtime         | .NET 8.0                                                                   |
| Web API         | ASP.NET Core                                                               |
| ORM             | Entity Framework Core                                                      |
| Database        | PostgreSQL                                                                 |
| Cache / Realtime| Redis                                                                      |
| Authentication  | JWT + Refresh Token + BCrypt                                               |
| Realtime        | SignalR                                                                    |
| AI              | Google Gemini API                                                          |
| Payment         | ZaloPay SDK (HMAC-SHA256)                                                  |
| Email           | FluentEmail / SendGrid                                                     |
| Hosting         | Railway / Docker                                                           |
| Storage ảnh     | Cloudinary (có thể thay bằng AWS S3)                                       |

## 📦 Cấu trúc thư mục
```text
src/
├── PersonalFinanceSys.Domain/
│   └── Entities/
│
├── PersonalFinanceSys.Application/
│   ├── DTOs/
│   ├── Interfaces/
│   ├── UseCases/
│   │   ├── Auth/
│   │   ├── Transaction/
│   │   ├── Investment/
│   │   ├── AI/
│   │   ├── Payment/
│   │   └── Social/
│   └── Mappings/
│
├── PersonalFinanceSys.Infrastructure/
│   ├── Data/
│   │   ├── AppDbContext.cs
│   │   └── Migrations/
│   ├── Repositories/
│   └── Services/
│       ├── GeminiService.cs
│       ├── ZaloPayService.cs
│       └── SignalRService.cs
│
├── PersonalFinanceSys.Api/
│   ├── Controllers/
│   ├── Hubs/
│   └── Program.cs
│
└── tests/
    └── PersonalFinanceSys.Tests/
```

🚀 Bắt đầu nhanh (Local Development)
📋 Yêu cầu hệ thống

.NET SDK 8.0
PostgreSQL 15+
Redis (chạy local hoặc Docker)
Google Gemini API Key
ZaloPay Sandbox Account (để test thanh toán)

```text
git clone https://github.com/nguyentienkhang09022005/Personal-Finance-System-BE.git
cd Personal-Finance-System-BE
```
