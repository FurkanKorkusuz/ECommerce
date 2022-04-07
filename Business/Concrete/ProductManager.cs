using Business.Abstract;
using Core.DataAccess.Abstract;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductManager : BaseEntityManager<Product>, IProductService
    {
        IProductDal _ProductDal;
        public ProductManager(IProductDal dal) : base(dal)
        {
            _ProductDal = dal;
        }

       
    }
}
