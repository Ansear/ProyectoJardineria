using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Boss
    {
        public string IdEmployee { get; set; }
        public string IdBoss { get; set; }
        public Employee Employees { get; set; }
    }
}