using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoAn.Common.Session;
using DoAn.Models.Dao.Admin;
using DoAn.Models.Dao.NhanVien;
using DoAn.Models.EF;
using DoAn.Models.Model.NhanVien;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAn.ViewComponents
{
    public class HoaDonTheoIdViewComponent:ViewComponent
    {
        readonly TraSuaEntities _db = new TraSuaEntities();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sessionBillid = HttpContext.Session.GetObjectFromJson<BillSession>(Common.Constants.BILL_SESSION);
            var list = new List<CTHDBanModel>();
            if (sessionBillid != null)
            {
                var listBill = await _db.ChiTietHDBs.Where(x => x.MaHDB == sessionBillid.Id.ToString()).ToListAsync();
                var chitietthuSession = HttpContext.Session.GetObjectFromJson<BillSession>(Common.Constants.CTTHU_SESSION);
                foreach (var item in listBill)
                {
                    var model = new CTHDBanModel();
                    var product = new ProductDao().getByid(item.MaSanPham);
                    var categorydao = new CategoryDao();
                    if (categorydao.getSPChinh(product.Id) == 1)
                    {
                        model.MaSanPham = item.MaSanPham;
                        model.Id = item.Id;
                        model.TenSanPham = product.TenSanPham;
                        model.Anh = product.Anh;
                        model.SoLuong = item.SoLuong;
                        ChiTietHDB cthdb = new CTHDBanDao().getById(item.Id);
                        model.CTThu = cthdb.ChiTietThu;
                        model.GiaBan = new CTHDBanDao().Tien1LyTraSua(sessionBillid.Id, product.Id, cthdb.ChiTietThu);
                        model.ThanhTien = model.GiaBan * model.SoLuong;
                        model.MoTa = new CTHDBanDao().getMoTa(sessionBillid.Id.ToString(), cthdb.ChiTietThu);
                        list.Add(model);
                    }
                }

            }
            return View(list);
        }
    }
}
