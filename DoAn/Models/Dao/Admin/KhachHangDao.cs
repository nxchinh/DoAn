using DoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn.Models.Dao.Admin
{
    public class KhachHangDao
    {
        TraSuaEntities db = null;
        public KhachHangDao()
        {
            db = new TraSuaEntities();
        }
        public KhachHang viewDetail(int? makhachhang)
        {
            return db.KhachHangs.FirstOrDefault(x => x.Id == makhachhang);
        }
        public int Delete(int makhachhang)
        {
            var khachhang = db.KhachHangs.Find(makhachhang);
            db.KhachHangs.Remove(khachhang);
            db.SaveChanges();
            return khachhang.Id;
        }

    }
}