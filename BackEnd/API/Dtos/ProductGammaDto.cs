using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ProductGammaDto
    {
        public string Id { get; set; }
        public string TextDescription { get; set; }
        public string HtmlDescription { get; set; }
        public string Image { get; set; }
    }
}