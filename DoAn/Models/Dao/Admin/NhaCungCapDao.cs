using DoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn.Models.Dao.Admin
{
    public class NhaCungCapDao
    {
        TraSuaEntities db = null;
        public NhaCungCapDao()
        {
            db = new TraSuaEntities();

        }
        public NhaCungCap getById(int? mancc)
        {
            return db.NhaCungCaps.FirstOrDefault(x => x.Id == mancc);
        }
    }
}