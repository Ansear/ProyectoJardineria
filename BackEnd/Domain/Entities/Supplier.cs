using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Supplier : BaseEntityInt
    {
        public int SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public int PhoneId { get; set; }
        public Phone Phones { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public int TypePersonId { get; set; }
        public TypePerson TypePerson { get; set; }
        public ICollection<ProductSupplier> ProductSuppliers { get; set; }
    }
}