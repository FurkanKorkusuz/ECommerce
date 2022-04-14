using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation.FluentValidation;
using Core.DataAccess.Dapper;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.Dapper;
using Entities.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class BrandManager : BaseEntityManager<Brand>, IBrandService
    {
        private IBrandDal _BrandDal;
        public BrandManager(IBrandDal dal) : base(dal)
        {
            _BrandDal = dal;
        }
        [CacheAspect(10)]
        public override IDataResult<List<Brand>> GetList(int rowNumber, Dictionary<string, string> filter, int rowPerPage = 20)
        {
            return base.GetList(rowNumber, filter, rowPerPage);
        }

        [ValidationAspect(typeof(BrandValidator), Priority = 1)]
        [CacheRemoveAspect("BrandManager.Get")]
        public override IDataResult<Brand> Add(Brand entity)
        {
            return base.Add(entity);
        }


        [CacheRemoveAspect("BrandManager.Get")]
        public override IDataResult<Brand> Update(Brand entity)
        {
            return base.Update(entity);
        }


        [TransactionScopeAspect]
        public IResult TransactionTest(Brand brand)
        {
            _BrandDal.Update(brand);
            return new SuccessResult();
        }
        [PerformanceAspect(1)]
        public List<Brand> GetList(QueryParameter parameter)
        {
            System.Threading.Thread.Sleep(1300);
            return _BrandDal.GetList(parameter);
        }

        [SecuredOperation("Product.List")]
        public override IDataResult<Brand> GetByID(int id)
        {
            return base.GetByID(id);
        }
    }
}
