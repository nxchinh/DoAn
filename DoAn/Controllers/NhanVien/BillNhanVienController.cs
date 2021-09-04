using DoAn.Common.Session;
using DoAn.Models.Dao.Admin;
using DoAn.Models.Dao.NhanVien;
using DoAn.Models.EF;
using DoAn.Models.Model.NhanVien;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers.NhanVien
{
    public class BillNhanVienController : Controller
    {
        // GET: BillNhanVien
        TraSuaEntities db = new TraSuaEntities();
        public IActionResult CreateBill()
        {
            return View();
        }

        public ActionResult CreateChiTietHD(string listproduct)
        {

            var sessionBillid = HttpContext.Session.GetObjectFromJson<BillSession>(Common.Constants.BILL_SESSION);
            var chitietthuSession = HttpContext.Session.GetObjectFromJson<BillSession>(Common.Constants.CTTHU_SESSION);

            if (sessionBillid != null)
            {
                var chitietthu = chitietthuSession.ChiTietThu + 1;
                string[] productsid = listproduct.Split(',');
                foreach (var item in productsid)
                {

                    ChiTietHDB cthdb = new ChiTietHDB();
                    cthdb.MaHDB = sessionBillid.Id;
                    cthdb.MaSanPham = int.Parse(item);
                    var product = new ProductDao().getByid(cthdb.MaSanPham);
                    cthdb.SoLuong = 1;
                    cthdb.ThanhTien = product.KhuyenMai * cthdb.SoLuong;
                    cthdb.ThuocSanPham = int.Parse(productsid[0]);
                    cthdb.ChiTietThu = chitietthu;
                    new CTHDBanDao().Insert_CT(cthdb);

                }
                chitietthuSession.ChiTietThu = chitietthu;
                HttpContext.Session.SetObjectAsJson(Common.Constants.CTTHU_SESSION, chitietthuSession);

            }
            else
            {
                // tạo mã hóa đơn
                var sessionNhanvien = HttpContext.Session.GetObjectFromJson<NhanVienSession>(Common.Constants.NHANVIEN_SESSION);
                DateTime now = DateTime.Now;
                var idbill = sessionNhanvien.Id.ToString() + now.Day.ToString() + now.Hour.ToString() + now.Minute.ToString() + now.Second.ToString();
                var billSession = new BillSession();
                billSession.Id = idbill;
                HttpContext.Session.SetObjectAsJson(Common.Constants.BILL_SESSION, billSession);

                billSession.ChiTietThu = 1;
                HttpContext.Session.SetObjectAsJson(Common.Constants.CTTHU_SESSION, billSession);

                var hoadonban = new HoaDonBan
                {
                    Id = idbill,
                    MaNhanVien = sessionNhanvien.Id,
                    MaKhach = 4,
                    GiamGia = 0,
                    TongTien = 0,
                    NgayBan = now,
                    DaThanhToan = 1,
                    Duyet = 1
                };

                var machinhanh = new Models.Dao.Admin.NhanVienDao().getByid(sessionNhanvien.Id).MaChiNhanh;
                hoadonban.MaChiNhanh = machinhanh;
                db.HoaDonBans.Add(hoadonban);
                db.SaveChanges();

                string[] productsid = listproduct.Split(',');
                foreach (var item in productsid)
                {
                    sessionBillid = HttpContext.Session.GetObjectFromJson<BillSession>(Common.Constants.BILL_SESSION);
                    ChiTietHDB cthdb = new ChiTietHDB();
                    cthdb.MaHDB = sessionBillid.Id;
                    cthdb.MaSanPham = int.Parse(item);
                    var product = new ProductDao().getByid(cthdb.MaSanPham);
                    var giaban = product.KhuyenMai;
                    cthdb.SoLuong = 1;
                    cthdb.ThuocSanPham = int.Parse(productsid[0]);
                    cthdb.ThanhTien = cthdb.SoLuong * giaban;
                    cthdb.ChiTietThu = 1;
                    new CTHDBanDao().Insert_CT(cthdb);

                }
            }
            var tongtienhd = db.HoaDonBans.Where(x => x.Id == sessionBillid.Id).Sum(x => x.TongTien);
            var hoadonban1 = db.HoaDonBans.Find(sessionBillid?.Id);
            hoadonban1.TongTien = tongtienhd;
            db.SaveChanges();
            return Redirect("/HomeNhanVien/Index");
        }
        public ActionResult UpdateChiTietHD(int productid, int? soluong, int? chitietthu)
        {
            var sessionBillid = HttpContext.Session.GetObjectFromJson<BillSession>(Common.Constants.BILL_SESSION);

            if (sessionBillid != null)
            {
                var billid = sessionBillid.Id;
                var list = new CTHDBanDao().layDSSP(productid, sessionBillid.Id, chitietthu);
                foreach (var item in list)
                {
                    var cart = new ChiTietHDB();
                    new CTHDBanDao().Update(item.Id, soluong);
                }
            }
            var tongtienhd = db.HoaDonBans.Where(x => x.Id == sessionBillid.Id).Sum(x => x.TongTien);
            var hoadonban = db.HoaDonBans.Find(sessionBillid?.Id);
            hoadonban.TongTien = tongtienhd;
            db.SaveChanges();
            return RedirectToAction("Index", "HomeNhanVien");
        }
        public ActionResult DeleteChiTietHD(int productid, int? chitietthu)
        {
            var sessionBillid = HttpContext.Session.GetObjectFromJson<BillSession>(Common.Constants.BILL_SESSION);
            if (sessionBillid != null)
            {
                var billid = sessionBillid.Id;
                var list = new CTHDBanDao().layDSSP(productid, sessionBillid.Id, chitietthu);
                foreach (var item in list)
                {

                    new CTHDBanDao().Delete(item.Id);
                }
            }
            var tongtienhd = db.HoaDonBans.Where(x => x.Id == sessionBillid.Id).Sum(x => x.TongTien);
            var hoadonban = db.HoaDonBans.Find(sessionBillid?.Id);
            hoadonban.TongTien = tongtienhd;
            db.SaveChanges();

            return RedirectToAction("Index", "HomeNhanVien");
        }

        public ActionResult ThanhToan()
        {
            var model = new List<CTHDBanModel>();
            var sessionBillid = HttpContext.Session.GetObjectFromJson<BillSession>(Common.Constants.BILL_SESSION);
            if (sessionBillid == null)
            {
                return RedirectToAction("Index", "HomeNhanVien");
            }
            var list = db.ChiTietHDBs.Where(x => x.MaHDB == sessionBillid.Id).ToList();
            var hoadonban = db.HoaDonBans.Find(sessionBillid.Id);
            hoadonban.TongTien = list.Sum(x => x.ThanhTien);
            db.SaveChanges();
            var i = 0;
            foreach (var item in list)
            {
                var loaispdao = new CategoryDao();
                var product = new ProductDao().getByid(item.MaSanPham);
                var itemmodel = new CTHDBanModel
                {
                    TenSanPham = product.TenSanPham,
                    GiaBan = product.KhuyenMai,
                    SoLuong = item.SoLuong,
                    ThanhTien = item.ThanhTien
                };
                if (loaispdao.getSPChinh(item.MaSanPham) == 1)
                {
                    i++;
                    itemmodel.STT = i;
                }
                if (product.MaLoaiSanPham != 12 && product.MaLoaiSanPham != 13)
                {
                    model.Add(itemmodel);
                }

            }
            ViewBag.MaHoaDon = sessionBillid.Id;
            return View(model);
        }
        public ActionResult DanhSachHoaDon_NhanVien()
        {
            var sessionNhanvien = HttpContext.Session.GetObjectFromJson<NhanVienSession>(Common.Constants.NHANVIEN_SESSION);
            var model = new List<HoaDonBanModel>();
            var nhanvien = db.NhanViens.FirstOrDefault(x => x.Id == sessionNhanvien.Id);
            if (nhanvien != null)
            {
                var list = db.HoaDonBans.Where(x => x.MaChiNhanh == nhanvien.MaChiNhanh && x.Duyet == 1 && x.DaThanhToan == 0).ToList();
                int i = 0;
                foreach (var item in list)
                {
                    i++;
                    var khachhang = db.KhachHangs.FirstOrDefault(x => x.Id == item.MaKhach);
                    var itemmodel = new HoaDonBanModel
                    {
                        Id = item.Id,
                        DiaChi = khachhang?.DiaChi,
                        SDT = khachhang?.SDT,
                        HoTen = khachhang?.HoTen,
                        STT = i,
                        TongTien = item.TongTien
                    };
                    model.Add(itemmodel);
                }
            }
            return View(model);
        }
        public ActionResult ChiTietHoaDon_Online(string mahoadon)
        {
            var model = new List<CTHDBanModel>();

            var list = db.ChiTietHDBs.Where(x => x.MaHDB == mahoadon).ToList();
            var i = 0;
            foreach (var item in list)
            {
                var itemmodel = new CTHDBanModel();
                var loaispdao = new CategoryDao();
                var product = new ProductDao().getByid(item.MaSanPham);
                itemmodel.TenSanPham = product.TenSanPham;
                itemmodel.GiaBan = product.KhuyenMai;
                itemmodel.SoLuong = item.SoLuong;
                itemmodel.ThanhTien = item.ThanhTien;
                if (loaispdao.getSPChinh(item.MaSanPham) == 1)
                {
                    i++;
                    itemmodel.STT = i;
                }
                if (product.MaLoaiSanPham != 12 && product.MaLoaiSanPham != 13)
                {
                    model.Add(itemmodel);
                }

            }
            var hoadonban = db.HoaDonBans.FirstOrDefault(x => x.Id == mahoadon);
            var khachhang = db.KhachHangs.FirstOrDefault(x => x.Id == hoadonban.MaKhach);
            ViewBag.KhachHang = khachhang;
            ViewBag.MaHoaDon = mahoadon;
            return View(model);
        }
        public ActionResult SuaThanhToanOffline_NhanVien(string mahdb)
        {
            var hoadonban = db.HoaDonBans.Find(mahdb);
            hoadonban.DaThanhToan = 1;
            db.SaveChanges();
            HttpContext.Session.Remove(DoAn.Common.Constants.MAHDN_SESSION);
            HttpContext.Session.Remove(DoAn.Common.Constants.CTTHU_SESSION);
            HttpContext.Session.Remove(DoAn.Common.Constants.SANPHAMTHU_SESSION);
            HttpContext.Session.Remove(DoAn.Common.Constants.BILL_SESSION);
            return RedirectToAction("Index", "HomeNhanVien");
        }
        public ActionResult SuaThanhToanOnline_NhanVien(string mahdb)
        {
            var hoadonban = db.HoaDonBans.Find(mahdb);
            hoadonban.DaThanhToan = 1;
            db.SaveChanges();
            HttpContext.Session.Remove(DoAn.Common.Constants.MAHDN_SESSION);
            HttpContext.Session.Remove(DoAn.Common.Constants.CTTHU_SESSION);
            HttpContext.Session.Remove(DoAn.Common.Constants.SANPHAMTHU_SESSION);
            HttpContext.Session.Remove(DoAn.Common.Constants.BILL_SESSION);
            return RedirectToAction("DanhSachHoaDon_NhanVien", "BillNhanVien");
        }
    }
}