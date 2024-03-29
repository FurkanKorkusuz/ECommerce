﻿using Core.DataAccess.Abstract;
using Core.DataAccess.Connections;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.Dapper
{
    public abstract class DapperGenericRepository<T> : IDapperGenericRepository<T> where T : class
    {
        private readonly string _tableName;

        protected string GetTableName<T>()
        {
            // Check if we've already set our custom table mapper to TableNameMapper.
            if (SqlMapperExtensions.TableNameMapper != null)
                return SqlMapperExtensions.TableNameMapper(typeof(T));

            // If not, we can use Dapper default method "SqlMapperExtensions.GetTableName(Type type)" which is unfortunately private, that's why we have to call it via reflection.
            string getTableName = "GetTableName";
            MethodInfo getTableNameMethod = typeof(SqlMapperExtensions).GetMethod(getTableName, BindingFlags.NonPublic | BindingFlags.Static);

            if (getTableNameMethod == null)
                throw new ArgumentOutOfRangeException($"Method '{getTableName}' is not found in '{nameof(SqlMapperExtensions)}' class.");

            return getTableNameMethod.Invoke(null, new object[] { typeof(T) }) as string;
        }


        protected DapperGenericRepository()
        {
            _tableName = GetTableName<T>();
        }

        /// <summary>
        /// Generate new connection based on connection string
        /// </summary>
        /// <returns></returns>
        private SqlConnection SqlConnection()
        {
            return new SqlConnectionTools().Connection;
        }

        /// <summary>
        /// Open new connection and return it for use
        /// </summary>
        /// <returns></returns>
        private IDbConnection CreateConnection()
        {
            var conn = SqlConnection();
            conn.Open();
            return conn;
        }

        private IEnumerable<PropertyInfo> GetProperties => typeof(T).GetProperties();

     
        public virtual List<T> GetList(int rowNumber, Dictionary<string, string> flt, int rowPerPage = 30)
        {
            List<T> list = null;

            Dictionary<string, object> parameters;
            string filter = Filter(flt, out parameters);
            string sort = flt.ContainsKey("Sort") && !string.IsNullOrEmpty(flt["Sort"])
                ? flt["Sort"].Replace('&', ' ')
                : "ID desc";
            parameters.Add("@sort", sort);
            parameters.Add("@rowPerPage", rowPerPage);
            parameters.Add("@rowNumber", rowNumber);


            string sqlQuery = @"select *
                            from " + _tableName + @"
                            where 1=1 " + filter + @"
                            order by  " + sort + @"
                            offset @rowNumber rows fetch next @rowPerPage rows only ";
            try
            {
                using (var connection = CreateConnection())
                {
                    list = connection.Query<T>(sqlQuery, parameters).ToList();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return list;
        }

        public virtual List<T> GetList(QueryParameter queryParameter)
        {
            List<T> list = null;
            Dictionary<string, object> parameters ;
            string sqlQuery = GenerateGetListQuery(queryParameter , out parameters);
            try
            {
                using (var connection = CreateConnection())
                {
                    list = connection.Query<T>(sqlQuery, parameters).ToList();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return list;
        }

        private string GenerateGetListQuery(QueryParameter qp, out Dictionary<string, object> parameters)
        {
            // SELECT
            var getQuery = new StringBuilder($"SELECT ");
            if (qp.Select == null)
            {
                getQuery.AppendLine("*");
            }
            else
            {
                qp.Select.ForEach(s => getQuery.AppendLine($"[{s}],"));
                getQuery.Remove(getQuery.Length - 1, 1); //remove last ,
            }

            // FROM
            getQuery.AppendLine($"FROM {qp.TableName}");

            // JOIN
            if (qp.Joins.Count > 0)
            {
                foreach (var join in qp.Joins)
                {
                    switch (join.joinType)
                    {
                        case JoinType.InnerJoin:
                            getQuery.Append($"JOIN");
                            break;
                        case JoinType.LeftJoin:
                            getQuery.Append($"LEFT JOIN");
                            break;
                        case JoinType.RightJoin:
                            getQuery.Append($"RIGHT JOIN");
                            break;
                        case JoinType.OuterJoin:
                            getQuery.Append($"OUTER JOIN");
                            break;
                    }

                    getQuery.AppendLine($"{join.RightTableName} on {join.RightTableName}.{join.RightTableColumns[0]} = {join.LeftTableName}.{join.LeftTableColumns[0]}");
                    break;
                }
            }

            // WHERE
            getQuery.AppendLine(Filter(qp.FilterList, out  parameters));

            // GROUP BY
            if (qp.GroupBy!=null)
            {
                getQuery.Append("GROUP BY ");
                qp.GroupBy.ForEach(g => getQuery.AppendLine(g + ","));
                getQuery.Remove(getQuery.Length - 1, 1); //remove last ,
            }

            // HAVING


            // ORDER BY
            if (qp.OrderBy.Count>0)
            {
                getQuery.Append("ORDER BY ");
                qp.OrderBy.ForEach(o => getQuery.AppendLine($"{o.OrderBy} {(o.IsAcending ? "" : "DESC")},"));
                getQuery.Remove(getQuery.Length - 1, 1); //remove last ,

                // OFFSET kullanılması için ORDER BY olması gerekir.
                //OFFSET @rowNumber rows fetch next @rowPerPage rows only
                getQuery.AppendLine($"OFFSET {qp.RowNumber} rows fetch next {qp.RowPerPage} rows only");


            }

            return getQuery.ToString();
        }
        private string Filter(Dictionary<string, string> filter, out Dictionary<string, object> prt)
        {
            prt = new Dictionary<string, object>();
            string rtn = "";
            foreach (KeyValuePair<string, string> item in filter)
            {
                switch (item.Key)
                {
                    case "FilterProductName":
                        {
                            if (!string.IsNullOrEmpty(item.Value))
                            {
                                List<string> list = new List<string>();
                                int i = 0;
                                foreach (string pName in item.Value.Trim().Split(' '))
                                {
                                    if (pName.Trim() == "" || list.Contains(pName.Trim()))
                                    {
                                        continue;
                                    }

                                    list.Add(pName.Trim());
                                    rtn += " and p.ProductName like @pName" + i;
                                    prt.Add("@pName" + i, "%" + pName + "%");
                                    i++;
                                }
                            }
                        }
                        break;
                    case "BrandName":
                        {
                            rtn += " and BrandName = @BrandName";
                            prt.Add("@BrandName", item.Value);
                        }
                        break;


                }
            }


            return rtn;
        }


        private string Filter(List<QueryFilter> filters,out Dictionary<string, object> parameters)
        {
            var qf = new StringBuilder("Where 1=1");
            parameters = new Dictionary<string, object>();
            foreach (QueryFilter filter in filters)
            {
                switch (filter.conditionOperator)
                {
                    case QueryFilter.ConditionOperator.Equals:
                        qf.Append($" and {filter.FilterKey} = @{filter.FilterKey},");
                        parameters.Add(filter.FilterKey, filter.FilterValue);
                        break;
                    case QueryFilter.ConditionOperator.EqualsNot:
                        qf.Append($" and {filter.FilterKey} != {filter.FilterKey},");
                        parameters.Add(filter.FilterKey, filter.FilterValue);
                        break;
                    case QueryFilter.ConditionOperator.Bigger:
                        qf.Append($" and {filter.FilterKey} > {filter.FilterKey},");
                        parameters.Add(filter.FilterKey, filter.FilterValue);
                        break;
                    case QueryFilter.ConditionOperator.Smaller:
                        qf.Append($" and {filter.FilterKey} < {filter.FilterKey},");
                        parameters.Add(filter.FilterKey, filter.FilterValue);
                        break;
                    case QueryFilter.ConditionOperator.EqBigger:
                        qf.Append($" and {filter.FilterKey} >= {filter.FilterKey},");
                        parameters.Add(filter.FilterKey, filter.FilterValue);
                        break;
                    case QueryFilter.ConditionOperator.EqSmaller:
                        qf.Append($" and {filter.FilterKey} <= {filter.FilterKey},");
                        parameters.Add(filter.FilterKey, filter.FilterValue);
                        break;
                    case QueryFilter.ConditionOperator.In:
                        qf.Append($" and {filter.FilterKey} in({filter.FilterKey}),");
                        parameters.Add(filter.FilterKey, filter.FilterValue);
                        break;
                    case QueryFilter.ConditionOperator.NotIn:
                        qf.Append($" and {filter.FilterKey} not in ({filter.FilterKey}),");
                        parameters.Add(filter.FilterKey, filter.FilterValue);
                        break;
                    case QueryFilter.ConditionOperator.Like:
                        qf.Append($" and {filter.FilterKey} like%{filter.FilterKey}%,");
                        parameters.Add(filter.FilterKey, filter.FilterValue);
                        break;
                    case QueryFilter.ConditionOperator.NotLike:
                        qf.Append($" and {filter.FilterKey} not like%{filter.FilterKey}%,");
                        parameters.Add(filter.FilterKey, filter.FilterValue);
                        break;
                    default:
                        qf.Append($" and {filter.FilterKey} = {filter.FilterKey},");
                        parameters.Add(filter.FilterKey, filter.FilterValue);
                        break;
                }
            }

            qf.Remove(qf.Length - 1, 1);

            if (filters.Count >0)
            {
                return qf.ToString();
            }

            return "";

        }


        public virtual void Delete(int id)
        {
            string strSQL = @$" DELETE FROM {_tableName} 
                                WHERE ID=@ID";
            try
            {
                using (var cn = CreateConnection())
                {
                    cn.Execute(strSQL, new { ID = id });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public virtual T GetByID(int id)
        {
            T entity = null;
            string strSql = @$" Select *
                            from {_tableName}
                            where ID=@ID ";

            try
            {
                using (var cn = CreateConnection())
                {
                    entity = cn.QuerySingleOrDefault<T>(strSql, new { ID = id });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return entity;
        }


        private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                    let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
                    select prop.Name).ToList();
        }

        public virtual int Add(T entity)
        {
            int id = 0;
            var insertQuery = GenerateAddQuery();
            try
            {
                using (var cn = CreateConnection())
                {
                    id = cn.ExecuteScalar<int>(insertQuery, entity);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return id;

        }

        private string GenerateAddQuery()
        {
            var insertQuery = new StringBuilder($"INSERT INTO {_tableName} ");

            insertQuery.Append("(");

            var properties = GenerateListOfProperties(GetProperties);

            properties.Remove("ID");


            properties.ForEach(prop => { insertQuery.Append($"[{prop}],"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(@")
                            OUTPUT INSERTED.ID
                            VALUES (");

            properties.ForEach(prop => { insertQuery.Append($"@{prop},"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(")");

            return insertQuery.ToString();
        }

        public virtual void Update(T entity)
        {
            var updateQuery = GenerateUpdateQuery();

            try
            {
                using (var cn = CreateConnection())
                {
                    cn.Execute(updateQuery, entity);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string GenerateUpdateQuery()
        {
            var updateQuery = new StringBuilder($"UPDATE {_tableName} SET ");
            var properties = GenerateListOfProperties(GetProperties);

            properties.ForEach(property =>
            {
                if (!property.Equals("ID"))
                {
                    updateQuery.Append($"{property}=@{property},");
                }
            });

            updateQuery.Remove(updateQuery.Length - 1, 1); //remove last comma
            updateQuery.Append(" WHERE Id=@Id");

            return updateQuery.ToString();
        }

    }
}
