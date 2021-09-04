using DoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DoAn.Models.Model.Admin;

namespace DoAn.Models.Dao.NguoiDung
{
    public class UserDao
    {
        TraSuaEntities db = null;
        public UserDao()
        {
            db = new TraSuaEntities();
        }
        public bool CheckEmail(string email)
        {
            return db.KhachHangs.Count(x => x.Email == email) > 0;
        }
        public bool CheckUserName(string username)
        {
            return db.KhachHangs.Count(x => x.TenDangNhap == username) > 0;
        }
        public KhachHang GetById(string username)
        {
            return db.KhachHangs.SingleOrDefault(x => x.TenDangNhap == username);
        }
        public KhachHang TimId(int id)
        {
            return db.KhachHangs.FirstOrDefault(x => x.Id == id);
        }
        public void Save()
        {
            db.SaveChanges();
        }
        public int Update(int? id)
        {
            var khachHang = db.KhachHangs.Find(id);
            khachHang.Active = true;
            db.SaveChanges();
            return khachHang.Id;
        }
        public int Delete(int makhachhang)
        {
            var khachhang = db.KhachHangs.Find(makhachhang);
            db.KhachHangs.Remove(khachhang);
            db.SaveChanges();
            return khachhang.Id;
        }
        public bool Login(string username, string password)
        {
            var result = db.KhachHangs.Count(x => x.TenDangNhap == username && x.MatKhau == password);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int Insert(KhachHang entity)
        {
            db.KhachHangs.Add(entity);
            db.SaveChanges();
            return (entity.Id);
        }
    }
}