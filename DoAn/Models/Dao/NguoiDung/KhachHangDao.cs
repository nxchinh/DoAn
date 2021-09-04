using DoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn.Models.Dao.NguoiDung
{
    public class KhachHangDao
    {
        TraSuaEntities db = null;
        public KhachHangDao()
        {
            db = new TraSuaEntities();
        }
        public KhachHang getById(int makhachhang)
        {
            return db.KhachHangs.FirstOrDefault(x => x.Id == makhachhang);
        }
        public KhachHang viewDetail(int? makhachhang)
        {
            return db.KhachHangs.FirstOrDefault(x => x.Id == makhachhang);
        }
        public void Update(int makhachhang, string hoten, string diachi, string sdt, string email)
        {
            var khachhang = db.KhachHangs.FirstOrDefault(x => x.Id == makhachhang);
            khachhang.HoTen = hoten;
            khachhang.DiaChi = diachi;
            khachhang.SDT = sdt;
            khachhang.Email = email;
            db.SaveChanges();
        }
        public bool CheckEmail(string email, string username)
        {
            return db.KhachHangs.Count(x => x.Email == email && x.TenDangNhap == username) > 0;
        }
        public KhachHang getKhachHang(string email, string username)
        {
            return db.KhachHangs.Where(x => x.Email == email && x.TenDangNhap == username).FirstOrDefault();
        }
    }
}