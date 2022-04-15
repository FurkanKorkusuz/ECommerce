using Core.DataAccess.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.Abstract
{
    public interface IDapperGenericRepository<T>
    {
        List<T>GetList(int rowNumber, Dictionary<string, string> flt, int rowPerPage);
        List<T> GetList(QueryParameter queryParameter);
        void Delete(int id);
        T GetByID(int id);
        void Update(T entity);
        int Add(T entity);
    }
}
