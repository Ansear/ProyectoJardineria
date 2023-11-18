using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TypePerson : BaseEntityInt
    {
        public string Type { get; set; }
        public ICollection<Supplier> Suppliers { get; set; }
        public ICollection<Customer> Customers { get; set; }
    }
}