using DoAn.Models.EF;
using DoAn.Models.Model.NhanVien;
using DoAn.Models.Model.QuanLy;
using System.Linq;
using DoAn.Common.Session;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers.QuanLy
{
    public class TaiKhoanQuanLyController : Controller
    {
        // GET: TaiKhoanQuanLy
        TraSuaEntities db = new TraSuaEntities();

        public IActionResult ThongTinTaiKhoan()
        {
            var sessionNhanvien = HttpContext.Session.GetObjectFromJson<NhanVienSession>(Common.Constants.NHANVIEN_SESSION);
            if (sessionNhanvien != null)
            {
                var nhanvien = db.NhanViens.FirstOrDefault(x => x.Id == sessionNhanvien.Id);
                var model = new NhanVienModel
                    {
                        Id = nhanvien.Id,
                        DiaChi = nhanvien.DiaChi,
                        HoTen = nhanvien.HoTen,
                        SDT = nhanvien.SDT,
                        TenDangNhap = nhanvien.TenDangNhap,
                        Luong = db.ChucVus.FirstOrDefault(x => x.Id == nhanvien.MaChucVu)?.Luong,
                        TenChucVu = db.ChucVus.FirstOrDefault(x => x.Id == nhanvien.MaChucVu)?.TenChucVu,
                        TenChiNhanh = db.ChiNhanhs.FirstOrDefault(x => x.Id == nhanvien.MaChiNhanh)?.TenChiNhanh
                    };
                    return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public IActionResult ThongTinTaiKhoan(NhanVienModel model)
        {

            if (!ModelState.IsValid)
            {
                var nhanvien = db.NhanViens.Find(model.Id);
                nhanvien.HoTen = model.HoTen;
                nhanvien.DiaChi = model.DiaChi;
                nhanvien.SDT = model.SDT;
                db.SaveChanges();

                var viewmodel = new NhanVienModel
                {
                    TenDangNhap = model.TenDangNhap,
                    Id = model.Id,
                    DiaChi = model.DiaChi,
                    HoTen = model.HoTen,
                    SDT = model.SDT,
                    Luong = db.ChucVus.FirstOrDefault(x => x.Id == nhanvien.MaChucVu)?.Luong
                };


                viewmodel.TenDangNhap = nhanvien.TenDangNhap;
                viewmodel.TenChucVu = db.ChucVus.FirstOrDefault(x => x.Id == nhanvien.MaChucVu)?.TenChucVu;
                viewmodel.TenChiNhanh = db.ChiNhanhs.FirstOrDefault(x => x.Id == nhanvien.MaChiNhanh)?.TenChiNhanh;

                ViewBag.Success = "Bạn đã thay đổi thông tin thành công";
                return View(viewmodel);
            }
            else
            {
                var nhanvien = db.NhanViens.Find(model.Id);
                var viewmodel = new NhanVienModel
                {
                    Id = model.Id,
                    DiaChi = model.DiaChi,
                    HoTen = model.HoTen,
                    SDT = model.SDT,
                    Luong = db.ChucVus.FirstOrDefault(x => x.Id == nhanvien.MaChucVu)?.Luong,
                    TenDangNhap = nhanvien.TenDangNhap,
                    TenChucVu = db.ChucVus.FirstOrDefault(x => x.Id == nhanvien.MaChucVu)?.TenChucVu,
                    TenChiNhanh = db.ChiNhanhs.FirstOrDefault(x => x.Id == nhanvien.MaChiNhanh)?.TenChiNhanh
                };
                return View(viewmodel);
            }
        }
        public IActionResult DoiMatKhau()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DoiMatKhau(DoiMatKhauQuanLy model)
        {
            if (ModelState.IsValid)
            {
                var session = HttpContext.Session.GetObjectFromJson<NhanVienSession>(Common.Constants.NHANVIEN_SESSION);
                if (session != null)
                {
                    var nhanvien = db.NhanViens.Find(session.Id);
                    if (nhanvien.MatKhau == Common.Function.Encrytor.MD5Hash(model.MatKhauCu))
                    {
                        nhanvien.MatKhau = Common.Function.Encrytor.MD5Hash(model.MatKhau);
                        db.SaveChanges();
                        ViewBag.Success = "Bạn đã đổi mật khẩu thành công";
                    }
                    else
                    {
                        ViewBag.Error = "Mật khẩu không đúng";
                    }

                }
            }
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/Home/Index");
        }
    }
}