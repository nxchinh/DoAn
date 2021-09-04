using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoAn.Models.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAn.ViewComponents
{
    public class HoaDonChuaPheDuyetViewComponent:ViewComponent
    {
        readonly TraSuaEntities _db = new TraSuaEntities();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var hoadonchuapheduyet = await _db.HoaDonBans.Where(x => x.Duyet == 0).ToListAsync();
            return View(hoadonchuapheduyet);
        }
    }
}
