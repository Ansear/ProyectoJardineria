using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class OrdersCountByStatusDto
    {
        public string Status { get; set; }
        public int OrdersCount { get; set; }
    }
}