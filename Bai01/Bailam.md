# PHẦN I: LÝ THUYẾT & CÂU HỎI NGẮN

## Câu 1: Sự khác nhau giữa Value Types và Reference Types trong C# (Stack vs Heap)

### 1. Cơ chế lưu trữ vùng nhớ (Stack vs Heap)
- **Value Types (Kiểu giá trị):**
  - **Vùng nhớ:** Được lưu trữ trực tiếp trên **Stack** (Bộ nhớ ngăn xếp) khi khai báo là biến cục bộ trong phương thức.
  - **Cơ chế:** Biến chứa **trực tiếp giá trị** dữ liệu.
  - **Quản lý bộ nhớ:** Stack được quản lý tự động theo cơ chế LIFO (Last In, First Out). Bộ nhớ được giải phóng ngay lập tức khi biến vượt ra khỏi phạm vi (scope) thực thi.
  - **Ví dụ:** Các kiểu dữ liệu cơ bản như `int`, `float`, `double`, `bool`, `char`, `struct`, `enum`.

- **Reference Types (Kiểu tham chiếu):**
  - **Vùng nhớ:** Được phân bổ trên **Heap** (Bộ nhớ đống). Tuy nhiên, biến/con trỏ (tham chiếu) đến địa chỉ vùng nhớ đó lại được lưu trên **Stack**.
  - **Cơ chế:** Biến không lưu dữ liệu thực sự mà chỉ lưu **địa chỉ (đối tượng tham chiếu)** trỏ tới vùng nhớ trên Heap.
  - **Quản lý bộ nhớ:** Được quản lý tự động bởi trình thu gom rác **Garbage Collector (GC)** của .NET Runtime.
  - **Ví dụ:** `string`, `object`, `class`, `interface`, `delegate`, `Array`.

---

### 2. Bảng so sánh chi tiết

| Tiêu chí | Value Types (Kiểu giá trị) | Reference Types (Kiểu tham chiếu) |
| :--- | :--- | :--- |
| **Vùng nhớ chính** | **Stack** (hoặc nằm trong Heap nếu là field của Class) | **Heap** (biến tham chiếu nằm trên Stack) |
| **Nội dung lưu giữ** | Lưu trữ trực tiếp **dữ liệu/giá trị** | Lưu trữ **địa chỉ ô nhớ** (tham chiếu) trỏ đến dữ liệu trên Heap |
| **Cơ chế gán (=)** | Copy toàn bộ giá trị sang ô nhớ mới (độc lập nhau). | Copy địa chỉ tham chiếu (cả 2 biến cùng trỏ đến 1 đối tượng). |
| **Tốc độ truy xuất** | Rất nhanh (do bản chất của Stack). | Chậm hơn một chút (do phải thông qua địa chỉ tham chiếu). |
| **Giá trị mặc định / Null** | Mặc định là giá trị `0`, `false` (Không thể nhận `null` trừ khi dùng `Nullable<T>`). | Có thể nhận giá trị `null` (chưa trỏ tới đối tượng nào). |
| **Quản lý bộ nhớ** | Tự động thu hồi ngay khi ra khỏi scope. | Do Garbage Collector (GC) quét và thu hồi khi không còn tham chiếu. |

---

### 3. Minh họa bằng mã nguồn C#

```csharp
// --- Value Types ---
int a = 10;
int b = a; // Copy giá trị 10 sang b
b = 20;    // Thay đổi b KHÔNG làm thay đổi a (a vẫn = 10)

// --- Reference Types ---
class Person { public string Name { get; set; } }

Person p1 = new Person { Name = "An" };
Person p2 = p1; // p2 và p1 cùng trỏ tới 1 vùng nhớ trên Heap
p2.Name = "Bình"; // Thay đổi qua p2 làm p1.Name cũng thay đổi thành "Bình"
