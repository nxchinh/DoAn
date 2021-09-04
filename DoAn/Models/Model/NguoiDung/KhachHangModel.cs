using System.ComponentModel.DataAnnotations;

namespace DoAn.Models.Model.NguoiDung
{
    public class KhachHangModel
    {
        [Key] public int Id { get; set; }

        public int STT { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập họ tên")]
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Yêu cầu nhập số điện thoại")]
        [Display(Name = "Số điện thoại")]
        public string SDT { get; set; }


        [StringLength(50)]
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Yêu cầu nhập tên đăng nhập")]
        public string TenDangNhap { get; set; }

        [StringLength(50)]
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }

        [StringLength(50)]
        [Display(Name = "Mật khẩu cũ")]
        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu cũ")]
        public string Password { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu ít nhất 6 kí tự")]
        public string MatKhau { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [Required(ErrorMessage = "Yêu cầu xác nhận mật khẩu")]
        [Compare("MatKhau", ErrorMessage = "Xác nhận mật khẩu không đúng")]
        public string XacNhanMatKhau { set; get; }

        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Yêu cầu nhập địa chỉ")]
        public string DiaChi { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Yêu cầu nhập email")]
        public string Email { set; get; }
    }
}