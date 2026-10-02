using System;

namespace AutoSpeedLogistics
{
    public class OTo : PhuongTien
    {
        private int _soChoNgoi;
        private double _dungTichDongCo;

        public int SoChoNgoi
        {
            get => _soChoNgoi;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Số chỗ ngồi phải > 0!");
                _soChoNgoi = value;
            }
        }

        public double DungTichDongCo
        {
            get => _dungTichDongCo;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Dung tích động cơ phải > 0!");
                _dungTichDongCo = value;
            }
        }

        public OTo(string maPT, string tenHang, int namSanXuat, decimal giaGoc, int soChoNgoi, double dungTichDongCo)
            : base(maPT, tenHang, namSanXuat, giaGoc)
        {
            SoChoNgoi = soChoNgoi;
            DungTichDongCo = dungTichDongCo;
        }

        public override decimal TinhGiaLanBanh()
        {
            if (SoChoNgoi <= 9)
            {
                // Giá lăn bánh = GiaGoc + 12% GiaGoc + 30% GiaGoc
                return GiaGoc + (GiaGoc * 0.12m) + (GiaGoc * 0.30m);
            }
            else
            {
                // Giá lăn bánh = GiaGoc + 10% GiaGoc
                return GiaGoc + (GiaGoc * 0.10m);
            }
        }

        public override string GetInfo()
        {
            return $"{base.GetInfo()} | Số chỗ: {SoChoNgoi} | Động cơ: {DungTichDongCo}L | Giá lăn bánh: {TinhGiaLanBanh():N0} VNĐ";
        }
    }
}
