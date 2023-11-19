using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities;
public class Rol : BaseEntityInt
{
    public string RolName { get; set; }
    public ICollection<User> Users { get; set; } = new HashSet<User>();
    public ICollection<UserRol> UsersRols { get; set; }
}
