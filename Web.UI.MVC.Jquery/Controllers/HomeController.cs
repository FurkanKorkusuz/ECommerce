using Business.Abstract;
using Core.DataAccess.Dapper;
using Core.Extensions;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Web.UI.MVC.Jquery.Models;
using static Core.DataAccess.Dapper.QueryFilter;
using static Core.DataAccess.Dapper.WhereCondinition;

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

            QueryParameter parameter3 = new QueryParameter();

            //QueryFilter filter2 = new QueryFilter();
            //filter2.FilterKey = "ID";
            //filter2.FilterValue = 1;

            //parameter3.FilterList.Add(filter2);


            parameter3.Select<Brand>();
            parameter3.Table("Brands");
            parameter3.FilterList.Filter("ID", 1).Filter("rrr", 1);
            parameter3.SortBy("ID").SortBy("BrandName");

                //.WhereEqual("ID", 1);
            var data = _BrandService.GetList(parameter3);
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
