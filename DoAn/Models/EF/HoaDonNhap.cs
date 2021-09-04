using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Models.EF
{
    [Table("HoaDonNhap")]
    public class HoaDonNhap
    {
        [StringLength(50)] public string Id { get; set; }

        public int? MaNhanVien { get; set; }

        public int? MaNCC { get; set; }

        public DateTime? NgayNhap { get; set; }

        public int? TongTien { get; set; }

        public int? ChietKhau { get; set; }

        public int? DaThanhToan { get; set; }
    }
}