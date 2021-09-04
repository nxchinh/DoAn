using System.ComponentModel.DataAnnotations;

namespace DoAn.Models.Model.NguoiDung
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Mời nhập Username")]
        public string Username { set; get; }

        [Required(ErrorMessage = "Mời nhập Password")]
        public string Password { set; get; }

        public bool RememberMe { set; get; }
    }
}