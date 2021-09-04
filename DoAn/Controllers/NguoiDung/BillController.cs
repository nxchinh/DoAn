using System;
using DoAn.Common.Function;
using DoAn.Models.Dao.NguoiDung;
using DoAn.Models.EF;
using DoAn.Models.Model.NguoiDung;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DoAn.Controllers.NguoiDung
{
    public class BillController : Controller
    {
        // GET: Bill
        TraSuaEntities _db = new TraSuaEntities();
        // GET: Bill
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public BillController(IWebHostEnvironment webHostEnvironment,IConfiguration iConfig)
        {
            _webHostEnvironment = webHostEnvironment;
            _configuration = iConfig;
        }
        public IActionResult CreateBill(int tongtien)
        {
            ViewBag.TotalMoney = tongtien;
            var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(Common.Constants.USER_SESSION);
            var khachhang = new KhachHangDao().getById(session.UserId);
            var model = new HoaDonBanModel
            {
                MaKhach = khachhang.Id,
                TongTien = tongtien,
                DiaChi = khachhang.DiaChi,
                HoTen = khachhang.HoTen,
                SDT = khachhang.SDT,
                Email = khachhang.Email
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateBill(HoaDonBanModel model)
        {
            if (ModelState.IsValid)
            {
                var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(Common.Constants.USER_SESSION);
                if (session != null)
                {

                    new KhachHangDao().Update(session.UserId, model.HoTen, model.DiaChi, model.SDT, model.Email);
                    // tạo idbill
                    DateTime now = DateTime.Now;
                    var idbill = session.UserId.ToString() + now.Day.ToString() + now.Hour.ToString() + now.Minute.ToString() + now.Second.ToString();
                    //insert order
                    var dao = new CartDao();
                    var cart = dao.GetProductsByIdUser(session.UserId);
                    var item = new HoaDonBan
                    {
                        Id = idbill.ToString(),
                        MaKhach = session.UserId,
                        MaNhanVien = 1,
                        DaThanhToan = 0,
                        Duyet = 0,
                        TongTien = model.TongTien,
                        NgayBan = now
                    };



                    var result = new BillDao().Insert(item);

                    foreach (var item_hdb in cart)
                    {
                        //insert orderdetail
                        var product = new ProductDao().viewDetail(item_hdb.MaSanPham);
                        var ctHdb = new ChiTietHDB
                        {
                            MaHDB = idbill.ToString(),
                            MaSanPham = item_hdb.MaSanPham,
                            SoLuong = item_hdb.SoLuong,
                            GiamGia = 0,
                            ThanhTien = product.KhuyenMai * item_hdb.SoLuong,
                            ThuocSanPham = item_hdb.ThuocSanPham,
                            ChiTietThu = item_hdb.SanPhamThu
                        };
                        new BillDao().Insert_Bill_Detail(ctHdb);
                        //delete cart
                        dao.Delete(item_hdb.Id);
                    }


                    var khachhang = new KhachHangDao().getById(session.UserId);
                    var webRoot = _webHostEnvironment.WebRootPath;
                    var file = System.IO.Path.Combine(webRoot, "neworder.html");
                    string content = System.IO.File.ReadAllText(file);
                    content = content.Replace("{{CustomerName}}", khachhang.HoTen);
                    content = content.Replace("{{Phone}}", khachhang.SDT);
                    content = content.Replace("{{Email}}", khachhang.Email);
                    content = content.Replace("{{Address}}", khachhang.DiaChi);
                    content = content.Replace("{{Total}}", $"{model.TongTien:0,0}");

                    try
                    {
                        var toEmail = _configuration.GetSection("MySettings").GetSection("ToEmailAddress").Value;
                        new MailHelper().SendMail(khachhang.Email, "Đơn hàng mới từ GongCha", content);
                        new MailHelper().SendMail(toEmail, "Đơn hàng mới từ GongCha", content);


                        ViewBag.TotalMoney = model.TongTien;
                        HttpContext.Session.Remove(DoAn.Common.Constants.SANPHAMTHU_SESSION);

                        var viewmodel = new HoaDonBanModel
                        {
                            MaKhach = khachhang.Id,
                            TongTien = model.TongTien,
                            DiaChi = khachhang.DiaChi,
                            HoTen = khachhang.HoTen,
                            SDT = khachhang.SDT,
                            Email = khachhang.Email
                        };

                        ViewBag.Success = "Bạn vừa đặt hàng thành công, kiếm tra email của bạn";
                        return View(viewmodel);
                    }
                    catch (Exception)
                    {
                        ViewBag.TotalMoney = model.TongTien;

                        var viewmodel = new HoaDonBanModel
                        {
                            MaKhach = khachhang.Id,
                            TongTien = model.TongTien,
                            DiaChi = khachhang.DiaChi,
                            HoTen = khachhang.HoTen,
                            SDT = khachhang.SDT,
                            Email = khachhang.Email
                        };
                        ViewBag.Error = "Gmail bạn nhập không chính xác, vui lòng kiểm tra lại";
                        return View(viewmodel);
                    }
                    
                }
                else
                {
                    return Redirect("/Login/Login");
                }
            }
            else
            {
                ViewBag.TotalMoney = model.TongTien;
                var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(Common.Constants.USER_SESSION);
                var khachhang = new KhachHangDao().getById(session.UserId);
                var modelview = new HoaDonBanModel
                {
                    MaKhach = khachhang.Id,
                    TongTien = model.TongTien,
                    DiaChi = khachhang.DiaChi,
                    HoTen = khachhang.HoTen,
                    SDT = khachhang.SDT,
                    Email = khachhang.Email
                };
                return View(modelview);
                
            }
            
        }
    }
}