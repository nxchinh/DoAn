using DoAn.Models.Dao.Admin;
using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers.Admin
{
    public class ChiNhanhAdminController : Controller
    {
        // GET: ChiNhanhAdmin
        readonly TraSuaEntities _db = new TraSuaEntities();
        readonly ChiNhanhDao dao = new ChiNhanhDao();

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
            var list = _db.ChiNhanhs.OrderByDescending(x => x.Id).ToList();

            int pageSize = 10;
            if (!String.IsNullOrEmpty(txtSearch))
            {
                ViewBag.txtSearch = txtSearch;
                list = list.Where(x=> x.TenChiNhanh.Contains(txtSearch)).OrderByDescending(x => x.Id).ToList();
            }
            var data = new List<ChiNhanhModel>();
            int i = 0;
            foreach (var item in list)
            {
                i++;
                var itemmodel = new ChiNhanhModel
                {
                    STT = i, Id = item.Id, TenChiNhanh = item.TenChiNhanh, DiaChi = item.DiaChi
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

            return Json(new { data = datamodel, pageCurrent = page, numSize = numSize },  new Newtonsoft.Json.JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult Add(ChiNhanhModel chinhanh)
        {
            dao.AddChiNhanh(chinhanh);
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
                    var pro = _db.ChiNhanhs.FirstOrDefault(x => x.Id == id);
                    ChiNhanhModel cPro = new ChiNhanhModel
                    {
                        Id = pro.Id,
                        DiaChi = pro.DiaChi,
                        TenChiNhanh = pro.TenChiNhanh,
                        GhiChu = pro.GhiChu
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
        public IActionResult Update(ChiNhanhModel chinhanh)
        {
            dao.updateChiNhanh(chinhanh);
            return RedirectToAction(nameof(Index));
        }
        public JsonResult Delete(int ID)
        {
            return Json(dao.Delete(ID),  new Newtonsoft.Json.JsonSerializerSettings());
        }
    }
}