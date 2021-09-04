using System;
using System.Collections.Generic;
using System.Linq;
using DoAn.Common.Function;
using DoAn.Models.Dao.Admin;
using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using Microsoft.AspNetCore.Mvc;


namespace DoAn.Controllers.Admin
{
    public class HomeAdminController : Controller
    {
        TraSuaEntities db = new TraSuaEntities();
        public IActionResult Index()
        {
            var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(Common.Constants.USER_SESSION);
            if (session == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }
        public ActionResult DoiMatKhau()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DoiMatKhau(DoiMatKhauAdmin model)
        {
            if (ModelState.IsValid)
            {
                var result = Common.Constants.USER_SESSION;
                var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(result.ToString());
                if (session != null)
                {
                    var khachhang = db.KhachHangs.Find(session.UserId);
                    if (khachhang.MatKhau == Encrytor.MD5Hash(model.MatKhauCu))
                    {
                        khachhang.MatKhau = Encrytor.MD5Hash(model.MatKhau);
                        db.SaveChanges();
                        ViewBag.DoiMatKhau = "Bạn đã đổi mật khẩu thành công";
                    }
                    else
                    {
                        ViewBag.Error = "Mật khẩu không đúng";
                    }

                }
            }
            return View();
        }

    }
}