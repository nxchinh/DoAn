using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoAn.Common.Session;
using DoAn.Models.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAn.ViewComponents
{
    public class TongTienSanPhamViewComponent: ViewComponent
    {
        readonly TraSuaEntities _db = new TraSuaEntities();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sessionBillid = HttpContext.Session.GetObjectFromJson<BillSession>(Common.Constants.BILL_SESSION);
            if (sessionBillid != null)
            {
                var list = await _db.ChiTietHDBs.Where(x => x.MaHDB == sessionBillid.Id).ToListAsync();
                ViewBag.TongTienSanPham = list.Sum(x => x.ThanhTien);
            }
            else
            {
                ViewBag.TongTienSanPham = 0;
            }

            return View();
        }
    }
}
