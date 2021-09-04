using DoAn.Common.Function;
using DoAn.Common.Session;
using DoAn.Models.Dao.Admin;
using DoAn.Models.Dao.NguoiDung;
using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using DoAn.Models.Model.NguoiDung;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;

namespace DoAn.Controllers.NguoiDung
{
    public class LoginController : Controller
    {
        // GET: Login
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _iconfiguration;
        const string SessionName = "_Code";

        public LoginController(IWebHostEnvironment webHostEnvironment, IConfiguration iconfiguration)
        {
            _webHostEnvironment = webHostEnvironment;
            _iconfiguration = iconfiguration;
        }
        TraSuaEntities db = new TraSuaEntities();
        public IActionResult Index()
        {
            
            return View();

        }
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                //Encrytor.MD5Hash(model.Password)
                var result = dao.Login(model.Username, Encrytor.MD5Hash(model.Password));
                if (result)
                {
                    var user = dao.GetById(model.Username);
                    var userSession = new UserLogin {UserId = user.Id, UserName = user.TenDangNhap};
                    HttpContext.Session.SetObjectAsJson(Common.Constants.USER_SESSION, userSession);

                    if (user.TenDangNhap == "admin")
                    {
                        return RedirectToAction("Index", "HomeAdmin");
                    }
                    else
                    {
                        if (user.Active == true)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        ViewBag.Error = "Tài khoản chưa được active";

                    }
                }
                else
                {
                    ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View("Index");
        }
        public IActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DangKy(Models.Model.NguoiDung.KhachHangModel model)
        {

            var dao = new UserDao();
            if (dao.CheckUserName(model.TenDangNhap))
            {
                ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
            }
            else
             if (dao.CheckEmail(model.Email))
            {
                ModelState.AddModelError("", "Email đã tồn tại");
            }
            else
            {
                var user = new KhachHang
                {
                    HoTen = model.HoTen,
                    TenDangNhap = model.TenDangNhap,
                    Email = model.Email,
                    MatKhau = Encrytor.MD5Hash(model.MatKhau),
                    SDT = model.SDT,
                    DiaChi = model.DiaChi,
                    Active = false,
                    CreatedTime = DateTime.Now
                };

                var result = dao.Insert(user);
                Random r = new Random();
                var code = r.Next(100000, 999999);
                BuildEmail(result,code);
                var nhanvienSession = new UserRegister()
                {
                    Code = code,
                    Id = result
                };
                HttpContext.Session.SetObjectAsJson(SessionName, nhanvienSession);
                if (result > 0)
                {
                    ViewBag.Success = "Đăng ký thành công. Vui lòng vào gmail để active";
                }
            }


            return View();
        }
        public void BuildEmail(int iDReg,int code) // id NewUser
        {
            var dao = new UserDao();
            var webRoot = _webHostEnvironment.WebRootPath;
            var file = System.IO.Path.Combine(webRoot, "Text.cshtml");
            string body = System.IO.File.ReadAllText(file);
            KhachHang regInfo = dao.TimId(iDReg); // get  user  from DataBase
            var url = "https://localhost:44312" + "/Login/Confirm?regId=" + iDReg;
            body = body.Replace("@ViewBag.ConfirmationLink", url);
            body = body.Replace("@ViewBag.MaCode", code.ToString());
            body = body.ToString();
            BuildEmail("Your Account Is Successfully Created", body, regInfo.Email);
        }

        public IActionResult RegisterConfirm(int regId, int code)
        {
            var dao = new UserDao();
            var sessionCode = HttpContext.Session.GetObjectFromJson<UserRegister>(SessionName);
            var msg = "";
            KhachHang user = db.KhachHangs.FirstOrDefault(x => x.Id == sessionCode.Id);
            //DateTime dtCreate = dao.TimId(regId).CreatedTime;
            DateTime dtCreate = user.CreatedTime;
            DateTime dtNow = DateTime.Now;
            DateTime dtExp = dtCreate.AddMinutes(1);
            int res = DateTime.Compare(dtNow, dtExp);
            var thoigian = DateTime.Now.AddMinutes(2);
            if (dtNow.ToUniversalTime() <= dtExp.ToUniversalTime())
            {
                if (!string.Equals(sessionCode.Code.ToString(), code.ToString(), StringComparison.Ordinal))
                {
                    msg = $"Nhập sai mã code ! Vui lòng nhập lại";
                    return new JsonResult(new { success = false, responseText = msg });

                }
                else
                {
                    msg = "Email hợp lệ";
                    user.Active = true;
                    dao.Update(user.Id);
                    return new JsonResult(new { success = true, responseText = msg });
                }
            }
            else
            {
                msg = $"Quá hạn thời gian.! Vui lòng đăng ký lại tài khoản";
                dao.Delete(user.Id);
                return new JsonResult(new { success = false, responseText = msg });
            }

        }

        public IActionResult Confirm(KhachHang reginfo)
        {
            ViewBag.regID = reginfo.Id; 
            ViewBag.Name = HttpContext.Session.GetObjectFromJson<UserRegister>(SessionName).Code;
            return View();

        }
        public static void SendEmail(MailMessage mail)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential("khoitedu99@gmail.com", "Fizz1999");
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void BuildEmail(string subjectText, string bodyText, string sendTo)
        {

            string from, to, bcc, cc, subject, body;
            from = "khoitedu99@gmail.com";
            to = sendTo.Trim();
            bcc = "";
            cc = "";
            subject = subjectText;
            StringBuilder sb = new StringBuilder();
            sb.Append(bodyText);
            body = sb.ToString();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            if (!string.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(new MailAddress(bcc));
            }
            if (!string.IsNullOrEmpty(cc))
            {
                mail.CC.Add(new MailAddress(cc));
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SendEmail(mail);

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/Home/Index");
        }
        public IActionResult QuenMatKhau()
        {

            return View();
        }
        [HttpPost]
        public IActionResult QuenMatKhau(Models.Model.NguoiDung.KhachHangModel model)
        {

            var dao = new DoAn.Models.Dao.NguoiDung.KhachHangDao();
            if (dao.CheckEmail(model.Email, model.TenDangNhap))
            {
                try
                {
                    var webRoot = _webHostEnvironment.WebRootPath;
                    var file = System.IO.Path.Combine(webRoot, "resetpassword.html");
                    string content = System.IO.File.ReadAllText(file);
                    content = content.Replace("{{CustomerName}}", model.TenDangNhap);
                    content = content.Replace("{{Password}}", "@123456");
                    var toEmail = _iconfiguration.GetSection("MySettings").GetSection("ToEmailAddress").Value;
                    new MailHelper().SendMail(model.Email, "Đổi mật khẩu từ GongCha", content);
                    new MailHelper().SendMail(toEmail, "Đổi mật khẩu từ GongCha", content);
                    var khachhang = dao.getKhachHang(model.Email, model.TenDangNhap);
                    var customer = db.KhachHangs.Find(khachhang.Id);
                    customer.MatKhau = Encrytor.MD5Hash("@123456");
                    db.SaveChanges();
                    ViewBag.DoiMatKhau = "Mật khẩu bạn đã được gửi đến gmail, mời bạn kiểm tra email";
                }

                catch
                {
                    ViewBag.Error = "Email bạn nhập không hợp lệ";
                }


                
            }
            else
            {
                ViewBag.SaiEmail = "Tên đăng nhập và email không khớp";
            }

            return View();
        }
        public IActionResult DangNhapNhanVien()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DangNhapNhanVien(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new Models.Dao.NguoiDung.NhanVienDao();
                var result = dao.DangNhapNhanVien(model.Username, Encrytor.MD5Hash(model.Password));
                if (result)
                {
                    var nhanvien = dao.getByName(model.Username);
                    var nhanvienSession = new NhanVienSession
                    {
                        Id = nhanvien.Id, UserName = nhanvien.TenDangNhap, MaChiNhanh = nhanvien.MaChiNhanh
                    };
                    HttpContext.Session.SetObjectAsJson(Common.Constants.NHANVIEN_SESSION, nhanvienSession);
                    if (nhanvien.MaChucVu == 3)
                    {
                        return RedirectToAction("Index", "HomeQuanLy");
                    }
                    else
                    {
                        return RedirectToAction("Index", "HomeNhanVien");
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("", "đăng nhập không đúng");
                }
            }
            return RedirectToAction("DangNhapNhanVien", "Login");
        }
        public IActionResult QuenMatKhauNhanVien()
        {

            return View();
        }
        [HttpPost]
        public IActionResult QuenMatKhauNhanVien(Models.Model.NhanVien.NhanVienModel model)
        {
            var dao = new DoAn.Models.Dao.NhanVien.NhanVienDao();
            if (dao.CheckEmail(model.Email, model.TenDangNhap))
            {
                try
                {
                    var webRoot = _webHostEnvironment.WebRootPath;
                    var file = System.IO.Path.Combine(webRoot, "resetpassword.html");
                    string content = System.IO.File.ReadAllText(file);
                    content = content.Replace("{{CustomerName}}", model.TenDangNhap);
                    content = content.Replace("{{Password}}", "@123456");
                    var toEmail = _iconfiguration.GetSection("MySettings").GetSection("ToEmailAddress").Value;
                    new MailHelper().SendMail(model.Email, "Đổi mật khẩu từ GongCha", content);
                    new MailHelper().SendMail(toEmail, "Đổi mật khẩu từ GongCha", content);
                    var nhanvien = dao.getNhanVien(model.Email, model.TenDangNhap);
                    var nv = db.NhanViens.Find(nhanvien.Id);
                    nv.MatKhau = Encrytor.MD5Hash("@123456");
                    db.SaveChanges();
                    ViewBag.DoiMatKhau = "Mật khẩu bạn đã được gửi đến gmail, mời bạn kiểm tra email";
                }

                catch
                {
                    ViewBag.Error = "Email bạn nhập không hợp lệ";
                }



            }
            else
            {
                ViewBag.SaiEmail = "Tên đăng nhập và email không khớp";
            }

            return View();
        }
    }
}