using DoAn.Models.Dao.Admin;
using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers.Admin
{
    public class NguyenLieuAdminController : Controller
    {
        // GET: NguyenLieu
        TraSuaEntities db = new TraSuaEntities();
        NguyenLieuDao dao = new NguyenLieuDao();
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
            var list = db.NguyenLieux.OrderByDescending(x => x.Id).ToList();

            int pageSize = 10;
            if (!String.IsNullOrEmpty(txtSearch))
            {
                ViewBag.txtSearch = txtSearch;
                list = list.Where(x => x.TenNguyenLieu.Contains(txtSearch)).OrderByDescending(x => x.Id).ToList();
            }
            var data = new List<NguyenLieuModel>();
            int i = 0;
            foreach (var item in list)
            {
                i++;
                var itemmodel = new NguyenLieuModel {Stt = i, Id = item.Id, TenNguyenLieu = item.TenNguyenLieu};
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
        public IActionResult Add(NguyenLieuModel nguyenLieu)
        {
            if (ModelState.IsValid)
            {
                dao.AddNguyenLieu(nguyenLieu);
            }
            return RedirectToAction("Index");
        }
        public JsonResult Delete(int ID)
        {
            return Json(dao.Delete(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }
    }
}