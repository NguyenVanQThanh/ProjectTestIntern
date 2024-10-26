# Hướng dẫn cơ bản về cài đặt và cấu hình:
## 1. Download từ repository:
Chạy câu lệnh 
`git clone https://github.com/NguyenVanQThanh/ProjectTestIntern`
ở git bash
## 2. Tạo các file cấu hình appsetting.json và sercet.json
### appsetting.json
Tạo file với các nội dung cơ bản, ngoài ra cấu hình thêm Apikey được lấy từ  https://www.weatherapi.com
### secret.json
Tạo file có nội dung như sau: </br>
```json
{
    "EMAIL_CONFIGURATION": {
        "EMAIL": "<your-email>",
        "PASSWORD": "<your-password>"
    }
}
```
## 3. Chạy chương trình
Chạy câu lệnh `dotnet watch run` và mở theo url hiển thị trên terminal

