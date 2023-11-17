using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities;
public class Phone : BaseEntityInt
{
    public string PhoneNumber { get; set; }
    public ICollection<Office> Offices { get; set; }
}