using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class OrderDetailDto
    {
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public string LineNumber { get; set; }
    }
}