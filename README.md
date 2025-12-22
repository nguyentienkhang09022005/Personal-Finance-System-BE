🚀 Walleto Backend API
<p align="center"> <img src="https://img.shields.io/badge/Backend-.NET%208-blueviolet" /> <img src="https://img.shields.io/badge/Database-PostgreSQL-blue" /> <img src="https://img.shields.io/badge/Auth-JWT%20%7C%20OTP-green" /> <img src="https://img.shields.io/badge/Payment-ZaloPay-orange" /> <img src="https://img.shields.io/badge/AI-Google%20Gemini-red" /> </p>

Walleto là hệ thống backend cho ứng dụng quản lý tài chính cá nhân & đầu tư, tích hợp mạng xã hội tài chính và trợ lý AI thông minh.

📌 Table of Contents

Giới thiệu

Tính năng chính

Kiến trúc hệ thống

Công nghệ sử dụng

Cấu trúc thư mục

Xác thực & Bảo mật

Thanh toán

AI Assistant

Database Design

Cài đặt & Chạy project

API Documentation

Roadmap

Author

📖 Giới thiệu

Trong bối cảnh tài chính số phát triển mạnh mẽ, người dùng thường phải sử dụng nhiều ứng dụng khác nhau để:

Quản lý chi tiêu

Theo dõi tiết kiệm

Đầu tư crypto/chứng khoán

Tương tác cộng đồng

👉 Walleto Backend được xây dựng nhằm cung cấp một nền tảng tài chính “all-in-one”, an toàn, mở rộng tốt và sẵn sàng cho production.

✨ Tính năng chính
👤 Người dùng & Xác thực

Đăng ký / đăng nhập

Xác thực Email bằng OTP

JWT Access Token & Refresh Token

Logout & Token Blacklist

Phân quyền Role / Permission

💰 Quản lý tài chính cá nhân

Ngân sách chi tiêu (Budget)

Giao dịch Thu / Chi

Báo cáo & thống kê theo thời gian

🏦 Tiết kiệm

Tạo mục tiêu tiết kiệm

Theo dõi tiến độ %

Lịch sử đóng góp

📈 Đầu tư

Quản lý quỹ đầu tư

Crypto / Stock

Mua – Bán tài sản

Tính Profit / Loss theo Average Cost

Lấy giá thị trường real-time từ API bên thứ 3

🌐 Mạng xã hội tài chính

Đăng bài viết + Snapshot dữ liệu tài chính

Like / Comment / Rating

Kết bạn & nhắn tin

Kiểm duyệt bài viết (Admin)

🤖 Trợ lý AI – Walleto

Tư vấn tài chính thông minh

Đọc ngữ cảnh dữ liệu người dùng

Lưu lịch sử chat

Bảo mật tuyệt đối dữ liệu nhạy cảm

💳 Thanh toán

Tích hợp ZaloPay

Xác thực chữ ký HMAC SHA256

Gói dịch vụ (Basic / Premium)

Tự động kích hoạt quyền sau thanh toán

🏗 Kiến trúc hệ thống
Client (Web / Mobile)
        │
        ▼
ASP.NET Core Web API
        │
        ├── Authentication & Authorization
        ├── Finance / Investment Module
        ├── Social Module
        ├── AI Module (Gemini)
        ├── Payment Module (ZaloPay)
        │
        ▼
PostgreSQL + Redis

🛠 Công nghệ sử dụng
Thành phần	Công nghệ
Backend	ASP.NET Core 8
Database	PostgreSQL
ORM	Entity Framework Core
Cache	Redis
Auth	JWT, OTP
Payment	ZaloPay
Realtime	SignalR
AI	Google Gemini
Email	FluentEmail / SendGrid
📂 Cấu trúc thư mục
src/
│
├── Controllers/
├── Handlers/
├── Services/
├── Repositories/
├── Models/
├── DTOs/
├── Helpers/
├── Middleware/
├── Infrastructure/
├── Configurations/
└── Program.cs

🔐 Xác thực & Bảo mật

Mật khẩu mã hóa BCrypt

JWT Access Token (ngắn hạn)

Refresh Token lưu HttpOnly Cookie

Token bị thu hồi → InvalidatedToken

Phân quyền chi tiết theo Permission

AI không bao giờ truy cập ID / Token / Password

💳 Thanh toán

Cổng: ZaloPay

Kiểm tra chữ ký callback (MAC)

Trạng thái: PENDING → SUCCESS

Không cho mua lại gói đang còn hạn

Rollback khi callback thất bại

🤖 AI Assistant

Tổng hợp dữ liệu:

Budget

Transaction

Saving

Investment

Serialize JSON (ẩn dữ liệu nhạy cảm)

Prompt System riêng cho AI

Lưu lịch sử chat trong Redis

🗄 Database Design

20+ bảng quan hệ

Chuẩn hóa dữ liệu

Hỗ trợ mở rộng

Phù hợp hệ thống tài chính thực tế

Chi tiết ERD & mô tả bảng xem trong thư mục /docs

▶️ Cài đặt & Chạy project
1️⃣ Clone repo
git clone https://github.com/your-username/walleto-backend.git

2️⃣ Cấu hình appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=...;Database=...;"
  },
  "Jwt": {
    "Key": "your-secret-key"
  }
}

3️⃣ Chạy project
dotnet restore
dotnet run

📑 API Documentation

Swagger:

http://localhost:5000/swagger

🛣 Roadmap

 Dockerize backend

 CI/CD pipeline

 Multi-currency support

 Investment AI recommendation

 Microservice refactor

👨‍💻 Author

Nguyễn Tiến Khang
🎓 UIT
💡 Backend Developer
📌 .NET • Java • Spring Boot • Golang🚀 Walleto Backend API
<p align="center"> <img src="https://img.shields.io/badge/Backend-.NET%208-blueviolet" /> <img src="https://img.shields.io/badge/Database-PostgreSQL-blue" /> <img src="https://img.shields.io/badge/Auth-JWT%20%7C%20OTP-green" /> <img src="https://img.shields.io/badge/Payment-ZaloPay-orange" /> <img src="https://img.shields.io/badge/AI-Google%20Gemini-red" /> </p>

Walleto là hệ thống backend cho ứng dụng quản lý tài chính cá nhân & đầu tư, tích hợp mạng xã hội tài chính và trợ lý AI thông minh.

📌 Table of Contents

Giới thiệu

Tính năng chính

Kiến trúc hệ thống

Công nghệ sử dụng

Cấu trúc thư mục

Xác thực & Bảo mật

Thanh toán

AI Assistant

Database Design

Cài đặt & Chạy project

API Documentation

Roadmap

Author

📖 Giới thiệu

Trong bối cảnh tài chính số phát triển mạnh mẽ, người dùng thường phải sử dụng nhiều ứng dụng khác nhau để:

Quản lý chi tiêu

Theo dõi tiết kiệm

Đầu tư crypto/chứng khoán

Tương tác cộng đồng

👉 Walleto Backend được xây dựng nhằm cung cấp một nền tảng tài chính “all-in-one”, an toàn, mở rộng tốt và sẵn sàng cho production.

✨ Tính năng chính
👤 Người dùng & Xác thực

Đăng ký / đăng nhập

Xác thực Email bằng OTP

JWT Access Token & Refresh Token

Logout & Token Blacklist

Phân quyền Role / Permission

💰 Quản lý tài chính cá nhân

Ngân sách chi tiêu (Budget)

Giao dịch Thu / Chi

Báo cáo & thống kê theo thời gian

🏦 Tiết kiệm

Tạo mục tiêu tiết kiệm

Theo dõi tiến độ %

Lịch sử đóng góp

📈 Đầu tư

Quản lý quỹ đầu tư

Crypto / Stock

Mua – Bán tài sản

Tính Profit / Loss theo Average Cost

Lấy giá thị trường real-time từ API bên thứ 3

🌐 Mạng xã hội tài chính

Đăng bài viết + Snapshot dữ liệu tài chính

Like / Comment / Rating

Kết bạn & nhắn tin

Kiểm duyệt bài viết (Admin)

🤖 Trợ lý AI – Walleto

Tư vấn tài chính thông minh

Đọc ngữ cảnh dữ liệu người dùng

Lưu lịch sử chat

Bảo mật tuyệt đối dữ liệu nhạy cảm

💳 Thanh toán

Tích hợp ZaloPay

Xác thực chữ ký HMAC SHA256

Gói dịch vụ (Basic / Premium)

Tự động kích hoạt quyền sau thanh toán

🏗 Kiến trúc hệ thống
Client (Web / Mobile)
        │
        ▼
ASP.NET Core Web API
        │
        ├── Authentication & Authorization
        ├── Finance / Investment Module
        ├── Social Module
        ├── AI Module (Gemini)
        ├── Payment Module (ZaloPay)
        │
        ▼
PostgreSQL + Redis

🛠 Công nghệ sử dụng
Thành phần	Công nghệ
Backend	ASP.NET Core 8
Database	PostgreSQL
ORM	Entity Framework Core
Cache	Redis
Auth	JWT, OTP
Payment	ZaloPay
Realtime	SignalR
AI	Google Gemini
Email	FluentEmail / SendGrid
📂 Cấu trúc thư mục
src/
│
├── Controllers/
├── Handlers/
├── Services/
├── Repositories/
├── Models/
├── DTOs/
├── Helpers/
├── Middleware/
├── Infrastructure/
├── Configurations/
└── Program.cs

🔐 Xác thực & Bảo mật

Mật khẩu mã hóa BCrypt

JWT Access Token (ngắn hạn)

Refresh Token lưu HttpOnly Cookie

Token bị thu hồi → InvalidatedToken

Phân quyền chi tiết theo Permission

AI không bao giờ truy cập ID / Token / Password

💳 Thanh toán

Cổng: ZaloPay

Kiểm tra chữ ký callback (MAC)

Trạng thái: PENDING → SUCCESS

Không cho mua lại gói đang còn hạn

Rollback khi callback thất bại

🤖 AI Assistant

Tổng hợp dữ liệu:

Budget

Transaction

Saving

Investment

Serialize JSON (ẩn dữ liệu nhạy cảm)

Prompt System riêng cho AI

Lưu lịch sử chat trong Redis

🗄 Database Design

20+ bảng quan hệ

Chuẩn hóa dữ liệu

Hỗ trợ mở rộng

Phù hợp hệ thống tài chính thực tế

Chi tiết ERD & mô tả bảng xem trong thư mục /docs

▶️ Cài đặt & Chạy project
1️⃣ Clone repo
git clone https://github.com/your-username/walleto-backend.git

2️⃣ Cấu hình appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=...;Database=...;"
  },
  "Jwt": {
    "Key": "your-secret-key"
  }
}

3️⃣ Chạy project
dotnet restore
dotnet run

📑 API Documentation

Swagger:

http://localhost:5000/swagger

🛣 Roadmap

 Dockerize backend

 CI/CD pipeline

 Multi-currency support

 Investment AI recommendation

 Microservice refactor

👨‍💻 Author

Nguyễn Tiến Khang
🎓 UIT
💡 Backend Developer
📌 .NET • Java • Spring Boot • Golang
