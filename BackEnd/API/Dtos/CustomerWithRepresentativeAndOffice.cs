using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class CustomerWithRepresentativeAndOffice
    {
        public string CustomerName { get; set; }
        public string RepresentativeName { get; set; }
        public string OfficeCity { get; set; }
    }
}