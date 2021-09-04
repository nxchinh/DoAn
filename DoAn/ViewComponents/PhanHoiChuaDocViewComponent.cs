using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoAn.Common.Function;
using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAn.ViewComponents
{
    public class PhanHoiChuaDocViewComponent:ViewComponent
    {
        readonly TraSuaEntities _db = new TraSuaEntities();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = await _db.PhanHois.Where(x => x.DaXem == 0).ToListAsync();
            var model = new List<PhanHoiModel>();
            DateTime now = DateTime.Now;
            foreach (var item in list)
            {
                var itemmodel = new PhanHoiModel();
                var khachhang = _db.KhachHangs.FirstOrDefault(x => x.Id == item.MaKhachHang);
                if (khachhang != null) 
                    itemmodel.UserName = khachhang.TenDangNhap;
                itemmodel.PhanHoiTu = new LamTronThoiGian().LamTron(now, item.ThoiGian);
                model.Add(itemmodel);
            }
            return View(model);
        }
    }
}
