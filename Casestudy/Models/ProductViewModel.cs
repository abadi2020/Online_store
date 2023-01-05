using System.Collections.Generic;
namespace Casestudy.Models
{
    public class ProductViewModel
    {
        public string BrandName { get; set; }
        public int BrandId { get; set; }
        public string BRAND { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public string Id { get; set; }
        public int Qty { get; set; }
        
        public string ProductName { get; set; }
        
        public string GraphicName { get; set; }

        public decimal CostPrice { get; set; }

        public decimal MSRB { get; set; }

        public int QtyOnHanad { get; set; }

        public int QtyOnBackOrder { get; set; }

        public string Description { get; set; }
        public string JsonData { get; set; }
    }
}