using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductSupplier
    {
        public int IdSupplier { get; set; }
        public Supplier Supplier { get; set; }
        public string IdProduct { get; set; }
        public Product Product { get; set; }
    }
}