using DoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn.Models.Dao.NguoiDung
{
    public class CategoryDao
    {
        TraSuaEntities db = null;
        public CategoryDao()
        {
            db = new TraSuaEntities();
        }
        public LoaiSanPham getById(int? categoryid)
        {
            return db.LoaiSanPhams.SingleOrDefault(x => x.Id == categoryid);
        }
        public int? getSPChinh(int? productid)
        {
            var sanpham = new ProductDao().getByid(productid);
            var loaisanpham = new CategoryDao().getById(sanpham.MaLoaiSanPham);
            return loaisanpham.SanPhamChinh;
        }
    }
}