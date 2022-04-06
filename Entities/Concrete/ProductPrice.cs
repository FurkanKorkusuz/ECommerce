using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class ProductPrice :IEntity
    {
        public int ID { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CampaingPrice { get; set; }
        public decimal DiscountPrice { get; set; }
    }
}
