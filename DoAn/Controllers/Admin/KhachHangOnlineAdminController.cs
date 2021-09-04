using DoAn.Models.Dao.Admin;
using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace DoAn.Controllers.Admin
{
    public class KhachHangOnlineAdminController : Controller
    {
        // GET: KhachHangOnlineAdmin
        TraSuaEntities db = new TraSuaEntities();
        KhachHangDao dao = new KhachHangDao();
       public IActionResult Index()
        {
            var result = Common.Constants.USER_SESSION;
            var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(result.ToString());
            if (session != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult Excel()
        {
            var list = db.KhachHangs.Where(x => x.Id != 3 && x.Id != 4).OrderByDescending(x => x.Id).ToList();
            var comlumHeadrs = new string[]
            {
                "STT",
                "Địa chỉ Email",
                "Họ Tên",
                "Địa Chỉ",
                "Số điện Thoại",
                "Tên đăng nhập"
            };

            byte[] result;

            using (var package = new ExcelPackage())
            {
                // add a new worksheet to the empty workbook

                var worksheet = package.Workbook.Worksheets.Add("DSKhachHang"); //Worksheet name
                using (var cells = worksheet.Cells[1, 1, 1, 5]) //(1,1) (1,5)
                {
                    cells.Style.Font.Bold = true;
                }

                //First add the headers
                for (var i = 0; i < comlumHeadrs.Count(); i++)
                {
                    worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                }

                //Add values
                IList<KhachHang> customerList = list;
                //int totalRows = customerList.Count();
                var j = 2; var stt = 1;
                foreach (var employee in customerList)
                {
                    worksheet.Cells["A" + j].Value = stt;
                    worksheet.Cells["B" + j].Value = employee.HoTen;
                    worksheet.Cells["C" + j].Value = employee.Email;
                    worksheet.Cells["D" + j].Value = employee.DiaChi;
                    worksheet.Cells["E" + j].Value = employee.SDT;
                    worksheet.Cells["F" + j].Value = employee.TenDangNhap;
                    stt++;
                    j++;
                }
                result = package.GetAsByteArray();
            }

            return File(result, "application/ms-excel", $"DSKhachHang.xlsx");
        }

        public IActionResult CSV()
        {
            var list = db.KhachHangs.Where(x => x.Id != 3 && x.Id != 4).OrderByDescending(x => x.Id).ToList();
            var comlumHeadrs = new string[]
            {
                "Địa chỉ Email",
                "Họ Tên",
                "Địa Chỉ",
                "Số điện Thoại",
                "Tên đăng nhập"
            };
            IList<KhachHang> customerList = list;

            var employeeRecords = (from employee in customerList
                                   select new object[]
                                    {
                                            employee.Email,
                                            $"{employee.HoTen}",
                                            $"\"{employee.DiaChi}\"", //Escaping ","
                                            $"\"{employee.SDT}\"",
                                            employee.TenDangNhap,
                                    }).ToList();

            // Build the file content
            var employeecsv = new StringBuilder();
            employeeRecords.ForEach(line =>
            {
                employeecsv.AppendLine(string.Join(",", line));
            });

            byte[] buffer = Encoding.Unicode.GetBytes($"{string.Join(",", comlumHeadrs)}\r\n{employeecsv.ToString()}");
            return File(buffer, "text/csv", $"DSKhachHang.csv");

        }
        public JsonResult List(string txtSearch, int? page)
        {
            var list = db.KhachHangs.Where(x=>x.Id !=3 && x.Id !=4).OrderByDescending(x => x.Id).ToList();

            int pageSize = 10;
            if (!String.IsNullOrEmpty(txtSearch))
            {
                ViewBag.txtSearch = txtSearch;
                list = list.Where(x => x.Id != 3 && x.Id != 4 && x.TenDangNhap.Contains(txtSearch)).OrderByDescending(x => x.Id).ToList();
            }
            var data = new List<KhachHangModel>();
            int i = 0;
            foreach (var item in list)
            {
                i++;
                var itemmodel = new KhachHangModel
                {
                    STT = i,
                    Id = item.Id,
                    TenDangNhap = item.TenDangNhap,
                    Email = item.Email,
                    SDT = item.SDT,
                    DiaChi = item.DiaChi
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
        
        public JsonResult Delete(int ID)
        {
            return Json(dao.Delete(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }
    }
}