using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Supplier : IEntity
    {
        public int ID { get; set; }
        public string SupplierName { get; set; }
        public int CountryID { get; set; }
    }
}
