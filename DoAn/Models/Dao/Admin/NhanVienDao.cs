using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
namespace DoAn.Models.Dao.Admin
{
    public class NhanVienDao
    {

        TraSuaEntities db = null;
        public NhanVienDao()
        {
            db = new TraSuaEntities();
        }

        public DoAn.Models.EF.NhanVien getByid(int? manhanvien)
        {
            return db.NhanViens.SingleOrDefault(x => x.Id == manhanvien);
        }
        public DoAn.Models.EF.NhanVien FindSanPham(int? manhanvien)
        {
            return (db.NhanViens.Find(manhanvien));
        }
        public int AddNhanVien(NhanVienModel nv)
        {
            var nhanvien = new DoAn.Models.EF.NhanVien();
            nhanvien.TenDangNhap = nv.TenDangNhap;
            nhanvien.HoTen = nv.HoTen;
            nhanvien.MaChiNhanh = nv.MaChiNhanh;
            nhanvien.MaChucVu = nv.MaChucVu;
            nhanvien.MatKhau = DoAn.Common.Function.Encrytor.MD5Hash(nv.MatKhau);
            db.NhanViens.Add(nhanvien);
            db.SaveChanges();
            return nhanvien.Id;
        }
        public int DeleteNhanVien(int manhanvien)
        {
            var nhanvien = db.NhanViens.Find(manhanvien);
            db.NhanViens.Remove(nhanvien);
            db.SaveChanges();
            return nhanvien.Id;
        }
    }
}