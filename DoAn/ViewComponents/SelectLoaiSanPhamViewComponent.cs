using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DoAn.ViewComponents
{
    public class SelectLoaiSanPhamViewComponent:ViewComponent
    {
        readonly TraSuaEntities _db = new TraSuaEntities();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new SanPhamModel();
            model.SelectMaLoai = new SelectList(_db.LoaiSanPhams.Where(x => x.SanPhamChinh == 1), "Id", "TenLoaiSanPham", 1);
            return View(model);
        }
    }
}
