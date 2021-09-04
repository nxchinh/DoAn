using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Models.EF
{
    [Table("BinhLuan")]
    public class BinhLuan
    {
        public int Id { get; set; }

        public string NoiDung { get; set; }

        public int? DanhGia { get; set; }

        public int? MaKhachHang { get; set; }

        public int? MaSanPham { get; set; }

        public DateTime? ThoiGian { get; set; }
    }
}