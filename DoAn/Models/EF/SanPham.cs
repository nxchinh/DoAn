using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace DoAn.Models.EF
{
    [Table("SanPham")]
    public class SanPham
    {
        public int Id { get; set; }

        public string TenSanPham { get; set; }

        public int? MaLoaiSanPham { get; set; }

        public int? GiaBan { get; set; }

        public int? KhuyenMai { get; set; }

        public string Anh { get; set; }
        [NotMapped]
        [Display(Name = "Product Picture")]
        public IFormFile ProductImage { get; set; }
        public string MoTa { get; set; }
    }
}