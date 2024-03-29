﻿using Core.DataAccess;
using Core.DataAccess.Abstract;
using Core.DataAccess.Dapper;
using Core.Entities;
using Core.Entities.Abstract;
using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Business
{
    public class BaseEntityManager<TEntity> : IEntityService<TEntity>
        where TEntity : class, IEntity, new()
        //where IDal : IEntityRepository<TEntity>

    {

        IDapperGenericRepository<TEntity> _dal;

        public BaseEntityManager(IDapperGenericRepository<TEntity> dal)
        {
            _dal = dal;
        }

        public virtual IDataResult<List<TEntity>> GetList(int rowNumber, Dictionary<string, string> filter, int rowPerPage = 20)
        {
            try
            {
                return new SuccessDataResult<List<TEntity>>(_dal.GetList(rowNumber, filter, rowPerPage));
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<TEntity>>(ex.Message);
            }
        }

        public virtual IDataResult<List<TEntity>> GetList(QueryParameter queryParameter)
        {
            try
            {
                return new SuccessDataResult<List<TEntity>>(_dal.GetList(queryParameter));
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<TEntity>>(ex.Message);
            }
        }
        public virtual IDataResult<TEntity> Add(TEntity entity)
        {
            try
            {
                entity.ID = _dal.Add(entity);
                return new SuccessDataResult<TEntity>(entity);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<TEntity>(ex.Message);
            }
        }

        public virtual IResult Delete(int id)
        {
            try
            {
                _dal.Delete(id);
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

        public virtual IDataResult<TEntity> GetByID(int id)
        {
            try
            {
                return new SuccessDataResult<TEntity>(_dal.GetByID(id));
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<TEntity>(ex.Message);
            }
        }

        public virtual IDataResult<TEntity> Update(TEntity entity)
        {
            try
            {
                _dal.Update(entity);
                return new SuccessDataResult<TEntity>(entity);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<TEntity>(ex.Message);
            }
        }


    
    }
}
