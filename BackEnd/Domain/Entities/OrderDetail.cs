using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderDetail : BaseEntityInt
    {
        public string ProductCode { get; set; }
        public Product Product { get; set; }
        public int OrderCode { get; set; }
        public Order Order { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public string LineNumber { get; set; }
    }
}