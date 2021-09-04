using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Models.EF
{
    [Table("GioHang")]
    public class GioHang
    {
        public int Id { get; set; }

        public int? MaKhachHang { get; set; }

        public int? MaSanPham { get; set; }

        public int? SoLuong { get; set; }

        public int? ThuocSanPham { get; set; }

        public int? SanPhamThu { get; set; }
    }
}