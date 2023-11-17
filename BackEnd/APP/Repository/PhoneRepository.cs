using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Persistence.Data;

namespace APP.Repository;
public class PhoneRepository : GenericRepositoryInt<Phone>, IPhone
{
    private readonly GardenContext _context;

    public PhoneRepository(GardenContext context) : base(context)
    {
        _context = context;
    }
}