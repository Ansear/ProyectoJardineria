using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IEmployee : IGenericRepositoryVarchar<Employee>
    {
        Task<IEnumerable<Employee>> GetEmployeesByIdBoss(string id);
        Task<IEnumerable<Employee>> GetEmployeeNotSalesRepresentative();
    }
}