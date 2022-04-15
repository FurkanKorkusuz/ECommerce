using Business.Abstract;
using Core.Aspects.Autofac.Performance;
using Core.DataAccess.Dapper;
using Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private IBrandService _brandService;
        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet("getlist")]
        public IActionResult GetList(int rowNumber, Dictionary<string, string> filter, int rowPerPage)
        {
            var result = _brandService.GetList(rowNumber, filter, rowPerPage);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result.InfoMessage);
        }

        [HttpGet("getlist2")]
        public IActionResult GetList(int id)
        {

            QueryParameter parameter2 = new QueryParameter();
            parameter2.FilterList.Filter("ID", id);
            parameter2.TableName = "Brands";

            var result = _brandService.GetList(parameter2);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result.InfoMessage);
        }


        [HttpGet("getbyid")]
       // [Authorize(Roles ="Brand.Get")]
        public IActionResult GetByID(int id)
        {
            var result = _brandService.GetByID(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.InfoMessage);
        }
    }
}
