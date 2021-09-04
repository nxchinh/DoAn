using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoAn.Common.Session;
using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAn.ViewComponents
{
    public class TenChiNhanhViewComponent:ViewComponent
    {
        readonly TraSuaEntities _db = new TraSuaEntities();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sessionNhanvien = HttpContext.Session.GetObjectFromJson<NhanVienSession>(Common.Constants.NHANVIEN_SESSION);
            var nhanvien = _db.NhanViens.FirstOrDefault(x => x.Id == sessionNhanvien.Id);
            ViewBag.TenChiNhanh = _db.ChiNhanhs.FirstOrDefault(x => x.Id == nhanvien.MaChiNhanh)?.TenChiNhanh;
            return View();
        }
    }
}
