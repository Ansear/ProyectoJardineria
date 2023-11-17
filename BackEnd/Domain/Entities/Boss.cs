using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Boss : BaseEntityVarchar
    {
        public string IdEmployee { get; set; }
        public Employee Employees { get; set; }
    }
}