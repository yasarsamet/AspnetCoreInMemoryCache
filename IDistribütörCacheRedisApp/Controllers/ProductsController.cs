using IDistribütörCacheRedisApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IDistribütörCacheRedisApp.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _distributedCache;
        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task<IActionResult> Index()    
        {
            DistributedCacheEntryOptions cep = new DistributedCacheEntryOptions();
            cep.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            await _distributedCache.SetStringAsync("name","Yaşar Samet ALIÇ Bilgisayar Mühendisi",cep);
            return View();
        }
        public IActionResult Show()
        {
            var name = _distributedCache.GetString("name");
            ViewBag.name = name;
            return View();
        }
        public IActionResult Delete()
        {
            _distributedCache.Remove("name");
            return View();
        }
        public IActionResult ImageShow()
        {
            byte[] resimByte = _distributedCache.Get("resim");
            return File(resimByte, "image/jpg");
        }
        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profile.jpg");
            byte[] imageByte = System.IO.File.ReadAllBytes(path);
            _distributedCache.Set("resim",imageByte);
            return View();
        }
    }
}
