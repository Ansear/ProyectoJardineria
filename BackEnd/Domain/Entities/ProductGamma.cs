using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductGamma : BaseEntityVarchar
    {
        public string TextDescription { get; set; }
        public string HtmlDescription { get; set; }
        public string Image { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}