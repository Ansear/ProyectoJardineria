using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product : BaseEntityVarchar
    {
        public string ProductName { get; set; }
        public string ProductDimensions { get; set; }
        public string ProductDescription { get; set; }
        public int ProductSalesPrice { get; set; }
        public int InStockQuantity { get; set; }
        public int SupplierPrice { get; set; }
        public string IdGamma { get; set; }
        public ProductGamma Gamma { get; set; }
        public ICollection<ProductSupplier> ProductSuppliers { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}