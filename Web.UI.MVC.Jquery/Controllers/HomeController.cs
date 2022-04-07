using Business.Abstract;
using Entities.Concrete;
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
            Dictionary<string, string> filter = new Dictionary<string, string>();
           
            var data = _BrandService.GetList(0, filter, 100);
           //var brand =_BrandService.Update(new Brand {ID=2, BrandName = " cache" });
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
