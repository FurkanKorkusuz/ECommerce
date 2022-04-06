using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    public class BrandController : Controller
    {
        IBrandService _BrandService;

        public BrandController(IBrandService BrandService)
        {
            _BrandService = BrandService;
        }


        public IActionResult Index()
        {
            var s=_BrandService.GetList(0, new Dictionary<string, string>());
            return View();
        }
    }
}
