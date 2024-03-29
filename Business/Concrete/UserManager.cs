﻿using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserManager : BaseEntityManager<User> , IUserService
    {
        private IUserDal _userDal;
        public UserManager(IUserDal userDal) : base(userDal)
        {
            _userDal = userDal;
        }

        public IDataResult<List<OperationClaim>> GetClaimsByUserID(int userID)
        {
            try
            {
                return new SuccessDataResult<List<OperationClaim>>(_userDal.GetClaimsByUserID(userID));
            }
            catch (Exception)
            {
                return new  ErrorDataResult<List<OperationClaim>>();
            }
            
        }

        public IDataResult<User> GetUserByEmail(string email)
        {
            Dictionary<string,string> filter = new Dictionary<string,string>();
            filter.Add("Email", email);        
            var data=  _userDal.GetList(0,filter,1);
            if (data != null && data.Count>0)
            {
                return new SuccessDataResult<User>(data[0]);
            }
            return new ErrorDataResult<User>();
        }

        public IDataResult<User> GetUserByEmailAndPassword(string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
