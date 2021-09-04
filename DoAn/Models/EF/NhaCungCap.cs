using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Models.EF
{
    [Table("NhaCungCap")]
    public class NhaCungCap
    {
        public int Id { get; set; }

        public string TenNCC { get; set; }

        public string DiaChi { get; set; }

        [StringLength(50)] public string DienThoai { get; set; }
    }
}