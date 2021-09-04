using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoAn.Models.Dao.Admin;
using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace DoAn.ViewComponents
{
    public class TopSpBanChayViewComponent:ViewComponent
    {
        readonly TraSuaEntities _db = new TraSuaEntities();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listTopProduct = new List<SanPhamModel>();
            var list = (from sp in _db.SanPhams.ToList()
                join cthdb in _db.ChiTietHDBs on sp.Id equals cthdb.MaSanPham
                group cthdb by cthdb.MaSanPham into g
                select new SanPhamModel
                {
                    Id = g.FirstOrDefault()?.MaSanPham,
                    TongSL = g.Sum(x => x.SoLuong),

                }).OrderByDescending(x => x.TongSL).ToList();
            var i = 0;
            foreach (var item in list)
            {
                i++;
                var product = new ProductDao().getByid(item.Id);
                var itemmodel = new SanPhamModel
                {
                    STT = i,
                    Id = item.Id,
                    Ten = product.TenSanPham,
                    Anh = product.Anh,
                    TongSL = item.TongSL
                };
                if (new CategoryDao().getSPChinh(itemmodel.Id) == 1)
                {
                    listTopProduct.Add(itemmodel);
                }

            }

            return View(listTopProduct.Take(5));
        }
    }
}
