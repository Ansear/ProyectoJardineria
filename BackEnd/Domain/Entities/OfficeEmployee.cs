using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OfficeEmployee : BaseEntityVarchar
    {
        public string IdEmployee { get; set; }
        public string IdOffice { get; set; }
        public Employee Employees { get; set; }
        public Office Offices { get; set; }
    }
}