using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Models.EF
{
    [Table("ChiNhanh")]
    public class ChiNhanh
    {
        public int Id { get; set; }

        public string TenChiNhanh { get; set; }

        public string DiaChi { get; set; }

        public string GhiChu { get; set; }
    }
}