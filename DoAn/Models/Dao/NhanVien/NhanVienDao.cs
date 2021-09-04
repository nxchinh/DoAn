using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DoAn.Models.EF;
namespace DoAn.Models.Dao.NhanVien
{
    public class NhanVienDao
    {
        TraSuaEntities db = new TraSuaEntities();
        public bool CheckEmail(string email, string username)
        {
            return db.NhanViens.Count(x => x.Email == email && x.TenDangNhap == username) > 0;
        }
        public DoAn.Models.EF.NhanVien getNhanVien(string email, string username)
        {
            return db.NhanViens.Where(x => x.Email == email && x.TenDangNhap == username).FirstOrDefault();
        }
    }
}