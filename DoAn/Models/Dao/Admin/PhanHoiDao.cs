using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DoAn.Models.Model.Admin;
using DoAn.Models.EF;

namespace DoAn.Models.Dao.Admin
{
    public class PhanHoiDao
    {
        TraSuaEntities db = null;
        public PhanHoiDao()
        {
           db = new TraSuaEntities();
        }
        public int Update(int? maphanhoi)
        {
            var phanhoi = db.PhanHois.Find(maphanhoi);
            phanhoi.DaXem = 1;
            db.SaveChanges();
            return phanhoi.Id;
        }
    }
}