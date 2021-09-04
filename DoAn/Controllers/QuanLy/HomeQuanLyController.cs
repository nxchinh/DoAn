using DoAn.Models.Model.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using DoAn.Common.Session;
using DoAn.Models.EF;
using DoAn.Models.Dao.QuanLy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAn.Controllers.QuanLy
{
    public class HomeQuanLyController : Controller
    {
        // GET: HomeQuanLy
        readonly TraSuaEntities _db = new TraSuaEntities();
        NhanVienDao _dao = new NhanVienDao();
        public IActionResult Index()
        {
            var sessionNhanvien = HttpContext.Session.GetObjectFromJson<NhanVienSession>(Common.Constants.NHANVIEN_SESSION);
            if (sessionNhanvien != null)
            {
                var model = new NhanVienModel();
                model.SelectChucVu = new SelectList(_db.ChucVus.Where(x => x.Id != 1 && x.Id != 3), "Id", "TenChucVu", 0);
                return View(model);
            }
            else
            {
                return RedirectToAction("DangNhapNhanVien", "Login");
            }
            

        }
        public JsonResult List(string txtSearch, int? page)
        {
            var sessionNhanvien = HttpContext.Session.GetObjectFromJson<NhanVienSession>(Common.Constants.NHANVIEN_SESSION);

            var list = _db.NhanViens.Where(x => x.MaChiNhanh == sessionNhanvien.MaChiNhanh && x.MaChucVu != 3).OrderByDescending(x => x.Id).ToList();

                int pageSize = 5;
                if (!String.IsNullOrEmpty(txtSearch))
                {
                    ViewBag.txtSearch = txtSearch;
                    list = list.Where(x => x.MaChiNhanh == sessionNhanvien.MaChiNhanh && x.MaChucVu != 3 && x.HoTen.Contains(txtSearch)).OrderByDescending(x => x.Id).ToList();
                }
                var data = new List<NhanVienModel>();
                int i = 0;
                foreach (var item in list)
                {
                    i++;
                    var itemmodel = new NhanVienModel
                    {
                        STT = i,
                        Id = item.Id,
                        HoTen = item.HoTen,
                        TenDangNhap = item.TenDangNhap,
                        DiaChi = item.DiaChi,
                        SDT = item.SDT,
                        TenChucVu = _db.ChucVus.FirstOrDefault(x => x.Id == item.MaChucVu)?.TenChucVu,
                        Luong = $"{_db.ChucVus.FirstOrDefault(x => x.Id == item.MaChucVu)?.Luong:0,0}"
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
        public PartialViewResult TenDangNhap()
        {

            return PartialView();
        }
    }
}