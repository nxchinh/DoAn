using DoAn.Models.Dao.Admin;
using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using DoAn.Common.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAn.Controllers.Admin
{
    public class HoaDonNhapAdminController : Controller
    {
        // GET: HoaDonNhapAdmin
        TraSuaEntities db = new TraSuaEntities();
        public IActionResult Index()
        {
            var result = Common.Constants.USER_SESSION;
            var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(result.ToString());
            if (session != null)
            {
                var model = new List<HoaDonNhapModel>();
                var list = db.HoaDonNhaps.ToList();
                int i = 0;
                foreach (var item in list)
                {
                    i++;
                    var itemmodel = new HoaDonNhapModel
                    {
                        Id = item.Id,
                        STT = i,
                        TenNCC = new NhaCungCapDao().getById(item.MaNCC).TenNCC,
                        TenDangNhap = new KhachHangDao().viewDetail(item.MaNhanVien).TenDangNhap,
                        TongTien = item.TongTien,
                        NgayNhap = item.NgayNhap,
                        ChietKhau = $"{item.ChietKhau:0,0}"
                    };
                    model.Add(itemmodel);

                }
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
           
        }

        public IActionResult CreateHdn()
        {
            var result = Common.Constants.MAHDN_SESSION;
            var sessionMahdn = HttpContext.Session.GetObjectFromJson<Common.Session.MaHoaDonNhap>(result.ToString());
            var listCthdn = new List<CTHDNhapModel>();
            var model = new HoaDonNhapModel();
            if (sessionMahdn != null)
            {
                var list = db.ChiTietHDNs.Where(x => x.MaHDN == sessionMahdn.Id).ToList();
                int i = 0;
                foreach (var item in list)
                {
                    i++;
                    var itemCthdn = new CTHDNhapModel
                    {
                        Id = item.Id,
                        STT = i,
                        TenNguyenLieu = new NguyenLieuDao().getByid(item.MaNguyenLieu).TenNguyenLieu,
                        TenDonViTinh = new DonViTinhDao().GetByid(item.MaDonViTinh).TenDonViTinh,
                        SoLuong = item.SoLuong.ToString(),
                        GiaNhap = item.GiaNhap.ToString(),
                        ChietKhau = item.ChietKhau.ToString(),
                        ThanhTien = item.ThanhTien
                    };
                    listCthdn.Add(itemCthdn);
                }
                var tongtienhdn = db.ChiTietHDNs.Where(x => x.MaHDN == sessionMahdn.Id).Sum(x => x.ThanhTien);
                model.TongTien = tongtienhdn;
            }
            else
            {
                listCthdn = null;
            }
            ViewBag.ListChiTietHDN = listCthdn;
            model.SelectNCC = new SelectList(db.NhaCungCaps, "Id", "TenNCC", 0);
            return View(model);
        }
        [HttpPost]
        public IActionResult CreateHdn(HoaDonNhapModel model)
        {
            if (ModelState.IsValid)
            {
                var result = Common.Constants.MAHDN_SESSION;
                var sessionHdn = HttpContext.Session.GetObjectFromJson<Common.Session.MaHoaDonNhap>(result.ToString());
                DateTime now = DateTime.Now;
                var hoadonnhap = new HoaDonNhap
                {
                    Id = sessionHdn.Id,
                    MaNCC = model.MaNhaCungCap,
                    MaNhanVien = 3,
                    NgayNhap = now,
                    TongTien = db.ChiTietHDNs.Where(x => x.MaHDN == sessionHdn.Id).Sum(x => x.ThanhTien),
                    ChietKhau = model.ChietKhau == null
                        ? 0
                        : new DoAn.Common.Function.ConvertMoney().ConvertTien(model.ChietKhau)
                };
                db.HoaDonNhaps.Add(hoadonnhap);
                HttpContext.Session.Remove(DoAn.Common.Constants.MAHDN_SESSION);
                db.SaveChanges();
                return RedirectToAction("Index", "HoaDonNhapAdmin");
            }
            else
            {
                var result = Common.Constants.MAHDN_SESSION;
                var sessionMahdn = HttpContext.Session.GetObjectFromJson<MaHoaDonNhap>(result.ToString());
                var listCthdn = new List<CTHDNhapModel>();
                var viewmodel = new HoaDonNhapModel();
                if (sessionMahdn != null)
                {
                    var list = db.ChiTietHDNs.Where(x => x.MaHDN == sessionMahdn.Id).ToList();
                    int i = 0;
                    foreach (var item in list)
                    {
                        i++;
                        var itemCthdn = new CTHDNhapModel
                        {
                            Id = item.Id,
                            STT = i,
                            TenNguyenLieu = new NguyenLieuDao().getByid(item.MaNguyenLieu).TenNguyenLieu,
                            TenDonViTinh = new DonViTinhDao().GetByid(item.MaDonViTinh).TenDonViTinh,
                            SoLuong = item.SoLuong.ToString(),
                            GiaNhap = item.GiaNhap.ToString(),
                            ChietKhau = item.ChietKhau.ToString(),
                            ThanhTien = item.ThanhTien
                        };
                        listCthdn.Add(itemCthdn);
                    }
                    var tongtienhdn = db.ChiTietHDNs.Where(x => x.MaHDN == sessionMahdn.Id).Sum(x => x.ThanhTien);
                    model.TongTien = tongtienhdn;
                }
                else
                {
                    listCthdn = null;
                }
                model.SelectNCC = new SelectList(db.NhaCungCaps, "Id", "TenNCC", 0);
                ViewBag.ListChiTietHDN = listCthdn;
                return View(model);
            }
        }
        public IActionResult XemChiTietHDN(string mahoadon)
        {
            var model = new List<CTHDNhapModel>();
            var list = db.ChiTietHDNs.Where(x => x.MaHDN == mahoadon).ToList();
            int i = 0;
            foreach (var item in list)
            {
                i++;
                var itemmodel = new CTHDNhapModel
                {
                    TenNguyenLieu = new NguyenLieuDao().getByid(item.MaNguyenLieu).TenNguyenLieu,
                    TenDonViTinh = new DonViTinhDao().GetByid(item.MaDonViTinh).TenDonViTinh,
                    GiaNhap = item.GiaNhap.ToString(),
                    SoLuong = item.SoLuong.ToString(),
                    ChietKhau = item.ChietKhau.ToString(),
                    ThanhTien = item.ThanhTien,
                    STT = i
                };

                model.Add(itemmodel);
            }
            var hoadonnhap = db.HoaDonNhaps.FirstOrDefault(x => x.Id == mahoadon);
            var hdnmodel = new HoaDonNhapModel
            {
                TenDangNhap = db.KhachHangs.FirstOrDefault(x => x.Id == hoadonnhap.MaNhanVien).TenDangNhap,
                NgayNhap = hoadonnhap.NgayNhap,
                TenNCC = db.NhaCungCaps.FirstOrDefault(x => x.Id == hoadonnhap.MaNCC).TenNCC,
                TongTien = hoadonnhap.TongTien,
                ChietKhau = String.Format("{0:0,0}", hoadonnhap.ChietKhau)
            };
            var chietkhau = new DoAn.Common.Function.ConvertMoney().ConvertTien(hdnmodel.ChietKhau);
            hdnmodel.PhaiTra = (hdnmodel.TongTien - chietkhau);
            ViewBag.HoaDonNhap = hdnmodel;
            return View(model);
        }
        public IActionResult CreateCTHDN()
        {
            var model = new CTHDNhapModel();
            model.SelectDonViTinh = new SelectList(db.DonViTinhs, "Id", "TenDonViTinh", 0);
            model.SelectNguyenLieu = new SelectList(db.NguyenLieux, "Id", "TenNguyenLieu", 0);
            return View(model);
        }
        [HttpPost]
        public IActionResult CreateCTHDN(CTHDNhapModel model)
        {
            if (ModelState.IsValid)
            {
                var cthdn = new ChiTietHDN();
                var result = Common.Constants.MAHDN_SESSION;
                var sessionHdn = HttpContext.Session.GetObjectFromJson<Common.Session.MaHoaDonNhap>(result.ToString());
                if (sessionHdn != null)
                {
                    cthdn.MaHDN = sessionHdn.Id;
                }
                else
                {
                    DateTime dt = DateTime.Now;
                    var mahdn = dt.Day.ToString() + dt.Month.ToString() + dt.Year.ToString() + dt.Hour.ToString() + dt.Minute.ToString() + dt.Second.ToString();
                    var sessionMahdnhap = new Common.Session.MaHoaDonNhap { Id = mahdn };
                    HttpContext.Session.SetObjectAsJson(Common.Constants.MAHDN_SESSION, sessionMahdnhap);
                    cthdn.MaHDN = mahdn;
                }

                cthdn.MaDonViTinh = model.MaDonViTinh;
                cthdn.MaNguyenLieu = model.MaNguyenLieu;
                
                cthdn.SoLuong = new DoAn.Common.Function.ConvertMoney().ConvertTien(model.SoLuong);
                cthdn.GiaNhap = new DoAn.Common.Function.ConvertMoney().ConvertTien(model.GiaNhap);
                cthdn.ChietKhau =new DoAn.Common.Function.ConvertMoney().ConvertTien(model.ChietKhau);
                cthdn.ThanhTien = cthdn.GiaNhap * cthdn.SoLuong - cthdn.ChietKhau;
                db.ChiTietHDNs.Add(cthdn);

                var nguyenlieuDonvi = new NguyenLieuDonViDao().getBy_NLId_DVId(model.MaNguyenLieu, model.MaDonViTinh);
                if (nguyenlieuDonvi != null)
                {
                    var nl_dv = db.NguyenLieu_DonVi.Find(nguyenlieuDonvi.Id);
                    int? soluong = nguyenlieuDonvi.SoLuong;
                    nl_dv.SoLuong = soluong + cthdn.SoLuong;
                    nl_dv.GiaNhap = cthdn.GiaNhap;
                    db.SaveChanges();
                }
                else
                {
                    var nl_dv = new NguyenLieu_DonVi();
                    nl_dv.MaNguyenLieu = cthdn.MaNguyenLieu;
                    nl_dv.MaDonViTinh = cthdn.MaDonViTinh;
                    nl_dv.SoLuong = cthdn.SoLuong;
                    nl_dv.GiaNhap = cthdn.GiaNhap;
                    db.NguyenLieu_DonVi.Add(nl_dv);
                }


                db.SaveChanges();
                return RedirectToAction("CreateHdn", "HoaDonNhapAdmin");
            }
            else
            {
                var viewmodel = new CTHDNhapModel();
                viewmodel.SelectDonViTinh = new SelectList(db.DonViTinhs, "Id", "TenDonViTinh", model.MaDonViTinh);
                viewmodel.SelectNguyenLieu = new SelectList(db.NguyenLieux, "Id", "TenNguyenLieu", model.MaNguyenLieu);
                viewmodel.SoLuong = model.SoLuong;

                return View(viewmodel);
            }

        }
        public IActionResult SuaCTHDN(int id, string soluong, string gianhap, string chietkhau)
        {
            var cthdn = db.ChiTietHDNs.FirstOrDefault(x => x.Id == id);
            var convert = new DoAn.Common.Function.ConvertMoney();
            if (cthdn != null)
            {
                cthdn.SoLuong = convert.ConvertTien(soluong);
                cthdn.GiaNhap = convert.ConvertTien(gianhap);
                cthdn.ChietKhau = convert.ConvertTien(chietkhau);
                cthdn.ThanhTien = cthdn.SoLuong * cthdn.GiaNhap - cthdn.ChietKhau;
            }

            db.SaveChanges();
            return RedirectToAction("CreateHdn", "HoaDonNhapAdmin");
        }
    }
}