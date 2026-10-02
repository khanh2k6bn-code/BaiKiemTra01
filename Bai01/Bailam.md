# I. PHẦN LÝ THUYẾT & CÂU HỎI NGẮN

## Câu 1: Phân biệt Value Types và Reference Types trong C#

### 1. Value Types (Kiểu giá trị)

**Value Types** là kiểu dữ liệu lưu trực tiếp giá trị của biến. Khi gán một biến Value Type cho một biến khác, **giá trị được sao chép** sang biến mới.

Ví dụ:

```csharp
int a = 10;
int b = a;

b = 20;
```

Khi đó:

* `a` vẫn có giá trị `10`.
* `b` có giá trị `20`.
* Hai biến chứa hai giá trị độc lập.

Các kiểu Value Types phổ biến:

* `int`
* `float`
* `double`
* `bool`
* `char`
* `struct`
* `enum`

### 2. Reference Types (Kiểu tham chiếu)

**Reference Types** không lưu trực tiếp đối tượng mà lưu **tham chiếu đến đối tượng** trong bộ nhớ.

Ví dụ:

```csharp
class Student
{
    public string Name;
}

Student sv1 = new Student();
sv1.Name = "An";

Student sv2 = sv1;
sv2.Name = "Bình";
```

Khi đó `sv1.Name` cũng sẽ là `"Bình"` vì `sv1` và `sv2` cùng tham chiếu đến một đối tượng.

Các Reference Types phổ biến:

* `class`
* `object`
* `string`
* `array`
* `interface`
* `delegate`

### 3. Stack và Heap

Trong cách giải thích cơ bản:

* **Stack** thường được sử dụng để lưu các biến cục bộ và thông tin liên quan đến lời gọi hàm.
* **Heap** là vùng nhớ được sử dụng để lưu các đối tượng được tạo động bằng `new`.
* Với Value Type, biến có thể nằm trên Stack hoặc bên trong một đối tượng trên Heap tùy vào ngữ cảnh. Vì vậy, không nên hiểu tuyệt đối rằng "Value Type luôn nằm trên Stack".
* Reference Type thường có đối tượng nằm trên Heap, còn biến tham chiếu chứa địa chỉ/tham chiếu đến đối tượng đó.

### Kết luận

| Đặc điểm             | Value Type                                   | Reference Type                       |
| -------------------- | -------------------------------------------- | ------------------------------------ |
| Lưu trữ              | Lưu trực tiếp giá trị                        | Lưu tham chiếu đến đối tượng         |
| Đối tượng thường nằm | Có thể ở Stack hoặc nằm trong đối tượng khác | Thường ở Heap                        |
| Khi gán biến         | Sao chép giá trị                             | Sao chép tham chiếu                  |
| Ví dụ                | `int`, `double`, `bool`, `struct`            | `class`, `string`, `array`, `object` |

---

## Câu 2: Init-only Properties (`init`) khác gì với `set` thông thường?

### 1. Thuộc tính sử dụng `set`

Với `set`, thuộc tính có thể được gán giá trị **sau khi đối tượng đã được tạo**.

Ví dụ:

```csharp
class Student
{
    public string Name { get; set; }
}

Student sv = new Student();
sv.Name = "Nguyễn Văn An";

sv.Name = "Nguyễn Văn Bình";
```

Trong trường hợp này, `Name` có thể thay đổi nhiều lần sau khi đối tượng được tạo.

### 2. Thuộc tính sử dụng `init`

`init` được giới thiệu trong **C# 9**. Nó cho phép thuộc tính được thiết lập trong quá trình khởi tạo đối tượng, nhưng **không thể gán lại sau khi quá trình khởi tạo hoàn tất**.

Ví dụ:

```csharp
class Student
{
    public string Name { get; init; }
}

Student sv = new Student
{
    Name = "Nguyễn Văn An"
};
```

Sau khi khởi tạo:

```csharp
// Không hợp lệ
sv.Name = "Nguyễn Văn Bình";
```

### 3. Trường hợp sử dụng thực tế

`init` phù hợp với những thông tin cần được thiết lập một lần và không muốn thay đổi trong suốt vòng đời của đối tượng.

Ví dụ:

```csharp
class Product
{
    public int ProductId { get; init; }
    public string ProductName { get; set; }
}
```

Khi tạo sản phẩm:

```csharp
Product p = new Product
{
    ProductId = 1001,
    ProductName = "Laptop"
};
```

`ProductId` có thể được thiết lập khi tạo đối tượng nhưng không thể thay đổi sau đó, trong khi `ProductName` vẫn có thể thay đổi.

### Kết luận

| Đặc điểm             | `set`                   | `init`                        |
| -------------------- | ----------------------- | ----------------------------- |
| Gán khi khởi tạo     | Có                      | Có                            |
| Gán sau khi khởi tạo | Có                      | Không                         |
| Phù hợp với          | Dữ liệu có thể thay đổi | Dữ liệu cần thiết lập một lần |
| Được giới thiệu      | Có từ lâu               | C# 9                          |

---

## Câu 3: Phân biệt `virtual` ở lớp cha và `override` ở lớp con trong tính Đa hình

### 1. `virtual` ở lớp cha

Từ khóa `virtual` được sử dụng để khai báo một phương thức mà **lớp con có thể ghi đè lại cách thực hiện**.

Ví dụ:

```csharp
class Animal
{
    public virtual void Sound()
    {
        Console.WriteLine("Động vật phát ra âm thanh");
    }
}
```

Phương thức `Sound()` có cách thực hiện mặc định ở lớp cha, nhưng cho phép lớp con thay đổi hành vi.

### 2. `override` ở lớp con

Từ khóa `override` được sử dụng ở lớp con để **ghi đè phương thức `virtual` hoặc `abstract` của lớp cha**.

Ví dụ:

```csharp
class Dog : Animal
{
    public override void Sound()
    {
        Console.WriteLine("Chó sủa");
    }
}
```

### 3. Thể hiện tính đa hình

```csharp
Animal animal = new Dog();
animal.Sound();
```

Mặc dù biến `animal` có kiểu `Animal`, đối tượng thực tế được tạo là `Dog`.

Kết quả:

```text
Chó sủa
```

Điều này thể hiện **đa hình (Polymorphism)**: cùng một lời gọi phương thức nhưng hành vi thực tế có thể khác nhau tùy theo đối tượng.

### Kết luận

* `virtual`: được khai báo ở **lớp cha**, cho phép phương thức được ghi đè.
* `override`: được sử dụng ở **lớp con**, để cung cấp cách triển khai mới cho phương thức của lớp cha.
* `virtual` và `override` kết hợp với nhau để hỗ trợ **đa hình động (runtime polymorphism)** trong C#.

---

## Câu 4: Tại sao thành phần `static` không thể truy xuất thông qua Object Instance?

### 1. Khái niệm `static`

Thành phần được khai báo `static` thuộc về **lớp (Class)** chứ không thuộc về một đối tượng cụ thể.

Ví dụ:

```csharp
class Student
{
    public static int Count = 0;
}
```

`Count` chỉ có **một bản dùng chung** cho toàn bộ lớp `Student`, thay vì mỗi đối tượng `Student` có một bản riêng.

Có thể truy xuất trực tiếp thông qua tên lớp:

```csharp
Student.Count++;
```

### 2. Đối tượng được tạo bằng `new`

Khi sử dụng:

```csharp
Student sv1 = new Student();
Student sv2 = new Student();
```

`sv1` và `sv2` là hai đối tượng khác nhau.

Nếu `Count` là thuộc tính thông thường:

```csharp
class Student
{
    public int Count = 0;
}
```

thì mỗi đối tượng sẽ có một `Count` riêng.

Nhưng nếu khai báo:

```csharp
public static int Count = 0;
```

thì `Count` thuộc về **class `Student`**, không thuộc riêng `sv1` hay `sv2`.

### 3. Cách truy xuất đúng

```csharp
Student.Count++;
Console.WriteLine(Student.Count);
```

Không nên truy xuất theo đối tượng:

```csharp
// Không hợp lệ
sv1.Count;
```

### 4. Tại sao?

Vì `static` đại diện cho thành viên **dùng chung ở cấp lớp**. Đối tượng được tạo bằng `new` chỉ đại diện cho một **instance cụ thể** của lớp.

Do đó:

* Thành viên thông thường → thuộc về từng object.
* Thành viên `static` → thuộc về class.
* Thành viên `static` nên được truy cập thông qua **tên lớp**.

### Ví dụ tổng hợp

```csharp
class Student
{
    public string Name { get; set; }

    public static int Count = 0;

    public Student()
    {
        Count++;
    }
}
```

Sử dụng:

```csharp
Student sv1 = new Student();
Student sv2 = new Student();
Student sv3 = new Student();

Console.WriteLine(Student.Count);
```

Kết quả:

```text
3
```

Ba đối tượng được tạo ra nhưng chỉ có **một biến `Count` dùng chung cho cả lớp**.

### Kết luận

`static` không thuộc về một Object Instance cụ thể mà thuộc về Class. Vì vậy, thành phần `static` được truy xuất thông qua **tên lớp**, giúp dữ liệu hoặc phương thức được dùng chung cho tất cả các đối tượng thuộc lớp đó.
