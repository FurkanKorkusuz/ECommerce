﻿using Business.Abstract;
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


        [HttpGet("getbyid")]
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