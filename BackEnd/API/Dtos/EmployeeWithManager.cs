using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class EmployeeWithManager
    {
        public string EmployeeName { get; set; }
        public string ManagerName { get; set; }
    }
}