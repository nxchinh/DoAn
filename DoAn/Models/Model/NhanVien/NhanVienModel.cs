using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAn.Models.Model.NhanVien
{
    public class NhanVienModel
    {
        public int STT { set; get; }

        public int Id { set; get; }

        [Display(Name = "Tên nhân viên")] public string HoTen { set; get; }


        [Display(Name = "Số điện thoại")] public string SDT { set; get; }


        [Display(Name = "Email")] public string Email { set; get; }


        [Display(Name = "Tên đăng nhập")] public string TenDangNhap { set; get; }


        [Display(Name = "Địa chỉ")] public string DiaChi { set; get; }


        [StringLength(50)]
        [Display(Name = "Mật khẩu cũ")]
        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu cũ")]
        public string Password { get; set; }

        [StringLength(50)]
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu")]
        public string MatKhau { set; get; }


        [StringLength(50)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Required(ErrorMessage = "Yêu cầu xác nhận mật khẩu")]
        public string XacNhanMatKhau { set; get; }


        [Display(Name = "Chức vụ")] public int MaChucVu { set; get; }

        [Display(Name = "Chi nhánh")] public int MaChiNhanh { set; get; }

        [Display(Name = "Chức vụ")] public string TenChucVu { set; get; }

        [Display(Name = "Chinh nhánh")] public string TenChiNhanh { set; get; }

        [Display(Name = "Lương")] public int? Luong { set; get; }

        public SelectList SelectChiNhanh { set; get; }

        public SelectList SelectChucVu { set; get; }
    }
}