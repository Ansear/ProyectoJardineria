using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace APP.Repository;
public class GenericRepositoryInt<T> : IGenericRepositoryInt<T> where T : BaseEntityInt
{
    private readonly GardenContext _context;

    public GenericRepositoryInt(GardenContext context)
    {
        _context = context;
    }

    public void Add(T Entity)
    {
        _context.Set<T>().Add(Entity);

    }

    public void AddRange(IEnumerable<T> Entities)
    {
        _context.Set<T>().AddRange(Entities);

    }

    public virtual IEnumerable<T> Find(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().Where(expression);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public virtual async Task<T> GetByIdAsync(int Id)
    {
        return await _context.Set<T>().FindAsync(Id);
    }

    public void Remove(T Entity)
    {
        _context.Set<T>().Remove(Entity);
    }

    public void RemoveRange(IEnumerable<T> Entities)
    {
        _context.Set<T>().RemoveRange(Entities);
    }

    public void Update(T Entity)
    {
        _context.Set<T>().Update(Entity);
    }
}