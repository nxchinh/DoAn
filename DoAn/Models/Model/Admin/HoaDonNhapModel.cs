using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAn.Models.Model.Admin
{
    public class HoaDonNhapModel
    {
        public string Id { set; get; }

        public int STT { set; get; }


        public int MaNhanVien { set; get; }

        [Required(ErrorMessage = "Mời chọn tên nhà cung cấp")]
        [Display(Name = "Nhà cung cấp")]
        public int MaNhaCungCap { set; get; }

        public DateTime? NgayNhap { set; get; }


        [Display(Name = "Tổng tiền")] public int? TongTien { set; get; }

        public int Status { set; get; }

        public string TenNCC { set; get; }

        public string TenDangNhap { set; get; }


        [Display(Name = "Giảm giá")] public string ChietKhau { set; get; }

        public SelectList SelectNCC { set; get; }

        public int? PhaiTra { set; get; }
    }
}