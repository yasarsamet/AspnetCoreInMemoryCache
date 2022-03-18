﻿using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class ListTypeController : Controller
    {

        private readonly RedisService _redisService;
        private readonly IDatabase db;
        private string listKey = "names";
        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(1);

        }
        public IActionResult Index()
        {
            List<string> namesList = new List<string>();
            if (db.KeyExists(listKey))
            {
                db.ListRange(listKey).ToList().ForEach(x =>
                {
                    namesList.Add(x.ToString());
                });
            }
            return View(namesList);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            db.ListRightPush(listKey,name); // sona ekler 
            //db.ListLeftPush(listKey,name); Başa Ekler
            return RedirectToAction("Index");
        }
        public IActionResult DeleteItem(string name)
        {
            db.ListRemoveAsync("names",name);
            return RedirectToAction("Index");
        }
        public IActionResult DeleteFirstItem ()
        {
            db.ListLeftPop(listKey); // başından siler 
            //db.ListRightPop(listKey); // sonundan siler
            return RedirectToAction("Index");
        }
    }
}
