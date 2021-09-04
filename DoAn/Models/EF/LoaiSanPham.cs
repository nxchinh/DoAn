using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Models.EF
{
    [Table("LoaiSanPham")]
    public class LoaiSanPham
    {
        public int Id { get; set; }

        public string TenLoaiSanPham { get; set; }

        public int? SanPhamChinh { get; set; }
    }
}