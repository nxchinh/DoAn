using DoAn.Common.Function;
using DoAn.Models.Dao.NguoiDung;
using DoAn.Models.EF;
using DoAn.Models.Model.NguoiDung;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers.NguoiDung
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        TraSuaEntities db = new TraSuaEntities();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ChiTietSanPham(int? masanpham)
        {
            ViewBag.SanPham = db.SanPhams.FirstOrDefault(x => x.Id == masanpham);

            var model = new List<DanhGiaModel>();
            var list = db.BinhLuans.Where(x => x.MaSanPham == masanpham).OrderByDescending(x => x.Id).ToList();
            foreach (var item in list)
            {
                DateTime dtime = DateTime.Now;
                var itemmodel = new DanhGiaModel
                {
                    Id = item.Id,
                    MaKhachHang = item.MaKhachHang,
                    MaSanPham = masanpham,
                    NoiDung = item.NoiDung,
                    TenDangNhap = new KhachHangDao().viewDetail(item.MaKhachHang).TenDangNhap,
                    DanhGia = item.DanhGia,
                    ThoiGian = new LamTronThoiGian().LamTron(dtime, item.ThoiGian)
                };
                model.Add(itemmodel);
            }
            var tongsao = list.Sum(x => x.DanhGia);
            var tongdanhgia = list.Count;
            if (tongdanhgia == 0)
            {
                ViewBag.Star = 0;
            }
            else
            {
                var chia = tongsao / tongdanhgia;
                ViewBag.Star = chia;
            }
            return View(model);

        }
    }
}