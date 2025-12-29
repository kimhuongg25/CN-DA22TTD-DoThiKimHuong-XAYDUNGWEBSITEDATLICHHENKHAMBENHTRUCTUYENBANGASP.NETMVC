# 🌐 XÂY DỰNG WEBSITE ĐẶT LỊCH HẸN KHÁM BỆNH TRỰC TUYẾN BẰNG ASP.NET MVC

Dự án tập trung xây dựng một nền tảng trực tuyến giúp bệnh nhân chủ động tra cứu thông tin và đặt lịch khám mọi lúc, mọi nơi.  
Hệ thống được thiết kế nhằm tối ưu hóa quy trình tiếp nhận tại các cơ sở y tế, giảm thiểu thời gian chờ đợi và nâng cao trải nghiệm chăm sóc sức khỏe cho người dân.

---

## 📌 Tính năng chính

Hệ thống được phân quyền cho **03 nhóm đối tượng** với các chức năng chuyên biệt:

### 👤 1. Đối với Bệnh nhân (Người dùng)

- **Đăng ký / Đăng nhập:**  
  Tạo tài khoản và quản lý thông tin cá nhân (họ tên, email, số điện thoại, địa chỉ,…).

- **Tra cứu thông tin:**  
  Xem danh sách bác sĩ, chuyên khoa, trình độ chuyên môn và lịch làm việc.

- **Đặt lịch khám:**  
  Lựa chọn bác sĩ hoặc chuyên khoa, ngày và khung giờ khám phù hợp.

- **Theo dõi lịch hẹn:**  
  Xem danh sách và trạng thái lịch khám (đang chờ, đã xác nhận, hoàn thành).

- **Hỏi đáp online:**  
  Gửi câu hỏi tư vấn sức khỏe và nhận phản hồi từ bác sĩ.

---

### 👨‍⚕️ 2. Đối với Bác sĩ

- **Quản lý lịch khám:**  
  Xem danh sách các lịch hẹn được phân công kèm thông tin bệnh nhân.

- **Tư vấn trực tuyến:**  
  Trả lời câu hỏi của bệnh nhân thông qua hệ thống hỏi đáp.

- **Cập nhật hồ sơ cá nhân:**  
  Quản lý thông tin cá nhân và trình độ chuyên môn.

---

### 🛡️ 3. Đối với Quản trị viên (Admin)

- **Quản lý người dùng & bác sĩ:**  
  Thêm, sửa, khóa hoặc xóa tài khoản người dùng và bác sĩ.

- **Quản lý hệ thống:**  
  Quản lý chuyên khoa, thông tin trung tâm y tế và phân quyền người dùng.

- **Quản lý nội dung:**  
  Kiểm duyệt hỏi đáp, quản lý tin tức và thông báo y tế.

---

## 🛠️ Công nghệ sử dụng

Hệ thống được xây dựng theo mô hình **ASP.NET MVC**, đảm bảo cấu trúc rõ ràng, dễ mở rộng và bảo trì.

| Thành phần | Công nghệ |
|----------|-----------|
| **Backend** | C#, ASP.NET MVC |
| **Frontend** | HTML, CSS, JavaScript, Bootstrap |
| **Cơ sở dữ liệu** | SQL Server |
| **Công cụ phát triển** | Visual Studio |

---

## 📂 Cấu trúc cơ sở dữ liệu

Hệ thống quản lý dữ liệu thông qua các bảng chính sau:

- `NguoiDung`: Lưu thông tin bệnh nhân.
- `QuanTri`: Quản lý tài khoản quản trị viên và bác sĩ.
- `LichKham`: Lưu thông tin lịch hẹn khám bệnh.
- `HoiDap`: Quản lý nội dung hỏi đáp tư vấn.
- `TinTuc`: Lưu các bài viết, thông báo y tế.
- `TrungTamGanNhat`: Thông tin các cơ sở y tế theo khu vực.
- `Huyen`, `GioiTinh`: Các bảng danh mục chuẩn hóa dữ liệu.

---

## 🚀 Hướng phát triển trong tương lai

- Tích hợp chức năng **nhắc lịch hẹn tự động** (Email/SMS).
- Phát triển **hồ sơ bệnh án điện tử** cho bệnh nhân.
- Tích hợp **tư vấn trực tuyến thời gian thực**.
- Mở rộng hệ thống sang **ứng dụng di động**.

---

## 👤 Thông tin tác giả

- **Sinh viên thực hiện:** Đỗ Thị Kim Hương  
- **Giảng viên hướng dẫn:** TS. Đoàn Phước Miền  
- **Đơn vị:** Trường Kỹ thuật và Công nghệ – Đại học Trà Vinh
