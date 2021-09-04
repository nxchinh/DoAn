using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoAn.Models.Dao.Admin;
using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace DoAn.ViewComponents
{
    public class Top5KhachHangMuaNhieuNhatViewComponent:ViewComponent
    {
        readonly TraSuaEntities _db = new TraSuaEntities();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listTopKhachHang = new List<HoaDonBanModel>();
            var list = (from hdb in _db.HoaDonBans.ToList()
                where hdb.MaKhach != 4
                group hdb by hdb.MaKhach into g
                select new
                {
                    MaKhach = g.FirstOrDefault()?.MaKhach,
                    TongTien = g.Sum(x => x.TongTien),
                    TongHoaDon = g.Count()
                }).OrderByDescending(x => x.TongTien).ToList();

            foreach (var item in list)
            {
                var khachhang = new KhachHangDao().viewDetail(item.MaKhach);
                var itemmodel = new HoaDonBanModel
                {
                    MaKhach = item.MaKhach,
                    TongSoHoaDon = item.TongHoaDon,
                    TenDangNhap = khachhang.TenDangNhap,
                    TongTienDaMua = item.TongTien
                };


                listTopKhachHang.Add(itemmodel);
            }

            return View(listTopKhachHang.Take(5));
        }
    }
}
