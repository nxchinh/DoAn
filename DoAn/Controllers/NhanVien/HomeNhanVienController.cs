using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using System.Collections.Generic;
using System.Linq;
using DoAn.Common.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAn.Controllers.NhanVien
{
    public class HomeNhanVienController : Controller
    {
        // GET: HomeNhanVien
        TraSuaEntities db = new TraSuaEntities();
        public IActionResult Index(int maloai = 0)
        {
            var sessionNhanvien = HttpContext.Session.GetObjectFromJson<NhanVienSession>(Common.Constants.NHANVIEN_SESSION);
            if (sessionNhanvien != null)
            {
                var list = new List<SanPhamModel>();
                var listproduct = (from sp in db.SanPhams
                                   join lsp in db.LoaiSanPhams on sp.MaLoaiSanPham equals lsp.Id
                                   where lsp.SanPhamChinh == 1
                                   select new
                                   {
                                       Id = sp.Id,
                                       TenSanPham = sp.TenSanPham,
                                       Anh = sp.Anh,
                                       Gia = sp.KhuyenMai
                                   }).OrderByDescending(x => x.Id);
                if (maloai == 0)
                {
                    
                }
                else
                {
                    listproduct = (from sp in db.SanPhams
                                       join lsp in db.LoaiSanPhams on sp.MaLoaiSanPham equals lsp.Id
                                       where lsp.SanPhamChinh == 1 & sp.MaLoaiSanPham == maloai
                                       select new
                                       {
                                           Id = sp.Id,
                                           TenSanPham = sp.TenSanPham,
                                           Anh = sp.Anh,
                                           Gia = sp.KhuyenMai
                                       }).OrderByDescending(x => x.Id);
                }
                


                int i = 0;

                foreach (var item in listproduct)
                {
                    i++;
                    var model = new SanPhamModel();
                    model.Id = item.Id;
                    model.STT = i;
                    model.Ten = item.TenSanPham;
                    model.Anh = item.Anh;
                    model.KhuyenMai = item.Gia;
                    list.Add(model);

                }
                ViewBag.MaLoaiSP = maloai;
                return View(list);
            }
            else
            {
                return RedirectToAction("DangNhapNhanVien", "Login");
            }

        }
        public PartialViewResult DSSP_HoaDon(int maloaisanpham = 0)
        {
            if (maloaisanpham == 0)
            {
                var list = new List<SanPhamModel>();

                var listproduct = (from sp in db.SanPhams
                                   join lsp in db.LoaiSanPhams on sp.MaLoaiSanPham equals lsp.Id
                                   where lsp.SanPhamChinh == 1
                                   select new
                                   {
                                       Id = sp.Id,
                                       TenSanPham = sp.TenSanPham,
                                       Anh = sp.Anh,
                                       Gia = sp.KhuyenMai
                                   }).OrderByDescending(x => x.Id);


                int i = 0;

                foreach (var item in listproduct)
                {
                    i++;
                    var model = new SanPhamModel();
                    model.Id = item.Id;
                    model.STT = i;
                    model.Ten = item.TenSanPham;
                    model.Anh = item.Anh;
                    model.KhuyenMai = item.Gia;
                    list.Add(model);

                }
                return PartialView(list);
            }
            else
            {
                ViewBag.MaLoaiSanPham = maloaisanpham;

                var list = new List<SanPhamModel>();

                var listproduct = (from sp in db.SanPhams
                                   join lsp in db.LoaiSanPhams on sp.MaLoaiSanPham equals lsp.Id
                                   where sp.MaLoaiSanPham == maloaisanpham
                                   select new
                                   {
                                       Id = sp.Id,
                                       TenSanPham = sp.TenSanPham,
                                       Anh = sp.Anh,
                                       Gia = sp.KhuyenMai
                                   }).OrderByDescending(x => x.Id);


                int i = 0;

                foreach (var item in listproduct)
                {
                    i++;
                    var model = new SanPhamModel
                    {
                        Id = item.Id,
                        STT = i,
                        Ten = item.TenSanPham,
                        Anh = item.Anh,
                        MaLoai = maloaisanpham,
                        KhuyenMai = item.Gia
                    };
                    list.Add(model);

                }
                return PartialView(list);

            }


        }
    }
}