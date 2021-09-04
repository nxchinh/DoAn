using DoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn.Models.Dao.Admin
{
    public class NguyenLieuDonViDao
    {
        TraSuaEntities db = null;
        public NguyenLieuDonViDao()
        {
            db = new TraSuaEntities();
        }

        public NguyenLieu_DonVi getBy_NLId_DVId(int? manguyenlieu, int? madonvi)
        {

            return db.NguyenLieu_DonVi.FirstOrDefault(x => x.MaDonViTinh == madonvi && x.MaNguyenLieu == manguyenlieu);
        }
    }
}