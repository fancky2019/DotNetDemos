using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreWebApplication.Models;

namespace NetCoreWebApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("GetMenus")]
        public IActionResult GetMenus([FromQuery]User user)
        {
            List<Menus> list = new List<Menus>();
            list.Add(new Menus()
            {
                ID = 1,
                ParentID = 0,
                MenuName = "OCG日志分析",
                URL = "#"
            });
            list.Add(new Menus()
            {
                ID = 2,
                ParentID = 1,
                MenuName = "FileUpLoad",
                URL = "/FileUpLoad"
            });
            list.Add(new Menus()
            {
                ID = 3,
                ParentID = 1,
                MenuName = "TPS",
                URL = "/TPS"
            });

            list.Add(new Menus()
            {
                ID = 4,
                ParentID = 0,
                MenuName = "UserManager",
                URL = "/UserManager"
            });

            list.Add(new Menus()
            {
                ID = 5,
                ParentID = 0,
                MenuName = "SystemManager",
                URL = "#"
            });
            list.Add(new Menus()
            {
                ID = 6,
                ParentID = 5,
                MenuName = "AccountManager",
                URL = "AccountManager"
            });

            return Json(list);
        }

        // POST api/values
        [HttpPost("Add")]
        public void Add([FromBody] User user)
        {
        }
    }
}
