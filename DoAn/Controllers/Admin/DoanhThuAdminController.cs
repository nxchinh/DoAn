using DoAn.Models.Dao.Admin;
using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers.Admin
{
    public class DoanhThuAdminController : Controller
    {
        // GET: DoanhThuAdmin
        TraSuaEntities db = new TraSuaEntities();
         public IActionResult Index()
        {
            // doanh thu theo tháng
            var result = Common.Constants.USER_SESSION;
            var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(result.ToString());
            if (session != null)
            {
                List<DoanhThuModel> lst_doanhthutheothang = new List<DoanhThuModel>();
                var list = db.HoaDonBans.Where(x => x.DaThanhToan == 1).ToList();
                string[] list_thang = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
                List<double> lst_tongtien = new List<double>();

                foreach (var item_thang in list_thang)
                {


                    double tongtien = 0;
                    int tongsoluong = 0;
                    foreach (var item in list)
                    {
                        var dao = new DoanhThuDao();
                        if (dao.thang_hoadon(item.NgayBan) == Convert.ToInt32(item_thang))
                        {
                            tongsoluong += new HoaDonBanDao().tongsoluong(item.Id);
                            tongtien += Convert.ToDouble(item.TongTien);
                        }
                    }
                    var ngayban = item_thang + "/" + DateTime.Now.Year;
                    var doanhthu = new DoanhThuModel(item_thang, tongtien, ngayban, tongsoluong);
                    lst_doanhthutheothang.Add(doanhthu);

                }
                ViewBag.ListDoanhThu = lst_doanhthutheothang;
                ViewBag.DataPoints = JsonConvert.SerializeObject(lst_doanhthutheothang);
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }




         public IActionResult DoanhThuTheoNgay(DateTime? from = null, DateTime? to = null)
        {
            var result = Common.Constants.USER_SESSION;
            var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(result.ToString());
            if (session != null)
            {
                List<DoanhThuModel> lst_doanhthutheothang = new List<DoanhThuModel>();
                DateTime dt = DateTime.Now;
                List<double> lst_tongtien = new List<double>();
                var list = db.HoaDonBans.ToList();

                if (from == null || to == null)
                {
                    var list_ngay_now = new DoanhThuDao().dsngaytrongthang(dt.Month, dt.Year);
                    var list_ngay_now_split = list_ngay_now.Split(',');
                    foreach (var item_ngay in list_ngay_now_split)
                    {
                        if (item_ngay != "")
                        {
                            var ngay_split = item_ngay.Split('/');
                            var ngay = ngay_split[0];
                            var doanhthu = new DoanhThuModel();
                            doanhthu.Label = item_ngay;
                            double tongtien = 0;
                            int tongsoluong = 0;
                            foreach (var item in list)
                            {
                                var dao = new DoanhThuDao();
                                if (dao.ngay_hoadon(item.NgayBan) == Convert.ToInt32(ngay) &&
                                    dao.thang_hoadon(item.NgayBan) == Convert.ToInt32(dt.Month) &&
                                    dao.nam_hoadon(item.NgayBan) == Convert.ToInt32(dt.Year))
                                {
                                    tongsoluong += new HoaDonBanDao().tongsoluong(item.Id);
                                    tongtien += Convert.ToDouble(item.TongTien);
                                }
                            }
                            doanhthu.NgayBan = item_ngay;
                            doanhthu.SoLuongBan = tongsoluong;
                            doanhthu.Y = tongtien;
                            lst_doanhthutheothang.Add(doanhthu);
                        }

                    }

                }
                else
                {
                    var ngayfrom1 = Convert.ToDateTime(from).Day;
                    var thangfrom1 = Convert.ToDateTime(from).Month;
                    var namfrom1 = Convert.ToDateTime(from).Year;
                    var ngayto1 = Convert.ToDateTime(to).Day;
                    var thangto1 = Convert.ToDateTime(to).Month;
                    var namto1 = Convert.ToDateTime(to).Year;

                    var ngayfrom = Convert.ToInt32(ngayfrom1);
                    var thangfrom = Convert.ToInt32(thangfrom1);
                    var namfrom = Convert.ToInt32(namfrom1);
                    var ngayto = Convert.ToInt32(ngayto1);
                    var thangto = Convert.ToInt32(thangto1);
                    var namto = Convert.ToInt32(namto1);



                    if (namto != namfrom || thangto < thangfrom || (thangto < thangfrom && ngayto < ngayfrom))
                    {

                    }
                    else
                    {
                        var tru = thangto - thangfrom;
                        string string_ngay = "";

                        if (tru == 0)
                        {
                            string_ngay += new DoanhThuDao().ds_ngay_tru_trongthang(ngayfrom, ngayto, thangfrom, namfrom);
                        }
                        else
                        {
                            string_ngay += new DoanhThuDao().ds_ngay_tru_thangfrom(ngayfrom, thangfrom, namfrom);
                            for (int i = thangfrom + 1; i < thangto; i++)
                            {
                                string_ngay += new DoanhThuDao().dsngaytrongthang(i, namfrom);
                            }
                            string_ngay += new DoanhThuDao().ds_ngay_tru_thangto(ngayto, thangto, namto);
                        }
                        var string_ngay_split = string_ngay.Split(',');
                        foreach (var item_ngay in string_ngay_split)
                        {
                            if (item_ngay != "")
                            {
                                var ngay_split = item_ngay.Split('/');
                                var ngay = ngay_split[0];
                                var thang = ngay_split[1];
                                var doanhthu = new DoanhThuModel();
                                doanhthu.Label = item_ngay;
                                double tongtien = 0;
                                int tongsoluong = 0;
                                foreach (var item in list)
                                {
                                    var dao = new DoanhThuDao();
                                    if (dao.ngay_hoadon(item.NgayBan) == Convert.ToInt32(ngay) &&
                                        dao.thang_hoadon(item.NgayBan) == Convert.ToInt32(thang) &&
                                        dao.nam_hoadon(item.NgayBan) == Convert.ToInt32(namto))
                                    {
                                        tongsoluong += new HoaDonBanDao().tongsoluong(item.Id);
                                        tongtien += Convert.ToDouble(item.TongTien);
                                    }
                                }
                                doanhthu.NgayBan = item_ngay;
                                doanhthu.SoLuongBan = tongsoluong;
                                doanhthu.Y = tongtien;
                                lst_doanhthutheothang.Add(doanhthu);
                            }
                        }
                    }
                }
                ViewBag.TongSoLuong = lst_doanhthutheothang;
                ViewBag.DoanhThuTheoNgay = JsonConvert.SerializeObject(lst_doanhthutheothang);
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            // doanh thu theo tháng

        }
    }
}