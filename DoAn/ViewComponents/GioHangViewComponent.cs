using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoAn.Models.Dao.NguoiDung;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.ViewComponents
{
    public class GioHangViewComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(Common.Constants.USER_SESSION);
            if (session != null)
            {
                var cart = new CartDao().GetProductsByIdUser(session.UserId);
                var countitem = cart.Max(x => x.SanPhamThu);
                ViewBag.CountItem = countitem;
                return View(cart);
            }
            return View();
        }
    }
}
