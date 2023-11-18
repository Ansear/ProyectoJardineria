using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : BaseEntityInt
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Customer Customer { get; set; }
        public Employee Employee { get; set; }
        public ICollection<Rol> Rols { get; set; } = new HashSet<Rol>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();
        public ICollection<UserRol> UsersRols { get; set; }
    }
}