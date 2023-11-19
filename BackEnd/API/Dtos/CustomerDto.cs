using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerLastName { get; set; }
        
    }
}