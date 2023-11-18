using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Persistence.Data;

namespace APP.Repository;
public class RolRepository : GenericRepositoryInt<Rol>, IRol
{
    private readonly GardenContext _context;

    public RolRepository(GardenContext context) : base(context)
    {
       _context = context;
    }
}
