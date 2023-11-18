using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers;
public class Authorization
{
    public enum Rols
    {
        Administrator,
        Customer,
        Emloyee
    }

    public const Rols rol_default = Rols.Customer;
}