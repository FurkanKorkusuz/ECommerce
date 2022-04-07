using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.Dapper
{
    public class QueryParameter
    {
        public int RowNumber { get; set; } = 0;
        public int RowPerPage { get; set; } = 30;
        public bool MaxRow { get; set; } = false;
        public string Sort { get; set; } = "ID";
        public bool OrderByAsc { get; set; } = true;
        public List<QueryFilter> FilterList { get; set; } = new List<QueryFilter>();
      
    }
}
