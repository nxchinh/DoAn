using DoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using DoAn.Models.Dao.Admin;
using DoAn.Models.Model.Admin;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers.Admin
{
    public class NguyenLieuDonViAdminController : Controller
    {
        // GET: NguyenLieuDonViAdmin
        TraSuaEntities db = new TraSuaEntities();
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
            var list = db.NguyenLieu_DonVi.OrderByDescending(x => x.Id).ToList();

            int pageSize = 10;
            
            var data = new List<NguyenLieuDonViModel>();
            int i = 0;
            foreach (var item in list)
            {
                i++;
                var itemmodel = new NguyenLieuDonViModel();
                itemmodel.STT = i;
                itemmodel.Id = item.Id;
                itemmodel.TenDonVi = db.DonViTinhs.FirstOrDefault(x => x.Id == item.MaDonViTinh)?.TenDonViTinh;
                itemmodel.TenNguyenLieu = db.NguyenLieux.FirstOrDefault(x => x.Id == item.MaNguyenLieu)?.TenNguyenLieu;

                itemmodel.SoLuong = item.SoLuong;
                itemmodel.GiaNhap =String.Format("{0:0,0}", item.GiaNhap);
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

    }
}