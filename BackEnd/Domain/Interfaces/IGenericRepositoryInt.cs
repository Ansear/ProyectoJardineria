using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces;
public interface IGenericRepositoryInt<T> where T : BaseEntityInt
{
    Task<T> GetByIdAsync(int Id);
    Task<IEnumerable<T>> GetAllAsync();
    IEnumerable<T> Find(Expression<Func<T, bool>> expression);
    void Add(T Entity);
    void AddRange(IEnumerable<T> Entities);
    void Remove(T Entity);
    void RemoveRange(IEnumerable<T> Entities);
    void Update(T Entity);

}