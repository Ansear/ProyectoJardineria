using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order : BaseEntityInt
    {
        public DateTime OrderDate { get; set; }
        public DateOnly ExpectedDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string OrderStatus { get; set; }
        public string OrderComments { get; set; }
        public Payment Payments { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<OrderCustomerEmployee> OrderCustomerEmployees { get; set; }
    }
}