using DoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn.Models.Dao.NguoiDung
{
    public class ProductDao
    {
        TraSuaEntities db = null;
        public ProductDao()
        {
            db = new TraSuaEntities();
        }
        public List<SanPham> getTopping()
        {
            return db.SanPhams.Where(x => x.MaLoaiSanPham == 1).ToList();
        }
        public List<SanPham> getDuong()
        {
            return db.SanPhams.Where(x => x.MaLoaiSanPham == 13).ToList();
        }
        public List<SanPham> getDa()
        {
            return db.SanPhams.Where(x => x.MaLoaiSanPham == 12).ToList();
        }
        public List<SanPham> getSize()
        {
            return db.SanPhams.Where(x => x.MaLoaiSanPham == 14).ToList();
        }
        public SanPham getByid(int? productid)
        {
            return db.SanPhams.SingleOrDefault(x => x.Id == productid);
        }
        public SanPham viewDetail(int? productid)
        {
            return (db.SanPhams.Find(productid));
        }
        
    }
}