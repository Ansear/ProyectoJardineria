using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order : BaseEntityInt
    {
        public DateTime OrderDate { get; set; }
        public DateTime ExpectedDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int IdStatus { get; set; }
        public StatusOrder StatusOrder { get; set; }
        public string OrderComments { get; set; }
        public int IdPayment { get; set; }
        public Payment Payment { get; set; }
        public int Total { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<OrderCustomerEmployee> OrderCustomerEmployees { get; set; }
    }
}