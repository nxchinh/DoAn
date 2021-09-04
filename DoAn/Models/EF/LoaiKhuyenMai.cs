using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Models.EF
{
    [Table("LoaiKhuyenMai")]
    public class LoaiKhuyenMai
    {
        public int Id { get; set; }

        public string TenLoaiKhuyenMai { get; set; }
    }
}