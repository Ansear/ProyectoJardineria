using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities;
public class Address : BaseEntityInt
{
    public string TypeRoad { get; set; }
    public int MainNumber { get; set; }
    public string MainLetter { get; set; }
    public string Bis { get; set; }
    public string SecondaryLetter { get; set; }
    public string CardinalPrimary { get; set; }
    public int SecondNumber { get; set; }
    public string CardinalSecondary { get; set; }
    public string Complement { get; set; }
    public string ZipCode { get; set; }
    public int IdCity { get; set; }
    public City Cities { get; set; }
    public ICollection<Office> Offices { get; set; }
}