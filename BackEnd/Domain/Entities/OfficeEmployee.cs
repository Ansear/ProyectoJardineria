using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OfficeEmployee
    {
        public int IdEmployee { get; set; }
        public int IdOffice { get; set; }
        public Employee Employees { get; set; }
        // public Office Offices { get; set; }
    }
}