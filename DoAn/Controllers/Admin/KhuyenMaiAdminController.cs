using System;
using DoAn.Common.Function;
using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using DoAn.Models.Dao.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace DoAn.Controllers.Admin
{
    public class KhuyenMaiAdminController : Controller
    {
        // GET: KhuyenMaiAdmin
        TraSuaEntities db = new TraSuaEntities();
        KhuyenMaiDao dao = new KhuyenMaiDao();

        public ActionResult Index()
        {
            var result = Common.Constants.USER_SESSION;
            var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(result.ToString());
            if (session != null)
            {
                ViewBag.NhomSanPham = db.LoaiSanPhams.Where(x => x.Id == 1 || x.SanPhamChinh == 1).ToList();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }



        public JsonResult List(string txtSearch, int? page)
        {
            var list = db.KhuyenMais.OrderByDescending(x => x.Id).ToList();

            int pageSize = 10;
            if (!String.IsNullOrEmpty(txtSearch))
            {
                ViewBag.txtSearch = txtSearch;
                list = list.Where(x => x.TenKhuyenMai.Contains(txtSearch)).OrderByDescending(x => x.Id).ToList();
            }
            var data = new List<KhuyenMaiModel>();
            int i = 0;
            foreach (var item in list)
            {
                i++;
                var itemmodel = new KhuyenMaiModel();
                itemmodel.STT = i;
                itemmodel.Id = item.Id;
                itemmodel.Ten = item.TenKhuyenMai;
                itemmodel.NgayBDShow = String.Format("{0:dd/MM/yyyy}", item.NgayBatDau);
                itemmodel.NgayKTShow = String.Format("{0:dd/MM/yyyy}", item.NgayKetThuc);
                itemmodel.PhanTram = item.PhanTram;
                var motasql = item.MoTa;
                string[] lstloaisp = motasql.Split(',');
                string lstloaisanpham = "";
                foreach (var itemlsp in lstloaisp)
                {
                    var maloaisp = Convert.ToInt32(itemlsp);
                    lstloaisanpham += db.LoaiSanPhams.FirstOrDefault(x => x.Id == maloaisp)?.TenLoaiSanPham + ", ";
                }
                itemmodel.MoTa = lstloaisanpham;
                itemmodel.Status = item.Status;
                data.Add(itemmodel);

            }
            page = page > 0 ? page : 1;
            int start = (int)(page - 1) * pageSize;

            ViewBag.pageCurrent = page;
            int totalPage = data.Count();
            float totalNumsize = (totalPage / (float)pageSize);
            int numSize = (int)Math.Ceiling(totalNumsize);
            ViewBag.numSize = numSize;
            var datamodel = data.Skip(start).Take(pageSize);

            return Json(new { data = datamodel, pageCurrent = page, numSize = numSize }, new Newtonsoft.Json.JsonSerializerSettings());
        }
        public string CreateVoucher(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
        public IActionResult CreateKhuyenMai()
        {

            var model = new KhuyenMaiModel();
            model.ListLoaiSanPham = db.LoaiSanPhams.Where(x => x.Id == 1 || x.SanPhamChinh == 1).ToList();
            model.SelectLoaiKM = new SelectList(db.LoaiKhuyenMais, "Id", "TenLoaiKhuyenMai", 0);
            return View(model);

        }
        [HttpPost]
        public ActionResult CreateKhuyenMai(KhuyenMaiModel model)
        {
            var khuyenmai = new KhuyenMai
            {
                TenKhuyenMai = model.Ten,
                PhanTram = model.PhanTram,
                NgayBatDau = model.NgayBatDau,
                NgayKetThuc = model.NgayKetThuc,
                Status = 1
            };

            var listloaisanpham = Request.Form["listlspdc"].ToString();
            if (listloaisanpham != "")
            {
                string[] listlsp = listloaisanpham.Split(',');
                foreach (var item in listlsp)
                {
                    var maloaisp = int.Parse(item);
                    var listsp = db.SanPhams.Where(x => x.MaLoaiSanPham == maloaisp).ToList();
                    foreach (var itemsp in listsp)
                    {
                        var product = db.SanPhams.Find(itemsp.Id);
                        product.KhuyenMai = product.GiaBan - (product.GiaBan * model.PhanTram / 100);
                        db.SanPhams.Update(product);
                        db.SaveChanges();
                    }
                }
            }
            khuyenmai.MoTa = listloaisanpham;

            db.KhuyenMais.Add(khuyenmai);
            db.SaveChanges();
            var motasql = khuyenmai.MoTa;
            string[] lstloaisp = motasql.Split(',');
            string lstloaisanpham = "";
            foreach (var itemlsp in lstloaisp)
            {
                var maloaisp = Convert.ToInt32(itemlsp);
                lstloaisanpham += db.LoaiSanPhams.FirstOrDefault(x => x.Id == maloaisp)?.TenLoaiSanPham + ", ";
            }
            //@Convert.ToDouble(voucher.TongTienDk).ToString("#,##0")
            List<KhachHang> khachHangs = db.KhachHangs.Where(x=> x.Active == true).ToList();
            foreach (var item in khachHangs)
            {
                MailMessage mm = new MailMessage("khoitedu99@gmail.com", item.Email);
                string content = "";
                content = content + "<h2>Chào mừng bạn đến với Trà sữa Gongcha.</h2>";
                content = content + "<h2>Nhân dịp " + khuyenmai.TenKhuyenMai + " chúng tôi đang giảm giá " + khuyenmai.PhanTram + "% với đơn hàng trên với các loại trà sữa" + lstloaisanpham + "</h2><br>";
                content = content + "<div>Cảm ơn bạn đã ủng hộ GongCha. Thân</div>";
                mm.Body = string.Format(content, item.HoTen);
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Credentials = new System.Net.NetworkCredential("khoitedu99@gmail.com", "Fizz1999");
                smtp.Send(mm);
            }
            db.SaveChanges();
            return RedirectToAction("Index", "KhuyenMaiAdmin");
        }
        public JsonResult Update(int ID)
        {
            return Json(dao.UpdateKhuyenMai(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }
    }
}