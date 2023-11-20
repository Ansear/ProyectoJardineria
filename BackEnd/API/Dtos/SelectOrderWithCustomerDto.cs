using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class SelectOrderWithCustomerDto
    {
        public int OrderCode { get; set; }
        public int CustomerCode { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime ExpectedDate { get; set; }

    }
}