using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreWebApplication.Controllers
{
    public class TPSController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}