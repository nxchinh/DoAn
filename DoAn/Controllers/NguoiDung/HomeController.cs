using DoAn.Models.Dao.NguoiDung;
using DoAn.Models.EF;
using DoAn.Models.Model.NguoiDung;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoAn.Common.Session;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers.NguoiDung
{
    public class HomeController : Controller
    {
        // GET: Home
        readonly TraSuaEntities _db = new TraSuaEntities();
        public IActionResult Index()
        {
            var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(Common.Constants.USER_SESSION);
            if (session == null)
                return View();
            var cart = new CartDao().GetProductsByIdUser(session.UserId);
            if(cart != null)
            {
                var countitem = cart.Max(x => x.SanPhamThu);
                var sessionSanphamthu = new SanPhamThuSession();
                sessionSanphamthu.SanPham_Thu =Convert.ToInt32( countitem);
                HttpContext.Session.SetObjectAsJson(Common.Constants.SANPHAMTHU_SESSION, sessionSanphamthu);
            }

            return View();
        }
        public IActionResult ThongTinTaiKhoan()
        {
            var model = new KhachHangModel();
            var result = Common.Constants.USER_SESSION;
            var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(result.ToString());
            var khachhang = new KhachHangDao().getById(session.UserId);
            model.Id = khachhang.Id;
            model.HoTen = khachhang.HoTen;
            model.Email = khachhang.Email;
            model.SDT = khachhang.SDT;
            model.DiaChi = khachhang.DiaChi;
            model.UserName = khachhang.TenDangNhap;
            return View(model);
        }
        [HttpPost]
        public IActionResult ThongTinTaiKhoan(KhachHangModel model)
        {
            if (!ModelState.IsValid)
            {
                var result = Common.Constants.USER_SESSION;
                var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(result.ToString());
                var khachhang = _db.KhachHangs.Find(session.UserId);
                khachhang.HoTen = model.HoTen;
                khachhang.DiaChi = model.DiaChi;
                khachhang.SDT = model.SDT;
                _db.SaveChanges();

                var viewmodel = new KhachHangModel
                {
                    Id = khachhang.Id,
                    HoTen = khachhang.HoTen,
                    Email = khachhang.Email,
                    SDT = khachhang.SDT,
                    DiaChi = khachhang.DiaChi,
                    UserName = khachhang.TenDangNhap
                };
                ViewBag.ThongTinTaiKhoan = "Bạn đã thay đổi thông tin thành công";
                return View(viewmodel);
            }
            else
            {
                var viewmodel = new KhachHangModel
                {
                    Id = model.Id,
                    HoTen = model.HoTen,
                    Email = model.Email,
                    SDT = model.SDT,
                    DiaChi = model.DiaChi,
                    UserName = model.TenDangNhap
                };
                ViewBag.ThongTinTaiKhoan = "Bạn đã thay đổi thông tin thành công";
                return View(viewmodel);
            }
            
        }
        public IActionResult MuaNgay(int productid)
        {
            var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(Common.Constants.USER_SESSION);
            if (session != null)
            {
                ProductDao dao = new ProductDao();
                ViewBag.Topping = dao.getTopping();
                ViewBag.Duong = dao.getDuong();
                ViewBag.Da = dao.getDa();
                ViewBag.Size = dao.getSize();
                var product = dao.getByid(productid);
                return View(product);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

        public IActionResult DoiMatKhau()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DoiMatKhau(DoiMatKhauNguoiDung model)
        {
            if (ModelState.IsValid)
            {
                var result = Common.Constants.USER_SESSION;
                var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(result.ToString());
                if (session != null)
                {
                    var khachhang = _db.KhachHangs.Find(session.UserId);
                    if(khachhang.MatKhau == Common.Function.Encrytor.MD5Hash(model.MatKhauCu))
                    {
                        khachhang.MatKhau = Common.Function.Encrytor.MD5Hash(model.MatKhau);
                        await _db.SaveChangesAsync();
                        ViewBag.DoiMatKhau = "Bạn đã đổi mật khẩu thành công";
                    }
                    else
                    {
                        ViewBag.Error = "Mật khẩu không đúng";
                    }
                    
                }
            }
            return View();
        }
        public async Task<IActionResult> PhanHoi(string noidung)
        {
            var result = Common.Constants.USER_SESSION;
            var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(result.ToString());
            DateTime now = DateTime.Now;
            var phanhoi = new PhanHoi {NoiDung = noidung, ThoiGian = now, MaKhachHang = session.UserId, DaXem = 0};
            _db.PhanHois.Add(phanhoi);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}