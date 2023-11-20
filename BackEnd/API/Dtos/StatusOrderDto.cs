using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace API.Dtos;
    public class StatusOrderDto : DtoBaseInt
    {
        public string Description { get; set; }
    }