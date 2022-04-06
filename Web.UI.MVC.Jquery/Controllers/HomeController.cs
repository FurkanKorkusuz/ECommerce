using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Web.UI.MVC.Jquery.Models;

namespace Web.UI.MVC.Jquery.Controllers
{
    public class HomeController : Controller
    {
        IBrandService _BrandService;

        public HomeController(IBrandService BrandService)
        {
            _BrandService = BrandService;
        }

        public IActionResult Index()
        {
            var s = _BrandService.GetList(0, new Dictionary<string, string>());
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
    }
}
