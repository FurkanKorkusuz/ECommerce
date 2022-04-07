using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.Dapper
{

    public class GenericQueries<T>
    {
        private readonly string _tableName;
        private readonly string _tableNameShort;

        public GenericQueries()
        {
            _tableName = GetTableName<T>();
        }
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
        private IEnumerable<PropertyInfo> GetProperties => typeof(T).GetProperties();
        private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                    let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
                    select prop.Name).ToList();
        }
        public virtual string GetAll( object filter, object join, bool orderbyID =true)
        {

            var Query = new StringBuilder("Select");

            var properties = GenerateListOfProperties(GetProperties);
            properties.ForEach(prop => { Query.Append($"[{prop}],"); });



            return Query.ToString();
        }
        private void TableNameShort()
        {

        }
        private string GenerateInsertQuery()
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
    }
}
