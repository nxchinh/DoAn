using System.ComponentModel.DataAnnotations;

namespace DoAn.Models.Model.Admin
{
    public class DonViTinhModel
    {
        public int Id { set; get; }
        [Required]
        public string TenDonVi { set; get; }

        public int Stt { set; get; }
    }
}