using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DoAn.Models.Dao.Admin;
using DoAn.Models.Model.Admin;
using DoAn.Models.EF;

namespace DoAn.Models.Dao.Admin
{
    public class HoaDonBanDao
    {
        TraSuaEntities db = null;
        public HoaDonBanDao()
        {
            db = new TraSuaEntities();
        }
        public int UpdateDuyet(HoaDonBanModel model)
        {
            var hoadon = db.HoaDonBans.Find(model.Id);
            hoadon.MaChiNhanh = model.MaChiNhanh;
            hoadon.Duyet = 1;
            db.SaveChanges();
            return 1;
        }
        public int UpdateThanhToan(int ID)
        {
            var hoadon = db.HoaDonBans.Find(ID);
            hoadon.DaThanhToan = 1;
            db.SaveChanges();
            return 1;
        }
        public int tongsoluong(string mahoadonban)
        {
            var tongsoluong = 0;
            var list = db.ChiTietHDBs.Where(x => x.MaHDB == mahoadonban);
            foreach (var item in list)
            {
                if (new CategoryDao().getSPChinh(item.MaSanPham) == 1)
                {
                    tongsoluong += 1;
                }
            }
            return tongsoluong;
        }
    }
}