using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Models.EF
{
    [Table("NhanVien")]
    public class NhanVien
    {
        public int Id { get; set; }

        [StringLength(100)] public string HoTen { get; set; }

        [StringLength(15)] public string SDT { get; set; }

        [StringLength(50)] public string TenDangNhap { get; set; }

        [StringLength(50)] public string MatKhau { get; set; }

        [StringLength(50)] public string Email { get; set; }

        public string DiaChi { get; set; }

        public int? MaChucVu { get; set; }

        public int? MaChiNhanh { get; set; }
    }
}