using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DoAn.Models.Model.Admin;
using DoAn.Models.EF;
using DoAn.Models.Dao.Admin;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAn.Controllers.Admin
{
    public class SanPhamAdminController : Controller
    {
        // GET: SanPhamAdmin
        private readonly TraSuaEntities _db = new TraSuaEntities();
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ProductDao _dao;

        public SanPhamAdminController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _dao = new ProductDao(hostingEnvironment);
        }
        [HttpPost]
        public string ProcessUpload(IFormFile file)
        {
            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "images");
            var filePath = Path.Combine(uploads, file.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyToAsync(fileStream);
            }
            return file.FileName;
        }
        public IActionResult SanPham()
        {
            var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(Common.Constants.USER_SESSION);
            if (session != null)
            {
                var model = new SanPhamModel
                {
                    SelectMaLoai = new SelectList(_db.LoaiSanPhams, "Id", "TenLoaiSanPham", 0)
                };
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public JsonResult List(string txtSearch, int? page)
        {
            var list = _db.SanPhams.Where(x=>x.MaLoaiSanPham !=12 && x.MaLoaiSanPham != 13 && x.MaLoaiSanPham !=14).OrderByDescending(x => x.Id).ToList();
            
            int pageSize = 10;
            if (!String.IsNullOrEmpty(txtSearch))
            {
                ViewBag.txtSearch = txtSearch;
                list = list.Where(x => x.MaLoaiSanPham != 12 && x.MaLoaiSanPham != 13 && x.MaLoaiSanPham != 14 && x.TenSanPham.Contains(txtSearch)).OrderByDescending(x => x.Id).ToList();
            }
            var data = new List<SanPhamModel>();
            int i = 0;
            foreach (var item in list)
            {
                i++;
                var itemmodel = new SanPhamModel
                {
                    STT = i,
                    Id = item.Id,
                    Ten = item.TenSanPham,
                    GiaBan = $"{item.GiaBan:0,0}",
                    Anh = item.Anh,
                    TenLoai = new CategoryDao().GetById(item.MaLoaiSanPham).TenLoaiSanPham
                };
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
        [HttpPost]
        public IActionResult Add(SanPhamModel product)
        {
            ViewBag.MaLoai = new SelectList(_db.LoaiSanPhams, "Id", "TenLoaiSanPham", product.MaLoai);
            if (ModelState.IsValid)
            {
                _dao.AddSanPham(product);
            }
            return RedirectToAction("SanPham");
        }
        //update
        [HttpGet]
        public IActionResult Update(int id)
        {
            var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(Common.Constants.USER_SESSION);
            if (session != null)
            {
                var pro = _db.SanPhams.FirstOrDefault(x => x.Id == id);

                    SanPhamModel cPro = new SanPhamModel
                    {
                        Id = pro?.Id,
                        Ten = pro?.TenSanPham,
                        Anh = pro?.Anh,
                        ProductImage = pro?.ProductImage,
                        GiaBan = pro?.GiaBan.ToString(),
                        MaLoai = pro?.MaLoaiSanPham,
                    };
                    ViewBag.MaLoai = new SelectList(_db.LoaiSanPhams, "Id", "TenLoaiSanPham", cPro.MaLoai);
                    return View(cPro);

            }
            return RedirectToAction(nameof(SanPham));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(SanPhamModel product)
        {
            ViewBag.MaLoai = new SelectList(_db.LoaiSanPhams, "Id", "TenLoaiSanPham", product.MaLoai);
            _dao.UpdateSanPham(product);
            return RedirectToAction(nameof(SanPham));
        }
        public IActionResult Delete(int ID)
        {
            var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(Common.Constants.USER_SESSION);
            if (session != null)
            {
                Task.Run(async () => await _dao.Delete(ID));
                return RedirectToAction(nameof(SanPham));
            }
            return RedirectToAction("Index", "Home");
        }
    }
}