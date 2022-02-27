using AspnetCoreInMemoryCache.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetCoreInMemoryCache.Controllers
{    
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            if (!_memoryCache.TryGetValue("zaman",out string zamancache)) // zanan keyi ne ait data varsa onu al zamancache adında anahtar
                // olustur cachle 
            {
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
                //options.AbsoluteExpiration = DateTime.Now.AddSeconds(10); // cachin süresini verdik
                options.SlidingExpiration = TimeSpan.FromSeconds(10); // 10 saniyede bir istek attıgımızda datanın ömrü artacak 10 saniye
                options.Priority = CacheItemPriority.High; // NeverRemove =  memory dolarsa bu datayı sakın  silme
                // High = Bu data benim için önemli
                // Low = Bu data benim için önemli değil memory dolarsa bu datayı ilk sil
                // Normal = Bu data benim için normal
                options.RegisterPostEvictionCallback((key, value, reason, state) => // cache silindiyse sebebini yazıyoruz
                {
                    _memoryCache.Set("callback",$"{key} -> {value} => sebep : {reason}"); 

                });
                CacheModel cm = new CacheModel { Id = 5, Name = "Yasar Samet", Surname = "ALIÇ" };
                _memoryCache.Set<CacheModel>("cacheModel", cm); // Model Cachledik
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString(),options );
            }
            return View();
        }
        public IActionResult Show()
        {
            _memoryCache.TryGetValue("zaman", out string zamancache); // zanan keyi ne ait data varsa onu al zamancache adında anahtar
            _memoryCache.TryGetValue("callback", out string callback);
            ViewBag.callback = callback;
            ViewBag.cacheModel = _memoryCache.Get<CacheModel>("cacheModel");
                ViewBag.zaman = zamancache;
            return View();
        }
    }
}
