﻿using Core.DataAccess.Abstract;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IUserDal :IDapperGenericRepository<User>
    {
        List<OperationClaim> GetClaimsByUserID(int userID);
    }
}
