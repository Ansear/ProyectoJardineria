using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities;
public class Office : BaseEntityVarchar
{
    [Required]
    public int IdPhone { get; set; }
    public Phone Phones { get; set; }
    [Required]
    public int IdAddress { get; set; }
    public Address Address { get; set; }
    public ICollection<OfficeEmployee> OfficeEmployees { get; set; }
}