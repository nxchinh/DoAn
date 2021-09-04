using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoAn.Common.Session;
using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DoAn.ViewComponents
{
    public class TongHoaDonViewComponent:ViewComponent
    {
        readonly TraSuaEntities _db = new TraSuaEntities();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sessionNhanvien = HttpContext.Session.GetObjectFromJson<NhanVienSession>(Common.Constants.NHANVIEN_SESSION);
            var nhanvien = _db.NhanViens.FirstOrDefault(x => x.Id == sessionNhanvien.Id);
            var model = await _db.HoaDonBans.Where(x => x.MaChiNhanh == nhanvien.MaChiNhanh && x.Duyet == 1 && x.DaThanhToan == 0).ToListAsync();
            ViewBag.TongHoaDon = model.Count;
            return View();
        }
    }
}
