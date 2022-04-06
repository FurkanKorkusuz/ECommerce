using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
     public class ProductImage : IEntity
    {
        public int ID { get; set; }
        public string Image_1 { get; set; }
        public string Image_2 { get; set; }
        public string Image_3 { get; set; }
        public string Image_4 { get; set; }
        public string Image_5 { get; set; }
    }
}
