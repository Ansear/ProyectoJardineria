using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PaymentForm : BaseEntityVarchar
    {
        public string PaymentFormName { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}