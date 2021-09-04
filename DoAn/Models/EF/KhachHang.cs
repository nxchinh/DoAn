using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Models.EF
{
    [Table("KhachHang")]
    public class KhachHang
    {
        public int Id { get; set; }

        public string HoTen { get; set; }

        [StringLength(50)] public string SDT { get; set; }

        [StringLength(50)] public string TenDangNhap { get; set; }

        [StringLength(50)] public string MatKhau { get; set; }

        public string DiaChi { get; set; }

        [StringLength(50)] public string Email { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}