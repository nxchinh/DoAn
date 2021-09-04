using DoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn.Models.Dao.Admin
{
    public class CategoryDao
    {
        TraSuaEntities db = null;
        public CategoryDao()
        {
            db = new TraSuaEntities();
        }
        public LoaiSanPham GetById(int? categoryid)
        {
            return db.LoaiSanPhams.SingleOrDefault(x => x.Id == categoryid);
        }
        public int? getSPChinh(int? productid)
        {
            var sanpham = new ProductDao().getByid(productid);
            var loaisanpham = new CategoryDao().GetById(sanpham.MaLoaiSanPham);
            return loaisanpham.SanPhamChinh;
        }
        public void Insert(LoaiSanPham loaisp)
        {
            db.LoaiSanPhams.Add(loaisp);
            db.SaveChanges();
        }
        public void Delete(int manhomsp)
        {
            var nhomsp = db.LoaiSanPhams.Find(manhomsp);
            db.LoaiSanPhams.Remove(nhomsp);
            db.SaveChanges();
        }
    }
}