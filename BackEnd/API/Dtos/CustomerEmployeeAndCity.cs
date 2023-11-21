using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class CustomerEmployeeAndCity
    {
        public string CustomerName { get; set; }
        public string CustomerLastName { get; set; }
        public string SalesRepresentativeName { get; set; }
        public string SalesRepresentativeLastName { get; set; }
        public string OfficeCity { get; set; }
    }
}