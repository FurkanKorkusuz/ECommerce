using Core.DataAccess.Dapper;
using Core.Utilities.Business;
using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IBrandService : IEntityService<Brand>
    {
        public IResult TransactionTest(Brand brand);

        public List<Brand> GetList(QueryParameter parameter);
    }
}
