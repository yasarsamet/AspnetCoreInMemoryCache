using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        public StringTypeController (RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0);

        }
        public IActionResult Index()
        {
            db.StringSet("name","Yaşar Samet ALIÇ");
            db.StringSet("ziyaretci",100);
            return View();
        }
        public IActionResult Show()
        {
            var value = db.StringGet("name");
            // 153
            //db.StringIncrement("ziyaretci", 10); // ziyaretci keyini bir bir arttıracak
            
            db.StringDecrementAsync("ziyaretci",1); ; // ziyaretci keyini bir bir düşrecek

            var ziyaretci = db.StringGet("ziyaretci");
            var getCharacter = db.StringGetRange("name", 0, 2); // name keyinin ilk iki harfini getirir
            if (value.HasValue)
            {
                ViewBag.ziyaretci = ziyaretci.ToString();
                ViewBag.getCharacter = getCharacter.ToString();
                ViewBag.value = value.ToString();
            }
            return View();
        }
    }
}
