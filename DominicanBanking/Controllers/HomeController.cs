using DominicanBanking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DominicanBanking.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public HomeController()
        {

        }

        [Authorize(Roles = "ADMINISTRATOR")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [Authorize(Roles = "CLIENT")]
        public IActionResult Client()
        {
            return View();
        }
    }
}
