using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers.NguoiDung
{
    public class GioiThieuController : Controller
    {
        // GET: GioiThieu
        public IActionResult Index()
        {
            return View();
        }
    }
}