using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DoAn.Models.Model.Admin;
using DoAn.Models.Dao.Admin;
using DoAn.Models.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAn.Controllers.Admin
{
    public class NhanVienAdminController : Controller
    {
        // GET: NhanVienAdmin
        TraSuaEntities db = new TraSuaEntities();
        NhanVienDao dao = new NhanVienDao();

        public ActionResult Index()
        {
            var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(Common.Constants.USER_SESSION);
            if (session != null)
            {
                var model = new NhanVienModel();
                model.SelectChiNhanh = new SelectList(db.ChiNhanhs, "Id", "TenChiNhanh", 0);
                model.SelectChucVu = new SelectList(db.ChucVus.Where(x => x.Id != 1), "Id", "TenChucVu", 0);
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        public IActionResult NhanVien_ChiNhanh(int machinhanh)
        {
            var list = db.NhanViens.Where(x => x.MaChiNhanh == machinhanh && x.MaChucVu !=1).ToList();
            var model = new List<NhanVienModel>();
            int i = 0;
            foreach(var item in list)
            {
                i++;

                var itemmodel = new NhanVienModel
                {
                    STT = i,
                    Id = item.Id,
                    TenDangNhap = item.TenDangNhap,
                    HoTen = item.HoTen,
                    Luong = String.Format("{0:0,0}", db.ChucVus.FirstOrDefault(x => x.Id == item.MaChucVu).Luong),
                    TenChucVu = db.ChucVus.FirstOrDefault(x => x.Id == item.MaChucVu).TenChucVu
                };
                model.Add(itemmodel);
            }
            ViewBag.ChiNhanh = db.ChiNhanhs.FirstOrDefault(x => x.Id == machinhanh);
            return View(model);
        }
       
        public JsonResult List(string txtSearch, int? page)
        {
            var list = db.NhanViens.OrderByDescending(x => x.Id).ToList();

            int pageSize = 10;
            if (!String.IsNullOrEmpty(txtSearch))
            {
                ViewBag.txtSearch = txtSearch;
                list = list.Where(x=>x.HoTen.Contains(txtSearch)).OrderByDescending(x => x.Id).ToList();
            }
            var data = new List<NhanVienModel>();
            int i = 0;
            foreach (var item in list)
            {
                i++;
                var itemmodel = new NhanVienModel();
                itemmodel.STT = i;
                itemmodel.Id = item.Id;
                itemmodel.HoTen = item.HoTen;
                itemmodel.TenDangNhap = item.TenDangNhap;
                itemmodel.TenChiNhanh = db.ChiNhanhs.FirstOrDefault(x=>x.Id == item.MaChiNhanh)?.TenChiNhanh;
                itemmodel.TenChucVu = db.ChucVus.FirstOrDefault(x=>x.Id == item.MaChucVu)?.TenChucVu;
                itemmodel.Luong = $"{db.ChucVus.FirstOrDefault(x => x.Id == item.MaChucVu)?.Luong:0,0}";
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
        public IActionResult Add(NhanVienModel nhanvien)
        {
            dao.AddNhanVien(nhanvien);
            return RedirectToAction("Index");
        }
        
        public JsonResult Delete(int ID)
        {
            return Json(dao.DeleteNhanVien(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }
    }
}