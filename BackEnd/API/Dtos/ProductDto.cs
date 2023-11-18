using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ProductDto
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public string ProductDimensions { get; set; }
        public string ProductDescription { get; set; }
        public int ProductSalesPrice { get; set; }
        public int InStockQuantity { get; set; }
        public int SupplierPrice { get; set; }
    }
}