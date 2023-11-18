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
        public Employee Employee { get; set; }
        public Office Office { get; set; }
    }
}