using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Boss : BaseEntityVarchar
    {
        public string Name { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}