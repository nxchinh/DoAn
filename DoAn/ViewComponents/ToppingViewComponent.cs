using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoAn.Models.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace DoAn.ViewComponents
{
    public class ToppingViewComponent:ViewComponent
    {
        readonly TraSuaEntities _db = new TraSuaEntities();
        private IMemoryCache _cache;
        private readonly TimeSpan _cacheSlidingExpiration = TimeSpan.FromMinutes(5);
        private readonly TimeSpan _cacheAbsoluteExpiration = TimeSpan.FromMinutes(30);

        public ToppingViewComponent(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<SanPham> cacheEntry;
            string cacheKey = "Topping";
            if (!_cache.TryGetValue(cacheKey, out cacheEntry))
            {
                cacheEntry = await _db.SanPhams.Where(x => x.MaLoaiSanPham == 1).ToListAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(_cacheSlidingExpiration)
                    .SetAbsoluteExpiration(_cacheAbsoluteExpiration);

                _cache.Set(cacheKey, cacheEntry, cacheEntryOptions);
            }

            return View(cacheEntry);
        }
    }
}
