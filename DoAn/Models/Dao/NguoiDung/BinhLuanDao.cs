using DoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn.Models.Dao.NguoiDung
{
    public class BinhLuanDao
    {
        TraSuaEntities db = null;
        public BinhLuanDao()
        {
            db = new TraSuaEntities();
        }

        public void Insert(BinhLuan binhluan)
        {
            db.BinhLuans.Add(binhluan);
            db.SaveChanges();
        }
    }
}