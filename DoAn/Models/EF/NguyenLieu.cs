using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Models.EF
{
    [Table("NguyenLieu")]
    public class NguyenLieu
    {
        public int Id { get; set; }

        public string TenNguyenLieu { get; set; }

        public string MoTa { get; set; }
    }
}