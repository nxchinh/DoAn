namespace DoAn.Models.Model.NguoiDung
{
    public class GioHangModel
    {
        public int Id { set; get; }

        public int? MaSanPham { set; get; }

        public string TenSanPham { set; get; }

        public string Anh { set; get; }

        public int? SoLuong { set; get; }

        public int? GiaBan { set; get; }

        public int? ThanhTien { set; get; }

        public int SanPhamChinh { set; get; }

        public int ThuocSanPham { set; get; }

        public int TongTien { set; get; }

        public int? SanPhamThu { set; get; }

        public string MoTa { set; get; }
    }
}