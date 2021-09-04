using DoAn.Models.Dao.Admin;
using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers.Admin
{
    public class DonViTinhAdminController : Controller
    {
        // GET: DonViTinhAdmin
        readonly TraSuaEntities _db = new TraSuaEntities();
        readonly DonViTinhDao _dao = new DonViTinhDao();
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
            var list = _db.DonViTinhs.OrderByDescending(x => x.Id).ToList();

            int pageSize = 10;
            if (!String.IsNullOrEmpty(txtSearch))
            {
                ViewBag.txtSearch = txtSearch;
                list = list.Where(x => x.TenDonViTinh.Contains(txtSearch)).OrderByDescending(x => x.Id).ToList();
            }
            var data = new List<DonViTinhModel>();
            int i = 0;
            foreach (var item in list)
            {
                i++;
                var itemmodel = new DonViTinhModel 
                    {Stt = i, Id = item.Id, TenDonVi = item.TenDonViTinh};
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
        [ValidateAntiForgeryToken]
        public IActionResult Add(DonViTinhModel donvitinh)
        {
            if (ModelState.IsValid)
            {
                _dao.AddDonViTinh(donvitinh);
            }
            return RedirectToAction("Index");
        }
        public JsonResult Delete(int ID)
        {
            return Json(_dao.Delete(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }
    }
}