using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities;
public class City : BaseEntityInt
{
    public string Name { get; set; }
    [Required]
    public int IdState { get; set; }
    public State States { get; set; }
    public ICollection<Address> Address { get; set; }
}