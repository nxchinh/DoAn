using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Models.EF
{
    [Table("HoaDonBan")]
    public class HoaDonBan
    {
        [StringLength(50)] public string Id { get; set; }

        public int? MaNhanVien { get; set; }

        public int? MaKhach { get; set; }

        public DateTime? NgayBan { get; set; }

        public int? GiamGia { get; set; }

        public int? TongTien { get; set; }

        public int? DaThanhToan { get; set; }

        public int? Duyet { get; set; }

        public int? MaChiNhanh { get; set; }
    }
}