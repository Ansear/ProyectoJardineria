using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers;
public class JWT
{
    public string HasKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public double DuracionInMinutes { get; set; }
}