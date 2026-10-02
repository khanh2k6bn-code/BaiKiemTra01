using System;

namespace AutoSpeedLogistics
{
    public class XeMay : PhuongTien
    {
        private int _dungTichXylanh;

        public int DungTichXylanh
        {
            get => _dungTichXylanh;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Dung tích xilanh phải > 0!");
                _dungTichXylanh = value;
            }
        }

        public XeMay(string maPT, string tenHang, int namSanXuat, decimal giaGoc, int dungTichXylanh)
            : base(maPT, tenHang, namSanXuat, giaGoc)
        {
            DungTichXylanh = dungTichXylanh;
        }

        public override decimal TinhGiaLanBanh()
        {
            if (DungTichXylanh < 175)
            {
                // Giá lăn bánh = GiaGoc + 2% GiaGoc
                return GiaGoc + (GiaGoc * 0.02m);
            }
            else
            {
                // Giá lăn bánh = GiaGoc + 5% GiaGoc
                return GiaGoc + (GiaGoc * 0.05m);
            }
        }

        public override string GetInfo()
        {
            return $"{base.GetInfo()} | Dung tích XL: {DungTichXylanh}cc | Giá lăn bánh: {TinhGiaLanBanh():N0} VNĐ";
        }
    }
}
