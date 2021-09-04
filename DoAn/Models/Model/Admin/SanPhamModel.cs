using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAn.Models.Model.Admin
{
    public class SanPhamModel
    {
        [Key] 
        public int? Id { set; get; }

        public int STT { set; get; }

        [Required(ErrorMessage = "Mời nhập tên sản phẩm")]
        [Display(Name = "Tên sản phẩm")]
        public string Ten { set; get; }

        [Required(ErrorMessage = "Mời chọn tên loại")]
        [Display(Name = "Loại sản phẩm")]
        public int? MaLoai { set; get; }


        public string TenLoai { set; get; }

        [Required(ErrorMessage = "Mời nhập giá bán")]
        [Display(Name = "Giá bán")]
        public string GiaBan { set; get; }

        public int? KhuyenMai { set; get; }

        public string Anh { set; get; }

        [Required(ErrorMessage = "Please choose product image")]
        [Display(Name = "Product Picture")]
        public IFormFile ProductImage { get; set; }
        [Display(Name = "Mô tả")] 
        [DataType(DataType.MultilineText)]
        public string MoTa { set; get; }

        public SelectList SelectMaLoai { set; get; }

        public int? TongSL { set; get; }
    }
}