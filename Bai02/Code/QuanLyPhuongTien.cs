using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoSpeedLogistics
{
    public class QuanLyPhuongTien
    {
        private List<PhuongTien> _danhSach;

        public QuanLyPhuongTien()
        {
            _danhSach = new List<PhuongTien>();
        }

        public void AddPhuongTien(PhuongTien pt)
        {
            if (pt != null)
                _danhSach.Add(pt);
        }

        public void DisplayAll()
        {
            Console.WriteLine("\n=== DANH SÁCH TOÀN BỘ PHƯƠNG TIỆN ===");
            foreach (var pt in _danhSach)
            {
                Console.WriteLine(pt.GetInfo());
            }
        }

        public PhuongTien FindMaxGiaLanBanh()
        {
            if (_danhSach.Count == 0) return null;
            return _danhSach.OrderByDescending(pt => pt.TinhGiaLanBanh()).FirstOrDefault();
        }

        public List<PhuongTien> SearchByName(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return new List<PhuongTien>();
            return _danhSach.Where(pt => pt.TenHang.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        }
    }
}
