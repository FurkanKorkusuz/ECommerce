using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.Dapper
{

    public class QueryParameter
    {    /// <summary>
         /// GetAll ? true >>>>> select * from table 
         /// : false >>>>> select * from table where... order by ... offset  @RowNumber rows fetch next @RowPerPage rows
         /// </summary>
        public bool GetAll { get; set; } = false;
        public int RowNumber { get; set; } = 0;
        public int RowPerPage { get; set; } = 30;
        public int TopRow { get; set; } = 0;
        public string Sort { get; set; } = "ID";
        public bool OrderByAsc { get; set; } = true;
        public List<QueryFilter> FilterList { get; set; } = new List<QueryFilter>();
      
    }
}
