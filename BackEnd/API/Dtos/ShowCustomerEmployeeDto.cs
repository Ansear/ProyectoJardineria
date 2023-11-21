using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ShowCustomerEmployeeDto
    {
        public string CustomerName { get; set; }
        public string SalesRepresentativeName { get; set; }
        public string SalesRepresentativeLastName { get; set; }
    }
}