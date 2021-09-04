using System;
using System.Collections.Generic;
using System.Linq;
using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using DoAn.Models.Dao.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAn.Controllers.Admin
{
    public class HoaDonBanAdminController : Controller
    {
        // GET: HoaDonBanAdmin
        readonly TraSuaEntities _db = new TraSuaEntities();
        readonly HoaDonBanDao _dao = new HoaDonBanDao();
        public IActionResult Index()
        {
            var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(Common.Constants.USER_SESSION);
            if (session != null)
            {
                var model = new HoaDonBanModel {SelectChiNhanh = new SelectList(_db.ChiNhanhs, "Id", "TenChiNhanh", 0)};
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
           
        }
        public IActionResult IndexDuyet()
        {
            return View();
        }
        public JsonResult List(int? page)
        {
            ViewBag.ChiNhanh = new SelectList(_db.ChiNhanhs, "Id", "TenChiNhanh");
            var list = _db.HoaDonBans.Where(x => x.MaKhach != 3 && x.MaKhach != 4 && x.Duyet==0).OrderByDescending(x => x.Id).ToList();

            int pageSize = 10;
            
            var data = new List<HoaDonBanModel>();
            int i = 0;
            foreach (var item in list)
            {
                i++;
                var khachhang = new KhachHangDao().viewDetail(item.MaKhach);
                var itemmodel = new HoaDonBanModel
                {
                    STT = i,
                    Id = item.Id,
                    NgayBanShow = String.Format("{0:d/M/yyyy}", item.NgayBan),
                    TenDangNhap = khachhang.TenDangNhap,
                    DiaChi = khachhang.DiaChi,
                    SDT = khachhang.SDT,
                    KhuyenMai = String.Format("{0:0,0}", item.GiamGia),
                    TongTienShow = String.Format("{0:0,0}", item.TongTien),
                    Status = item.Duyet,
                    DaThanhToan = item.DaThanhToan
                };


                data.Add(itemmodel);

            }
            page = page > 0 ? page : 1;
            int start = (int)(page - 1) * pageSize;

            ViewBag.pageCurrent = page;
            int totalPage = data.Count();
            float totalNumsize = (totalPage / (float)pageSize);
            int numSize = (int)Math.Ceiling(totalNumsize);
            ViewBag.numSize = numSize;
            var datamodel = data.Skip(start).Take(pageSize);

            return Json(new { data = datamodel, pageCurrent = page, numSize = numSize }, new Newtonsoft.Json.JsonSerializerSettings());
        }


        public JsonResult ListDuyet(int? page)
        {
            ViewBag.ChiNhanh = new SelectList(_db.ChiNhanhs, "Id", "TenChiNhanh");

            var list = _db.HoaDonBans.Where(x => x.MaKhach != 3 && x.MaKhach != 4 && x.Duyet == 1).OrderByDescending(x => x.Id).ToList();

            int pageSize = 10;

            var data = new List<HoaDonBanModel>();
            int i = 0;
            foreach (var item in list)
            {
                i++;
                var khachhang = new KhachHangDao().viewDetail(item.MaKhach);
                var itemmodel = new HoaDonBanModel
                {
                    STT = i,
                    Id = item.Id,
                    NgayBanShow = $"{item.NgayBan:d/M/yyyy}",
                    TenDangNhap = khachhang.TenDangNhap,
                    DiaChi = khachhang.DiaChi,
                    SDT = khachhang.SDT,
                    KhuyenMai = $"{item.GiamGia:0,0}",
                    TongTienShow = $"{item.TongTien:0,0}",
                    Status = item.Duyet,
                    DaThanhToan = item.DaThanhToan
                };
                data.Add(itemmodel);

            }
            page = page > 0 ? page : 1;
            int start = (int)(page - 1) * pageSize;

            ViewBag.pageCurrent = page;
            int totalPage = data.Count();
            float totalNumsize = (totalPage / (float)pageSize);
            int numSize = (int)Math.Ceiling(totalNumsize);
            ViewBag.numSize = numSize;
            var datamodel = data.Skip(start).Take(pageSize);

            return Json(new { data = datamodel, pageCurrent = page, numSize = numSize }, new Newtonsoft.Json.JsonSerializerSettings());
        }
        [HttpGet]
        public IActionResult Update(string id)
        {
            var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(Common.Constants.USER_SESSION);
            if (session != null)
            {
                var item = _db.HoaDonBans.FirstOrDefault(x => x.Id == id);

                var khachhang = new KhachHangDao().viewDetail(item.MaKhach);
                var itemmodel = new HoaDonBanModel
                {
                    Id = item.Id,
                    NgayBanShow = $"{item.NgayBan:d/M/yyyy}",
                    TenDangNhap = khachhang.TenDangNhap,
                    DiaChi = khachhang.DiaChi,
                    SDT = khachhang.SDT,
                    Email = khachhang.Email,
                    KhuyenMai = $"{item.GiamGia:0,0}",
                    TongTienShow = $"{item.TongTien:0,0}",
                    Status = item.Duyet,
                    DaThanhToan = item.DaThanhToan
                };
                ViewBag.ChiNhanh = new SelectList(_db.ChiNhanhs, "Id", "TenChiNhanh", itemmodel.MaChiNhanh);
                return View(itemmodel);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult Update(HoaDonBanModel hoadonban)
        {
            _dao.UpdateDuyet(hoadonban);
            return RedirectToAction(nameof(Index));
        }
    }
}