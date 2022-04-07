using Core.DataAccess.Connections;
using Core.DataAccess.Dapper;
using Dapper;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.Dapper
{
    public class DpBrandDal : DapperGenericRepository<Brand>, IBrandDal
    {
        public List<Brand> GetList(QueryParameter parameter)
        {
            List<Brand> list = null;
            Dictionary<string,object> prt =new Dictionary<string,object>();
            parameter.FilterList.ForEach(p => { prt.Add(p.FilterKey,p.FilterValue); });
            string sqlQuery = @"select *
                                from Brands
                                where ID > @ID";
            try
            {
                using (IDbConnection cn = new SqlConnectionTools().Connection)
                {
                    cn.Open();
                    list = cn.Query<Brand>(sqlQuery, prt).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }
    }
}
