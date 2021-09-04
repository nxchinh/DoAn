using DoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAn.Models.Model.Admin
{
    public class KhuyenMaiModel
    {
        public int Id { set; get; }

        [Required(ErrorMessage = "Mời nhập tên khuyến mãi")]
        [Display(Name = "Tên khuyến mãi")]
        public string Ten { set; get; }

        [Required(ErrorMessage = "Mời nhập phần trăm")]
        [Display(Name = "Phần trăm")]
        public int? PhanTram { set; get; }

        [Required(ErrorMessage = "Mời chọn loại khuyến mãi")]
        [Display(Name = "Loại khuyến mãi")]
        public int LoaiKhuyenMai { set; get; }

        public string TenLoaiKhuyenMai { set; get; }

        public SelectList SelectLoaiKM { set; get; }

        [Required(ErrorMessage = "Mời chọn loại khuyến mãi")]
        [Display(Name = "Loại khuyến mãi")]
        public int MaLoaiKhuyenMai { set; get; }

        public string MoTa { set; get; }
        [Required(ErrorMessage = "Mời chọn ngày bắt đầu")]
        [Display(Name = "Ngày bắt đầu")]
        public DateTime NgayBatDau { set; get; }

        [Required(ErrorMessage = "Mời chọn ngày kết thúc")]
        [Display(Name = "Ngày kết thúc")]
        public DateTime? NgayKetThuc { set; get; }

        public int MaLoaiSanPham { set; get; }

        public SelectList Select { set; get; }

        public int? Status { set; get; }

        public List<LoaiSanPham> ListLoaiSanPham { set; get; }

        public int STT { set; get; }


        public string NgayBDShow { set; get; }

        public string NgayKTShow { set; get; }
    }
}