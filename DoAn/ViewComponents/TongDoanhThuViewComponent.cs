using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoAn.Models.EF;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.ViewComponents
{
    public class TongDoanhThuViewComponent:ViewComponent
    {
        readonly TraSuaEntities _db = new TraSuaEntities();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = _db.HoaDonBans.ToList();
            ViewBag.TongDoanhThu = list.Sum(x => x.TongTien);
            return View();
        }
    }
}
