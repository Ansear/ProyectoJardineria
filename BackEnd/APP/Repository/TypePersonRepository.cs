using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APP.Repository;
using Domain.Entities;
using Domain.Interfaces;
using Persistence.Data;

namespace APP.Interfaces
{
    public class TypePersonRepository : GenericRepositoryInt<TypePerson>, ITypePerson
    {
        private readonly GardenContext _context;

        public TypePersonRepository(GardenContext context) : base(context)
        {
            _context = context;
        }
    }
}