using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DoAn.Models.EF;
namespace DoAn.Models.Dao.Admin
{
    public class NhomSanPhamDao
    {
        TraSuaEntities db = null;
        public NhomSanPhamDao()
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
        public LoaiSanPham viewdetail(int maloai)
        {
            return db.LoaiSanPhams.FirstOrDefault(x => x.Id==maloai);
        }
        public int Add(LoaiSanPham loaisp)
        {
            var nhomsp = new LoaiSanPham {TenLoaiSanPham = loaisp.TenLoaiSanPham, SanPhamChinh = 1};
            db.LoaiSanPhams.Add(nhomsp);
            db.SaveChanges();
            return nhomsp.Id;
        }
        public int Update(LoaiSanPham loaisp)
        {
            var nhomsp = db.LoaiSanPhams.Find(loaisp.Id);
            nhomsp.TenLoaiSanPham = loaisp.TenLoaiSanPham;
            db.SaveChanges();
            return nhomsp.Id;
        }
        public int Delete(int manhomsp)
        {
            var nhomsp = db.LoaiSanPhams.Find(manhomsp);
            db.LoaiSanPhams.Remove(nhomsp);
            db.SaveChanges();
            return nhomsp.Id;
        }
    }
}