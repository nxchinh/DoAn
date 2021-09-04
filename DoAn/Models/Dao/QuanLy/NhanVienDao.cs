using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DoAn.Models.EF;
using DoAn.Models.Model.Admin;

namespace DoAn.Models.Dao.QuanLy
{
    public class NhanVienDao
    {
        TraSuaEntities db = null;
        public NhanVienDao()
        {
            db = new TraSuaEntities();
        }
        public int AddNhanVien(NhanVienModel nv, int? machinhanh)
        {
            
            var nhanvien = new DoAn.Models.EF.NhanVien();
            nhanvien.HoTen = nv.HoTen;
            nhanvien.DiaChi = nv.DiaChi;
            nhanvien.TenDangNhap = nv.TenDangNhap;
            nhanvien.SDT = nv.SDT;
            nhanvien.MatKhau = DoAn.Common.Function.Encrytor.MD5Hash("123456");
            nhanvien.MaChiNhanh = machinhanh;
            nhanvien.MaChucVu = nv.MaChucVu;
            db.NhanViens.Add(nhanvien);
            db.SaveChanges();
            return nhanvien.Id;
        }
        public int UpdateNhanVien(NhanVienModel nv)
        {
            var nhanvien = db.NhanViens.Find(nv.Id);
            nhanvien.MaChucVu = nv.MaChucVu;
            db.SaveChanges();
            return nhanvien.Id;
        }
        public int DeleteNhanVien(int Id)
        {
            var nhanvien = db.NhanViens.Find(Id);
            db.NhanViens.Remove(nhanvien);
            db.SaveChanges();
            return nhanvien.Id;
        }
    }
}