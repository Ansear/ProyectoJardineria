using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public DateTime PaymentDate { get; set; }
        public int Total { get; set; }

    }
}