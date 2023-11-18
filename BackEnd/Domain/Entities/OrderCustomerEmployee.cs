using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderCustomerEmployee 
    {
        public int IdOrder { get; set; }
        public Order Order { get; set; }
        public int IdCustomer { get; set; }
        public Customer Customer { get; set; }
        public string IdEmployee { get; set; }
        public Employee Employee { get; set; }
    }
}