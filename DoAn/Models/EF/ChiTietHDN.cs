using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Models.EF
{
    [Table("ChiTietHDN")]
    public class ChiTietHDN
    {
        public int Id { get; set; }

        [StringLength(50)] public string MaHDN { get; set; }

        public int? MaNguyenLieu { get; set; }

        public int? MaDonViTinh { get; set; }

        public int? GiaNhap { get; set; }

        public int? SoLuong { get; set; }

        public int? ChietKhau { get; set; }

        public int? ThanhTien { get; set; }
    }
}