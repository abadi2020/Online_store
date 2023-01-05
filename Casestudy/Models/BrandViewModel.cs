

/*using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
namespace Casestudy.Models
{
    public class BrandViewModel
    {
        public BrandViewModel() { }
        public string BrandName { get; set; }
        public int Id { get; set; }
        public List<Brand> Brands { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<SelectListItem> GetBrands()
        {
            return Brands.Select(brand => new SelectListItem { Text = brand.Name, Value = brand.Id.ToString() });
        }
    }
}*/

using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace Casestudy.Models
{
    public class BrandViewModel
    {
        private List<Brand> _brands;
        public IEnumerable<Product> Products { get; set; }
        [Required]
        public int Qty { get; set; }
        public string Id { get; set; }
        public string BrandName { get; set; }
        public int BrandId { get; set; }
        public IEnumerable<SelectListItem> GetBrands()
        {
            return _brands.Select(brand => new SelectListItem { Text = brand.Name, Value = Convert.ToString(brand.Id) });
        }
        public void SetBrands(List<Brand> brs)
        {
            _brands = brs;
        }
    }
}