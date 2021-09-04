using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Models.EF
{
    [Table("PhanHoi")]
    public class PhanHoi
    {
        public int Id { get; set; }

        public string NoiDung { get; set; }

        public DateTime? ThoiGian { get; set; }

        public int? MaKhachHang { get; set; }

        public int? DaXem { get; set; }
    }
}