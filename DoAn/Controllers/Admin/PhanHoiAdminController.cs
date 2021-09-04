using System;
using System.Collections.Generic;
using System.Linq;
using DoAn.Models.Model.Admin;
using DoAn.Models.EF;
using DoAn.Models.Dao.Admin;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers.Admin
{
    
    public class PhanHoiAdminController : Controller
    {
        // GET: PhanHoiAdmin
        TraSuaEntities db = new TraSuaEntities();
        PhanHoiDao dao = new PhanHoiDao();
        public ActionResult Index()
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
        public JsonResult List(int? page)
        {
            int pageSize = 10;
            var list = db.PhanHois.OrderByDescending(x => x.Id).ToList();
            
            var data = new List<PhanHoiModel>();
            int i = 0;
            foreach (var item in list)
            {
                i++;
                var itemmodel = new PhanHoiModel();
                itemmodel.STT = i;
                itemmodel.Id = item.Id;
                var khachhang = new KhachHangDao();
                itemmodel.UserName = khachhang.viewDetail(item.MaKhachHang).TenDangNhap;
                itemmodel.Content = item.NoiDung;
                DateTime now = DateTime.Now;
                var convert = new DoAn.Common.Function.LamTronThoiGian();
                itemmodel.PhanHoiTu = convert.LamTron(now, item.ThoiGian);
                itemmodel.DaXem = item.DaXem;
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
        public JsonResult Update(int? ID)
        {
            return Json(dao.Update(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }
    }
}