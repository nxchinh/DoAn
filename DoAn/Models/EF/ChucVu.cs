using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Models.EF
{
    [Table("ChucVu")]
    public class ChucVu
    {
        public int Id { get; set; }

        public string TenChucVu { get; set; }

        public int? Luong { get; set; }
    }
}