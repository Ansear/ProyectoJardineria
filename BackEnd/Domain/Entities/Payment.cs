using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Payment : BaseEntityInt
    {
        public DateTime PaymentDate { get; set; }
        public string IdFormPay { get; set; }
        public PaymentForm PaymentForm { get; set; }
        public Order Order { get; set; }
    }
}