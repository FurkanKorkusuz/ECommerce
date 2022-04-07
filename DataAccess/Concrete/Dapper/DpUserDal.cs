using Core.DataAccess.Dapper;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.Dapper
{
    public class DpUserDal : DapperGenericRepository<User>, IUserDal
    {
        public List<OperationClaim> GetOperationClaims(int userid)
        {
            throw new NotImplementedException();
        }
    }
}
