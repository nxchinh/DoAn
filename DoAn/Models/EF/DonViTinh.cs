using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Models.EF
{
    [Table("DonViTinh")]
    public class DonViTinh
    {
        public int Id { get; set; }

        public string TenDonViTinh { get; set; }
    }
}