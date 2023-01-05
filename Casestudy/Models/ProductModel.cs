using System.Collections.Generic;
using System.Linq;
namespace Casestudy.Models
{
    public class ProductModel
    {
        private AppDbContext _db;
        public ProductModel(AppDbContext context)
        {
            _db = context;
        }
        public List<Product> GetAll()
        {
            return _db.Products.ToList();
        }
        public List<Product> GetAllByBrand(int id)
        {
            return _db.Products.Where(item => item.Brand.Id == id).ToList();
        }

        public Product GetById(string id)
        {
            return _db.Products.Where(item => item.Id == id).FirstOrDefault();
        }
    }
}