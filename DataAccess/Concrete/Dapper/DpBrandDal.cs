﻿using Core.DataAccess.Connections;
using Core.DataAccess.Dapper;
using Dapper;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.Dapper
{
    public class DpBrandDal : DapperGenericRepository<Brand>, IBrandDal
    {

    }
}
