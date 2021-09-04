using DoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DoAn.Models.Model.Admin;
using Microsoft.AspNetCore.Hosting;

namespace DoAn.Models.Dao.Admin
{
    public class ProductDao
    {
        readonly TraSuaEntities _db = null;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductDao()
        {
            _db = new TraSuaEntities();
        }
        public ProductDao(IWebHostEnvironment webHostEnvironment)
        {
            this._webHostEnvironment = webHostEnvironment;
            _db = new TraSuaEntities();
        }
        
        public SanPham getByid(int? productid)
        {
            return _db.SanPhams.SingleOrDefault(x => x.Id == productid);
        }
        public SanPham FindSanPham(int? productid)
        {
            return (_db.SanPhams.Find(productid));
        }
        private string UploadedFile(SanPhamModel model)
        {
            string uniqueFileName = null;

            if (model.ProductImage != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProductImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProductImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        public int AddSanPham(SanPhamModel sp)
        {
            string uniqueFileName = UploadedFile(sp);
            var sanpham = new SanPham();
            var convert = new DoAn.Common.Function.ConvertMoney();
            sanpham.TenSanPham = sp.Ten;
            sanpham.MaLoaiSanPham = sp.MaLoai;
            sanpham.GiaBan = convert.ConvertTien(sp.GiaBan);
            sanpham.KhuyenMai = sanpham.GiaBan;
            sanpham.Anh = uniqueFileName;
            sanpham.ProductImage = sp.ProductImage;
            _db.SanPhams.Add(sanpham);
            _db.SaveChanges();
            return sanpham.Id;
        }
        public int UpdateSanPham(SanPhamModel sp)
        {
            var convert = new Common.Function.ConvertMoney();
            var sanpham = _db.SanPhams.Find(sp.Id);
            sanpham.TenSanPham = sp.Ten;
            sanpham.GiaBan = convert.ConvertTien(sp.GiaBan);
            sanpham.MaLoaiSanPham = sp.MaLoai;
            _db.SaveChanges();
            return sanpham.Id;
        }
        public async Task<int> Delete(int masanpham)
        {
            var product = _db.SanPhams.Find(masanpham);
            _db.SanPhams.Remove(product);
            await _db.SaveChangesAsync();
            return product.Id;
        }
    }
}