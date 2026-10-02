using System;
using System.Text;

namespace AutoSpeedLogistics
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            QuanLyPhuongTien ql = new QuanLyPhuongTien();

            Console.WriteLine("================ CHẠY KỊCH BẢN KIỂM THỬ (TEST CASES) ================\n");

            // TC01: Kiểm tra Validation Năm sản xuất
            Console.WriteLine("[TC01] Kiểm tra Validation NamSanXuat = 1850:");
            try
            {
                OTo otoLoi = new OTo("OT001", "Toyota", 1850, 500000000m, 5, 2.0);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"-> Bắt lỗi thành công: {ex.Message}");
            }

            // Khởi tạo dữ liệu hợp lệ cho TC02 & TC03
            OTo oto5Cho = new OTo("OT002", "Toyota Camry", 2023, 1000000000m, 5, 2.5); // TC02
            XeMay xeMay150 = new XeMay("XM001", "Honda SH", 2024, 50000000m, 150);     // TC03

            // TC02: Kiểm tra Giá Lăn Bánh Ô tô 5 chỗ
            Console.WriteLine("\n[TC02] Giá lăn bánh Ô tô 5 chỗ (Gốc 1 tỷ):");
            Console.WriteLine($"-> Giá lăn bánh: {oto5Cho.TinhGiaLanBanh():N0} VNĐ (Kỳ vọng: 1,420,000,000 VNĐ)");

            // TC03: Kiểm tra Giá Lăn Bánh Xe máy 150cc
            Console.WriteLine("\n[TC03] Giá lăn bánh Xe máy 150cc (Gốc 50 triệu):");
            Console.WriteLine($"-> Giá lăn bánh: {xeMay150.TinhGiaLanBanh():N0} VNĐ (Kỳ vọng: 51,000,000 VNĐ)");

            // TC04: Kiểm tra Đa hình List<PhuongTien>
            Console.WriteLine("\n[TC04] Kiểm tra Tính Đa hình khi nạp vào List<PhuongTien>:");
            ql.AddPhuongTien(oto5Cho);
            ql.AddPhuongTien(xeMay150);
            ql.DisplayAll();

            // TC05: Kiểm tra Tìm Giá Lăn Bánh Max
            Console.WriteLine("\n[TC05] Phương tiện có giá lăn bánh cao nhất:");
            PhuongTien maxPt = ql.FindMaxGiaLanBanh();
            if (maxPt != null)
            {
                Console.WriteLine($"-> Trả về: {maxPt.TenHang} - Giá lăn bánh: {maxPt.TinhGiaLanBanh():N0} VNĐ");
            }

            Console.WriteLine("\n====================================================================");
            Console.ReadLine();
        }
    }
}
