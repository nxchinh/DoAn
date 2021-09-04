using System;
using System.Collections.Generic;
using System.Linq;
using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using DoAn.Models.Dao;
using DoAn.Models.Dao.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAn.Controllers.Admin
{
    public class NhomSanPhamAdminController : Controller
    {
        // GET: NhomSanPhamAdmin
        TraSuaEntities db = new TraSuaEntities();
        NhomSanPhamDao dao = new NhomSanPhamDao();
        
        public IActionResult Index()
        {
            var result = Common.Constants.USER_SESSION;
            var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(result.ToString());
            if (session != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public JsonResult List(string txtSearch, int? page)
        {
            var list = db.LoaiSanPhams.Where(x => x.Id != 12 && x.Id != 13 && x.Id != 14).OrderByDescending(x => x.Id).ToList();

            int pageSize = 10;
            if (!String.IsNullOrEmpty(txtSearch))
            {
                ViewBag.txtSearch = txtSearch;
                list = list.Where(x => x.Id != 12 && x.Id != 13 && x.Id != 14 && x.TenLoaiSanPham.Contains(txtSearch)).OrderByDescending(x => x.Id).ToList();
            }
            var data = new List<NhomSanPhamModel>();
            int i = 0;
            foreach (var item in list)
            {
                i++;
                var itemmodel = new NhomSanPhamModel {STT = i, Id = item.Id, TenNhomSanPham = item.TenLoaiSanPham};

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
        [HttpPost]
        public IActionResult Add(LoaiSanPham product)
        {
            dao.Add(product);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Update(int id = 0)
        {
            var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(Common.Constants.USER_SESSION);
            if (session != null)
            {
                if (id != 0)
                {
                    var pro = db.LoaiSanPhams.FirstOrDefault(x => x.Id == id);

                    LoaiSanPham cPro = new LoaiSanPham
                    {
                        Id = pro.Id,
                        TenLoaiSanPham = pro.TenLoaiSanPham
                    };
                    return View(cPro);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update([Bind("Id,TenLoaiSanPham")] LoaiSanPham product)
        {
            dao.Update(product);
            return RedirectToAction(nameof(Index));
        }
        public JsonResult Delete(int ID)
        {
            return Json(dao.Delete(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }
    }
}