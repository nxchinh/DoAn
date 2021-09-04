using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAn.Models.Model.Admin
{
    public class CTHDNhapModel
    {
        public int Id { set; get; }

        public int STT { set; get; }

        public string MaHDN { set; get; }

        [Required(ErrorMessage = "Mời chọn nguyên liệu")]
        [Display(Name = "Nguyên liệu")]
        public int MaNguyenLieu { set; get; }

        public string TenNguyenLieu { set; get; }

        [Required(ErrorMessage = "Mời chọn đơn vị tính")]
        [Display(Name = "Đơn vị tính")]
        public int MaDonViTinh { set; get; }

        public string TenDonViTinh { set; get; }

        [Required(ErrorMessage = "Mời nhập số lượng")]
        [Display(Name = "Số lượng")]
        public string SoLuong { set; get; }

        [Required(ErrorMessage = "Mời nhập giá nhập")]
        [Display(Name = "Giá nhập")]
        public string GiaNhap { set; get; }

        [Required(ErrorMessage = "Mời nhập chiết khấu")]
        [Display(Name = "Chiết khấu")]
        public string ChietKhau { set; get; }


        public int? ThanhTien { set; get; }

        public SelectList SelectNguyenLieu { set; get; }

        public SelectList SelectDonViTinh { set; get; }
    }
}