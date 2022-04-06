using Business.Abstract;
using Core.Utilities.Business;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class BrandManager : BaseEntityManager<Brand>, IBrandService
    {
        IBrandDal _BrandDal;
        public BrandManager(IBrandDal dal) : base(dal)
        {
            _BrandDal = dal;
        }


    }
}
