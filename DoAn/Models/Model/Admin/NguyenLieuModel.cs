using System.ComponentModel.DataAnnotations;

namespace DoAn.Models.Model.Admin
{
    public class NguyenLieuModel
    {
        public int Id { set; get; }

        [Required]
        public string TenNguyenLieu { set; get; }

        public int Stt { set; get; }
    }
}