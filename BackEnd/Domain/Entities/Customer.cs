using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Customer : BaseEntityInt
    {
        public string CustomerName { get; set; }
        public string CustomerLastName { get; set; }
        public int CustomerPhoneId { get; set; }
        public Phone Phones { get; set; }
        public string CustomerFax { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public decimal CreditLimit { get; set; }
        public int TypePersonId { get; set; }
        public TypePerson TypePerson { get; set; }
        public ICollection<OrderCustomerEmployee> OrderCustomerEmployees { get; set; }
    }
}