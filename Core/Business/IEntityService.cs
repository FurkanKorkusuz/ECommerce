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
    public interface IEntityService<T> where T:class, IEntity, new()
    {
        IDataResult<List<T>> GetList(int rowNumber, Dictionary<string, string> filter, int rowPerPage = 30);

        IDataResult<T> GetByID(int id);

        IDataResult<T> Insert(T entity);

        IDataResult<T> Update(T entity);
        IResult Delete(int id);
    }
}
