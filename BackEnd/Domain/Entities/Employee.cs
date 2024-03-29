using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Employee : BaseEntityVarchar
    {
        public string EmployeeName { get; set; }
        public string EmployeeLastName { get; set; }
        public string Extension { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeePosition { get; set; }
        public int IdUser { get; set; }
        public User User { get; set; }
        public string IdBoss { get; set; }
        public Employee Boss { get; set; }
        public ICollection<OrderCustomerEmployee> OrderCustomerEmployees { get; set; }
        public ICollection<OfficeEmployee> OfficeEmployee { get; set; }
    }
}