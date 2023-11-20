using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities;
    public class StatusOrder : BaseEntityInt
    {
        public string Description { get; set; }
        public ICollection<Order> Orders { get; set; }
    }