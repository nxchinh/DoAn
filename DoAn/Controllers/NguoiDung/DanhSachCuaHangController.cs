using System.Linq;
using DoAn.Models.EF;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers.NguoiDung
{
    public class DanhSachCuaHangController : Controller
    {
        // GET: DanhSachCuaHang
        TraSuaEntities db = new TraSuaEntities();
        public IActionResult Index()
        {
            var list = db.ChiNhanhs.ToList();
            return View(list);
        }
    }
}