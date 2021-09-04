using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoAn.Models.Dao.NguoiDung;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.ViewComponents
{
    public class TenDangNhapViewComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
