namespace DoAn.Models.EF
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("KhuyenMai")]
    public partial class KhuyenMai
    {
        public int Id { get; set; }

        public string TenKhuyenMai { get; set; }

        public int? PhanTram { get; set; }

        public string MoTa { get; set; }

        public DateTime NgayBatDau { get; set; }

        public DateTime? NgayKetThuc { get; set; }

        public int? Status { get; set; }
    }
}
