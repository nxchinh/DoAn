using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace DoAn.ViewComponents
{
    public class TopPhanHoiViewComponent:ViewComponent
    {
        readonly TraSuaEntities _db = new TraSuaEntities();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = (from ph in _db.PhanHois.ToList()
                join kh in _db.KhachHangs on ph.MaKhachHang equals kh.Id

                select new
                {
                    Id = ph.Id,
                    MaKhachHang = ph.MaKhachHang,
                    NoiDung = ph.NoiDung,
                    TenKhachHang = kh.TenDangNhap,
                    DaXem = ph.DaXem
                }).OrderByDescending(x => x.Id).Take(5).ToList();

            var model = new List<PhanHoiModel>();
            var i = 0;
            foreach (var item in list)
            {
                i++;
                var itemmodel = new PhanHoiModel
                {
                    STT = i,
                    Id = item.Id,
                    UserId = item.MaKhachHang,
                    Content = item.NoiDung,
                    UserName = item.TenKhachHang,
                    DaXem = item.DaXem
                };
                model.Add(itemmodel);
            }
            return View(model);
        }
    }
}
